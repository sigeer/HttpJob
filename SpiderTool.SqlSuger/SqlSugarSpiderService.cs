using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.Service;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.SqlSugar
{
    internal class SqlSugarSpiderService : SpiderBaseService, ISpiderService
    {
        readonly ISqlSugarClient _dbContext;
        public SqlSugarSpiderService(ISqlSugarClient dbContext, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain) : base(spiderDomain, templateDomain, taskDomain)
        {
            _dbContext = dbContext;
        }

        public bool IsConnected => _dbContext.Ado.Connection.State != System.Data.ConnectionState.Closed;
    }
}
