using Dapper;
using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Tasks;
using SpiderTool.IDomain;
using System.Data;

namespace SpiderTool.Dapper.Domain
{
    internal class TaskDomain : ITaskDomain
    {
        readonly IDbConnection _dbConn;
        readonly string taskTable = typeof(DB_Task).GetTableName();

        public TaskDomain(IDbConnection dbConn)
        {
            _dbConn = dbConn;
        }

        public int AddTask(TaskEditDto model)
        {
            return _dbConn.QueryFirstOrDefault<int>($"insert into {taskTable} (`Description`, `RootUrl`, `SpiderId`, `CreateTime`) values(@Description, @RootUrl, @SpiderId, now(6)); select last_insert_id()", model);
        }

        public async Task<int> AddTaskAsync(TaskEditDto model)
        {
            return await _dbConn.QueryFirstOrDefaultAsync<int>($"insert into {taskTable} (`Description`, `RootUrl`, `SpiderId`, `CreateTime`) values(@Description, @RootUrl, @SpiderId, now(6)); select last_insert_id()", model);
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            var sql = $"select * from {taskTable} where status != ${(int)TaskType.Canceled} order by createTime desc";
            return _dbConn.Query<DB_Task>(sql).ToList().Select(x => new TaskListItemViewModel()
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

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            var sql = $"select * from {taskTable} where status != ${(int)TaskType.Canceled} order by createTime desc";
            return (await _dbConn.QueryAsync<DB_Task>(sql)).ToList().Select(x => new TaskListItemViewModel()
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
            _dbConn.ExecuteScalar($"update {taskTable} set Description = @Description, Status = @Status where id = @Id", model);
        }

        public async Task UpdateTaskAsync(TaskEditDto model)
        {
            await _dbConn.ExecuteScalarAsync($"update {taskTable} set Description = @Description, Status = @Status where id = @Id", model);
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            _dbConn.ExecuteScalar($"update {taskTable} set status = @taskStatus, completetime = if({taskStatus == (int)TaskType.Completed}, now(6), null) where id = @taskid");
        }

        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            await _dbConn.ExecuteScalarAsync($"update {taskTable} set status = @taskStatus, completetime = if({taskStatus == (int)TaskType.Completed}, now(6), null) where id = @taskid");
        }
    }
}
