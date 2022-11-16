﻿using AutoMapper;
using Grpc.Net.Client;
using SpiderService;
using SpiderTool.Constants;
using SpiderTool.Data;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderRemoteServiceClient.Services
{
    public class SpiderRemoteService : ISpiderRemoteService
    {
        readonly SpiderWorkerProtoService.SpiderWorkerProtoServiceClient _client;
        readonly IMapper Mapper;
        readonly WorkerController _controller;
        readonly GrpcChannel _channel;
        public WorkerController Controller => _controller;

        public bool CanConnect() => _channel.State == Grpc.Core.ConnectivityState.Connecting;
        public Task<bool> CanConnectAsync() => Task.FromResult(_channel.State == Grpc.Core.ConnectivityState.Connecting);

        public SpiderRemoteService(GrpcChannel channel, IMapper mapper, WorkerController controller)
        {
            _client = new SpiderWorkerProtoService.SpiderWorkerProtoServiceClient(channel);
            Mapper = mapper;
            _controller = controller;
            _channel = channel;
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

        private bool PingSync()
        {
            var data = _client.Ping(new Google.Protobuf.WellKnownTypes.Empty());
            return data.Data == "ok";
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
            var postModel = new IntModel { Data = taskId };
            _client.RemoveTask(postModel);
        }

        public async Task RemoveTaskAsync(int taskId)
        {
            var postModel = new IntModel { Data = taskId };
            await _client.RemoveTaskAsync(postModel);
        }

        public void StopTask(int taskId)
        {
            var postModel = new IntModel { Data = taskId };
            _client.StopTask(postModel);
        }

        public void StopAllTask()
        {
            _client.StopAllTask(new Google.Protobuf.WellKnownTypes.Empty());
        }
    }
}
