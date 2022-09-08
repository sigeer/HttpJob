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

        readonly string resouceTable = typeof(DB_Resource).GetTableName();

        public List<ResourceDto> GetResourceDtoList()
        {
            return _dbConn.Query<ResourceDto>("select Id, Name, Url from " + resouceTable).ToList();
        }

        public string Submit(ResourceSetter model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            using var dbTran = _dbConn.BeginTransaction();

            var dbModel = _dbConn.QueryFirstOrDefault<DB_Resource>($"select id, createtime, lastUpdatedTime  from {resouceTable} where id = @id", model, dbTran);
            if (dbModel == null)
            {
                dbModel = new DB_Resource
                {
                    CreateTime = DateTime.Now
                };
                dbModel.Id = _dbConn.QueryFirstOrDefault<int>($"insert into {resouceTable} (createtime, lastUpdatedTime) values(Now(6), Now(6)); select last_insert_id()", transaction: dbTran);
            }

            dbModel.Description = model.Description;
            dbModel.Name = model.Name;
            dbModel.Url = model.Url;
            dbModel.LastUpdatedTime = DateTime.Now;
            _dbConn.Update(dbModel, dbTran);

            return StatusMessage.Success;
        }
        public string Delete(ResourceSetter model)
        {
            var dbModel = new DB_Resource() { Id = model.Id };
            _dbConn.Delete(dbModel);
            return StatusMessage.Success;
        }

    }
}
