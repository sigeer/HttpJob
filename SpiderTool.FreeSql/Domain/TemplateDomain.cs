using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
using SpiderTool.IDomain;

namespace SpiderTool.FreeSql.Domain
{
    public class TemplateDomain : ITemplateDomain
    {
        readonly IFreeSql _dbContext;

        public TemplateDomain(IFreeSql dbContext)
        {
            _dbContext = dbContext;
        }

        public string Delete(TemplateEditDto model)
        {
            _dbContext.Transaction(() =>
            {
                _dbContext.Delete<DB_ReplacementRule>().Where(x => x.TemplateId == model.Id).ExecuteAffrows();
                _dbContext.Delete<DB_Template>().Where(x => x.Id == model.Id).ExecuteAffrows();
            });
            return StatusMessage.Success;
        }

        public async Task<string> DeleteAsync(TemplateEditDto model)
        {
            return await Task.FromResult(Delete(model));
        }

        public List<TemplateDetailViewModel> GetTemplateDtoList()
        {
            var allTemplates = _dbContext.Select<DB_Template>().ToList();

            var templateRules = _dbContext.Select<DB_ReplacementRule>().ToList();


            return (from a in allTemplates
                    let b = templateRules.Where(x => x.TemplateId == a.Id).ToList()
                    select new TemplateDetailViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        TemplateStr = a.TemplateStr,
                        LinkedSpiderId = a.LinkedSpiderId,
                        Type = a.Type,
                        ReplacementRules = b.Select(x => new ReplacementRuleDto
                        {
                            Id = x.Id,
                            ReplacementOldStr = x.ReplacementOldStr,
                            ReplacementNewlyStr = x.ReplacementNewlyStr,
                            IgnoreCase = x.IgnoreCase
                        }).ToList()
                    }).ToList();
        }

        public async Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync()
        {
            var allTemplates = await _dbContext.Select<DB_Template>().ToListAsync();

            var templateRules = await _dbContext.Select<DB_ReplacementRule>().ToListAsync();

            return (from a in allTemplates
                    let b = templateRules.Where(x => x.TemplateId == a.Id).ToList()
                    select new TemplateDetailViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        TemplateStr = a.TemplateStr,
                        LinkedSpiderId = a.LinkedSpiderId,
                        Type = a.Type,
                        ReplacementRules = b.Select(x => new ReplacementRuleDto
                        {
                            Id = x.Id,
                            ReplacementOldStr = x.ReplacementOldStr,
                            ReplacementNewlyStr = x.ReplacementNewlyStr,
                            IgnoreCase = x.IgnoreCase
                        }).ToList()
                    }).ToList();
        }

        public string Submit(TemplateEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            _dbContext.Transaction(() =>
            {
                var dbModel = _dbContext.Select<DB_Template>().Where(x => x.Id == model.Id).First();
                if (dbModel == null)
                {
                    dbModel = new DB_Template
                    {
                        CreateTime = DateTime.Now,
                        LastUpdatedTime = DateTime.Now
                    };
                    dbModel.Id = (int)_dbContext.Insert<DB_Template>(dbModel).ExecuteIdentity();
                }

                dbModel.Name = model.Name;
                dbModel.TemplateStr = model.TemplateStr;
                dbModel.Type = model.Type;
                dbModel.LastUpdatedTime = DateTime.Now;
                dbModel.LinkedSpiderId = model.LinkedSpiderId;
                _dbContext.Update<DB_Template>(dbModel).Where(x => x.Id == dbModel.Id).ExecuteAffrows();

                _dbContext.Delete<DB_ReplacementRule>().Where(x => x.TemplateId == dbModel.Id).ExecuteAffrows();
                var data = model.ReplacementRules.Select(x => new DB_ReplacementRule
                {
                    TemplateId = dbModel.Id,
                    ReplacementNewlyStr = x.ReplacementNewlyStr,
                    ReplacementOldStr = x.ReplacementOldStr,
                    IgnoreCase = x.IgnoreCase
                }).ToList();
                _dbContext.Insert<DB_ReplacementRule>(data).ExecuteAffrows();
            });
            return StatusMessage.Success;
        }

        public async Task<string> SubmitAsync(TemplateEditDto model)
        {
            return await Task.FromResult(Submit(model));
        }
    }
}
