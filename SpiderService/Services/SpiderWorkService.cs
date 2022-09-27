using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SpiderTool;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;
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

        public override Task<ResultIntModel> AddTask(TaskEditDto request, ServerCallContext context)
        {
            var result = _service.AddTask(new TaskSetter()
            {
                RootUrl = request.RootUrl,
                SpiderId = request.SpiderId,
                Description = request.Description,
                Status = request.Status
            });
            return Task.FromResult(new ResultIntModel
            {
                 Data = result
            });
        }

        public override Task<Empty> UpdateTask(TaskEditDto request, ServerCallContext context)
        {
            _service.UpdateTask(new TaskSetter()
            {
                Id = request.Id,
                RootUrl = request.RootUrl,
                SpiderId = request.SpiderId,
                Description = request.Description,
                Status = request.Status
            });
            return Task.FromResult(new Empty());
        }

        public override Task<TaskListResult> GetTaskList(Empty request, ServerCallContext context)
        {
            var list = _service.GetTaskList();
            var resultModel = new TaskListResult();
            list.ForEach(x =>
            {
                resultModel.List.Add(new TaskEditDto
                {
                    Description = x.Description,
                    Id = x.Id,
                    RootUrl = x.RootUrl,
                    SpiderId = x.SpiderId,
                    Status = x.Status
                });
            });
            return Task.FromResult(resultModel);
        }

        public override Task<ResultModel> SubmitSpider(SpiderEditDto request, ServerCallContext context)
        {
            var submitResult = _service.SubmitSpider(new SpiderDtoSetter
            {
                Id = request.Id,
                Description = request.Description,
                Headers = request.Headers,
                Method = request.Method,
                Name = request.Name,
                NextPageTemplateId = request.NextPageId,
                PostObjStr = request.PostObjStr,
                Templates = request.Templates.ToList()
            });
            return Task.FromResult(new ResultModel
            {
                Status = submitResult
            });
        }

        public override Task<ResultStringModel> DeleteSpider(SpiderEditDto request, ServerCallContext context)
        {
            return base.DeleteSpider(request, context);
        }

        public override Task<SpiderProtoDto> GetSpider(IntModel request, ServerCallContext context)
        {
            return base.GetSpider(request, context);
        }

        public override Task<SpiderListResult> GetSpiderList(Empty request, ServerCallContext context)
        {
            return base.GetSpiderList(request, context);
        }

        public override Task<TemplateListResult> GetTemplateConfigList(Empty request, ServerCallContext context)
        {
            return base.GetTemplateConfigList(request, context);
        }

        public override Task<ResultStringModel> SubmitTemplateConfig(TemplateProtoDto request, ServerCallContext context)
        {
            return base.SubmitTemplateConfig(request, context);
        }

        public override Task<ResultStringModel> DeleteTemplateConfig(TemplateProtoDto request, ServerCallContext context)
        {
            return base.DeleteTemplateConfig(request, context);
        }
    }
}
