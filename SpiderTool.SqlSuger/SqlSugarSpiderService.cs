using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.Service;
using SqlSugar;

namespace SpiderTool.SqlSugar
{
    public class SqlSugarSpiderService : SpiderBaseService, ISpiderService
    {
        readonly ISqlSugarClient _dbContext;
        public SqlSugarSpiderService(ISqlSugarClient dbContext, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain) : base(spiderDomain, templateDomain, taskDomain)
        {
            _dbContext = dbContext;
        }


        public bool CanConnect()
        {
            try
            {
                _dbContext.Ado.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _dbContext.Ado.Close();
            }
        }

        public Task<bool> CanConnectAsync()
        {
            throw new NotImplementedException();
        }
    }
}
