namespace SpiderTool.Data.Dto.Spider
{
    public class ReplacementRuleDto
    {
        public int Id { get; set; }
        public string? ReplacementOldStr { get; set; }
        public string? ReplacementNewlyStr { get; set; }
        public bool IgnoreCase { get; set; } = false;
    }
}
