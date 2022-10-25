using Microsoft.EntityFrameworkCore;
using SpiderTool.Data.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Tasks;
using SpiderTool.EntityFrameworkCore.ContextModel;
using SpiderTool.IDomain;

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
            return _dbContext.Tasks.Where(x => x.Status != (int)TaskType.Canceled).OrderByDescending(x => x.CreateTime).Take(GlobalVariable.TaskListMaxCount).Select(x => new TaskListItemViewModel()
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
            var taskDbModel = _dbContext.Tasks.FirstOrDefault(x => x.Id == model.Id);
            if (taskDbModel != null)
            {
                taskDbModel.Status = model.Status;
                taskDbModel.Description = model.Description;
                _dbContext.SaveChanges();
            }
        }

        public async Task UpdateTaskAsync(TaskEditDto model)
        {
            var taskDbModel = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (taskDbModel != null)
            {
                taskDbModel.Status = model.Status;
                taskDbModel.Description = model.Description;
                await _dbContext.SaveChangesAsync();
            }
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            var taskDbModel = _dbContext.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (taskDbModel != null)
            {
                taskDbModel.Status = taskStatus;
                if (taskStatus == (int)TaskType.Completed)
                    taskDbModel.CompleteTime = DateTime.Now;
                _dbContext.SaveChanges();
            }
        }

        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            var taskDbModel = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            if (taskDbModel != null)
            {
                taskDbModel.Status = taskStatus;
                if (taskStatus == (int)TaskType.Completed)
                    taskDbModel.CompleteTime = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            return await _dbContext.Tasks.Where(x => x.Status != (int)TaskType.Canceled).OrderByDescending(x => x.CreateTime).Take(GlobalVariable.TaskListMaxCount).Select(x => new TaskListItemViewModel()
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
    }
}
