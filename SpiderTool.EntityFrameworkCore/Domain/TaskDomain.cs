using ICSharpCode.SharpZipLib.Zip;
using Microsoft.EntityFrameworkCore;
using SpiderTool.Data.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Tasks;
using SpiderTool.EntityFrameworkCore.ContextModel;
using SpiderTool.IDomain;
using System.Threading.Tasks;

namespace SpiderTool.EntityFrameworkCore.Domain
{
    internal class TaskDomain : ITaskDomain
    {
        readonly SpiderDbContext _dbContext;

        public TaskDomain(SpiderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddTask(TaskEditDto model)
        {
            var dbModel = new DB_Task()
            {
                Description = model.Description,
                RootUrl = model.RootUrl,
                SpiderId = model.SpiderId,
                CreateTime = DateTime.Now
            };
            _dbContext.Tasks.Add(dbModel);
            _dbContext.SaveChanges();
            return dbModel.Id;
        }

        public async Task<int> AddTaskAsync(TaskEditDto model)
        {
            var dbModel = new DB_Task()
            {
                Description = model.Description,
                RootUrl = model.RootUrl,
                SpiderId = model.SpiderId,
                CreateTime = DateTime.Now
            };
            _dbContext.Tasks.Add(dbModel);
            await _dbContext.SaveChangesAsync();
            return dbModel.Id;
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            return _dbContext.Tasks.OrderByDescending(x => x.CreateTime).Take(GlobalVariable.TaskListMaxCount).Select(x => new TaskListItemViewModel()
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
            _dbContext.Tasks.Where(x => x.Id == model.Id)
                .ExecuteUpdate(x =>
                    x.SetProperty(x => x.Status, model.Status)
                    .SetProperty(x => x.Description, model.Description));
        }

        public async Task UpdateTaskAsync(TaskEditDto model)
        {
            await _dbContext.Tasks.Where(x => x.Id == model.Id)
                 .ExecuteUpdateAsync(x =>
                     x.SetProperty(x => x.Status, model.Status)
                     .SetProperty(x => x.Description, model.Description));
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            var set = _dbContext.Tasks.Where(x => x.Id == taskId);
            if (taskStatus == (int)TaskType.Completed)
                set.ExecuteUpdate(x => x.SetProperty(x => x.Status, taskStatus).SetProperty(x => x.CompleteTime, DateTime.Now));
            else
                set.ExecuteUpdate(x => x.SetProperty(x => x.Status, taskStatus));
        }

        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            var set = _dbContext.Tasks.Where(x => x.Id == taskId);
            if (taskStatus == (int)TaskType.Completed)
                await set.ExecuteUpdateAsync(x => x.SetProperty(x => x.Status, taskStatus).SetProperty(x => x.CompleteTime, DateTime.Now));
            else
                await set.ExecuteUpdateAsync(x => x.SetProperty(x => x.Status, taskStatus));
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            return await _dbContext.Tasks.OrderByDescending(x => x.CreateTime).Take(GlobalVariable.TaskListMaxCount).Select(x => new TaskListItemViewModel()
            {
                Id = x.Id,
                Status = x.Status,
                CompleteTime = x.CompleteTime,
                CreateTime = x.CreateTime,
                CronExpression = x.CronExpression,
                Description = x.Description,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).ToListAsync();
        }

        public List<TaskSimpleViewModel> GetTaskHistoryList()
        {
            return _dbContext.Tasks.OrderByDescending(x => x.CreateTime).Select(x => new TaskSimpleViewModel()
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).ToList();
        }

        public async Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            return await _dbContext.Tasks.OrderByDescending(x => x.CreateTime).Select(x => new TaskSimpleViewModel()
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).ToListAsync();
        }

        public void BulkUpdateTaskStatus(IEnumerable<int> tasks, int taskStatus)
        {
            _dbContext.Tasks.Where(x => tasks.Contains(x.Id)).ExecuteDelete();
        }

        public async Task BulkUpdateTaskStatusAsync(IEnumerable<int> tasks, int taskStatus)
        {
            await _dbContext.Tasks.Where(x => tasks.Contains(x.Id)).ExecuteDeleteAsync();
        }

        public void RemoveTask(int taskId)
        {
            _dbContext.Tasks.Where(x => x.Id == taskId).ExecuteDelete();
        }

        public async Task RemoveTaskAsync(int taskId)
        {
            await _dbContext.Tasks.Where(x => x.Id == taskId).ExecuteDeleteAsync();
        }
    }
}
