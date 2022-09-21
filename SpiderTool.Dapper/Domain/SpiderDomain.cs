using Dapper;
using Dapper.Contrib.Extensions;
using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
using SpiderTool.IDomain;
using System.Data;

namespace SpiderTool.Dapper.Domain
{
    public class SpiderDomain : ISpiderDomain
    {
        readonly IDbConnection _dbConn;

        public SpiderDomain(IDbConnection dbConn)
        {
            _dbConn = dbConn;
        }

        readonly string spiderTable = typeof(DB_Spider).GetTableName();
        readonly string spiderTemplateTable = typeof(DB_SpiderTemplate).GetTableName();
        readonly string templateTable = typeof(DB_Template).GetTableName();
        public string Delete(SpiderDtoSetter model)
        {
            using var dbTrans = _dbConn.BeginTransaction();
            _dbConn.ExecuteScalar($"delete from {spiderTemplateTable} where SpiderId=@id; delete from {spiderTable} where id = @id;", model, dbTrans);
            dbTrans.Commit();
            return StatusMessage.Success;
        }

        public SpiderDto? GetSpiderDto(int id)
        {
            var sql = $"select a.Id, a.Description, a.Name, a.Headers, a.Method, a.NextPageTemplateId, a.PostObjStr, " +
                $"b.Id, b.Name, b.TemplateStr, b.Type," +
                $"d.Id, d.Name, d.TemplateStr, d.Type from {spiderTable} a" +
                $"join {templateTable} b on a.nextPageTemplateId = b.id" +
                $"join {spiderTemplateTable} c on a.id = c.spiderId" +
                $"join {templateTable} d on b.templateId = d.id" +
                $"where a.id = @id";

            var ids = new Dictionary<int, SpiderDto>();
            return _dbConn.Query<SpiderDto, TemplateDto, TemplateDto, SpiderDto>(sql, (a, b, c) =>
            {
                SpiderDto? temp;
                if (!ids.TryGetValue(a.Id, out temp))
                {
                    temp = a;
                    ids.Add(a.Id, temp);
                }
                if (temp.NextPageTemplate == null)
                    temp.NextPageTemplate = b;
                if (temp.TemplateList == null)
                {
                    temp.TemplateList = new List<TemplateDto>();
                    temp.Templates = new List<int>();
                }
                else
                {
                    temp.Templates.Add(c.Id);
                    temp.TemplateList.Add(c);
                }

                return a;
            }, param: new { id }).FirstOrDefault();
        }

        public List<SpiderDtoSetter> GetSpiderDtoList()
        {
            var sql = $"select a.Id, a.Name, a.Method, a.Headers, a.Description, a.NextPageTemplateId, a.PostObjStr, b.TemplateId " +
                $"from {spiderTable} a join {spiderTemplateTable} b on a.Id = b.SpiderId";

            var ids = new Dictionary<int, SpiderDtoSetter>();
            var data = _dbConn.Query<SpiderDtoSetter, int, SpiderDtoSetter>(sql, (x, y) =>
            {
                SpiderDtoSetter? temp;
                if (!ids.TryGetValue(x.Id, out temp))
                {
                    temp = x;
                    ids.Add(x.Id, temp);
                }
                if (temp.Templates == null)
                    temp.Templates = new List<int>();
                temp.Templates.Add(y);
                return x;
            }, splitOn: "TemplateId").ToList();

            return ids.Values.ToList();
        }

        public List<SpiderTemplateDto> GetSpiderTemplatesDto(int spiderId)
        {
            return new List<SpiderTemplateDto>();
        }

        public string Submit(SpiderDtoSetter model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            using var dbTrans = _dbConn.BeginTransaction();
            var dbModel = _dbConn.QueryFirstOrDefault<DB_Spider>($"select id, createtime, lastUpdatedTime from {spiderTable} where id = @id", model);
            if (dbModel == null)
            {
                dbModel = new DB_Spider();
                dbModel.Id = _dbConn.QueryFirstOrDefault<int>($"insert into {spiderTable} (`createtime`, `lastUpdatedTime`) values(now(6), now(6)); select last_insert_id()");
            }

            dbModel.Name = model.Name;
            dbModel.Method = model.Method;
            dbModel.Description = model.Description;
            dbModel.Headers = model.Headers;
            dbModel.PostObjStr = model.PostObjStr;
            dbModel.NextPageTemplateId = model.NextPageTemplateId;
            dbModel.LastUpdatedTime = DateTime.Now;

            _dbConn.Update(dbModel);

            _dbConn.ExecuteScalar($"delete from {spiderTemplateTable} where spiderId = @id", dbModel);
            _dbConn.Insert(model.Templates.Select(x => new DB_SpiderTemplate { SpiderId = dbModel.Id, TemplateId = x }));
            dbTrans.Commit();
            return StatusMessage.Success;
        }
    }
}
