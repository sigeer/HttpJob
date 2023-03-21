using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiderTool.Data.DataBase
{
    [Table("db_replacementrule")]
    public class DB_ReplacementRule
    {
        [Key]
        public int Id { get; set; }
        public int TemplateId { get; set; }
        [MaxLength(100)]
        public string ReplacementOldStr { get; set; } = null!;
        [MaxLength(100)]
        public string? ReplacementNewlyStr { get; set; }
        public bool IgnoreCase { get; set; }

    }
}
