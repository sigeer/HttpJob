using HtmlAgilityPack;
using SpiderTool.Constants;
using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using SpiderTool.Tasks;
using System.Text;
using System.Text.RegularExpressions;
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
        /// 会经过下一页设置进行变换
        /// </summary>
        private string? _currentUrl;
        private string? _currentDir;
        public string CurrentDir
        {
            get
            {
                if (string.IsNullOrEmpty(_currentDir))
                {
                    _currentDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "download", $"task{_taskId}");
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
        private int _taskId;
        private string HostUrl => _rootUrl.GetHostUrl();

        readonly ISpiderService _service;

        public event EventHandler<string>? OnTaskComplete;
        public event EventHandler<string>? OnLog;
        public event EventHandler<int>? OnTaskStart;
        public event EventHandler<int>? OnTaskStatusChanged;

        public SpiderWorker(ISpiderService service)
        {
            _service = service;
        }

        public async Task Start(string url, int spiderId)
        {
            _spider = _service.GetSpider(spiderId);
            if (Spider == null)
                return;

            _rootUrl = url;
            _taskId = _service.AddTask(new Tasks.TaskSetter
            {
                RootUrl = _rootUrl,
                SpiderId = spiderId,
                Status = (int)TaskType.NotEffective
            });
            OnTaskStart?.Invoke(this, _taskId);
            OnTaskStatusChanged?.Invoke(this, _taskId);
            await ProcessUrl(url);
            CompleteTask();
        }

        public void CompleteTask()
        {
            //_service.SubmitResouceHistory(new ResourceHistorySetter()
            //{
            //    Url = _rootUrl,
            //    Name = DocumentTitle,
            //    SpiderId = Spider.Id
            //});
            _service.SetTaskStatus(_taskId, (int)TaskType.Completed);
            OnTaskComplete?.Invoke(this, CurrentDir);
            OnTaskStatusChanged?.Invoke(this, _taskId);
            MergeTextFile(CurrentDir);
        }

        public async Task<string> LoadDocumentContent()
        {
            HttpResponseMessage res;
            var url = _currentUrl!.GetTotalUrl(HostUrl);
            if (Spider.Method == RequestMethod.POST)
                res = await HttpRequest.PostRawAsync(url, Spider.PostObj, Spider.GetHeaders());
            else
                res = await HttpRequest.GetRawAsync(url, Spider.GetHeaders());

            var responseStream = await res.Content.ReadAsStreamAsync();
            return responseStream.DecodeData(res.Content.Headers.ContentType?.CharSet);
        }

        public async Task ProcessUrl(string currentUrl)
        {
            _currentUrl = currentUrl;

            var documentContent = await LoadDocumentContent();
            _currentDoc.LoadHtml(documentContent);

            _service.UpdateTask(new Tasks.TaskSetter
            {
                Id = _taskId,
                Description = DocumentTitle,
                Status = (int)TaskType.InProgress
            });
            OnTaskStatusChanged?.Invoke(this, _taskId);

            ProcessContent();
            await MoveToNextPage();
            //OnLog?.Invoke(this, "====开始合并====");

            //Console.WriteLine("====开始打包");
            //var filePath = CurrentDir.PackZip();
            //Console.WriteLine("打包完成：" + filePath);
        }

        private void ProcessContent()
        {
            foreach (var rule in Spider.TemplateList)
            {
                var nodes = string.IsNullOrEmpty(rule.TemplateStr)
                    ? new HtmlNodeCollection(_currentDoc.DocumentNode)
                    : _currentDoc.DocumentNode.SelectNodes(rule.TemplateStr ?? "");
                if (nodes == null)
                    continue;

                if (rule.Type == (int)TemplateTypeEnum.Object)
                {
                    var urlList = nodes.Select(item => (item.Attributes["src"] ?? item.Attributes["data-src"]).Value.GetTotalUrl(HostUrl)).ToList();
                    SpiderUtility.BulkDownload(CurrentDir, urlList);
                }
                if (rule.Type == (int)TemplateTypeEnum.Text)
                {
                    foreach (var item in nodes)
                    {
                        SpiderUtility.SaveText(CurrentDir, ReadHtmlNodeInnerHtml(item, rule));
                    }
                }
                if (rule.Type == (int)TemplateTypeEnum.Html)
                {
                    foreach (var item in nodes)
                    {
                        SpiderUtility.SaveText(CurrentDir, item.InnerHtml);
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
                        newSpider.OnTaskStart += (obj, evt) =>
                        {
                            OnTaskStart?.Invoke(obj, evt);
                        };
                        newSpider.OnTaskComplete += (obj, evt) =>
                        {
                            OnTaskComplete?.Invoke(obj, evt);
                        };
                        newSpider.OnTaskStatusChanged += (obj, evt) =>
                        {
                            OnTaskStatusChanged?.Invoke(obj, evt);
                        };
                        newSpider.OnLog += (obj, log) =>
                        {
                            OnLog?.Invoke(obj, log);
                        };
                        ThreadStart childref = new ThreadStart(async () =>
                        {
                            OnLog?.Invoke(this, $"子爬虫{index}开始，Url:{url} -- thread: {Thread.CurrentThread.ManagedThreadId}");
                            await newSpider.Start(url, rule.LinkedSpiderId ?? 0);
                            OnLog?.Invoke(this, $"子爬虫{index}结束，Url:{url} -- thread: {Thread.CurrentThread.ManagedThreadId}");
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
                    await ProcessUrl(_currentUrl);
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
            var snowFlake = Utility.GuidHelper.Snowflake.GetInstance(1);
            var data  = urls.Distinct().ToDictionary(x => x, x => snowFlake.NextId().ToString());
            var dirRoot = dir.GetDirectory();
            using var httpRequestPool = new HttpClientPool();
            Parallel.ForEach(data, async url =>
             {
                 var client = httpRequestPool.GetHttpClient();
                 var uri = new Uri(url.Key);
                 var result = await client.HttpGetCore(uri.ToString());
                 var fileName = uri.Segments.Last();
                 if (!TryGetExtension(fileName, out var extension))
                 {
                     var contentType = result.Content.Headers.FirstOrDefault(x => x.Key.ToLower() == "content-type").Value.FirstOrDefault()?.ToLower();
                     extension = GetExtensionFromContentType(contentType);
                     if (extension == null)
                         return;

                     fileName = url.Value + extension;
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
