using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.DataBase
{
    [Table("db_replacementrule")]
    public class DB_ReplacementRule
    {
        public int Id { get; set; }
        public string? ReplacementOldStr { get; set; }
        public string? ReplacementNewlyStr { get; set; }

    }
    [Table("db_templatereplacementrule")]
    public class DB_TemplateReplacementRule
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public int TemplateId { get; set; }
    }
}
