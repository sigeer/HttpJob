using SpiderTool.Data;
using SpiderTool.IDomain;
using SpiderTool.IService;
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
                _dbConn.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _dbConn.Close();
            }
        }

        public async Task<bool> CanConnectAsync()
        {
            return await Task.Run(() => CanConnect());
        }
    }
}
