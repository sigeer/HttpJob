using Microsoft.EntityFrameworkCore;
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

        public int AddTask(TaskSetter model)
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

        public async Task<int> AddTaskAsync(TaskSetter model)
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

        public List<TaskDto> GetTaskList()
        {
            return _dbContext.Tasks.Where(x => x.Status != (int)TaskType.Canceled).OrderByDescending(x => x.CreateTime).Take(10).Select(x => new TaskDto()
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

        public void UpdateTask(TaskSetter model)
        {
            var taskDbModel = _dbContext.Tasks.FirstOrDefault(x => x.Id == model.Id);
            if (taskDbModel != null)
            {
                taskDbModel.Status = model.Status;
                taskDbModel.Description = model.Description;
                _dbContext.SaveChanges();
            }
        }

        public async Task UpdateTaskAsync(TaskSetter model)
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

        public async Task<List<TaskDto>> GetTaskListAsync()
        {
            return await _dbContext.Tasks.Where(x => x.Status != (int)TaskType.Canceled).OrderByDescending(x => x.CreateTime).Take(10).Select(x => new TaskDto()
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
    }
}
