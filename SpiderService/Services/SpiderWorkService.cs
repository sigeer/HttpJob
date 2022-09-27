using Google.Protobuf.WellKnownTypes;
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
            var worker = new SpiderWorker(request.SpiderId, _service);
            await worker.Start(request.Url);
            return new ResultModel { Status = "" };
        }

        public override Task<ResultModel> Ping(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new ResultModel
            {
                Status = "ok"
            });
        }
    }
}
