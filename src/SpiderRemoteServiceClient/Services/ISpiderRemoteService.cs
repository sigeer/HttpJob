using SpiderTool.Data.IService;

namespace SpiderRemoteServiceClient.Services
{
    public interface ISpiderRemoteService : ISpiderService
    {
        Task<bool> Ping();

        string Crawl(int spiderId, string url);
        Task<string> CrawlAsync(int spiderId, string url);
    }
}
