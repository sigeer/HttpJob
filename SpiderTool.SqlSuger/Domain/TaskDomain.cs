using SpiderTool.DataBase;
using SpiderTool.Dto.Tasks;
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

        public int AddTask(TaskSetter model)
        {
            return _dbContext.Insertable<DB_Task>(new DB_Task()
            {
                Description = model.Description,
                RootUrl = model.RootUrl,
                SpiderId = model.SpiderId,
                CreateTime = DateTime.Now
            }).ExecuteReturnIdentity();
        }

        public async Task<int> AddTaskAsync(TaskSetter model)
        {
            return await _dbContext.Insertable<DB_Task>(new DB_Task()
            {
                Description = model.Description,
                RootUrl = model.RootUrl,
                SpiderId = model.SpiderId,
                CreateTime = DateTime.Now
            }).ExecuteReturnIdentityAsync();
        }

        public List<TaskDto> GetTaskList()
        {
            return _dbContext.Queryable<DB_Task>().Where(x => x.Status != (int)TaskType.Canceled).OrderByDescending(x => x.CreateTime).Take(10).Select(x => new TaskDto()
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
            var taskDbModel = _dbContext.Queryable<DB_Task>().First(x => x.Id == model.Id);
            if (taskDbModel != null)
            {
                taskDbModel.Description = model.Description;
                taskDbModel.Status = model.Status;
                _dbContext.Updateable<DB_Task>(taskDbModel).Where(x => x.Id == model.Id).ExecuteCommand();
            }
        }

        public async Task UpdateTaskAsync(TaskSetter model)
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

        public async Task<List<TaskDto>> GetTaskListAsync()
        {
            return await _dbContext.Queryable<DB_Task>().Where(x => x.Status != (int)TaskType.Canceled).OrderByDescending(x => x.CreateTime).Take(10).Select(x => new TaskDto()
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
