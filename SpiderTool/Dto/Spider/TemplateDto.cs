using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Extensions;

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

    public enum TemplateTypeEnum
    {
        Html = 1,
        Text = 2,
        Object = 3,
        JumpLink = 4
    }

    public class TemplateType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public static List<TemplateType> GetAll()
        {
            return new List<TemplateType>()
            {
                new TemplateType
                {
                    Id = TemplateTypeEnum.Html.ToInt(),
                    Name = nameof(TemplateTypeEnum.Html)
                },
                new TemplateType
                {
                    Id = TemplateTypeEnum.Text.ToInt(),
                    Name = nameof(TemplateTypeEnum.Text)
                },
                new TemplateType
                {
                    Id = TemplateTypeEnum.Object.ToInt(),
                    Name = nameof(TemplateTypeEnum.Object)
                },
                new TemplateType
                {
                    Id = TemplateTypeEnum.JumpLink.ToInt(),
                    Name = nameof(TemplateTypeEnum.JumpLink)
                }
            };
        }
    }
}
