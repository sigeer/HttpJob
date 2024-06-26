﻿using SpiderTool.Data.Dto.Spider;
using SpiderTool.Data.Dto.Tasks;

namespace SpiderTool.Data.IService
{
    public interface ISpiderBaseService
    {
        WorkerController GetController();
        List<TaskSimpleViewModel> GetTaskHistoryList();
        Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync();

        List<SpiderListItemViewModel> GetSpiderDtoList();
        Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync();
        string SubmitSpider(SpiderEditDto model);
        Task<string> SubmitSpiderAsync(SpiderEditDto model);
        string DeleteSpider(SpiderEditDto model);
        Task<string> DeleteSpiderAsync(SpiderEditDto model);

        List<TemplateDetailViewModel> GetTemplateDtoList();
        Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync();
        string SubmitTemplate(TemplateEditDto model);
        Task<string> SubmitTemplateAsync(TemplateEditDto model);
        string DeleteTemplate(TemplateEditDto model);
        Task<string> DeleteTemplateAsync(TemplateEditDto model);

        List<TaskListItemViewModel> GetTaskList();
        Task<List<TaskListItemViewModel>> GetTaskListAsync();
        Task<List<TaskListItemViewModel>> GetTaskPageListAsync(int pageIndex, int pageSize);
        int AddTask(TaskEditDto model);
        Task<int> AddTaskAsync(TaskEditDto model);
        void UpdateTask(TaskEditDto model);
        Task UpdateTaskAsync(TaskEditDto model);
        void SetTaskStatus(int taskId, int taskStatus);
        Task SetTaskStatusAsync(int taskId, int taskStatus);

        void SetLinkedSpider(SpiderDetailViewModel detail);
        SpiderDetailViewModel? GetSpider(int id);
        Task<SpiderDetailViewModel?> GetSpiderAsync(int id);

        void BulkUpdateTaskStatus(IEnumerable<int> tasks, int taskStatus);
        Task BulkUpdateTaskStatusAsync(IEnumerable<int> tasks, int taskStatus);
        void RemoveTask(int taskId);
        Task RemoveTaskAsync(int taskId);

        void StopTask(int taskId);
        void StopAllTask();
    }
    public interface ISpiderService : ISpiderBaseService
    {
        bool CanConnect();
        Task<bool> CanConnectAsync();

    }
}
