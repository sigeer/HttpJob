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
        public string Delete(SpiderEditDto model)
        {
            using var dbTrans = _dbConn.BeginTransaction();
            _dbConn.ExecuteScalar($"delete from {spiderTemplateTable} where SpiderId=@id; delete from {spiderTable} where id = @id;", model, dbTrans);
            dbTrans.Commit();
            return StatusMessage.Success;
        }

        public async Task<string> DeleteAsync(SpiderEditDto model)
        {
            using var dbTrans = _dbConn.BeginTransaction();
            await _dbConn.ExecuteScalarAsync($"delete from {spiderTemplateTable} where SpiderId=@id; delete from {spiderTable} where id = @id;", model, dbTrans);
            dbTrans.Commit();
            return StatusMessage.Success;
        }

        public void SetLinkedSpider(SpiderDetailViewModel detail)
        {
            if (detail.TemplateList != null)
            {
                foreach (var template in detail.TemplateList)
                {
                    if (template.LinkedSpiderId != null)
                        template.LinkedSpiderDetail = GetSpiderDto(template.LinkedSpiderId.Value);

                    if (template.LinkedSpiderDetail != null)
                        SetLinkedSpider(template.LinkedSpiderDetail);
                }
            }
        }


        public SpiderDetailViewModel? GetSpiderDto(int id)
        {
            var sql = $"select a.Id, a.Description, a.Name, a.Headers, a.Method, a.NextPageTemplateId, a.PostObjStr, " +
                $"b.Id, b.Name, b.TemplateStr, b.Type," +
                $"d.Id, d.Name, d.TemplateStr, d.Type from {spiderTable} a" +
                $"join {templateTable} b on a.nextPageTemplateId = b.id" +
                $"join {spiderTemplateTable} c on a.id = c.spiderId" +
                $"join {templateTable} d on b.templateId = d.id" +
                $"where a.id = @id";

            var ids = new Dictionary<int, SpiderDetailViewModel>();
            return _dbConn.Query<SpiderDetailViewModel, TemplateDetailViewModel, TemplateDetailViewModel, SpiderDetailViewModel>(sql, (a, b, c) =>
            {
                SpiderDetailViewModel? temp;
                if (!ids.TryGetValue(a.Id, out temp))
                {
                    temp = a;
                    ids.Add(a.Id, temp);
                }
                if (temp.NextPageTemplate == null)
                    temp.NextPageTemplate = b;
                if (temp.TemplateList == null)
                {
                    temp.TemplateList = new List<TemplateDetailViewModel>();
                }
                else
                {
                    temp.TemplateList.Add(c);
                }

                return a;
            }, param: new { id }).FirstOrDefault();
        }

        public async Task<SpiderDetailViewModel?> GetSpiderDtoAsync(int id)
        {
            var sql = $"select a.Id, a.Description, a.Name, a.Headers, a.Method, a.NextPageTemplateId, a.PostObjStr, " +
                $"b.Id, b.Name, b.TemplateStr, b.Type," +
                $"d.Id, d.Name, d.TemplateStr, d.Type from {spiderTable} a" +
                $"join {templateTable} b on a.nextPageTemplateId = b.id" +
                $"join {spiderTemplateTable} c on a.id = c.spiderId" +
                $"join {templateTable} d on b.templateId = d.id" +
                $"where a.id = @id";

            var ids = new Dictionary<int, SpiderDetailViewModel>();
            return (await _dbConn.QueryAsync<SpiderDetailViewModel, TemplateDetailViewModel, TemplateDetailViewModel, SpiderDetailViewModel>(sql, (a, b, c) =>
            {
                SpiderDetailViewModel? temp;
                if (!ids.TryGetValue(a.Id, out temp))
                {
                    temp = a;
                    ids.Add(a.Id, temp);
                }
                if (temp.NextPageTemplate == null)
                    temp.NextPageTemplate = b;
                if (temp.TemplateList == null)
                {
                    temp.TemplateList = new List<TemplateDetailViewModel>();
                }
                else
                {
                    temp.TemplateList.Add(c);
                }

                return a;
            }, param: new { id })).FirstOrDefault();
        }

        public List<SpiderListItemViewModel> GetSpiderDtoList()
        {
            var sql = $"select a.Id, a.Name from {spiderTable}";
            return _dbConn.Query<SpiderListItemViewModel>(sql).ToList();
        }

        public async Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync()
        {
            var sql = $"select a.Id, a.Name from {spiderTable}";
            return (await _dbConn.QueryAsync<SpiderListItemViewModel>(sql)).ToList();
        }

        public string Submit(SpiderEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            var selectedTemplates = _dbConn.Query<int>($"select LinkedSpiderId from {templateTable} where {model.Id} in @ids and LinkedSpiderId is not null", model.Templates).ToList();
            if (selectedTemplates.Contains(model.Id))
                return "可能出现递归调用";

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

        public async Task<string> SubmitAsync(SpiderEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            var selectedTemplates = (await _dbConn.QueryAsync<int>($"select LinkedSpiderId from {templateTable} where {model.Id} in @ids and LinkedSpiderId is not null", model.Templates)).ToList();
            if (selectedTemplates.Contains(model.Id))
                return "可能出现递归调用";

            using var dbTrans = _dbConn.BeginTransaction();
            var dbModel = await _dbConn.QueryFirstOrDefaultAsync<DB_Spider>($"select id, createtime, lastUpdatedTime from {spiderTable} where id = @id", model);
            if (dbModel == null)
            {
                dbModel = new DB_Spider();
                dbModel.Id = await _dbConn.QueryFirstOrDefaultAsync<int>($"insert into {spiderTable} (`createtime`, `lastUpdatedTime`) values(now(6), now(6)); select last_insert_id()");
            }

            dbModel.Name = model.Name;
            dbModel.Method = model.Method;
            dbModel.Description = model.Description;
            dbModel.Headers = model.Headers;
            dbModel.PostObjStr = model.PostObjStr;
            dbModel.NextPageTemplateId = model.NextPageTemplateId;
            dbModel.LastUpdatedTime = DateTime.Now;

            await _dbConn.UpdateAsync(dbModel);

            await _dbConn.ExecuteScalarAsync($"delete from {spiderTemplateTable} where spiderId = @id", dbModel);
            await _dbConn.InsertAsync(model.Templates.Select(x => new DB_SpiderTemplate { SpiderId = dbModel.Id, TemplateId = x }));
            dbTrans.Commit();
            return StatusMessage.Success;
        }
    }
}
