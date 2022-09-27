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
    }
}
