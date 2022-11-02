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
                rootSpider.UpdateTaskStatus(TaskType.Canceled, $"task {rootSpider.TaskId} canceled | from method ProcessContentAsync ");
                return true;
            }
            return false;
        }

        protected virtual async Task ProcessObject(SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {
            var savePath = Path.Combine(rootSpider.CurrentDir, $"Rule{rule.Id.ToString()}");
            var urlList = nodes.Select(item => (item.Attributes["src"] ?? item.Attributes["data-src"]).Value.GetTotalUrl(rootSpider.HostUrl)).ToList();
            await SpiderUtility.BulkDownload(savePath, urlList, (log) =>
            {
                rootSpider.CallLog(log);
            }, cancellationToken);
        }

        protected virtual async Task ProcessText(SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {
            var savePath = Path.Combine(rootSpider.CurrentDir, $"Rule{rule.Id.ToString()}");
            foreach (var item in nodes)
            {
                if (IsCanceled(rootSpider, cancellationToken))
                    return;

                await SpiderUtility.SaveTextAsync(savePath, SpiderUtility.ReadHtmlNodeInnerHtml(item, rule));
            }
        }

        protected virtual async Task ProcessJumpLink(SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {
            foreach (var item in nodes)
            {
                if (IsCanceled(rootSpider, cancellationToken))
                    return;

                var resource = (item.Attributes["href"] ?? item.Attributes["data-href"])?.Value;
                if (resource == null)
                    continue;

                var url = resource.GetTotalUrl(rootSpider.HostUrl);

                if (rule.LinkedSpiderDetail != null)
                {
                    var spider = new SpiderWorker(rule.LinkedSpiderDetail, url);
                    rootSpider.MountChildTaskEvent(spider);
                    await spider.Start(cancellationToken);
                }
            }
        }

        protected virtual async Task ProcessHtml(SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default)
        {
            var savePath = Path.Combine(rootSpider.CurrentDir, $"Rule{rule.Id.ToString()}");
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
            for (int i = 0; i < templateRules.Count; i++)
            {
                var rule = templateRules[i];

                if (IsCanceled(rootSpider, cancellationToken))
                    return;

                var nodes = string.IsNullOrEmpty(rule.TemplateStr)
                    ? new HtmlNodeCollection(currentDoc.DocumentNode)
                    : currentDoc.DocumentNode.SelectNodes(rule.TemplateStr ?? "");
                if (nodes == null)
                    continue;

                if (rule.Type == (int)TemplateTypeEnum.Object)
                {
                    await ProcessObject(rootSpider, nodes, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.Text)
                {
                    await ProcessText(rootSpider, nodes, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.Html)
                {
                    await ProcessHtml(rootSpider, nodes, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.JumpLink)
                {
                    await ProcessJumpLink(rootSpider, nodes, rule, cancellationToken);
                }
            }
        }
    }
}
