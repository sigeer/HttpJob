using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.Dto.Spider
{
    public class TemplateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Html文本, textContent文本, 下载链接, 跳转链接
        /// </summary>
        public int Type { get; set; }
        public string? TemplateStr { get; set; }

        /// <summary>
        /// type == 跳转链接时 为必填
        /// </summary>
        public int? LinkedSpiderId { get; set; }
        public List<ReplacementRuleDto> ReplacementRules { get; set; } = new List<ReplacementRuleDto>();

        public bool FormValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(TemplateStr) && Type > 0 && (Type != 4 || LinkedSpiderId.HasValue) && ReplacementRules.All(x => !string.IsNullOrEmpty(x.ReplacementOldStr));
        }
    }
}
