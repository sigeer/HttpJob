using Microsoft.EntityFrameworkCore;
using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
using SpiderTool.EntityFrameworkCore.ContextModel;
using SpiderTool.IDomain;

namespace SpiderTool.EntityFrameworkCore.Domain
{
    public class SpiderDomain : ISpiderDomain
    {
        readonly SpiderDbContext _dbContext;

        public SpiderDomain(SpiderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string Delete(SpiderDtoSetter model)
        {
            var dbModel = _dbContext.Spiders.FirstOrDefault(x => x.Id == model.Id);
            if (dbModel == null)
                return StatusMessage.Error;

            _dbContext.Spiders.Remove(dbModel);
            _dbContext.RemoveRange(_dbContext.SpiderTemplates.Where(x => x.SpiderId == dbModel.Id));
            _dbContext.SaveChanges();
            return StatusMessage.Success;
        }

        public SpiderDto? GetSpiderDto(int id)
        {
            var spiderTemplateInfo = from a in _dbContext.SpiderTemplates
                                     join b in _dbContext.Templates on a.TemplateId equals b.Id
                                     select new
                                     {
                                         a.SpiderId,
                                         TemplateInfo = b
                                     };

            var data = (from a in _dbContext.Spiders.Where(x => x.Id == id)
                        join b in _dbContext.Templates on a.NextPageTemplateId equals b.Id into bss
                        from bs in bss.DefaultIfEmpty()
                        let c = spiderTemplateInfo.Where(x => x.SpiderId == a.Id).ToList()
                        select new
                        {
                            spider = a,
                            nextPage = bs,
                            templates = c
                        }).AsSplitQuery().ToList();

            return data.Select(x => new SpiderDto
            {
                Id = x.spider.Id,
                Description = x.spider.Description,
                Name = x.spider.Name,
                Headers = x.spider.Headers,
                Method = x.spider.Method,
                NextPageTemplateId = x.spider.NextPageTemplateId,
                PostObjStr = x.spider.PostObjStr,
                Templates = x.templates.Select(y => y.TemplateInfo.Id).ToList(),
                TemplateList = x.templates.Select(y => new TemplateDto
                {
                    Id = y.TemplateInfo.Id,
                    Name = y.TemplateInfo.Name,
                    TemplateStr = y.TemplateInfo.TemplateStr,
                    Type = y.TemplateInfo.Type,
                }).ToList(),
                NextPageTemplate = x.nextPage == null ? null : new TemplateDto
                {
                    Id = x.nextPage.Id,
                    Name = x.nextPage.Name,
                    TemplateStr = x.nextPage.TemplateStr,
                    Type = x.nextPage.Type
                },
            }).FirstOrDefault();
        }

        public List<SpiderDtoSetter> GetSpiderDtoList()
        {
            return (from x in _dbContext.Spiders
                    let b = _dbContext.SpiderTemplates.Where(a => a.SpiderId == x.Id).ToList()
                    select new SpiderDtoSetter
                    {
                        Description = x.Description,
                        Headers = x.Headers,
                        Method = x.Method,
                        Name = x.Name,
                        Id = x.Id,
                        NextPageTemplateId = x.NextPageTemplateId,
                        PostObjStr = x.PostObjStr,
                        Templates = b.Select(y => y.TemplateId).ToList()
                    }).AsSplitQuery().ToList();
        }

        public List<SpiderTemplateDto> GetSpiderTemplatesDto(int spiderId)
        {
            var templates = from a in _dbContext.SpiderTemplates.Where(x => x.SpiderId == spiderId)
                            join b in _dbContext.Templates on a.TemplateId equals b.Id
                            select b;

            return new List<SpiderTemplateDto>();
        }

        public string Submit(SpiderDtoSetter model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            using var dbTrans = _dbContext.Database.BeginTransaction();
            var dbModel = _dbContext.Spiders.FirstOrDefault(x => x.Id == model.Id);
            if (dbModel == null)
            {
                dbModel = new DB_Spider
                {
                    CreateTime = DateTime.Now
                };
                _dbContext.Spiders.Add(dbModel);
            }

            dbModel.Name = model.Name;
            dbModel.Method = model.Method;
            dbModel.Description = model.Description;
            dbModel.Headers = model.Headers;
            dbModel.PostObjStr = model.PostObjStr;
            dbModel.NextPageTemplateId = model.NextPageTemplateId;
            dbModel.LastUpdatedTime = DateTime.Now;

            _dbContext.SaveChanges();

            _dbContext.SpiderTemplates.RemoveRange(_dbContext.SpiderTemplates.Where(x => x.SpiderId == dbModel.Id));
            _dbContext.SpiderTemplates.AddRange(model.Templates.Select(x => new DB_SpiderTemplate { SpiderId = dbModel.Id, TemplateId = x }));
            _dbContext.SaveChanges();
            dbTrans.Commit();
            return StatusMessage.Success;
        }
    }
}
