using Grpc.Net.Client;
using SpiderRemoteServiceClient.Services;
using SpiderService;
using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderWin.Services
{
    public class SpiderRemoteService : ISpiderRemoteService
    {
        SpiderWorkerProtoService.SpiderWorkerProtoServiceClient _client;

        public SpiderRemoteService(SpiderWorkerProtoService.SpiderWorkerProtoServiceClient client)
        {
            _client = client;
        }

        public int AddTask(TaskSetter model)
        {
            throw new NotImplementedException();
        }

        public string DeleteResource(ResourceHistorySetter model)
        {
            throw new NotImplementedException();
        }

        public string DeleteSpider(SpiderDtoSetter model)
        {
            throw new NotImplementedException();
        }

        public string DeleteTemplate(TemplateDto model)
        {
            throw new NotImplementedException();
        }

        public List<ResourceHistoryDto> GetResourceHistoryDtoList()
        {
            throw new NotImplementedException();
        }

        public SpiderDto? GetSpider(int id)
        {
            throw new NotImplementedException();
        }

        public List<SpiderDtoSetter> GetSpiderDtoList()
        {
            throw new NotImplementedException();
        }

        public List<TaskDto> GetTaskList()
        {
            throw new NotImplementedException();
        }

        public List<TemplateDto> GetTemplateDtoList()
        {
            throw new NotImplementedException();
        }

        public Task<List<TemplateDto>> GetTemplateDtoListAsync()
        {
            throw new NotImplementedException();
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            throw new NotImplementedException();
        }

        public string SubmitResouceHistory(ResourceHistorySetter model)
        {
            throw new NotImplementedException();
        }

        public string SubmitSpider(SpiderDtoSetter model)
        {
            throw new NotImplementedException();
        }

        public string SubmitTemplate(TemplateDto model)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(TaskSetter model)
        {
            throw new NotImplementedException();
        }
    }
}
