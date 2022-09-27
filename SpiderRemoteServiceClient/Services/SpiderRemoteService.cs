using AutoMapper;
using Google.Protobuf.Collections;
using SpiderRemoteServiceClient.Services;
using SpiderService;
using SpiderTool.Dto.Resource;
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

        public async Task<List<TaskDto>> GetTaskListAsync()
        {
            var data = await _client.GetTaskListAsync(null);
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

        public async Task<string> SubmitSpider(SpiderDtoSetter model)
        {
            var editModel = new SpiderEditProtoDto
            {
                Description = model.Description,
                Headers = model.Headers,
                Id = model.Id,
                Method = model.Method,
                Name = model.Name,
                NextPageId = model.NextPageTemplateId ?? 0,
                PostObjStr = model.PostObjStr,
            };
            editModel.Templates.AddRange(model.Templates);
            return (await _client.SubmitSpiderAsync(editModel)).Data;
        }

        public async Task<List<SpiderDtoSetter>> GetSpiderDtoListAsync()
        {
            var result = await _client.GetSpiderListAsync(null);
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
            var data = await _client.GetTemplateConfigListAsync(null);
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
    }
}
