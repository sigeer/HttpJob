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

        public override async Task<ResultStringModel> DeleteSpider(SpiderEditDto request, ServerCallContext context)
        {
            var result = await _service.DeleteSpiderAsync(new SpiderDtoSetter
            {
                Id = request.Id
            });
            return new ResultStringModel
            {
                Data = result
            };
        }

        public override async Task<SpiderProtoDto> GetSpider(IntModel request, ServerCallContext context)
        {
            var model = await _service.GetSpiderAsync(request.Data);
            if (model == null)
                return new SpiderProtoDto();

            var data = new SpiderProtoDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Headers = model.Headers,
                Method = model.Method,
                NextPageId = model.NextPageTemplateId ?? 0,
                PostObjStr = model.PostObjStr,
                NextPage = model.NextPageTemplate == null ? null : new TemplateProtoDto
                {
                    Id = model.NextPageTemplate.Id,
                    Name = model.NextPageTemplate.Name,
                    LinkedSpiderId = model.NextPageTemplate.LinkedSpiderId ?? 0,
                    XPath = model.NextPageTemplate.TemplateStr,
                    Type = model.NextPageTemplate.Type
                }
            };
            model.TemplateList.ForEach(x =>
            {
                data.TemplateList.Add(new TemplateProtoDto
                {
                    Id = x.Id,
                    LinkedSpiderId = x.LinkedSpiderId ?? 0,
                    Name = x.Name,
                    Type = x.Type,
                    XPath = x.TemplateStr
                });
                data.Templates.Add(x.Id);
            });
            return data;
        }

        public override async Task<SpiderListResult> GetSpiderList(Empty request, ServerCallContext context)
        {
            var result = await _service.GetSpiderDtoListAsync();
            var data = new SpiderListResult();
            result.ForEach(x =>
            {
                data.List.Add(new SpiderEditDto
                {
                    Id = x.Id,
                    Name = x.Name
                });
            });
            return data;
        }

        public override async Task<TemplateListResult> GetTemplateConfigList(Empty request, ServerCallContext context)
        {
            var result = await _service.GetTemplateDtoListAsync();
            var data = new TemplateListResult();
            result.ForEach(x =>
            {
                data.List.Add(new TemplateProtoDto
                {
                    Id = x.Id,
                    LinkedSpiderId = x.LinkedSpiderId ?? 0,
                    Name = x.Name,
                    Type = x.Type,
                    XPath = x.TemplateStr
                });
            });
            return data;
        }

        public override async Task<ResultStringModel> SubmitTemplateConfig(TemplateProtoDto request, ServerCallContext context)
        {
            var editModel = new TemplateDto
            {
                Id = request.Id,
                TemplateStr = request.XPath,
                LinkedSpiderId = request.LinkedSpiderId,
                Name = request.Name,
                Type = request.Type,
                ReplacementRules = request.Rules.Select(x => new ReplacementRuleDto { ReplacementNewlyStr = x.NewStr, ReplacementOldStr = x.OldStr }).ToList()
            };
            var submitResult = await _service.SubmitTemplateAsync(editModel);
            return new ResultStringModel { Data = submitResult };
        }

        public override async Task<ResultStringModel> DeleteTemplateConfig(TemplateProtoDto request, ServerCallContext context)
        {
            var result = await _service.DeleteTemplateAsync(new TemplateDto
            {
                Id = request.Id
            });
            return new ResultStringModel
            {
                Data = result
            };
        }
    }
}
