using SpiderTool.Dto.Tasks;

namespace SpiderTool.IDomain
{
    public interface ITaskDomain
    {
        List<TaskListItemViewModel> GetTaskList();
        Task<List<TaskListItemViewModel>> GetTaskListAsync();
        int AddTask(TaskEditDto model);
        Task<int> AddTaskAsync(TaskEditDto model);
        void UpdateTask(TaskEditDto model);
        Task UpdateTaskAsync(TaskEditDto model);
        void SetTaskStatus(int taskId, int taskStatus);
        Task SetTaskStatusAsync(int taskId, int taskStatus);
    }
}
