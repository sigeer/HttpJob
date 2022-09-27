using System;

namespace SpiderTool.Dto.Resource
{
    public class ResourceHistoryDto : ResourceHistorySetter
    {

        public DateTime CreateTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }


    }

    public class ResourceHistorySetter
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public int SpiderId { get; set; }
        public string? Description { get; set; }
        public bool FormValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Url);
        }
    }
}
