using SpiderTool.Dto.Tasks;

namespace SpiderTool.IDomain
{
    public interface ITaskDomain
    {
        List<TaskDto> GetTaskList();
        Task<List<TaskDto>> GetTaskListAsync();
        int AddTask(TaskSetter model);
        Task<int> AddTaskAsync(TaskSetter model);
        void UpdateTask(TaskSetter model);
        Task UpdateTaskAsync(TaskSetter model);
        void SetTaskStatus(int taskId, int taskStatus);
        Task SetTaskStatusAsync(int taskId, int taskStatus);
    }
}
