using HtmlAgilityPack;
using SpiderTool.Data.Dto.Spider;
using System.Data;

namespace SpiderTool
{
    public class DefaultSpiderProcessor : ISpiderProcessor
    {
        protected virtual async Task ProcessObject(string savePath, SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {

            var urlList = nodes.Select(item => (item.Attributes["src"] ?? item.Attributes["data-src"]).Value.GetTotalUrl(rootSpider.HostUrl)).ToList();
            await SpiderUtility.BulkDownload(savePath, urlList, (log) =>
            {
                rootSpider.CallLog(log);
            }, cancellationToken);
        }

        protected virtual async Task ProcessText(string savePath, SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {
            foreach (var item in nodes)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await SpiderUtility.SaveTextAsync(savePath, SpiderUtility.ReadHtmlNodeInnerHtml(item, rule));
            }
        }

        protected virtual async Task ProcessJumpLink(SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {
            await Parallel.ForEachAsync(nodes, cancellationToken, async (item, ctx) =>
            {
                ctx.ThrowIfCancellationRequested();

                var resource = (item.Attributes["href"] ?? item.Attributes["data-href"])?.Value;
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
            //foreach (var item in nodes)
            //{
            //    if (IsCanceled(rootSpider, cancellationToken))
            //        return;

            //    var resource = (item.Attributes["href"] ?? item.Attributes["data-href"])?.Value;
            //    if (resource == null)
            //        continue;

            //    if (resource.StartsWith("javascript:"))
            //        continue;

            //    var url = resource.GetTotalUrl(rootSpider.HostUrl);

            //    if (rule.LinkedSpiderDetail != null)
            //    {
            //        var spider = new SpiderWorker(rule.LinkedSpiderDetail, url, rootSpider);
            //        rootSpider.MountChildTaskEvent(spider);
            //        await spider.Start();
            //    }
            //}
        }

        protected virtual async Task ProcessHtml(string savePath, SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {
            foreach (var item in nodes)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await SpiderUtility.SaveTextAsync(savePath, item.InnerHtml);
            }
        }

        public virtual async Task ProcessContentAsync(SpiderWorker rootSpider, string documentContent, List<TemplateDetailViewModel> templateRules, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (templateRules.Count == 0)
            {
                await SpiderUtility.SaveTextAsync(rootSpider.CurrentDir, documentContent);
            }

            var currentDoc = new HtmlDocument();
            currentDoc.LoadHtml(documentContent);

            for (int i = 0; i < templateRules.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var rule = templateRules[i];
                var nodes = string.IsNullOrEmpty(rule.TemplateStr)
                    ? new HtmlNodeCollection(currentDoc.DocumentNode)
                    : currentDoc.DocumentNode.SelectNodes(rule.TemplateStr ?? "");
                if (nodes == null)
                    continue;

                var savePath = Path.Combine(rootSpider.CurrentDir, $"Rule{rule.Id.ToString()}");

                if (rule.Type == (int)TemplateTypeEnum.Object)
                {
                    await ProcessObject(savePath, rootSpider, nodes, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.Text)
                {
                    await ProcessText(savePath, rootSpider, nodes, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.Html)
                {
                    await ProcessHtml(savePath, rootSpider, nodes, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.JumpLink)
                {
                    await ProcessJumpLink(rootSpider, nodes, rule, cancellationToken);
                }
            }
        }
    }
}
