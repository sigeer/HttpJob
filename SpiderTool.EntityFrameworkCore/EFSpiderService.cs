using SpiderTool.EntityFrameworkCore.ContextModel;
using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.EntityFrameworkCore
{
    internal class EFSpiderService : SpiderBaseService, ISpiderService
    {
        readonly SpiderDbContext _dbContext;
        public EFSpiderService(SpiderDbContext dbContext, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain) : base(spiderDomain, templateDomain, taskDomain)
        {
            _dbContext = dbContext;
        }


        public bool CanConnect()
        {
            return _dbContext.Database.CanConnect();
        }
    }
}
