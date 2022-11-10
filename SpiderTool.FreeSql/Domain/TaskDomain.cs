﻿using SpiderTool.Data.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Tasks;
using SpiderTool.IDomain;

namespace SpiderTool.FreeSql.Domain
{
    public class TaskDomain : ITaskDomain
    {
        readonly IFreeSql _dbContext;

        public TaskDomain(IFreeSql dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddTask(TaskEditDto model)
        {
            return (int)_dbContext.Insert<DB_Task>(new DB_Task()
            {
                Description = model.Description,
                RootUrl = model.RootUrl,
                SpiderId = model.SpiderId,
                CreateTime = DateTime.Now
            }).ExecuteIdentity();
        }

        public async Task<int> AddTaskAsync(TaskEditDto model)
        {
            return (int)await _dbContext.Insert<DB_Task>(new DB_Task()
            {
                Description = model.Description,
                RootUrl = model.RootUrl,
                SpiderId = model.SpiderId,
                CreateTime = DateTime.Now
            }).ExecuteIdentityAsync();
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            return _dbContext.Select<DB_Task>().OrderByDescending(x => x.CreateTime).Take(GlobalVariable.TaskListMaxCount).ToList(x => new TaskListItemViewModel()
            {
                Id = x.Id,
                Status = x.Status,
                CompleteTime = x.CompleteTime,
                CreateTime = x.CreateTime,
                CronExpression = x.CronExpression,
                Description = x.Description,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).ToList();
        }

        public void UpdateTask(TaskEditDto model)
        {
            var taskDbModel = _dbContext.Select<DB_Task>().Where(x => x.Id == model.Id).First();
            if (taskDbModel != null)
            {
                taskDbModel.Description = model.Description;
                taskDbModel.Status = model.Status;
                _dbContext.Update<DB_Task>(taskDbModel).ExecuteAffrows();
            }
        }

        public async Task UpdateTaskAsync(TaskEditDto model)
        {
            var taskDbModel = await _dbContext.Select<DB_Task>().Where(x => x.Id == model.Id).FirstAsync();
            if (taskDbModel != null)
            {
                taskDbModel.Description = model.Description;
                taskDbModel.Status = model.Status;
                await _dbContext.Update<DB_Task>(taskDbModel).ExecuteAffrowsAsync();
            }
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            var taskDbModel = _dbContext.Select<DB_Task>().Where(x => x.Id == taskId).First();
            if (taskDbModel != null)
            {
                taskDbModel.Status = taskStatus;
                if (taskStatus == (int)TaskType.Completed)
                    taskDbModel.CompleteTime = DateTime.Now;
                _dbContext.Update<DB_Task>(taskDbModel).Where(x => x.Id == taskId).ExecuteAffrows();
            }
        }

        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            var taskDbModel = await _dbContext.Select<DB_Task>().Where(x => x.Id == taskId).FirstAsync();
            if (taskDbModel != null)
            {
                taskDbModel.Status = taskStatus;
                if (taskStatus == (int)TaskType.Completed)
                    taskDbModel.CompleteTime = DateTime.Now;
                await _dbContext.Update<DB_Task>(taskDbModel).Where(x => x.Id == taskId).ExecuteAffrowsAsync();
            }
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            return await _dbContext.Select<DB_Task>().OrderByDescending(x => x.CreateTime).Take(GlobalVariable.TaskListMaxCount).ToListAsync(x => new TaskListItemViewModel()
            {
                Id = x.Id,
                Status = x.Status,
                CompleteTime = x.CompleteTime,
                CreateTime = x.CreateTime,
                CronExpression = x.CronExpression,
                Description = x.Description,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            });
        }

        public List<TaskSimpleViewModel> GetTaskHistoryList()
        {
            return _dbContext.Select<DB_Task>().OrderByDescending(x => x.CreateTime).ToList(x => new TaskSimpleViewModel()
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).ToList().DistinctBy(x => x.RootUrl).ToList();
        }

        public async Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            return (await _dbContext.Select<DB_Task>().OrderByDescending(x => x.CreateTime).ToListAsync(x => new TaskSimpleViewModel()
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            })).DistinctBy(x => x.RootUrl).ToList();
        }

        public void BulkUpdateTaskStatus(IEnumerable<int> tasks, int taskStatus)
        {
            _dbContext.Update<DB_Task>().Set(x => x.Status == taskStatus).Where(x => tasks.Contains(x.Id)).ExecuteAffrows();
        }

        public async Task BulkUpdateTaskStatusAsync(IEnumerable<int> tasks, int taskStatus)
        {
            await _dbContext.Update<DB_Task>().Set(x => x.Status == taskStatus).Where(x => tasks.Contains(x.Id)).ExecuteAffrowsAsync();
        }

        public void RemoveTask(int taskId)
        {
            _dbContext.Delete<DB_Task>(taskId).ExecuteAffrows();
        }

        public async Task RemoveTaskAsync(int taskId)
        {
            await _dbContext.Delete<DB_Task>(taskId).ExecuteAffrowsAsync();
        }
    }
}
