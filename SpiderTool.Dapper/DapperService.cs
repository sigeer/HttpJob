﻿using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.Dapper
{
    public class DapperService : SpiderBaseService, ISpiderService
    {
        readonly IDbConnection _dbConn;
        public DapperService(IDbConnection dbConn, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain) : base(spiderDomain, templateDomain, taskDomain)
        {
            _dbConn = dbConn;
        }

        public bool IsConnected => _dbConn.State != ConnectionState.Closed;

    }
}
