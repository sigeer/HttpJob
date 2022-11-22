using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using SpiderService;
using SpiderTool.Data;
using SpiderTool.Data.Constants;
using SpiderTool.Data.Dto.Spider;
using SpiderTool.Data.Dto.Tasks;

namespace SpiderRemoteServiceClient.Services
{
    public class SpiderRemoteService : ISpiderRemoteService
    {
        readonly SpiderWorkerProtoService.SpiderWorkerProtoServiceClient _client;
        readonly IMapper Mapper;
        readonly GrpcChannel _channel;

        public bool CanConnect() => _channel.State == Grpc.Core.ConnectivityState.Connecting;
        public Task<bool> CanConnectAsync() => Task.FromResult(_channel.State == Grpc.Core.ConnectivityState.Connecting);

        public SpiderRemoteService(GrpcChannel channel, IMapper mapper, WorkerController controller)
        {
            _client = new SpiderWorkerProtoService.SpiderWorkerProtoServiceClient(channel);
            Mapper = mapper;
            _channel = channel;
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            var data = await _client.GetTaskListAsync(new Empty());
            return data.List.Select(x => Mapper.Map<TaskListItemViewModel>(x)).ToList();
        }

        public async Task<int> AddTaskAsync(TaskEditDto model)
        {
            return (await _client.AddTaskAsync(Mapper.Map<TaskProtoEditDto>(model))).Value;
        }

        public async Task UpdateTaskAsync(TaskEditDto model)
        {
            await _client.UpdateTaskAsync(Mapper.Map<TaskProtoEditDto>(model));
        }

        public async Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync()
        {
            var result = await _client.GetSpiderListAsync(new Empty());
            return Mapper.Map<List<SpiderListItemViewModel>>(result.List);
        }

        public async Task<string> SubmitSpiderAsync(SpiderEditDto model)
        {
            return (await _client.SubmitSpiderAsync(Mapper.Map<SpiderProtoEditDto>(model))).Value;
        }

        public async Task<string> DeleteSpiderAsync(SpiderEditDto model)
        {
            return (await _client.DeleteSpiderAsync(Mapper.Map<SpiderProtoEditDto>(model))).Value;
        }

        public async Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync()
        {
            var data = await _client.GetTemplateConfigListAsync(new Empty());
            return Mapper.Map<List<TemplateDetailViewModel>>(data.List);
        }

        public async Task<string> SubmitTemplateAsync(TemplateEditDto model)
        {
            return (await _client.SubmitTemplateConfigAsync(Mapper.Map<TemplateProtoDto>(model))).Value;
        }

        public async Task<string> DeleteTemplateAsync(TemplateEditDto model)
        {
            return (await _client.DeleteTemplateConfigAsync(Mapper.Map<TemplateProtoDto>(model))).Value;
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
            var data = await _client.GetSpiderAsync(new Int32Value { Value = id });
            return Mapper.Map<SpiderDetailViewModel>(data);
        }

        private bool PingSync()
        {
            var data = _client.Ping(new Empty());
            return data.Value == "ok";
        }

        public async Task<bool> Ping()
        {
            var data = await _client.PingAsync(new Empty());
            return data.Value == "ok";
        }

        public List<SpiderListItemViewModel> GetSpiderDtoList()
        {
            var result = _client.GetSpiderList(new Empty());
            return Mapper.Map<List<SpiderListItemViewModel>>(result.List);
        }

        public string SubmitSpider(SpiderEditDto model)
        {
            return _client.SubmitSpider(Mapper.Map<SpiderProtoEditDto>(model)).Value;
        }

        public string DeleteSpider(SpiderEditDto model)
        {
            return _client.DeleteSpider(Mapper.Map<SpiderProtoEditDto>(model)).Value;
        }

        public List<TemplateDetailViewModel> GetTemplateDtoList()
        {
            var data = _client.GetTemplateConfigList(new Empty());
            return Mapper.Map<List<TemplateDetailViewModel>>(data.List);
        }

        public string SubmitTemplate(TemplateEditDto model)
        {
            return _client.SubmitTemplateConfig(Mapper.Map<TemplateProtoDto>(model)).Value;
        }

        public string DeleteTemplate(TemplateEditDto model)
        {
            return _client.DeleteTemplateConfig(Mapper.Map<TemplateProtoDto>(model)).Value;
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            var data = _client.GetTaskList(new Empty());
            return data.List.Select(x => Mapper.Map<TaskListItemViewModel>(x)).ToList();
        }

        public int AddTask(TaskEditDto model)
        {
            return _client.AddTask(Mapper.Map<TaskProtoEditDto>(model)).Value;
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
            var data = _client.GetSpider(new Int32Value { Value = id });
            return Mapper.Map<SpiderDetailViewModel>(data);
        }

        public List<TaskSimpleViewModel> GetTaskHistoryList()
        {
            var data = _client.GetTaskHistoryList(new Empty());
            return Mapper.Map<List<TaskSimpleViewModel>>(data.List);
        }

        public async Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            var data = await _client.GetTaskHistoryListAsync(new Empty());
            return Mapper.Map<List<TaskSimpleViewModel>>(data.List);
        }

        public string Crawl(int spiderId, string url)
        {
            _client.Crawl(new RequestModel
            {
                SpiderId = spiderId,
                Url = url
            });
            return StatusMessage.Success;
        }

        public async Task<string> CrawlAsync(int spiderId, string url)
        {
            await _client.CrawlAsync(new RequestModel
            {
                SpiderId = spiderId,
                Url = url
            });
            return StatusMessage.Success;
        }

        public void SetLinkedSpider(SpiderDetailViewModel detail)
        {
            _client.SetLinkedSpider(Mapper.Map<SpiderProtoDetailViewModel>(detail));
        }

        public void BulkUpdateTaskStatus(IEnumerable<int> tasks, int taskStatus)
        {
            var postModel = new TaskProtoBulkEditDto
            {
                TaskStatus = taskStatus
            };
            postModel.Tasks.AddRange(tasks);
            _client.BulkUpdateTaskStatus(postModel);
        }

        public async Task BulkUpdateTaskStatusAsync(IEnumerable<int> tasks, int taskStatus)
        {
            var postModel = new TaskProtoBulkEditDto
            {
                TaskStatus = taskStatus
            };
            postModel.Tasks.AddRange(tasks);
            await _client.BulkUpdateTaskStatusAsync(postModel);
        }

        public void RemoveTask(int taskId)
        {
            var postModel = new Int32Value { Value = taskId };
            _client.RemoveTask(postModel);
        }

        public async Task RemoveTaskAsync(int taskId)
        {
            var postModel = new Int32Value { Value = taskId };
            await _client.RemoveTaskAsync(postModel);
        }

        public void StopTask(int taskId)
        {
            var postModel = new Int32Value { Value = taskId };
            _client.StopTask(postModel);
        }

        public void StopAllTask()
        {
            _client.StopAllTask(new Empty());
        }

        public WorkerController GetController()
        {
            throw new NotImplementedException();
        }
    }
}
