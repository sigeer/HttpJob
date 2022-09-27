using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.Dto.Spider
{
    public class SpiderTemplateDto
    {
        public int SpiderId { get; set; }
        public List<TemplateDto> Templates { get; set; } = new List<TemplateDto>();
    }
}
