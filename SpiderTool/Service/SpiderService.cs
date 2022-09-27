using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;
using SpiderTool.IDomain;
using SpiderTool.IService;

namespace SpiderTool.Service
{
    public class SpiderService : ISpiderService
    {
        readonly IResourceDomain _resourceDomain;
        readonly ISpiderDomain _spiderDomain;
        readonly ITemplateDomain _templateDomain;
        readonly ITaskDomain _taskDomain;

        public SpiderService(IResourceDomain resourceDomain, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain)
        {
            _resourceDomain = resourceDomain;
            _spiderDomain = spiderDomain;
            _templateDomain = templateDomain;
            _taskDomain = taskDomain;
        }

        public int AddTask(TaskSetter model)
        {
            return _taskDomain.AddTask(model);
        }

        public string DeleteResource(ResourceHistorySetter model)
        {
            return _resourceDomain.Delete(model);
        }

        public string DeleteSpider(SpiderDtoSetter model)
        {
            return _spiderDomain.Delete(model);
        }

        public string DeleteTemplate(TemplateDto model)
        {
            return _templateDomain.Delete(model);
        }

        public List<ResourceHistoryDto> GetResourceHistoryDtoList()
        {
            return _resourceDomain.GetResourceDtoList();
        }

        public SpiderDto? GetSpider(int id)
        {
            return _spiderDomain.GetSpiderDto(id);
        }

        public List<SpiderDtoSetter> GetSpiderDtoList()
        {
            return _spiderDomain.GetSpiderDtoList();
        }

        public List<TaskDto> GetTaskList()
        {
            return _taskDomain.GetTaskList();
        }

        public List<TemplateDto> GetTemplateDtoList()
        {
            return _templateDomain.GetTemplateDtoList();
        }

        public async Task<List<TemplateDto>> GetTemplateDtoListAsync()
        {
            return await _templateDomain.GetTemplateDtoListAsync();
        }

        public void UpdateTask(TaskSetter model)
        {
            _taskDomain.UpdateTask(model);
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            _taskDomain.SetTaskStatus(taskId, taskStatus);
        }

        public string SubmitResouceHistory(ResourceHistorySetter model)
        {
            return _resourceDomain.Submit(model);
        }

        public string SubmitSpider(SpiderDtoSetter model)
        {
            return _spiderDomain.Submit(model);
        }

        public string SubmitTemplate(TemplateDto model)
        {
            return _templateDomain.Submit(model);
        }

        public async Task<List<TaskDto>> GetTaskListAsync()
        {
            return await _taskDomain.GetTaskListAsync();
        }

        public async Task<int> AddTaskAsync(TaskSetter model)
        {
            return await _taskDomain.AddTaskAsync(model);
        }

        public async Task UpdateTaskAsync(TaskSetter model)
        {
            await _taskDomain.UpdateTaskAsync(model);
        }

        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            await _taskDomain.SetTaskStatusAsync(taskId, taskStatus);
        }

        public async Task<List<SpiderDtoSetter>> GetSpiderDtoListAsync()
        {
            return await _spiderDomain.GetSpiderDtoListAsync();
        }

        public async Task<string> SubmitSpiderAsync(SpiderDtoSetter model)
        {
            return await _spiderDomain.SubmitAsync(model);
        }

        public async Task<string> DeleteSpiderAsync(SpiderDtoSetter model)
        {
            return await _spiderDomain.DeleteAsync(model);
        }

        public async Task<string> SubmitTemplateAsync(TemplateDto model)
        {
            return await _templateDomain.SubmitAsync(model);
        }

        public async Task<string> DeleteTemplateAsync(TemplateDto model)
        {
            return await _templateDomain.DeleteAsync(model);
        }

        public Task<SpiderDto?> GetSpiderAsync(int id)
        {
            return _spiderDomain.GetSpiderDtoAsync(id);
        }
    }
}
