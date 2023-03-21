using HtmlAgilityPack;
using SpiderTool.Data.Dto.Spider;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Utility.Extensions;
using Utility.Http;

namespace SpiderTool
{
    public static class SpiderUtility
    {
        public static readonly char[] InvalidFolderSymbol = new char[] { '\\', '/', '|', ':', '*', '^', '<', '>', '?' };
        public static string RenameFolder(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in str)
            {
                if (InvalidFolderSymbol.Contains(c))
                    sb.Append('_');
                else
                    sb.Append(c);
            }
            return sb.ToString();
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

        public static async Task BulkDownload(string dir, List<string> urls, Action<string>? log = null, CancellationToken cancellationToken = default)
        {
            var snowFlake = Utility.GuidHelper.Snowflake.GetInstance(1);
            var data = urls.Distinct().ToDictionary(x => x, x => snowFlake.NextId().ToString());
            var dirRoot = dir.GetDirectory();
            using var httpRequestPool = new HttpClientPool();
            await Parallel.ForEachAsync(data, cancellationToken, async (item, ct) =>
            {
                if (string.IsNullOrEmpty(item.Key))
                    return;

                var client = httpRequestPool.GetInstance();
                var uri = new Uri(item.Key);
                var result = await client.HttpGetCore(uri.ToString(), cancellationToken: ct);
                log?.Invoke($"BulkDownload 请求 {item}");
                var fileName = uri.Segments.Last();
                if (!TryGetExtension(fileName, out var extension))
                {
                    var contentType = result.Content.Headers.ContentType?.MediaType;
                    extension = GetExtensionFromContentType(contentType);
                    if (extension == null)
                        return;

                    fileName = item.Value + extension;
                }
                var path = Path.Combine(dirRoot, fileName);
                var fileBytes = await result.Content.ReadAsByteArrayAsync(cancellationToken: ct);
                if (fileBytes.Length > 0)
                    await File.WriteAllBytesAsync(path, fileBytes, ct);
                else
                    log?.Invoke($"BulkDownload for {item}, file bytes = 0");
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
            var tempExtension = fileName.Substring(lastIndex, fileName.Length - lastIndex);
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
                if (StringExtension.MimeMapping[key].ToLower() == contentType.ToLower())
                {
                    return key;
                }
            }
            return null;
        }

        public static async Task SaveTextAsync(string dir, string str, CancellationToken cancellationToken = default)
        {
            try
            {
                var path = Path.Combine(dir.GetDirectory(), DateTime.Now.Ticks + ".txt");
                await File.WriteAllTextAsync(path, str, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static string ReadHtmlNodeInnerHtml(HtmlNode item, TemplateDetailViewModel rule)
        {
            var finalText = HttpUtility.HtmlDecode(item.InnerHtml);
            foreach (var handle in rule.ReplacementRules)
            {
                finalText = Regex.Replace(finalText, handle.ReplacementOldStr.ToDBC(), handle.ReplacementNewlyStr ?? "", RegexOptions.IgnoreCase);
                finalText = Regex.Replace(finalText, handle.ReplacementOldStr, handle.ReplacementNewlyStr ?? "", RegexOptions.IgnoreCase);
            }
            var temp = new HtmlDocument();
            temp.LoadHtml(finalText);
            return temp.DocumentNode.InnerText;
        }

        public static async Task MergeTextFileAsync(string rootDir, CancellationToken cancellationToken = default)
        {
            var rootDirInfo = new DirectoryInfo(rootDir);
            if (!rootDirInfo.Exists)
                return;

            var dirs = rootDirInfo.GetDirectories();
            var dirsCount = dirs.Count();
            foreach (var currentDirInfo in dirs)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var allFiles = currentDirInfo.GetFiles().OrderBy(x => x.CreationTime).ToList();
                if (allFiles.Count == 0)
                {
                    Directory.Delete(currentDirInfo.FullName);
                    continue;
                }

                var files = allFiles.Where(x => x.Extension.ToLower() == ".txt").OrderBy(x => x.CreationTime).Select(x => x.FullName).ToList();
                if (files.Count != 0)
                {
                    var fileName = GetDirToName(rootDirInfo, currentDirInfo) + ".txt";
                    var filePath = Path.Combine(rootDir, fileName);
                    foreach (var file in files)
                    {
                        var txt = await File.ReadAllTextAsync(file, cancellationToken);
                        await File.AppendAllTextAsync(filePath, txt, cancellationToken);
                        File.Delete(file);
                    }
                }

                allFiles = currentDirInfo.GetFiles().OrderBy(x => x.CreationTime).ToList();
                if (dirsCount == 1)
                {
                    allFiles.ForEach(file =>
                    {
                        File.Move(file.FullName, Path.Combine(rootDir, file.Name));
                    });
                    Directory.Delete(currentDirInfo.FullName);
                }
            }
        }

        private static string GetDirToName(DirectoryInfo rootDirInfo, DirectoryInfo currentDirInfo)
        {
            List<string> sb = new List<string>();
            DirectoryInfo? point = new DirectoryInfo(currentDirInfo.FullName);
            while (point != null && point.FullName != rootDirInfo.FullName)
            {
                sb.Add(point.Name);
                point = point.Parent;
            }
            sb.Add(rootDirInfo.Name);
            sb.Reverse();
            return string.Join('_', sb);
        }

        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

    }
}
