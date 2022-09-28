using SpiderTool.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWin.Services
{
    public class SpiderServiceFactory
    {
        public static ISpiderService? Service { get; set; }
    }
}
