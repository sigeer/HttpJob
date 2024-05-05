﻿using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SpiderTool.Data.Dto.Spider;
using System.Data;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Utility.Common;
using Utility.Extensions;

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

        public static async Task BulkDownload(string dir, List<string> urls, CancellationToken cancellationToken = default)
        {
            var snowFlake = Utility.GuidHelper.Snowflake.GetInstance(1);
            var data = urls.Distinct().ToDictionary(x => x, x => snowFlake.NextId().ToString());
            var dirRoot = dir.GetDirectory();
            using var httpRequestPool = new WorkPool<HttpClient>();
            await Parallel.ForEachAsync(data, cancellationToken, async (item, ct) =>
            {
                if (string.IsNullOrEmpty(item.Key))
                    return;

                var client = httpRequestPool.GetInstance();
                try
                {
                    var uri = new Uri(item.Key);
                    var result = await client.GetAsync(uri.ToString(), cancellationToken: ct);
                    Log.Logger.LogInformation($"BulkDownload 请求 {item}");
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
                        Log.Logger.LogInformation($"BulkDownload for {item}, file bytes = 0");
                }
                catch (Exception ex)
                {
                    Log.Logger.LogError(ex.ToString());
                }
                finally
                {
                    httpRequestPool.Return(client);
                }
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

        public static string ReadHtmlNodeInnerContent(HtmlNode item, List<ReplacementRuleDto> replaceRules)
        {
            var finalText = item.InnerHtml;
            foreach (var handle in replaceRules)
            {
                finalText = Regex.Replace(finalText, handle.ReplacementOldStr, handle.ReplacementNewlyStr ?? "", handle.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
            }
            return finalText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public static string Html2Text(string htmlStr)
        {
            htmlStr = Regex.Replace(htmlStr, "<[bB][rR]\\s?/?>", Environment.NewLine);
            var temp = new HtmlDocument();
            temp.LoadHtml(htmlStr);
            return WebUtility.HtmlDecode(temp.DocumentNode.InnerText);
        }

        public static async Task MergeTextFileAsync(string rootDir, CancellationToken cancellationToken = default)
        {
            var rootDirInfo = new DirectoryInfo(rootDir);
            if (!rootDirInfo.Exists)
                return;

            var dirs = rootDirInfo.GetDirectories();
            var dirsCount = dirs.Count();

            if (dirsCount > 0)
            {
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
                            await File.AppendAllTextAsync(filePath, txt.TrimEnd(), cancellationToken);
                            File.Delete(file);
                        }
                    }
                }
            }
            else 
            {
                var allFiles = rootDirInfo.GetFiles().OrderBy(x => x.CreationTime).ToList();
                var files = allFiles.Where(x => x.Extension.ToLower() == ".txt").OrderBy(x => x.CreationTime).Select(x => x.FullName).ToList();
                if (files.Count != 0)
                {
                    var fileName = rootDirInfo.Name + ".txt";
                    var filePath = Path.Combine(rootDir, fileName);
                    foreach (var file in files)
                    {
                        var txt = await File.ReadAllTextAsync(file, cancellationToken);
                        await File.AppendAllTextAsync(filePath, txt.TrimEnd(), cancellationToken);
                        File.Delete(file);
                    }
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
        public static string FullWidth2HalfWidth(this string input)
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

    public static class Log
    {
        public static ILogger Logger { get; set; } = null!;
    }
}
