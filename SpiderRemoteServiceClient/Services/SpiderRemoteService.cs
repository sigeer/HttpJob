using Google.Protobuf.Collections;
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

        public List<TaskDto> GetTaskList()
        {
            var data = _client.GetTaskList(null);
            return data.List.Select(x => new TaskDto()
            {
                Id = x.Id
            }).ToList();
        }

        public int AddTask(TaskSetter model)
        {
            return _client.AddTask(new TaskEditDto()
            {
                RootUrl = model.RootUrl,
                Status = model.Status,
                Description = model.Description,
                SpiderId = model.SpiderId
            }).Data;
        }

        public void UpdateTask(TaskSetter model)
        {
            _client.AddTask(new TaskEditDto()
            {
                Id = model.Id,
                RootUrl = model.RootUrl,
                Status = model.Status,
                Description = model.Description,
                SpiderId = model.SpiderId
            });
        }

        public string SubmitSpider(SpiderDtoSetter model)
        {
            var editModel = new SpiderEditDto
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
            return _client.SubmitSpider(editModel).Status;
        }

        public string DeleteSpider(SpiderDtoSetter model)
        {
            throw new NotImplementedException();
        }

        public SpiderDto? GetSpider(int id)
        {
            throw new NotImplementedException();
        }

        public string DeleteTemplate(TemplateDto model)
        {
            throw new NotImplementedException();
        }



        public List<SpiderDtoSetter> GetSpiderDtoList()
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


        public string SubmitTemplate(TemplateDto model)
        {
            throw new NotImplementedException();
        }
    }
}
