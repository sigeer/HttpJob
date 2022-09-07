using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.Dto.Resource
{
    public class ResourceDto : ResourceSetter
    {

        public DateTime CreateTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }


    }

    public class ResourceSetter
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public bool FormValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Url);
        }
    }
}
