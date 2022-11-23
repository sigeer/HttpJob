using SpiderTool.Data;
using SpiderTool.Data.IService;
using SpiderTool.IDomain;
using SpiderTool.Service;
using SqlSugar;

namespace SpiderTool.SqlSugar
{
    public class SqlSugarSpiderService : SpiderBaseService, ISpiderService
    {
        readonly ISqlSugarClient _dbContext;
        public SqlSugarSpiderService(ISqlSugarClient dbContext, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain, WorkerController controller) : base(spiderDomain, templateDomain, taskDomain, controller)
        {
            _dbContext = dbContext;
        }


        public bool CanConnect()
        {
            try
            {
                return _dbContext.Ado.SqlQuerySingle<int>("select 1") == 1;
            }
            catch { return false; }
        }

        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await _dbContext.Ado.SqlQuerySingleAsync<int>("select 1") == 1;
            }
            catch { return false; }
        }
    }
}
