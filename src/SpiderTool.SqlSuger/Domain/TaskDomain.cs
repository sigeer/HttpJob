using SpiderTool.Data.Constants;
using SpiderTool.Data.DataBase;
using SpiderTool.Data.Dto.Tasks;
using SpiderTool.IDomain;
using SqlSugar;

namespace SpiderTool.SqlSugar.Domain
{
    public class TaskDomain : ITaskDomain
    {
        readonly ISqlSugarClient _dbContext;

        public TaskDomain(ISqlSugarClient dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddTask(TaskEditDto model)
        {
            return _dbContext.Insertable<DB_Task>(new DB_Task()
            {
                Description = model.Description,
                RootUrl = model.RootUrl,
                SpiderId = model.SpiderId,
                CreateTime = DateTime.Now
            }).ExecuteReturnIdentity();
        }

        public async Task<int> AddTaskAsync(TaskEditDto model)
        {
            return await _dbContext.Insertable<DB_Task>(new DB_Task()
            {
                Description = model.Description,
                RootUrl = model.RootUrl,
                SpiderId = model.SpiderId,
                CreateTime = DateTime.Now
            }).ExecuteReturnIdentityAsync();
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            return _dbContext.Queryable<DB_Task>().OrderByDescending(x => x.CreateTime).Take(GlobalVariable.TaskListMaxCount).Select(x => new TaskListItemViewModel()
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
            var taskDbModel = _dbContext.Queryable<DB_Task>().First(x => x.Id == model.Id);
            if (taskDbModel != null)
            {
                taskDbModel.Description = model.Description;
                taskDbModel.Status = model.Status;
                _dbContext.Updateable<DB_Task>(taskDbModel).Where(x => x.Id == model.Id).ExecuteCommand();
            }
        }

        public async Task UpdateTaskAsync(TaskEditDto model)
        {
            var taskDbModel = await _dbContext.Queryable<DB_Task>().FirstAsync(x => x.Id == model.Id);
            if (taskDbModel != null)
            {
                taskDbModel.Description = model.Description;
                taskDbModel.Status = model.Status;
                await _dbContext.Updateable<DB_Task>(taskDbModel).Where(x => x.Id == model.Id).ExecuteCommandAsync();
            }
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            var taskDbModel = _dbContext.Queryable<DB_Task>().First(x => x.Id == taskId);
            if (taskDbModel != null)
            {
                taskDbModel.Status = taskStatus;
                if (taskStatus == (int)TaskType.Completed)
                    taskDbModel.CompleteTime = DateTime.Now;
                _dbContext.Updateable<DB_Task>(taskDbModel).Where(x => x.Id == taskId).ExecuteCommand();
            }
        }

        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            var taskDbModel = await _dbContext.Queryable<DB_Task>().FirstAsync(x => x.Id == taskId);
            if (taskDbModel != null)
            {
                taskDbModel.Status = taskStatus;
                if (taskStatus == (int)TaskType.Completed)
                    taskDbModel.CompleteTime = DateTime.Now;
                await _dbContext.Updateable<DB_Task>(taskDbModel).Where(x => x.Id == taskId).ExecuteCommandAsync();
            }
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            return await _dbContext.Queryable<DB_Task>().OrderByDescending(x => x.CreateTime).Take(GlobalVariable.TaskListMaxCount).Select(x => new TaskListItemViewModel()
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
            return _dbContext.Queryable<DB_Task>().OrderByDescending(x => x.CreateTime).Select(x => new TaskSimpleViewModel()
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).ToList().DistinctBy(x => x.RootUrl).ToList();
        }

        public async Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            return (await _dbContext.Queryable<DB_Task>().OrderByDescending(x => x.CreateTime).Select(x => new TaskSimpleViewModel()
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).ToListAsync()).DistinctBy(x => x.RootUrl).ToList();
        }

        public void BulkUpdateTaskStatus(IEnumerable<int> tasks, int taskStatus)
        {
            _dbContext.Updateable<DB_Task>().SetColumns(x => x.Status == taskStatus).Where(x => tasks.Contains(x.Id)).ExecuteCommand();
        }

        public async Task BulkUpdateTaskStatusAsync(IEnumerable<int> tasks, int taskStatus)
        {
            await _dbContext.Updateable<DB_Task>().SetColumns(x => x.Status == taskStatus).Where(x => tasks.Contains(x.Id)).ExecuteCommandAsync();
        }

        public void RemoveTask(int taskId)
        {
            _dbContext.Deleteable<DB_Task>(taskId).ExecuteCommand();
        }

        public async Task RemoveTaskAsync(int taskId)
        {
            await _dbContext.Deleteable<DB_Task>(taskId).ExecuteCommandAsync();
        }
    }
}
