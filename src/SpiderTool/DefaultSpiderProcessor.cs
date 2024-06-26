﻿using HtmlAgilityPack;
using SpiderTool.Data.Dto.Spider;
using System.Data;

namespace SpiderTool
{
    public class DefaultSpiderProcessor : BaseSpiderProcessor
    {
        protected override async Task ProcessObject(string savePath, SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {

            var urlList = nodes.Where(x => x.GetAttributeValue(rule.ReadAttribute, null) != null)
                .Select(x => x.GetAttributeValue(rule.ReadAttribute, "").GetTotalUrl(rootSpider.HostUrl)).ToList();
            urlList.AddRange(nodes.Where(x => x.Attributes["srcset"] != null || x.Attributes["data-srcset"] != null).SelectMany(item =>
            {
                return (item.Attributes["srcset"] ?? item.Attributes["data-srcset"]).Value.Split(',').Select(x => x.Trim().Split(' ')[0].GetTotalUrl(rootSpider.HostUrl));
            }));
            await SpiderUtility.BulkDownload(savePath, urlList, cancellationToken);
        }
        protected override async Task ProcessJumpLink(SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {
            await Parallel.ForEachAsync(nodes, cancellationToken, async (item, ctx) =>
            {
                ctx.ThrowIfCancellationRequested();

                var resource = (item.Attributes[rule.ReadAttribute] ?? item.Attributes["href"] ?? item.Attributes["data-href"])?.Value;
                if (resource == null)
                    return;

                else if (resource.StartsWith("javascript:"))
                    return;

                else
                {
                    var url = resource.GetTotalUrl(rootSpider.HostUrl);

                    if (rule.LinkedSpiderDetail != null)
                    {
                        var spider = new SpiderWorker(rule.LinkedSpiderDetail, url, rootSpider);
                        rootSpider.MountChildTaskEvent(spider);
                        await spider.Start();
                    }
                }
            });
        }
        protected virtual string ReadElementContent(HtmlNode htmlNode, string templateStr)
        {
            var provider = new DefaultStringTokenProvider(htmlNode);
            return provider.Serialize(templateStr);
        }

        protected async Task ProcessTextOrHtml(string savePath, SpiderWorker rootSpider, HtmlDocument pageDocument, TemplateDetailViewModel rule, bool allowHtml = false, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(rule.TemplateStr))
                return;

            if (rule.TemplateStr.Contains(DefaultStringTokenProvider.Name_XPathHtml) || rule.TemplateStr.Contains(DefaultStringTokenProvider.Name_XPathText))
            {
                var content = ReadElementContent(pageDocument.DocumentNode, rule.TemplateStr);
                var value = SpiderUtility.ReplaceContent(content, rule.ReplacementRules);
                await SpiderUtility.SaveTextAsync(savePath, allowHtml ? value : SpiderUtility.Html2Text(value), cancellationToken);
            }
            else
            {
                var nodes = pageDocument.DocumentNode.SelectNodes(rule.TemplateStr);
                foreach (var item in nodes)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var value = SpiderUtility.ReplaceContent(item.InnerHtml, rule.ReplacementRules);
                    await SpiderUtility.SaveTextAsync(savePath, allowHtml ? value : SpiderUtility.Html2Text(value), cancellationToken);
                }
            }
        }

        protected override async Task ProcessText(string savePath, SpiderWorker rootSpider, HtmlDocument pageDocument, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {
            await ProcessTextOrHtml(savePath, rootSpider, pageDocument, rule, false, cancellationToken);
        }

        protected override async Task ProcessHtml(string savePath, SpiderWorker rootSpider, HtmlDocument pageDocument, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {
            await ProcessTextOrHtml(savePath, rootSpider, pageDocument, rule, true, cancellationToken);
        }
    }
}
