using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.Dto.Spider
{
    public class ReplacementRuleDto
    {
        public int Id { get; set; }
        public string? ReplacementOldStr { get; set; }
        public string? ReplacementNewlyStr { get; set; }
    }
}
