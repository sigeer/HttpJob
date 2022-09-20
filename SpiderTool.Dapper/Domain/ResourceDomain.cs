using Dapper;
using Dapper.Contrib.Extensions;
using SpiderTool.Constants;
using SpiderTool.Dapper.Utility;
using SpiderTool.DataBase;
using SpiderTool.Dto.Resource;
using SpiderTool.IDomain;
using System.Data;

namespace SpiderTool.Dapper.Domain
{
    public class ResourceDomain : IResourceDomain
    {
        readonly IDbConnection _dbConn;

        public ResourceDomain(IDbConnection dbConn)
        {
            _dbConn = dbConn;
        }

        readonly string resouceTable = typeof(DB_ResourceHistory).GetTableName();

        public List<ResourceHistoryDto> GetResourceDtoList()
        {
            return _dbConn.Query<ResourceHistoryDto>("select Id, Name, Url from " + resouceTable + " order by lastUpdatedTime desc").ToList();
        }

        public async Task<List<ResourceHistoryDto>> GetResourceDtoListAsync()
        {
            return (await _dbConn.QueryAsync<ResourceHistoryDto>("select Id, Name, Url from " + resouceTable + " order by lastUpdatedTime desc")).ToList();
        }

        public string Submit(ResourceHistorySetter model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            using var dbTran = _dbConn.BeginTransaction();

            var dbModel = _dbConn.QueryFirstOrDefault<DB_ResourceHistory>($"select id, createtime, lastUpdatedTime  from {resouceTable} where url = @url and spiderId  = @spiderId", model, dbTran);
            if (dbModel == null)
            {
                dbModel = new DB_ResourceHistory
                {
                    Url = model.Url,
                    CreateTime = DateTime.Now
                };
                dbModel.Id = _dbConn.QueryFirstOrDefault<int>($"insert into {resouceTable} (createtime, lastUpdatedTime) values(Now(6), Now(6)); select last_insert_id()", transaction: dbTran);
            }

            dbModel.Description = model.Description;
            dbModel.Name = model.Name;
            dbModel.Url = model.Url;
            dbModel.SpiderId = model.SpiderId;
            dbModel.LastUpdatedTime = DateTime.Now;
            _dbConn.Update(dbModel, dbTran);

            return StatusMessage.Success;
        }
        public string Delete(ResourceHistorySetter model)
        {
            var dbModel = new DB_ResourceHistory() { Id = model.Id };
            _dbConn.Delete(dbModel);
            return StatusMessage.Success;
        }

    }
}
