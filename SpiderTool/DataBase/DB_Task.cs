using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiderTool.DataBase
{
    [Table("db_task")]
    public class DB_Task
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        [MaxLength(500)]
        public string? RootUrl { get; set; }
        public int SpiderId { get; set; }
        /// <summary>
        /// 0未开始 1正在执行 2完成
        /// </summary>
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        [MaxLength(50)]
        public string? CronExpression { get; set; }
    }
}
