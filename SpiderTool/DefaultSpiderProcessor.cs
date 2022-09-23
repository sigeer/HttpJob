using SpiderTool.Dto.Spider;
using HtmlAgilityPack;
using SpiderTool.IService;

namespace SpiderTool
{
    public class DefaultSpiderProcessor: ISpiderProcessor
    {
        readonly ISpiderService _service;

        public DefaultSpiderProcessor(ISpiderService service)
        {
            _service = service;
        }

        public async Task ProcessContentAsync(SpiderWorker rootSpider, string documentContent, List<TemplateDto> templateRules)
        {
            var currentDoc = new HtmlDocument();
            currentDoc.Load(documentContent);
            foreach (var rule in templateRules)
            {
                var nodes = string.IsNullOrEmpty(rule.TemplateStr)
                    ? new HtmlNodeCollection(currentDoc.DocumentNode)
                    : currentDoc.DocumentNode.SelectNodes(rule.TemplateStr ?? "");
                if (nodes == null)
                    continue;

                if (rule.Type == (int)TemplateTypeEnum.Object)
                {
                    var urlList = nodes.Select(item => (item.Attributes["src"] ?? item.Attributes["data-src"]).Value.GetTotalUrl(rootSpider.HostUrl)).ToList();
                    SpiderUtility.BulkDownload(rootSpider.CurrentDir, urlList);
                }
                if (rule.Type == (int)TemplateTypeEnum.Text)
                {
                    foreach (var item in nodes)
                    {
                        await SpiderUtility.SaveTextAsync(rootSpider.CurrentDir, SpiderUtility.ReadHtmlNodeInnerHtml(item, rule));
                    }
                }
                if (rule.Type == (int)TemplateTypeEnum.Html)
                {
                    foreach (var item in nodes)
                    {
                        await SpiderUtility.SaveTextAsync(rootSpider.CurrentDir, item.InnerHtml);
                    }
                }
                if (rule.Type == (int)TemplateTypeEnum.JumpLink)
                {
                    //新增
                    foreach (var item in nodes)
                    {
                        var resource = (item.Attributes["href"] ?? item.Attributes["data-href"])?.Value;
                        if (resource == null)
                            continue;

                        var url = resource.GetTotalUrl(rootSpider.HostUrl);

                        var newSpider = new SpiderWorker(rule.LinkedSpiderId ?? 0, _service, this);
                        rootSpider.CallNewWorker(new SpiderWorkUnit
                        {
                            SpiderWorker = newSpider,
                            Url = url
                        });
                    }
                }
            }
        }
    }
}
