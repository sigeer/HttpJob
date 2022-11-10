using SpiderTool.Data;
using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.Service;

namespace SpiderTool.FreeSql
{
    public class FreeSqlSpiderService : SpiderBaseService, ISpiderService
    {
        readonly IFreeSql _freeSql;
        public FreeSqlSpiderService(IFreeSql freeSql, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain, WorkerController controller) : base(spiderDomain, templateDomain, taskDomain, controller)
        {
            _freeSql = freeSql;
        }

        public bool CanConnect()
        {
            return _freeSql.Ado.ExecuteConnectTest();
        }

        public async Task<bool> CanConnectAsync()
        {
            return await _freeSql.Ado.ExecuteConnectTestAsync();
        }
    }
}