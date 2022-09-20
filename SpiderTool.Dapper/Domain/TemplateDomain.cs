using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
using SpiderTool.IDomain;
using System.Data;
using Dapper;
using Dapper.Contrib;
using SpiderTool.Dapper.Utility;
using Dapper.Contrib.Extensions;

namespace SpiderTool.Dapper.Domain
{
    public class TemplateDomain : ITemplateDomain
    {
        readonly IDbConnection _dbConn;

        public TemplateDomain(IDbConnection dbConn)
        {
            _dbConn = dbConn;
        }

        private string templateTable = typeof(DB_Template).GetTableName();
        private string replacementRule = typeof(DB_ReplacementRule).GetTableName();

        public string Delete(TemplateDto model)
        {
            var dbModel = new DB_Template() { Id = model.Id };
            _dbConn.ExecuteScalar($"delete from {templateTable} where id = @id;" +
                $" delete from {replacementRule} where templateId = @id", dbModel);
            return StatusMessage.Success;
        }

        public List<TemplateDto> GetTemplateDtoList()
        {
            var sql = $"select a.TemplateId as Id, c.Name, c.TemplateStr, c.LinkedSpiderId, c.Type, a.ReplacementOldStr, a.ReplacementNewlyStr " +
                $"from {replacementRule} a" +
                $"join {templateTable} c on c.id = a.templateId";

            var ids = new Dictionary<int, TemplateDto>();
            _dbConn.Query<TemplateDto, ReplacementRuleDto, TemplateDto>(sql, (a, b) =>
            {
                TemplateDto? tmp;
                if (!ids.TryGetValue(a.Id, out tmp))
                {
                    tmp = a;
                    ids.Add(a.Id, tmp);
                }
                if (tmp.ReplacementRules == null)
                    tmp.ReplacementRules = new List<ReplacementRuleDto>();
                tmp.ReplacementRules.Add(b);
                return tmp;
            });
            return ids.Values.ToList();
        }

        public async Task<List<TemplateDto>> GetTemplateDtoListAsync()
        {
            var sql = $"select a.TemplateId as Id, c.Name, c.TemplateStr, c.LinkedSpiderId, c.Type, a.ReplacementOldStr, a.ReplacementNewlyStr " +
                $"from {replacementRule} a" +
                $"join {templateTable} c on c.id = a.templateId";

            var ids = new Dictionary<int, TemplateDto>();
            await _dbConn.QueryAsync<TemplateDto, ReplacementRuleDto, TemplateDto>(sql, (a, b) =>
            {
                TemplateDto? tmp;
                if (!ids.TryGetValue(a.Id, out tmp))
                {
                    tmp = a;
                    ids.Add(a.Id, tmp);
                }
                if (tmp.ReplacementRules == null)
                    tmp.ReplacementRules = new List<ReplacementRuleDto>();
                tmp.ReplacementRules.Add(b);
                return tmp;
            });
            return ids.Values.ToList();
        }

        public string Submit(TemplateDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            using var dbTrans = _dbConn.BeginTransaction();

            var dbModel = _dbConn.QueryFirst<DB_Template>($"select id, createtime, lastUpdatedTime from {templateTable}", transaction: dbTrans);
            if (dbModel == null)
            {
                dbModel = new DB_Template();
                dbModel.Id = _dbConn.QueryFirstOrDefault<int>($"insert into {templateTable} (`createtime`, `lastUpdatedTime`) values(now(6), now(6)); select last_insert_id()");
            }

            dbModel.Name = model.Name;
            dbModel.TemplateStr = model.TemplateStr;
            dbModel.Type = model.Type;
            dbModel.LastUpdatedTime = DateTime.Now;
            dbModel.LinkedSpiderId = model.LinkedSpiderId;
            _dbConn.Update(dbModel, dbTrans);

            _dbConn.ExecuteScalar($"delete from {replacementRule} where templateId = @id", dbModel, dbTrans);
            _dbConn.Insert(model.ReplacementRules.Select(x => new DB_ReplacementRule
            {
                TemplateId = dbModel.Id,
                ReplacementOldStr = x.ReplacementOldStr,
                ReplacementNewlyStr = x.ReplacementNewlyStr
            }), dbTrans);
            dbTrans.Commit();

            return StatusMessage.Success;
        }
    }
}
