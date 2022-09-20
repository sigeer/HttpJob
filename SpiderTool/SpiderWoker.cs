using HtmlAgilityPack;
using SpiderTool.Constants;
using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.IDomain;
using SpiderTool.IService;
using System.Web;
using Utility.Extensions;
using Utility.Http;

namespace SpiderTool
{
    public class SpiderWorker
    {
        private SpiderDto? _spider;
        protected SpiderDto Spider
        {
            get
            {
                ArgumentNullException.ThrowIfNull(_spider);
                return _spider;
            }
        }
        private string DocumentTitle => HttpUtility.HtmlDecode(_currentDoc?.DocumentNode?.SelectSingleNode("//title")?.InnerText) ?? _rootUrl;
        /// <summary>
        /// 经过下一页设置进行变换
        /// </summary>
        private string? _currentUrl;
        private string? _currentDir;
        public string CurrentDir
        {
            get
            {
                if (string.IsNullOrEmpty(_currentDir))
                {
                    var sub = string.Empty;
                    var docTitle = _currentDoc.DocumentNode.SelectSingleNode("//title");
                    if (docTitle != null)
                        sub = HttpUtility.HtmlDecode(docTitle.InnerText);
                    _currentDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "download", sub ?? DateTime.Now.Ticks.ToString());
                    _currentDir.GetDirectory();
                }
                return _currentDir;
            }
        }
        private HtmlDocument _currentDoc = new HtmlDocument();
        /// <summary>
        /// 在同一个爬虫任务内始终一致
        /// </summary>
        private string _rootUrl = string.Empty;
        private string HostUrl => _rootUrl.GetHostUrl();

        readonly ISpiderService _service;

        public event EventHandler<string>? TaskComplete;
        public event EventHandler<string>? OnLog;

        public SpiderWorker(ISpiderService service)
        {
            _service = service;
        }

        public async Task Start(string url, int spiderId)
        {
            _spider = _service.GetSpider(spiderId);
            if (Spider == null)
                return;

            _currentUrl = url;
            _rootUrl = url;

            await Process();
            _service.SubmitResouceHistory(new ResourceHistorySetter()
            {
                Url = _rootUrl,
                Name = DocumentTitle,
                SpiderId = spiderId
            });
            TaskComplete?.Invoke(this, CurrentDir);
        }

        public async Task<string> LoadDocumentContent()
        {
            string documentContent;
            var url = _currentUrl!.GetTotalUrl(HostUrl);
            if (Spider.Method == RequestMethod.POST)
                documentContent = await HttpRequest.PostAsync(url, Spider.PostObj, Spider.GetHeaders());
            else
                documentContent = await HttpRequest.GetAsync(url, Spider.GetHeaders());
            return documentContent;
        }

        public async Task Process()
        {
            var documentContent = await LoadDocumentContent();
            _currentDoc.LoadHtml(documentContent);
            ExtractContent();
            await MoveToNextPage();
            OnLog?.Invoke(this, "====开始合并====");
            MergeTextFile(CurrentDir);
            //Console.WriteLine("====开始打包");
            //var filePath = CurrentDir.PackZip();
            //Console.WriteLine("打包完成：" + filePath);
        }

        private void ExtractContent()
        {
            foreach (var rule in Spider.TemplateList)
            {
                var nodes = string.IsNullOrEmpty(rule.TemplateStr)
                    ? new HtmlNodeCollection(_currentDoc.DocumentNode)
                    : _currentDoc.DocumentNode.SelectNodes(rule.TemplateStr ?? "");
                if (nodes == null)
                    return;

                if (rule.Type == (int)TemplateTypeEnum.Object)
                {
                    var urlList = nodes.Select(item => (item.Attributes["src"] ?? item.Attributes["data-src"]).Value.GetTotalUrl(HostUrl)).ToList();
                    SpiderUtility.BulkDownload(CurrentDir, urlList);
                }
                if (rule.Type == (int)TemplateTypeEnum.Text)
                {
                    foreach (var item in nodes)
                    {
                        var finalText = HttpUtility.HtmlDecode(item.InnerText);
                        SpiderUtility.SaveText(CurrentDir, finalText);
                    }
                }
                if (rule.Type == (int)TemplateTypeEnum.Html)
                {
                    foreach (var item in nodes)
                    {
                        SpiderUtility.SaveText(CurrentDir, ReadHtmlNodeInnerHtml(item, rule));
                    }
                }
                if (rule.Type == (int)TemplateTypeEnum.JumpLink)
                {
                    //新增
                    var index = 1;
                    foreach (var item in nodes)
                    {
                        var resource = (item.Attributes["href"] ?? item.Attributes["data-href"])?.Value;
                        if (resource == null)
                            continue;

                        var url = resource.GetTotalUrl(HostUrl);

                        var newSpider = new SpiderWorker(_service);
                        newSpider.TaskComplete += (obj, evt) =>
                        {
                            TaskComplete?.Invoke(obj, evt);
                        };
                        newSpider.OnLog += (obj, log) =>
                        {
                            OnLog?.Invoke(obj, log);
                        };
                        ThreadStart childref = new ThreadStart(async () =>
                        {
                            OnLog?.Invoke(this, $"{Spider.Name}_{index}号爬虫开始 Url:{url} -- thread: {Thread.CurrentThread.ManagedThreadId}");
                            await newSpider.Start(url, rule.LinkedSpiderId ?? 0);
                            OnLog?.Invoke(this, $"{Spider.Name}_{index}号爬虫结束 Url:{url} -- thread: {Thread.CurrentThread.ManagedThreadId}");
                        });
                        var th = new Thread(childref);
                        th.Start();
                        index++;
                    }
                }
            }
        }

        private string ReadHtmlNodeInnerHtml(HtmlNode item, TemplateDto rule)
        {
            var finalText = HttpUtility.HtmlDecode(item.InnerHtml);
            foreach (var handle in rule.ReplacementRules)
            {
                finalText = finalText.Replace(handle.ReplacementOldStr!, handle.ReplacementNewlyStr);
            }
            var temp = new HtmlDocument();
            temp.LoadHtml(finalText);
            return temp.DocumentNode.InnerText;
        }

        private async Task MoveToNextPage()
        {
            if (Spider.NextPageTemplate == null || string.IsNullOrEmpty(Spider.NextPageTemplate.TemplateStr))
                return;
            var nextPageNode = _currentDoc.DocumentNode.SelectSingleNode(Spider.NextPageTemplate.TemplateStr);
            if (nextPageNode != null)
            {
                _currentUrl = (nextPageNode.Attributes["href"] ?? nextPageNode.Attributes["data-href"])?.Value;
                if (_currentUrl != null)
                    await Start(_currentUrl, Spider.NextPageTemplate.LinkedSpiderId!.Value);
            }
        }

        public void MergeTextFile(string dir)
        {
            var dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
                throw new Exception("dir not exist");

            var files = dirInfo.GetFiles().Where(x => x.Extension.ToLower() == ".txt").OrderBy(x => x.CreationTime).Select(x => x.FullName).ToList();
            if (files.Count == 0)
                return;

            var filePath = Path.Combine(dir, dirInfo.Name + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            foreach (var file in files)
            {
                File.AppendAllText(filePath, File.ReadAllText(file));
            }
        }
    }

    public static class SpiderUtility
    {
        public static readonly char[] InvalidFolderSymbol = new char[] { '\\', '/', '|', ':', '*', '^', '<', '>', '\'' };
        public static string RenameFolder(this string str)
        {
            var disbaledSymbol = new char[] { '\\', '/', '|', ':', '?', '^', '<', '>', '"', '*' };
            return new string(str.ToArray().Where(x => !InvalidFolderSymbol.Contains(x)).ToArray());
        }
        public static string GetHostUrl(this Uri uri)
        {
            return $"{uri.Scheme}://{uri.Host}:{uri.Port}";
        }

        public static string GetHostUrl(this string url)
        {
            return GetHostUrl(new Uri(url));
        }

        public static string GetTotalUrl(this string url, string hostUrl)
        {
            if (url.StartsWith("/"))
            {
                return hostUrl + url;
            }
            return url;
        }

        public static string GetDirectory(this string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        public static void BulkDownload(string dir, List<string> urls)
        {
            var dirRoot = dir.GetDirectory();
            using var httpRequestPool = new HttpClientPool();
            Parallel.ForEach(urls, async url =>
             {
                 var client = httpRequestPool.GetHttpClient();
                 var uri = new Uri(url);
                 var result = await client.HttpGetCore(uri.ToString());
                 var fileName = uri.Segments.Last();
                 if (!TryGetExtension(fileName, out var extension))
                 {
                     var contentType = result.Content.Headers.FirstOrDefault(x => x.Key.ToLower() == "content-type").Value.FirstOrDefault()?.ToLower();
                     extension = GetExtensionFromContentType(contentType);
                     if (extension == null)
                         return;

                     fileName = Utility.GuidHelper.Snowflake.GetInstance(1).NextId().ToString() + extension;
                 }
                 var path = Path.Combine(dirRoot, fileName);
                 await File.WriteAllBytesAsync(path, await result.Content.ReadAsByteArrayAsync());
                 httpRequestPool.Return(client);
             });
        }

        private static bool TryGetExtension(string fileName, out string? extension)
        {
            extension = null;
            if (string.IsNullOrEmpty(fileName))
                return false;

            if (fileName.IndexOf('.') == -1)
                return false;

            var lastIndex = fileName.LastIndexOf('.');
            var tempExtension = fileName.Substring(lastIndex, fileName.Length - lastIndex - 1);
            if (tempExtension.GetMimeType() != String.Empty)
            {
                extension = tempExtension;
                return true;
            }
            return false;
        }

        private static string? GetExtensionFromContentType(string? contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return null;
            foreach (var key in StringExtension.MimeMapping.Keys)
            {
                if (StringExtension.MimeMapping[key] == contentType)
                {
                    return key;
                }
            }
            return null;
        }

        public static void SaveText(string dir, string str)
        {
            try
            {
                var path = Path.Combine(dir.GetDirectory(), DateTime.Now.Ticks + ".txt");
                File.WriteAllText(path, str);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
