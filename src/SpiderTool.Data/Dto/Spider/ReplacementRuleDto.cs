namespace SpiderTool.Data.Dto.Spider
{
    public class ReplacementRuleDto
    {
        public int Id { get; set; }
        public string ReplacementOldStr { get; set; } = null!;
        public string? ReplacementNewlyStr { get; set; }
        public bool IgnoreCase { get; set; } = true;
    }
}
