using System.ComponentModel;
using Utility.Extensions;

namespace SpiderTool.Dto.Tasks
{
    public class TaskEditDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? RootUrl { get; set; } = String.Empty;
        public int SpiderId { get; set; }
        /// <summary>
        /// 0未开始 1正在执行 2完成 3取消
        /// </summary>
        public int Status { get; set; }
        public string? CronExpression { get; set; }
    }

    public class TaskListItemViewModel : TaskSimpleViewModel
    {
        public string? Description { get; set; }
        /// <summary>
        /// 0未开始 1正在执行 2完成 3取消
        /// </summary>
        public int Status { get; set; }
        public string? CronExpression { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public string StatusName => ((TaskType)Status).GetDescription();
        public bool IsWorking => Status == (int)TaskType.InProgress || Status == (int)TaskType.NotEffective;
        public bool IsFinished => Status == (int)TaskType.Completed || Status == (int)TaskType.Canceled;
    }

    public class TaskDetailViewModel : TaskListItemViewModel
    {

    }

    public class TaskSimpleViewModel
    {
        public int Id { get; set; }
        public string? RootUrl { get; set; } = string.Empty;
        public int SpiderId { get; set; }
    }

    public enum TaskType
    {
        [Description("尚未开始")]
        NotEffective = 0,
        [Description("执行中")]
        InProgress = 1,
        [Description("已完成")]
        Completed = 2,
        [Description("已取消")]
        Canceled = 3
    }
}
