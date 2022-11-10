using HtmlAgilityPack;
using SpiderTool.Dto.Spider;
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
            await Parallel.ForEachAsync(data, cancellationToken, async (url, ct) =>
            {
                var client = httpRequestPool.GetInstance();
                var uri = new Uri(url.Key);
                var result = await client.HttpGetCore(uri.ToString(), cancellationToken: ct);
                log?.Invoke($"BulkDownload 请求 {url}");
                var fileName = uri.Segments.Last();
                if (!TryGetExtension(fileName, out var extension))
                {
                    var contentType = result.Content.Headers.ContentType?.MediaType;
                    extension = GetExtensionFromContentType(contentType);
                    if (extension == null)
                        return;

                    fileName = url.Value + extension;
                }
                var path = Path.Combine(dirRoot, fileName);
                var fileBytes = await result.Content.ReadAsByteArrayAsync(cancellationToken: ct);
                if (fileBytes.Length > 0)
                    await File.WriteAllBytesAsync(path, fileBytes, cancellationToken);
                else
                    log?.Invoke($"BulkDownload for {url}, file bytes = 0");
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

        public static async Task SaveTextAsync(string dir, string str)
        {
            try
            {
                var path = Path.Combine(dir.GetDirectory(), DateTime.Now.Ticks + ".txt");
                await File.WriteAllTextAsync(path, str);
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
                finalText = Regex.Replace(finalText, handle.ReplacementOldStr!, handle.ReplacementNewlyStr ?? "", RegexOptions.IgnoreCase);
                // finalText = finalText.Replace(handle.ReplacementOldStr!, handle.ReplacementNewlyStr, handle.IgnoreCase, System.Globalization.CultureInfo.CurrentCulture);
            }
            var temp = new HtmlDocument();
            temp.LoadHtml(finalText);
            return temp.DocumentNode.InnerText;
        }

        public static async Task MergeTextFileAsync(string rootDir)
        {
            var rootDirInfo = new DirectoryInfo(rootDir);
            if (!rootDirInfo.Exists)
                return;

            var dirs = rootDirInfo.GetDirectories();
            foreach (var currentDirInfo in dirs)
            {
                var files = currentDirInfo.GetFiles().Where(x => x.Extension.ToLower() == ".txt").OrderBy(x => x.CreationTime).Select(x => x.FullName).ToList();
                if (files.Count == 0)
                    return;

                var fileName = GetDirToName(rootDirInfo, currentDirInfo) + ".txt";
                var filePath = Path.Combine(rootDir, fileName);
                foreach (var file in files)
                {
                    await File.AppendAllTextAsync(filePath, File.ReadAllText(file));
                    File.Delete(file);
                }

                var allFiles = currentDirInfo.GetFiles().OrderBy(x => x.CreationTime).Select(x => x.FullName).ToList();
                if (allFiles.Count == 0)
                    Directory.Delete(currentDirInfo.FullName);
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

    }
}
