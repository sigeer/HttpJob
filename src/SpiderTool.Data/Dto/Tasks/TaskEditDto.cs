using System.ComponentModel;
using Utility.Extensions;

namespace SpiderTool.Data.Dto.Tasks
{
    public class TaskEditDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public string RootUrl { get; set; } = null!;
        public int SpiderId { get; set; }
        /// <summary>
        /// 0未开始 1正在执行 2完成 3取消
        /// </summary>
        public int Status { get; set; }
        public string? CronExpression { get; set; }
    }

    public class TaskListItemViewModel : TaskSimpleViewModel
    {
        public string Description { get; set; } = null!;
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
        public string RootUrl { get; set; } = null!;
        public int SpiderId { get; set; }
    }

    public enum TaskType
    {
        /// <summary>
        /// 加入记录，尚未请求
        /// </summary>
        [Description("尚未开始")]
        NotEffective = 0,
        /// <summary>
        /// 请求完成，未处理返回结果
        /// </summary>
        [Description("执行中")]
        InProgress = 1,
        /// <summary>
        /// 任务完成
        /// </summary>
        [Description("已完成")]
        Completed = 2,
        /// <summary>
        /// 任务中断，取消
        /// </summary>
        [Description("已取消")]
        Canceled = 3
    }
}
