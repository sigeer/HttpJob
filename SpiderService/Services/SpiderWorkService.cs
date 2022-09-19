using Grpc.Core;
using SpiderTool;
using SpiderTool.IService;

namespace SpiderService.Services
{
    public class SpiderWorkService : SpiderWorkerProtoService.SpiderWorkerProtoServiceBase
    {
        readonly ISpiderService _service;

        public SpiderWorkService(ISpiderService service)
        {
            _service = service;
        }

        public override async Task<ResultModel> Crawl(RequestModel request, ServerCallContext context)
        {
            var worker = new SpiderWorker(_service);
            await worker.Start(request.Url, request.SpiderId);
            return new ResultModel { Status = "" };
        }
    }
}
