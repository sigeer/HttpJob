using SpiderTool.Dto.Tasks;

namespace SpiderTool.IDomain
{
    public interface ITaskDomain
    {
        List<TaskDto> GetTaskList();
        int AddTask(TaskSetter model);
        void UpdateTask(TaskSetter model);
        void SetTaskStatus(int taskId, int taskStatus);
    }
}
