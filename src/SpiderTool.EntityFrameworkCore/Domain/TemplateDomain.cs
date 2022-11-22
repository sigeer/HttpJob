using Microsoft.EntityFrameworkCore;
using SpiderTool.Data.Constants;
using SpiderTool.Data.DataBase;
using SpiderTool.Data.Dto.Spider;
using SpiderTool.EntityFrameworkCore.ContextModel;
using SpiderTool.IDomain;

namespace SpiderTool.EntityFrameworkCore.Domain
{
    public class TemplateDomain : ITemplateDomain
    {
        readonly SpiderDbContext _dbContext;

        public TemplateDomain(SpiderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string Delete(TemplateEditDto model)
        {
            _dbContext.Templates.Where(x => x.Id == model.Id).ExecuteDelete();
            return StatusMessage.Success;
        }

        public async Task<string> DeleteAsync(TemplateEditDto model)
        {
            await _dbContext.Templates.Where(x => x.Id == model.Id).ExecuteDeleteAsync();
            return StatusMessage.Success;
        }

        public List<TemplateDetailViewModel> GetTemplateDtoList()
        {
            return (from a in _dbContext.Templates
                    let b = _dbContext.ReplacementRules.Where(x => x.TemplateId == a.Id).ToList()
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
                    }).AsSplitQuery().ToList();
        }

        public async Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync()
        {
            return await (from a in _dbContext.Templates
                          let b = _dbContext.ReplacementRules.Where(x => x.TemplateId == a.Id).ToList()
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
                          }).AsSplitQuery().ToListAsync();
        }

        public string Submit(TemplateEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            using var dbTrans = _dbContext.Database.BeginTransaction();

            var dbModel = _dbContext.Templates.FirstOrDefault(x => x.Id == model.Id);
            if (dbModel == null)
            {
                dbModel = new DB_Template
                {
                    CreateTime = DateTime.Now
                };
                _dbContext.Templates.Add(dbModel);
            }

            dbModel.Name = model.Name;
            dbModel.TemplateStr = model.TemplateStr;
            dbModel.Type = model.Type;
            dbModel.LastUpdatedTime = DateTime.Now;
            dbModel.LinkedSpiderId = model.LinkedSpiderId;
            _dbContext.SaveChanges();

            _dbContext.ReplacementRules.RemoveRange(_dbContext.ReplacementRules.Where(x => x.TemplateId == dbModel.Id));
            _dbContext.ReplacementRules.AddRange(model.ReplacementRules.Select(x => new DB_ReplacementRule
            {
                TemplateId = dbModel.Id,
                ReplacementNewlyStr = x.ReplacementNewlyStr,
                ReplacementOldStr = x.ReplacementOldStr,
                IgnoreCase = x.IgnoreCase
            }));
            _dbContext.SaveChanges();
            dbTrans.Commit();

            return StatusMessage.Success;
        }

        public async Task<string> SubmitAsync(TemplateEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            using var dbTrans = await _dbContext.Database.BeginTransactionAsync();

            var dbModel = await _dbContext.Templates.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (dbModel == null)
            {
                dbModel = new DB_Template
                {
                    CreateTime = DateTime.Now
                };
                _dbContext.Templates.Add(dbModel);
            }

            dbModel.Name = model.Name;
            dbModel.TemplateStr = model.TemplateStr;
            dbModel.Type = model.Type;
            dbModel.LastUpdatedTime = DateTime.Now;
            dbModel.LinkedSpiderId = model.LinkedSpiderId;
            await _dbContext.SaveChangesAsync();

            _dbContext.ReplacementRules.RemoveRange(_dbContext.ReplacementRules.Where(x => x.TemplateId == dbModel.Id));
            _dbContext.ReplacementRules.AddRange(model.ReplacementRules.Select(x => new DB_ReplacementRule
            {
                TemplateId = dbModel.Id,
                ReplacementNewlyStr = x.ReplacementNewlyStr,
                ReplacementOldStr = x.ReplacementOldStr,
                IgnoreCase = x.IgnoreCase
            }));
            await _dbContext.SaveChangesAsync();
            await dbTrans.CommitAsync();

            return StatusMessage.Success;
        }
    }
}
