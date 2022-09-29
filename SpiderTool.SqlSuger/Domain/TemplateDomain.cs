using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
using SpiderTool.IDomain;
using SqlSugar;

namespace SpiderTool.SqlSugar.Domain
{
    public class TemplateDomain : ITemplateDomain
    {
        readonly ISqlSugarClient _dbContext;

        public TemplateDomain(ISqlSugarClient dbContext)
        {
            _dbContext = dbContext;
        }

        public string Delete(TemplateEditDto model)
        {
            _dbContext.Ado.BeginTran();
            _dbContext.Deleteable<DB_ReplacementRule>(x => x.TemplateId == model.Id).ExecuteCommand();
            _dbContext.Deleteable<DB_Template>(x => x.Id == model.Id).ExecuteCommand();
            _dbContext.Ado.CommitTran();
            return StatusMessage.Success;
        }

        public async Task<string> DeleteAsync(TemplateEditDto model)
        {
            await _dbContext.Ado.UseTranAsync(async () =>
            {
                await _dbContext.Deleteable<DB_ReplacementRule>(x => x.TemplateId == model.Id).ExecuteCommandAsync();
                await _dbContext.Deleteable<DB_Template>(x => x.Id == model.Id).ExecuteCommandAsync();
            });
            return StatusMessage.Success;
        }

        public List<TemplateDetailViewModel> GetTemplateDtoList()
        {
            var allTemplates = _dbContext.Queryable<DB_Template>().ToList();

            var templateRules = _dbContext.Queryable<DB_ReplacementRule>().ToList();


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
                            ReplacementNewlyStr = x.ReplacementNewlyStr
                        }).ToList()
                    }).ToList();
        }

        public async Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync()
        {
            var allTemplates = await _dbContext.Queryable<DB_Template>().ToListAsync();

            var templateRules = await _dbContext.Queryable<DB_ReplacementRule>().ToListAsync();

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
                            ReplacementNewlyStr = x.ReplacementNewlyStr
                        }).ToList()
                    }).ToList();
        }

        public string Submit(TemplateEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            try
            {
                _dbContext.Ado.BeginTran();

                var dbModel = _dbContext.Queryable<DB_Template>().First(x => x.Id == model.Id);
                if (dbModel == null)
                {
                    dbModel = new DB_Template
                    {
                        CreateTime = DateTime.Now,
                        LastUpdatedTime = DateTime.Now
                    };
                    dbModel.Id = _dbContext.Insertable<DB_Template>(dbModel).ExecuteReturnIdentity();
                }

                dbModel.Name = model.Name;
                dbModel.TemplateStr = model.TemplateStr;
                dbModel.Type = model.Type;
                dbModel.LastUpdatedTime = DateTime.Now;
                dbModel.LinkedSpiderId = model.LinkedSpiderId;
                _dbContext.Updateable<DB_Template>(dbModel).Where(x => x.Id == dbModel.Id).ExecuteCommand();

                _dbContext.Deleteable<DB_ReplacementRule>(x => x.TemplateId == dbModel.Id).ExecuteCommand();
                var data = model.ReplacementRules.Select(x => new DB_ReplacementRule
                {
                    TemplateId = dbModel.Id,
                    ReplacementNewlyStr = x.ReplacementNewlyStr,
                    ReplacementOldStr = x.ReplacementOldStr
                }).ToList();
                _dbContext.Insertable<DB_ReplacementRule>(data).ExecuteCommand();

                _dbContext.Ado.CommitTran();

                return StatusMessage.Success;
            }
            catch (Exception ex)
            {
                _dbContext.Ado.RollbackTran();
                return ex.Message;
            }
        }

        public async Task<string> SubmitAsync(TemplateEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            try
            {
                _dbContext.Ado.BeginTran();

                var dbModel = await _dbContext.Queryable<DB_Template>().FirstAsync(x => x.Id == model.Id);
                if (dbModel == null)
                {
                    dbModel = new DB_Template
                    {
                        CreateTime = DateTime.Now,
                        LastUpdatedTime = DateTime.Now
                    };
                    dbModel.Id = await _dbContext.Insertable<DB_Template>(dbModel).ExecuteReturnIdentityAsync();
                }

                dbModel.Name = model.Name;
                dbModel.TemplateStr = model.TemplateStr;
                dbModel.Type = model.Type;
                dbModel.LastUpdatedTime = DateTime.Now;
                dbModel.LinkedSpiderId = model.LinkedSpiderId;
                await _dbContext.Updateable<DB_Template>(dbModel).Where(x => x.Id == dbModel.Id).ExecuteCommandAsync();

                await _dbContext.Deleteable<DB_ReplacementRule>(x => x.TemplateId == dbModel.Id).ExecuteCommandAsync();
                var data = model.ReplacementRules.Select(x => new DB_ReplacementRule
                {
                    TemplateId = dbModel.Id,
                    ReplacementNewlyStr = x.ReplacementNewlyStr,
                    ReplacementOldStr = x.ReplacementOldStr
                }).ToList();
                await _dbContext.Insertable<DB_ReplacementRule>(data).ExecuteCommandAsync();

                _dbContext.Ado.CommitTran();

                return StatusMessage.Success;
            }
            catch (Exception ex)
            {
                _dbContext.Ado.RollbackTran();
                return ex.Message;
            }
        }
    }
}
