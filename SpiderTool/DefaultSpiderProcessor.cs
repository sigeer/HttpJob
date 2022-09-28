using SpiderTool.Dto.Spider;
using HtmlAgilityPack;
using SpiderTool.IService;

namespace SpiderTool
{
    public class DefaultSpiderProcessor : ISpiderProcessor
    {
        readonly ISpiderService _service;

        public DefaultSpiderProcessor(ISpiderService service)
        {
            _service = service;
        }

        private bool IsCanceled(SpiderWorker rootSpider, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                rootSpider.CallLog($"task {rootSpider.TaskId} canceled | from method ProcessContentAsync ");
                rootSpider.CallCancelTask("ProcessContentAsync");
                return true;
            }
            return false;
        }

        public async Task ProcessContentAsync(SpiderWorker rootSpider, string documentContent, List<TemplateDto> templateRules, CancellationToken cancellationToken = default)
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

                var savePath = Path.Combine(rootSpider.CurrentDir, rule.Id.ToString());
                if (rule.Type == (int)TemplateTypeEnum.Object)
                {
                    var urlList = nodes.Select(item => (item.Attributes["src"] ?? item.Attributes["data-src"]).Value.GetTotalUrl(rootSpider.HostUrl)).ToList();
                    SpiderUtility.BulkDownload(savePath, urlList, (log) =>
                    {
                        rootSpider.CallLog(log);
                    }, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.Text)
                {
                    foreach (var item in nodes)
                    {
                        if (IsCanceled(rootSpider, cancellationToken))
                            return;

                        await SpiderUtility.SaveTextAsync(savePath, SpiderUtility.ReadHtmlNodeInnerHtml(item, rule));
                    }
                }
                if (rule.Type == (int)TemplateTypeEnum.Html)
                {
                    foreach (var item in nodes)
                    {
                        if (IsCanceled(rootSpider, cancellationToken))
                            return;

                        await SpiderUtility.SaveTextAsync(savePath, item.InnerHtml);
                    }
                }
                if (rule.Type == (int)TemplateTypeEnum.JumpLink)
                {
                    //新增
                    foreach (var item in nodes)
                    {
                        if (IsCanceled(rootSpider, cancellationToken))
                            return;

                        var resource = (item.Attributes["href"] ?? item.Attributes["data-href"])?.Value;
                        if (resource == null)
                            continue;

                        var url = resource.GetTotalUrl(rootSpider.HostUrl);

                        var spider = new SpiderWorker(rule.LinkedSpiderId ?? 0, _service, this);
                        rootSpider.MountChildTaskEvent(spider);
                        await spider.Start(url);
                    }
                }
            }
        }
    }
}
