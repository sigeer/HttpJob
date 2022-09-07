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
            var dbModel = _dbConn.QueryFirst<DB_Resource>($"select * from {resouceTable} where id = @id", model);
            if (dbModel == null)
            {
                dbModel = new DB_Resource
                {
                    CreateTime = DateTime.Now
                };
                _dbConn.Insert(dbModel);
            }

            dbModel.Description = model.Description;
            dbModel.Name = model.Name;
            dbModel.Url = model.Url;
            dbModel.LastUpdatedTime = DateTime.Now;
            _dbConn.Update(dbModel);

            return StatusMessage.Success;
        }
        public string Delete(ResourceSetter model)
        {
            var dbModel = new DB_Resource() { Id = model.Id }; ;
            _dbConn.Delete(dbModel);
            return StatusMessage.Success;
        }

    }
}
