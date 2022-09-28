using SpiderTool.IService;

namespace SpiderRemoteServiceClient.Services
{
    public interface ISpiderRemoteService : ISpiderService
    {
        Task<bool> Ping();
    }
}
