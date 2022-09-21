using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.Tasks
{
    public interface ITaskDomain
    {
        List<TaskDto> GetTaskList();
        int AddTask(TaskSetter model);
        void UpdateTask(TaskSetter model);
        void SetTaskStatus(int taskId, int taskStatus);
    }
}
