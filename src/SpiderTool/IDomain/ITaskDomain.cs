using SpiderTool.Dto.Tasks;

namespace SpiderTool.IDomain
{
    public interface ITaskDomain
    {
        List<TaskListItemViewModel> GetTaskList();
        Task<List<TaskListItemViewModel>> GetTaskListAsync();
        List<TaskSimpleViewModel> GetTaskHistoryList();
        Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync();
        int AddTask(TaskEditDto model);
        Task<int> AddTaskAsync(TaskEditDto model);
        void UpdateTask(TaskEditDto model);
        Task UpdateTaskAsync(TaskEditDto model);
        void SetTaskStatus(int taskId, int taskStatus);
        Task SetTaskStatusAsync(int taskId, int taskStatus);
        void BulkUpdateTaskStatus(IEnumerable<int> tasks, int taskStatus);
        Task BulkUpdateTaskStatusAsync(IEnumerable<int> tasks, int taskStatus);

        void RemoveTask(int taskId);
        Task RemoveTaskAsync(int taskId);
    }
}
