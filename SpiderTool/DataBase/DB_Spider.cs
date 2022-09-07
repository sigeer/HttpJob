using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiderTool.DataBase
{
    [Table("db_spider")]
    public class DB_Spider
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        /// <summary>
        /// "POST" or "GET"
        /// </summary>
        [MaxLength(10)]
        public string? Method { get; set; }
        /// <summary>
        /// object => json
        /// </summary>
        public string? PostObjStr { get; set; }
        /// <summary>
        /// dictionary => json
        /// </summary>
        public string? Headers { get; set; }

        public int? NextPageTemplateId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
