using System.ComponentModel;
using Utility.Extensions;

namespace SpiderTool.Dto.Spider
{
    public class TemplateEditDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Html文本, textContent文本, 下载链接, 跳转链接
        /// </summary>
        public int Type { get; set; }
        public string? TemplateStr { get; set; }

        /// <summary>
        /// type == 跳转链接时 为必填，仅在作为子爬虫时使用
        /// </summary>
        public int? LinkedSpiderId { get; set; }
        public List<ReplacementRuleDto> ReplacementRules { get; set; } = new List<ReplacementRuleDto>();

        public bool FormValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(TemplateStr)
                && Type > 0 && (Type != 4 || LinkedSpiderId.HasValue)
                && ReplacementRules.All(x => !string.IsNullOrEmpty(x.ReplacementOldStr)
                && !ReplacementRules.GroupBy(x => new { x.ReplacementOldStr, x.ReplacementNewlyStr }).Any(x => x.Count() > 1));
        }
    }

    public class TemplateDetailViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Html文本, textContent文本, 下载链接, 跳转链接
        /// </summary>
        public int Type { get; set; }
        public string TypeName => ((TemplateTypeEnum)Type).GetDescription();
        public string? TemplateStr { get; set; }

        /// <summary>
        /// type == 跳转链接时 为必填，仅在作为子爬虫时使用
        /// </summary>
        public int? LinkedSpiderId { get; set; }
        public SpiderDetailViewModel? LinkedSpiderDetail { get; set; }
        public List<ReplacementRuleDto> ReplacementRules { get; set; } = new List<ReplacementRuleDto>();

        public TemplateEditDto ToEditModel()
        {
            return new TemplateEditDto
            {
                Id = Id,
                Name = Name,
                LinkedSpiderId = LinkedSpiderId,
                ReplacementRules = ReplacementRules,
                TemplateStr = TemplateStr,
                Type = Type
            };
        }
    }

    public enum TemplateTypeEnum
    {
        [Description("错误数据")]
        Default = 0,
        [Description("Html")]
        Html = 1,
        [Description("Text")]
        Text = 2,
        [Description("资源文件")]
        Object = 3,
        [Description("跳转链接")]
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
