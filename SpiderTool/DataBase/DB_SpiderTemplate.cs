using System.ComponentModel.DataAnnotations.Schema;

namespace SpiderTool.DataBase
{
    [Table("db_spidertemplate")]
    public class DB_SpiderTemplate
    {
        public int Id { get; set; }
        public int SpiderId { get; set; }
        public int TemplateId { get; set; }
    }
}
