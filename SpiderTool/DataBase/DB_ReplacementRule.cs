﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int TemplateId { get; set; }
        [MaxLength(100)]
        public string? ReplacementOldStr { get; set; }
        [MaxLength(100)]
        public string? ReplacementNewlyStr { get; set; }

    }
}
