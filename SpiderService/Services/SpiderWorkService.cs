using AutoMapper;
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
        readonly IMapper _mapper;
        readonly ILogger<SpiderWorker> _spiderLogger;

        public SpiderWorkService(ISpiderService service, IMapper mapper, ILogger<SpiderWorker> spiderLogger)
        {
            _service = service;
            _mapper = mapper;
            _spiderLogger = spiderLogger;
        }

        public override async Task<StringModel> Crawl(RequestModel request, ServerCallContext context)
        {
            var worker = new SpiderWorker(_spiderLogger, request.SpiderId, request.Url, _service);
            await worker.Start();
            return new StringModel { Data = "" };
        }

        public override Task<StringModel> Ping(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new StringModel
            {
                Data = "ok"
            });
        }

        public override async Task<IntModel> AddTask(TaskProtoEditDto request, ServerCallContext context)
        {
            return new IntModel
            {
                Data = await _service.AddTaskAsync(_mapper.Map<TaskEditDto>(request))
            };
        }

        public override async Task<Empty> UpdateTask(TaskProtoEditDto request, ServerCallContext context)
        {
            await _service.UpdateTaskAsync(_mapper.Map<TaskEditDto>(request));
            return new Empty();
        }

        public override Task<TaskListResult> GetTaskList(Empty request, ServerCallContext context)
        {
            var list = _service.GetTaskList();
            var resultModel = new TaskListResult();
            list.ForEach(x =>
            {
                resultModel.List.Add(_mapper.Map<TaskProtoViewModel>(request));
            });
            return Task.FromResult(resultModel);
        }

        public override Task<StringModel> SubmitSpider(SpiderProtoEditDto request, ServerCallContext context)
        {
            var submitResult = _service.SubmitSpider(_mapper.Map<SpiderEditDto>(request));
            return Task.FromResult(new StringModel
            {
                Data = submitResult
            });
        }

        public override async Task<StringModel> DeleteSpider(SpiderProtoEditDto request, ServerCallContext context)
        {
            var result = await _service.DeleteSpiderAsync(_mapper.Map<SpiderEditDto>(request));
            return new StringModel
            {
                Data = result
            };
        }

        public override async Task<SpiderProtoDetailViewModel> GetSpider(IntModel request, ServerCallContext context)
        {
            var model = await _service.GetSpiderAsync(request.Data);
            if (model == null)
                return new SpiderProtoDetailViewModel();

            return _mapper.Map<SpiderProtoDetailViewModel>(model);
        }

        public override async Task<SpiderListResult> GetSpiderList(Empty request, ServerCallContext context)
        {
            var result = await _service.GetSpiderDtoListAsync();
            var data = new SpiderListResult();
            result.ForEach(x =>
            {
                data.List.Add(_mapper.Map<SpiderProtoListItemViewModel>(x));
            });
            return data;
        }

        public override async Task<TemplateListResult> GetTemplateConfigList(Empty request, ServerCallContext context)
        {
            var result = await _service.GetTemplateDtoListAsync();
            var data = new TemplateListResult();
            result.ForEach(x =>
            {
                data.List.Add(_mapper.Map<TemplateProtoDto>(x));
            });
            return data;
        }

        public override async Task<StringModel> SubmitTemplateConfig(TemplateProtoDto request, ServerCallContext context)
        {
            var editModel = new TemplateEditDto
            {
                Id = request.Id,
                TemplateStr = request.XPath,
                LinkedSpiderId = request.LinkedSpiderId,
                Name = request.Name,
                Type = request.Type,
                ReplacementRules = request.Rules.Select(x => new ReplacementRuleDto { ReplacementNewlyStr = x.NewStr, ReplacementOldStr = x.OldStr }).ToList()
            };
            var submitResult = await _service.SubmitTemplateAsync(editModel);
            return new StringModel { Data = submitResult };
        }

        public override async Task<StringModel> DeleteTemplateConfig(TemplateProtoDto request, ServerCallContext context)
        {
            var result = await _service.DeleteTemplateAsync(new TemplateEditDto
            {
                Id = request.Id
            });
            return new StringModel
            {
                Data = result
            };
        }

        public override async Task<StringModel> SetTaskStatus(TaskProtoEditDto request, ServerCallContext context)
        {
            await _service.SetTaskStatusAsync(request.Id, request.Status);
            return new StringModel
            {
                Data = "ok"
            };
        }

        public override async Task<TaskProtoSimpleListResult> GetTaskHistoryList(Empty request, ServerCallContext context)
        {
            var result = await _service.GetTaskHistoryListAsync();
            var data = new TaskProtoSimpleListResult();
            result.ForEach(x =>
            {
                data.List.Add(_mapper.Map<TaskProtoSimpleViewModel>(x));
            });
            return data;
        }

        public override async Task<Empty> BulkUpdateTaskStatus(TaskProtoBulkEditDto request, ServerCallContext context)
        {
            await _service.BulkUpdateTaskStatusAsync(request.Tasks.AsEnumerable(), request.TaskStatus);
            return new Empty();
        }

        public override async Task<Empty> RemoveTask(IntModel request, ServerCallContext context)
        {
            await _service.RemoveTaskAsync(request.Data);
            return new Empty();
        }

        public override Task<Empty> StopTask(IntModel request, ServerCallContext context)
        {
            _service.StopTask(request.Data);
            return Task.FromResult(new Empty());
        }

        public override async Task<Empty> StopAllTask(Empty request, ServerCallContext context)
        {
            _service.StopAllTask();
            return Task.FromResult(new Empty());
        }
    }
}
