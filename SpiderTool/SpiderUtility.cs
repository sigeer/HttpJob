using HtmlAgilityPack;
using SpiderTool.Dto.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utility.Extensions;
using Utility.Http;

namespace SpiderTool
{
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

        public static async Task BulkDownload(string dir, List<string> urls, Action<string>? log = null, CancellationToken cancellationToken = default)
        {
            var snowFlake = Utility.GuidHelper.Snowflake.GetInstance(1);
            var data = urls.Distinct().ToDictionary(x => x, x => snowFlake.NextId().ToString());
            var dirRoot = dir.GetDirectory();
            using var httpRequestPool = new HttpClientPool();
            await Parallel.ForEachAsync(data, cancellationToken, async (url, ct) =>
            {
                var client = httpRequestPool.GetHttpClient();
                var uri = new Uri(url.Key);
                var result = await client.HttpGetCore(uri.ToString(), cancellationToken: ct);
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
                    log?.Invoke($"for {url}, file bytes = 0");
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

        public static string ReadHtmlNodeInnerHtml(HtmlNode item, TemplateEditDto rule)
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

        public static async Task MergeTextFileAsync(string dir)
        {
            var dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
                throw new Exception("dir not exist");

            var files = dirInfo.GetFiles().Where(x => x.Extension.ToLower() == ".txt").OrderBy(x => x.CreationTime).Select(x => x.FullName).ToList();
            if (files.Count == 0)
                return;

            var filePath = Path.Combine(dir, $"{dirInfo.Name}.txt");
            foreach (var file in files)
            {
                await File.AppendAllTextAsync(filePath, File.ReadAllText(file));
                File.Delete(file);
            }

            var dirs = dirInfo.GetDirectories();
            foreach (var childDir in dirs)
            {
                await MergeTextFileAsync(childDir.FullName);
            }
        }

    }
}
