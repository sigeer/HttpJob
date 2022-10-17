using SpiderTool.EntityFrameworkCore.ContextModel;
using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.Service;

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
