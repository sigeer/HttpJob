using HtmlAgilityPack;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderTool
{
    public class DefaultSpiderProcessor : ISpiderProcessor
    {
        protected bool IsCanceled(SpiderWorker rootSpider, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                rootSpider.UpdateTaskStatus(TaskType.Canceled);
                return true;
            }
            return false;
        }

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
                if (IsCanceled(rootSpider, cancellationToken))
                    return;

                await SpiderUtility.SaveTextAsync(savePath, SpiderUtility.ReadHtmlNodeInnerHtml(item, rule));
            }
        }

        protected virtual async Task ProcessJumpLink(SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {
            try
            {
                await Parallel.ForEachAsync(nodes, cancellationToken, async (item, ctx) =>
                {
                    if (IsCanceled(rootSpider, ctx))
                        ctx.ThrowIfCancellationRequested();

                    var resource = (item.Attributes["href"] ?? item.Attributes["data-href"])?.Value;
                    if (resource == null)
                        ctx.ThrowIfCancellationRequested();

                    else if (resource.StartsWith("javascript:"))
                        ctx.ThrowIfCancellationRequested();

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
            catch (OperationCanceledException ex)
            {
                return;
            }
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
                if (IsCanceled(rootSpider, cancellationToken))
                    return;

                await SpiderUtility.SaveTextAsync(savePath, item.InnerHtml);
            }
        }

        public virtual async Task ProcessContentAsync(SpiderWorker rootSpider, string documentContent, List<TemplateDetailViewModel> templateRules, CancellationToken cancellationToken = default)
        {
            if (IsCanceled(rootSpider, cancellationToken))
                return;

            var currentDoc = new HtmlDocument();
            currentDoc.LoadHtml(documentContent);

            var savePath = rootSpider.CurrentDir;
            for (int i = 0; i < templateRules.Count; i++)
            {
                if (IsCanceled(rootSpider, cancellationToken))
                    return;

                var rule = templateRules[i];
                var nodes = string.IsNullOrEmpty(rule.TemplateStr)
                    ? new HtmlNodeCollection(currentDoc.DocumentNode)
                    : currentDoc.DocumentNode.SelectNodes(rule.TemplateStr ?? "");
                if (nodes == null)
                    continue;

                savePath = Path.Combine(rootSpider.CurrentDir, $"Rule{rule.Id.ToString()}");

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
