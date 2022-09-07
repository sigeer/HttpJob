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
        private string templateReplacementRule = typeof(DB_TemplateReplacementRule).GetTableName();
        private string replacementRule = typeof(DB_TemplateReplacementRule).GetTableName();

        public string Delete(TemplateDto model)
        {
            var dbModel = new DB_Template() { Id = model.Id };
            _dbConn.ExecuteScalar($"delete from {templateTable} where id = @id;" +
                $" delete from {templateReplacementRule} where templateId = @id", dbModel);
            return StatusMessage.Success;
        }

        public List<TemplateDto> GetTemplateDtoList()
        {
            var sql = $"select a.TemplateId as Id, c.Name, c.TemplateStr, c.LinkedSpiderId, c.Type, b.Id as ReplacmentRuleId, b.ReplacementOldStr, b.ReplacementNewlyStr " +
                $"from {templateReplacementRule} a" +
                $"join {replacementRule} b on a.RuleId = b.Id" +
                $"join {templateTable} c on c.id = a.templateId";

            return _dbConn.Query<TemplateDto, ReplacementRuleDto, TemplateDto>(sql, (a, b) =>
            {
                a.ReplacementRules ??= new List<ReplacementRuleDto>();
                a.ReplacementRules.Add(b);
                return a;
            }).ToList();
        }

        public async Task<List<TemplateDto>> GetTemplateDtoListAsync()
        {
            var sql = $"select a.TemplateId as Id, c.Name, c.TemplateStr, c.LinkedSpiderId, c.Type, b.Id as ReplacmentRuleId, b.ReplacementOldStr, b.ReplacementNewlyStr " +
                $"from {templateReplacementRule} a" +
                $"join {replacementRule} b on a.RuleId = b.Id" +
                $"join {templateTable} c on c.id = a.templateId";

            return (await _dbConn.QueryAsync<TemplateDto, ReplacementRuleDto, TemplateDto>(sql, (a, b) =>
            {
                a.ReplacementRules ??= new List<ReplacementRuleDto>();
                a.ReplacementRules.Add(b);
                return a;
            }, splitOn: "ReplacmentRuleId")).ToList();
        }

        public string Submit(TemplateDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            using var dbTrans = _dbConn.BeginTransaction();

            var dbModel = _dbConn.QueryFirst<DB_Template>($"select * from {templateTable}");
            if (dbModel == null)
            {
                dbModel = new DB_Template
                {
                    CreateTime = DateTime.Now
                };
                _dbConn.Insert(dbModel, dbTrans);
            }

            dbModel.Name = model.Name;
            dbModel.TemplateStr = model.TemplateStr;
            dbModel.Type = model.Type;
            dbModel.LastUpdatedTime = DateTime.Now;
            dbModel.LinkedSpiderId = model.LinkedSpiderId;
            _dbConn.Update(dbModel, dbTrans);

            _dbConn.ExecuteScalar($"delete from {templateReplacementRule} where templateId = @id", dbModel, dbTrans);
            _dbConn.Insert(model.ReplacementRules.Select(x => new DB_TemplateReplacementRule
            {
                RuleId = x.Id,
                TemplateId = dbModel.Id
            }), dbTrans);
            dbTrans.Commit();

            return StatusMessage.Success;
        }
    }
}
