using Dapper;
using SpiderTool.Data.Constants;
using SpiderTool.Data.DataBase;
using SpiderTool.Data.Dto.Tasks;
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
            var sql = $"select * from {taskTable} order by createTime desc";
            return _dbConn.Query<TaskListItemViewModel>(sql).ToList();
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            var sql = $"select * from {taskTable} order by createTime desc";
            return (await _dbConn.QueryAsync<TaskListItemViewModel>(sql)).ToList();
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

        public List<TaskSimpleViewModel> GetTaskHistoryList()
        {
            var sql = $"select * from {taskTable} order by createTime desc";
            return _dbConn.Query<TaskSimpleViewModel>(sql).DistinctBy(x => x.RootUrl).ToList();
        }

        public async Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            var sql = $"select * from {taskTable} order by createTime desc";
            return (await _dbConn.QueryAsync<TaskSimpleViewModel>(sql)).DistinctBy(x => x.RootUrl).ToList();
        }

        public void BulkUpdateTaskStatus(IEnumerable<int> tasks, int taskStatus)
        {
            _dbConn.ExecuteScalar($"update {taskTable} set Status = @Status where id in @Id", new { Id = tasks, Status = taskStatus });
        }

        public async Task BulkUpdateTaskStatusAsync(IEnumerable<int> tasks, int taskStatus)
        {
            await _dbConn.ExecuteScalarAsync($"update {taskTable} set Status = @Status where id in @Id", new { Id = tasks, Status = taskStatus });
        }

        public void RemoveTask(int taskId)
        {
            _dbConn.ExecuteScalar($"delete from {taskTable} where id = @id", new { id = taskId });
        }

        public async Task RemoveTaskAsync(int taskId)
        {
            await _dbConn.ExecuteScalarAsync($"delete from {taskTable} where id = @id", new { id = taskId });
        }
    }
}
