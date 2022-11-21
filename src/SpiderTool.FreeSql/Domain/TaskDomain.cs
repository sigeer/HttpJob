using SpiderTool.Data.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Tasks;
using SpiderTool.IDomain;

namespace SpiderTool.FreeSql.Domain
{
    public class TaskDomain : ITaskDomain
    {
        readonly IFreeSql _freeSql;

        public TaskDomain(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }

        public int AddTask(TaskEditDto model)
        {
            return (int)_freeSql.Insert<DB_Task>(new DB_Task()
            {
                Description = model.Description,
                RootUrl = model.RootUrl,
                SpiderId = model.SpiderId,
                CreateTime = DateTime.Now
            }).ExecuteIdentity();
        }

        public async Task<int> AddTaskAsync(TaskEditDto model)
        {
            return (int)await _freeSql.Insert<DB_Task>(new DB_Task()
            {
                Description = model.Description,
                RootUrl = model.RootUrl,
                SpiderId = model.SpiderId,
                CreateTime = DateTime.Now
            }).ExecuteIdentityAsync();
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            return _freeSql.Select<DB_Task>().OrderByDescending(x => x.CreateTime).Take(GlobalVariable.TaskListMaxCount).ToList(x => new TaskListItemViewModel()
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
            var taskDbModel = _freeSql.Select<DB_Task>().Where(x => x.Id == model.Id).ToOne();
            if (taskDbModel != null)
            {
                taskDbModel.Description = model.Description;
                taskDbModel.Status = model.Status;
                _freeSql.Update<DB_Task>().SetSource(taskDbModel).UpdateColumns(a => new { a.Description, a.Status }).ExecuteAffrows();
            }
        }

        public async Task UpdateTaskAsync(TaskEditDto model)
        {
            var taskDbModel = await _freeSql.Select<DB_Task>().Where(x => x.Id == model.Id).ToOneAsync();
            if (taskDbModel != null)
            {
                taskDbModel.Description = model.Description;
                taskDbModel.Status = model.Status;
                await _freeSql.Update<DB_Task>().SetSource(taskDbModel).UpdateColumns(a => new { a.Description, a.Status }).ExecuteAffrowsAsync();
            }
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            var expression = _freeSql.Update<DB_Task>().Set(x => x.Status, taskStatus);
            if (taskStatus == (int)TaskType.Completed)
                expression = expression.Set(x => x.CompleteTime, DateTime.Now);
            expression.Where(x => x.Id == taskId).ExecuteAffrows();
        }

        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            var expression = _freeSql.Update<DB_Task>().Set(x => x.Status, taskStatus);
            if (taskStatus == (int)TaskType.Completed)
                expression = expression.Set(x => x.CompleteTime, DateTime.Now);
            await expression.Where(x => x.Id == taskId).ExecuteAffrowsAsync();
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            return await _freeSql.Select<DB_Task>().OrderByDescending(x => x.CreateTime).Take(GlobalVariable.TaskListMaxCount).ToListAsync(x => new TaskListItemViewModel()
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
            return _freeSql.Select<DB_Task>().OrderByDescending(x => x.CreateTime).ToList(x => new TaskSimpleViewModel()
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).ToList().DistinctBy(x => x.RootUrl).ToList();
        }

        public async Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            return (await _freeSql.Select<DB_Task>().OrderByDescending(x => x.CreateTime).ToListAsync(x => new TaskSimpleViewModel()
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            })).DistinctBy(x => x.RootUrl).ToList();
        }

        public void BulkUpdateTaskStatus(IEnumerable<int> tasks, int taskStatus)
        {
            _freeSql.Update<DB_Task>().Set(x => x.Status == taskStatus).Where(x => tasks.Contains(x.Id)).ExecuteAffrows();
        }

        public async Task BulkUpdateTaskStatusAsync(IEnumerable<int> tasks, int taskStatus)
        {
            await _freeSql.Update<DB_Task>().Set(x => x.Status == taskStatus).Where(x => tasks.Contains(x.Id)).ExecuteAffrowsAsync();
        }

        public void RemoveTask(int taskId)
        {
            _freeSql.Delete<DB_Task>(taskId).ExecuteAffrows();
        }

        public async Task RemoveTaskAsync(int taskId)
        {
            await _freeSql.Delete<DB_Task>(taskId).ExecuteAffrowsAsync();
        }
    }
}
