using AutoMapper;
using SpiderService;
using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;
using SpiderTool.IService;

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

        public async Task<List<TaskDto>> GetTaskListAsync()
        {
            var data = await _client.GetTaskListAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return data.List.Select(x => Mapper.Map<TaskDto>(x)).ToList();
        }

        public async Task<int> AddTaskAsync(TaskSetter model)
        {
            return (await _client.AddTaskAsync(Mapper.Map<TaskProtoDto>(model))).Data;
        }

        public async Task UpdateTaskAsync(TaskSetter model)
        {
            await _client.UpdateTaskAsync(Mapper.Map<TaskProtoDto>(model));
        }

        public async Task<List<SpiderDtoSetter>> GetSpiderDtoListAsync()
        {
            var result = await _client.GetSpiderListAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return Mapper.Map<List<SpiderDtoSetter>>(result.List);
        }

        public async Task<string> SubmitSpiderAsync(SpiderDtoSetter model)
        {
            return (await _client.SubmitSpiderAsync(Mapper.Map<SpiderEditProtoDto>(model))).Data;
        }

        public async Task<string> DeleteSpiderAsync(SpiderDtoSetter model)
        {
            return (await _client.DeleteSpiderAsync(Mapper.Map<SpiderEditProtoDto>(model))).Data;
        }

        public async Task<List<TemplateDto>> GetTemplateDtoListAsync()
        {
            var data = await _client.GetTemplateConfigListAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return Mapper.Map<List<TemplateDto>>(data.List);
        }

        public async Task<string> SubmitTemplateAsync(TemplateDto model)
        {
            return (await _client.SubmitTemplateConfigAsync(Mapper.Map<TemplateProtoDto>(model))).Data;
        }

        public async Task<string> DeleteTemplateAsync(TemplateDto model)
        {
            return (await _client.DeleteTemplateConfigAsync(Mapper.Map<TemplateProtoDto>(model))).Data;
        }


        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            await _client.SetTaskStatusAsync(new TaskProtoDto
            {
                Id = taskId,
                Status = taskStatus
            });
        }

        public async Task<SpiderDto?> GetSpiderAsync(int id)
        {
            var data = await _client.GetSpiderAsync(new IntModel { Data = id });
            return Mapper.Map<SpiderDto>(data);
        }

        public async Task<bool> Ping()
        {
            var data = await _client.PingAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return data.Data == "ok";
        }

        public List<ResourceHistoryDto> GetResourceHistoryDtoList()
        {
            throw new NotImplementedException();
        }

        public string SubmitResouceHistory(ResourceHistorySetter model)
        {
            throw new NotImplementedException();
        }

        public string DeleteResource(ResourceHistorySetter model)
        {
            throw new NotImplementedException();
        }

        public List<SpiderDtoSetter> GetSpiderDtoList()
        {
            var result = _client.GetSpiderList(new Google.Protobuf.WellKnownTypes.Empty());
            return Mapper.Map<List<SpiderDtoSetter>>(result.List);
        }

        public string SubmitSpider(SpiderDtoSetter model)
        {
            return _client.SubmitSpider(Mapper.Map<SpiderEditProtoDto>(model)).Data;
        }

        public string DeleteSpider(SpiderDtoSetter model)
        {
            return _client.DeleteSpider(Mapper.Map<SpiderEditProtoDto>(model)).Data;
        }

        public List<TemplateDto> GetTemplateDtoList()
        {
            var data = _client.GetTemplateConfigList(new Google.Protobuf.WellKnownTypes.Empty());
            return Mapper.Map<List<TemplateDto>>(data.List);
        }

        public string SubmitTemplate(TemplateDto model)
        {
            return _client.SubmitTemplateConfig(Mapper.Map<TemplateProtoDto>(model)).Data;
        }

        public string DeleteTemplate(TemplateDto model)
        {
            return _client.DeleteTemplateConfig(Mapper.Map<TemplateProtoDto>(model)).Data;
        }

        public List<TaskDto> GetTaskList()
        {
            var data = _client.GetTaskList(new Google.Protobuf.WellKnownTypes.Empty());
            return data.List.Select(x => Mapper.Map<TaskDto>(x)).ToList();
        }

        public int AddTask(TaskSetter model)
        {
            return _client.AddTask(Mapper.Map<TaskProtoDto>(model)).Data;
        }

        public void UpdateTask(TaskSetter model)
        {
            _client.UpdateTask(Mapper.Map<TaskProtoDto>(model));
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            _client.SetTaskStatus(new TaskProtoDto
            {
                Id = taskId,
                Status = taskStatus
            });
        }

        public SpiderDto? GetSpider(int id)
        {
            var data = _client.GetSpider(new IntModel { Data = id });
            return Mapper.Map<SpiderDto>(data);
        }
    }
}
