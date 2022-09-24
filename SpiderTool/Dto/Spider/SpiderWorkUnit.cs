using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.Dto.Spider
{
    public class SpiderWorkUnit
    {
        public SpiderWorker? SpiderWorker { get; set; }
        public string? Url { get; set; }
    }

    public class SpiderWorkTaskUnit
    {
        public int SpiderId { get; set; }
        public string? Url { get; set; }
    }
}
