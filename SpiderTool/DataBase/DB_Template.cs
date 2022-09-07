using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiderTool.DataBase
{
    [Table("db_template")]
    public class DB_Template
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        /// <summary>
        /// Html文本, textContent文本, 下载链接, 跳转链接
        /// </summary>
        public int Type { get; set; }
        [MaxLength(100)]
        public string? TemplateStr { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }

        /// <summary>
        /// 当type = 跳转link时， 跳转的url使用的爬虫
        /// </summary>
        public int? LinkedSpiderId { get; set; }
    }
}
