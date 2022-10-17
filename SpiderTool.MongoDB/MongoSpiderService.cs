using MongoDB.Driver;
using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.MongoDB
{
    internal class MongoSpiderService : SpiderBaseService, ISpiderService
    {
        readonly IMongoClient _db;
        public MongoSpiderService(IMongoClient db, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain) : base(spiderDomain, templateDomain, taskDomain)
        {
            _db = db;
        }

        public bool CanConnect()
        {
            return _db.Cluster.Settings.DirectConnection ?? false;
        }
    }
}
