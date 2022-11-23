using SpiderTool.Data;
using SpiderTool.Data.IService;
using SpiderTool.EntityFrameworkCore.ContextModel;
using SpiderTool.IDomain;
using SpiderTool.Service;

namespace SpiderTool.EntityFrameworkCore
{
    internal class EFSpiderService : SpiderBaseService, ISpiderService
    {
        readonly SpiderDbContext _dbContext;
        public EFSpiderService(SpiderDbContext dbContext, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain, WorkerController controller) : base(spiderDomain, templateDomain, taskDomain, controller)
        {
            _dbContext = dbContext;
        }


        public bool CanConnect()
        {
            try
            {
                return _dbContext.Database.CanConnect();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await _dbContext.Database.CanConnectAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}
