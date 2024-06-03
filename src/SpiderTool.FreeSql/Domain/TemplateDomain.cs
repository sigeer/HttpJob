using SpiderTool.Data.Constants;
using SpiderTool.Data.DataBase;
using SpiderTool.Data.Dto.Spider;
using SpiderTool.IDomain;

namespace SpiderTool.FreeSql.Domain
{
    public class TemplateDomain : ITemplateDomain
    {
        readonly IFreeSql _freeSql;

        public TemplateDomain(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }

        public string Delete(TemplateEditDto model)
        {
            _freeSql.Transaction(() =>
            {
                _freeSql.Delete<DB_ReplacementRule>().Where(x => x.TemplateId == model.Id).ExecuteAffrows();
                _freeSql.Delete<DB_Template>().Where(x => x.Id == model.Id).ExecuteAffrows();
            });
            return StatusMessage.Success;
        }

        public async Task<string> DeleteAsync(TemplateEditDto model)
        {
            return await Task.FromResult(Delete(model));
        }

        public List<TemplateDetailViewModel> GetTemplateDtoList()
        {
            var allTemplates = _freeSql.Select<DB_Template>().ToList();

            var templateRules = _freeSql.Select<DB_ReplacementRule>().ToList();


            return (from a in allTemplates
                    let b = templateRules.Where(x => x.TemplateId == a.Id).ToList()
                    select new TemplateDetailViewModel(a, b)).ToList();
        }

        public async Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync()
        {
            var allTemplates = await _freeSql.Select<DB_Template>().ToListAsync();

            var templateRules = await _freeSql.Select<DB_ReplacementRule>().ToListAsync();

            return (from a in allTemplates
                    let b = templateRules.Where(x => x.TemplateId == a.Id).ToList()
                    select new TemplateDetailViewModel(a, b)).ToList();
        }

        public string Submit(TemplateEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            _freeSql.Transaction(() =>
            {
                var dbModel = _freeSql.Select<DB_Template>().Where(x => x.Id == model.Id).ToOne();
                if (dbModel == null)
                {
                    dbModel = new DB_Template
                    {
                        CreateTime = DateTime.Now,
                        LastUpdatedTime = DateTime.Now
                    };
                    dbModel.Id = (int)_freeSql.Insert<DB_Template>(dbModel).ExecuteIdentity();
                }

                dbModel.Name = model.Name;
                dbModel.TemplateStr = model.TemplateStr;
                dbModel.ReadAttribute = model.ReadAttribute;
                dbModel.Type = model.Type;
                dbModel.LastUpdatedTime = DateTime.Now;
                dbModel.LinkedSpiderId = model.LinkedSpiderId;
                _freeSql.Update<DB_Template>().SetSource(dbModel).IgnoreColumns(x => x.CreateTime).Where(x => x.Id == dbModel.Id).ExecuteAffrows();

                _freeSql.Delete<DB_ReplacementRule>().Where(x => x.TemplateId == dbModel.Id).ExecuteAffrows();
                var data = model.ReplacementRules.Select(x => new DB_ReplacementRule
                {
                    TemplateId = dbModel.Id,
                    ReplacementNewlyStr = x.ReplacementNewlyStr,
                    ReplacementOldStr = x.ReplacementOldStr,
                    IgnoreCase = x.UseRegex
                }).ToList();
                _freeSql.Insert<DB_ReplacementRule>(data).ExecuteAffrows();
            });
            return StatusMessage.Success;
        }

        public async Task<string> SubmitAsync(TemplateEditDto model)
        {
            return await Task.FromResult(Submit(model));
        }
    }
}
