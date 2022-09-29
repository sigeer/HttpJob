using AutoMapper;
using SpiderService;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderRemoteServiceClient.Services
{
    public class SpiderRemoteService : ISpiderRemoteService
    {
        readonly SpiderWorkerProtoService.SpiderWorkerProtoServiceClient _client;
        readonly IMapper Mapper;

        public SpiderRemoteService(SpiderWorkerProtoService.SpiderWorkerProtoServiceClient client, IMapper mapper)
        {
            _client = client;
            Mapper = mapper;
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            var data = await _client.GetTaskListAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return data.List.Select(x => Mapper.Map<TaskListItemViewModel>(x)).ToList();
        }

        public async Task<int> AddTaskAsync(TaskEditDto model)
        {
            return (await _client.AddTaskAsync(Mapper.Map<TaskProtoEditDto>(model))).Data;
        }

        public async Task UpdateTaskAsync(TaskEditDto model)
        {
            await _client.UpdateTaskAsync(Mapper.Map<TaskProtoEditDto>(model));
        }

        public async Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync()
        {
            var result = await _client.GetSpiderListAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return Mapper.Map<List<SpiderListItemViewModel>>(result.List);
        }

        public async Task<string> SubmitSpiderAsync(SpiderEditDto model)
        {
            return (await _client.SubmitSpiderAsync(Mapper.Map<SpiderProtoEditDto>(model))).Data;
        }

        public async Task<string> DeleteSpiderAsync(SpiderEditDto model)
        {
            return (await _client.DeleteSpiderAsync(Mapper.Map<SpiderProtoEditDto>(model))).Data;
        }

        public async Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync()
        {
            var data = await _client.GetTemplateConfigListAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return Mapper.Map<List<TemplateDetailViewModel>>(data.List);
        }

        public async Task<string> SubmitTemplateAsync(TemplateEditDto model)
        {
            return (await _client.SubmitTemplateConfigAsync(Mapper.Map<TemplateProtoDto>(model))).Data;
        }

        public async Task<string> DeleteTemplateAsync(TemplateEditDto model)
        {
            return (await _client.DeleteTemplateConfigAsync(Mapper.Map<TemplateProtoDto>(model))).Data;
        }


        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            await _client.SetTaskStatusAsync(new TaskProtoEditDto
            {
                Id = taskId,
                Status = taskStatus
            });
        }

        public async Task<SpiderDetailViewModel?> GetSpiderAsync(int id)
        {
            var data = await _client.GetSpiderAsync(new IntModel { Data = id });
            return Mapper.Map<SpiderDetailViewModel>(data);
        }

        public async Task<bool> Ping()
        {
            var data = await _client.PingAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return data.Data == "ok";
        }

        public List<SpiderListItemViewModel> GetSpiderDtoList()
        {
            var result = _client.GetSpiderList(new Google.Protobuf.WellKnownTypes.Empty());
            return Mapper.Map<List<SpiderListItemViewModel>>(result.List);
        }

        public string SubmitSpider(SpiderEditDto model)
        {
            return _client.SubmitSpider(Mapper.Map<SpiderProtoEditDto>(model)).Data;
        }

        public string DeleteSpider(SpiderEditDto model)
        {
            return _client.DeleteSpider(Mapper.Map<SpiderProtoEditDto>(model)).Data;
        }

        public List<TemplateDetailViewModel> GetTemplateDtoList()
        {
            var data = _client.GetTemplateConfigList(new Google.Protobuf.WellKnownTypes.Empty());
            return Mapper.Map<List<TemplateDetailViewModel>>(data.List);
        }

        public string SubmitTemplate(TemplateEditDto model)
        {
            return _client.SubmitTemplateConfig(Mapper.Map<TemplateProtoDto>(model)).Data;
        }

        public string DeleteTemplate(TemplateEditDto model)
        {
            return _client.DeleteTemplateConfig(Mapper.Map<TemplateProtoDto>(model)).Data;
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            var data = _client.GetTaskList(new Google.Protobuf.WellKnownTypes.Empty());
            return data.List.Select(x => Mapper.Map<TaskListItemViewModel>(x)).ToList();
        }

        public int AddTask(TaskEditDto model)
        {
            return _client.AddTask(Mapper.Map<TaskProtoEditDto>(model)).Data;
        }

        public void UpdateTask(TaskEditDto model)
        {
            _client.UpdateTask(Mapper.Map<TaskProtoEditDto>(model));
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            _client.SetTaskStatus(new TaskProtoEditDto
            {
                Id = taskId,
                Status = taskStatus
            });
        }

        public SpiderDetailViewModel? GetSpider(int id)
        {
            var data = _client.GetSpider(new IntModel { Data = id });
            return Mapper.Map<SpiderDetailViewModel>(data);
        }

        public List<TaskSimpleViewModel> GetTaskHistoryList()
        {
            var data = _client.GetTaskHistoryList(new Google.Protobuf.WellKnownTypes.Empty());
            return Mapper.Map<List<TaskSimpleViewModel>>(data.List);
        }

        public async Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            var data = await _client.GetTaskHistoryListAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return Mapper.Map<List<TaskSimpleViewModel>>(data.List);
        }
    }
}
