﻿using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;
using SpiderTool.IDomain;
using SpiderTool.IService;
using System.Threading.Tasks;

namespace SpiderTool.Service
{
    public class SpiderBaseService : ISpiderBaseService
    {
        readonly ISpiderDomain _spiderDomain;
        readonly ITemplateDomain _templateDomain;
        readonly ITaskDomain _taskDomain;

        public SpiderBaseService(ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain)
        {
            _spiderDomain = spiderDomain;
            _templateDomain = templateDomain;
            _taskDomain = taskDomain;
        }

        public int AddTask(TaskEditDto model)
        {
            return _taskDomain.AddTask(model);
        }

        public string DeleteSpider(SpiderEditDto model)
        {
            return _spiderDomain.Delete(model);
        }

        public string DeleteTemplate(TemplateEditDto model)
        {
            return _templateDomain.Delete(model);
        }

        public SpiderDetailViewModel? GetSpider(int id)
        {
            return _spiderDomain.GetSpiderDto(id);
        }

        public List<SpiderListItemViewModel> GetSpiderDtoList()
        {
            return _spiderDomain.GetSpiderDtoList();
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            return _taskDomain.GetTaskList();
        }

        public List<TemplateDetailViewModel> GetTemplateDtoList()
        {
            return _templateDomain.GetTemplateDtoList();
        }

        public async Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync()
        {
            return await _templateDomain.GetTemplateDtoListAsync();
        }

        public void UpdateTask(TaskEditDto model)
        {
            _taskDomain.UpdateTask(model);
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            _taskDomain.SetTaskStatus(taskId, taskStatus);
        }

        public string SubmitSpider(SpiderEditDto model)
        {
            return _spiderDomain.Submit(model);
        }

        public string SubmitTemplate(TemplateEditDto model)
        {
            return _templateDomain.Submit(model);
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            return await _taskDomain.GetTaskListAsync();
        }

        public async Task<int> AddTaskAsync(TaskEditDto model)
        {
            return await _taskDomain.AddTaskAsync(model);
        }

        public async Task UpdateTaskAsync(TaskEditDto model)
        {
            await _taskDomain.UpdateTaskAsync(model);
        }

        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            await _taskDomain.SetTaskStatusAsync(taskId, taskStatus);
        }

        public async Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync()
        {
            return await _spiderDomain.GetSpiderDtoListAsync();
        }

        public async Task<string> SubmitSpiderAsync(SpiderEditDto model)
        {
            return await _spiderDomain.SubmitAsync(model);
        }

        public async Task<string> DeleteSpiderAsync(SpiderEditDto model)
        {
            return await _spiderDomain.DeleteAsync(model);
        }

        public async Task<string> SubmitTemplateAsync(TemplateEditDto model)
        {
            return await _templateDomain.SubmitAsync(model);
        }

        public async Task<string> DeleteTemplateAsync(TemplateEditDto model)
        {
            return await _templateDomain.DeleteAsync(model);
        }

        public Task<SpiderDetailViewModel?> GetSpiderAsync(int id)
        {
            return _spiderDomain.GetSpiderDtoAsync(id);
        }

        public List<TaskSimpleViewModel> GetTaskHistoryList()
        {
            return _taskDomain.GetTaskHistoryList();
        }

        public async Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            return await _taskDomain.GetTaskHistoryListAsync();
        }

        public void SetLinkedSpider(SpiderDetailViewModel detail)
        {
            SetLinkedSpiderCore(detail);
        }

        private void SetLinkedSpiderCore(SpiderDetailViewModel detail)
        {
            if (detail.TemplateList != null)
            {
                foreach (var template in detail.TemplateList)
                {
                    if (template.LinkedSpiderId != null)
                        template.LinkedSpiderDetail = _spiderDomain.GetSpiderDto(template.LinkedSpiderId.Value);

                    if (template.LinkedSpiderDetail != null)
                        SetLinkedSpiderCore(template.LinkedSpiderDetail);
                }
            }
        }

        public void BulkUpdateTaskStatus(IEnumerable<int> tasks, int taskStatus)
        {
            _taskDomain.BulkUpdateTaskStatus(tasks, taskStatus);
        }

        public async Task BulkUpdateTaskStatusAsync(IEnumerable<int> tasks, int taskStatus)
        {
            await _taskDomain.BulkUpdateTaskStatusAsync(tasks, taskStatus);
        }

        public void RemoveTask(int taskId)
        {
            _taskDomain.RemoveTask(taskId);
        }

        public async Task RemoveTaskAsync(int taskId)
        {
            await _taskDomain.RemoveTaskAsync(taskId);
        }

        public void StopTask(int taskId)
        {
            WorkerController.GetInstance().Cancel(taskId);
        }

        public void StopAllTask()
        {
            WorkerController.GetInstance().CancelAll();
        }
    }
}
