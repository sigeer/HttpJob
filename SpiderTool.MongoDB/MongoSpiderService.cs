﻿using MongoDB.Driver;
using SpiderTool.Data;
using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.Service;

namespace SpiderTool.MongoDB
{
    public class MongoSpiderService : SpiderBaseService, ISpiderService
    {
        readonly IMongoClient _db;
        readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public MongoSpiderService(IMongoClient db, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, ITaskDomain taskDomain, WorkerController controller) : base(spiderDomain, templateDomain, taskDomain, controller)
        {
            _db = db;
        }

        public bool CanConnect()
        {
            try
            {
                Task.Delay(3000).ContinueWith((d) =>
                {
                    _tokenSource.Cancel();
                });
                _db.ListDatabases(_tokenSource.Token);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CanConnectAsync()
        {
            try
            {
                await Task.Delay(3000).ContinueWith((d) =>
                {
                    _tokenSource.Cancel();
                });
                await _db.ListDatabasesAsync(_tokenSource.Token);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
