using Dapper;
using SpiderTool.Data;
using SpiderTool.Data.IService;
using SpiderTool.IDomain;
using SpiderTool.Service;
using System.Data;

namespace SpiderTool.Dapper
{
    public class DapperService : SpiderBaseService, ISpiderService
    {
        readonly IDbConnection _dbConn;
        public DapperService(IDbConnection dbConn, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain, WorkerController controller) : base(spiderDomain, templateDomain, taskDomain, controller)
        {
            _dbConn = dbConn;
        }

        public bool CanConnect()
        {
            try
            {
                return _dbConn.ExecuteScalar<int>("select 1") == 1;
            }
            catch { return false; }
        }

        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await _dbConn.ExecuteScalarAsync<int>("select 1") == 1;
            }
            catch { return false; }
        }
    }
}
