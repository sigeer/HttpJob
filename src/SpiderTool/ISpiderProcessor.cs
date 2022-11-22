using SpiderTool.Data.Dto.Spider;

namespace SpiderTool
{
    public interface ISpiderProcessor
    {
        Task ProcessContentAsync(SpiderWorker rootSpider, string documentContent, List<TemplateDetailViewModel> templateRules, CancellationToken cancellationToken = default);
    }
}
