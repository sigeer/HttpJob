using Microsoft.EntityFrameworkCore;
using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
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

        public string Delete(TemplateDto model)
        {
            var dbModel = new DB_Template() { Id = model.Id };
            _dbContext.Templates.Attach(dbModel).State = EntityState.Deleted;
            _dbContext.SaveChanges();
            _dbContext.TemplateReplacementRules.RemoveRange(_dbContext.TemplateReplacementRules.Where(x => x.TemplateId == model.Id));
            _dbContext.SaveChanges();
            return StatusMessage.Success;
        }

        public List<TemplateDto> GetTemplateDtoList()
        {
            var templateRules = from a in _dbContext.TemplateReplacementRules
                                join b in _dbContext.ReplacementRules on a.RuleId equals b.Id
                                select new
                                {
                                    a.TemplateId,
                                    b
                                };

            return (from a in _dbContext.Templates
                    let b = templateRules.Where(x => x.TemplateId == a.Id).ToList()
                    select new TemplateDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        TemplateStr = a.TemplateStr,
                        LinkedSpiderId = a.LinkedSpiderId,
                        Type = a.Type,
                        ReplacementRules = b.Select(x => new ReplacementRuleDto
                        {
                            Id = x.b.Id,
                            ReplacementOldStr = x.b.ReplacementOldStr,
                            ReplacementNewlyStr = x.b.ReplacementNewlyStr
                        }).ToList()
                    }).AsSplitQuery().ToList();
        }

        public async Task<List<TemplateDto>> GetTemplateDtoListAsync()
        {
            var templateRules = from a in _dbContext.TemplateReplacementRules
                                join b in _dbContext.ReplacementRules on a.RuleId equals b.Id
                                select new
                                {
                                    a.TemplateId,
                                    b
                                };

            return await (from a in _dbContext.Templates
                          let b = templateRules.Where(x => x.TemplateId == a.Id).ToList()
                          select new TemplateDto
                          {
                              Id = a.Id,
                              Name = a.Name,
                              TemplateStr = a.TemplateStr,
                              LinkedSpiderId = a.LinkedSpiderId,
                              Type = a.Type,
                              ReplacementRules = b.Select(x => new ReplacementRuleDto
                              {
                                  Id = x.b.Id,
                                  ReplacementOldStr = x.b.ReplacementOldStr,
                                  ReplacementNewlyStr = x.b.ReplacementNewlyStr
                              }).ToList()
                          }).AsSplitQuery().ToListAsync();
        }

        public string Submit(TemplateDto model)
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

            _dbContext.TemplateReplacementRules.RemoveRange(_dbContext.TemplateReplacementRules.Where(x => x.TemplateId == dbModel.Id));
            _dbContext.TemplateReplacementRules.AddRange(model.ReplacementRules.Select(x => new DB_TemplateReplacementRule
            {
                RuleId = x.Id,
                TemplateId = dbModel.Id
            }));
            _dbContext.SaveChanges();
            dbTrans.Commit();

            return StatusMessage.Success;
        }
    }
}
