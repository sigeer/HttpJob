using Microsoft.EntityFrameworkCore;
using SpiderTool.Data.Constants;
using SpiderTool.Data.DataBase;
using SpiderTool.Data.Dto.Spider;
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

        public string Delete(SpiderEditDto model)
        {
            _dbContext.SpiderTemplates.Where(x => x.SpiderId == model.Id).ExecuteDelete();
            _dbContext.Spiders.Where(x => x.Id == model.Id).ExecuteDelete();

            return StatusMessage.Success;
        }

        public async Task<string> DeleteAsync(SpiderEditDto model)
        {
            await _dbContext.SpiderTemplates.Where(x => x.SpiderId == model.Id).ExecuteDeleteAsync();
            await _dbContext.Spiders.Where(x => x.Id == model.Id).ExecuteDeleteAsync();
            return StatusMessage.Success;
        }
        public SpiderDetailViewModel? GetSpiderDto(int id)
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

            return data.Select(x => new SpiderDetailViewModel
            {
                Id = x.spider.Id,
                Description = x.spider.Description,
                Name = x.spider.Name,
                HeaderStr = x.spider.Headers,
                Method = x.spider.Method,
                PostObjStr = x.spider.PostObjStr,
                TemplateList = x.templates.Select(y => new TemplateDetailViewModel(y.TemplateInfo)).ToList(),
                NextPageTemplate = x.nextPage == null ? null : new TemplateDetailViewModel(x.nextPage,isNextPage: true),
            }).FirstOrDefault();
        }

        public async Task<SpiderDetailViewModel?> GetSpiderDtoAsync(int id)
        {
            var spiderTemplateInfo = from a in _dbContext.SpiderTemplates
                                     join b in _dbContext.Templates on a.TemplateId equals b.Id
                                     select new
                                     {
                                         a.SpiderId,
                                         TemplateInfo = b
                                     };

            var data = await (from a in _dbContext.Spiders.Where(x => x.Id == id)
                              join b in _dbContext.Templates on a.NextPageTemplateId equals b.Id into bss
                              from bs in bss.DefaultIfEmpty()
                              let c = spiderTemplateInfo.Where(x => x.SpiderId == a.Id).ToList()
                              select new
                              {
                                  spider = a,
                                  nextPage = bs,
                                  templates = c
                              }).AsSplitQuery().ToListAsync();

            return data.Select(x => new SpiderDetailViewModel
            {
                Id = x.spider.Id,
                Description = x.spider.Description,
                Name = x.spider.Name,
                HeaderStr = x.spider.Headers,
                Method = x.spider.Method,
                PostObjStr = x.spider.PostObjStr,
                TemplateList = x.templates.Select(y => new TemplateDetailViewModel(y.TemplateInfo)).ToList(),
                NextPageTemplate = x.nextPage == null ? null : new TemplateDetailViewModel(x.nextPage, isNextPage: true),
            }).FirstOrDefault();
        }

        public List<SpiderListItemViewModel> GetSpiderDtoList()
        {
            return _dbContext.Spiders.Select(x => new SpiderListItemViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public async Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync()
        {
            return await _dbContext.Spiders.Select(x => new SpiderListItemViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
        }

        public string Submit(SpiderEditDto model)
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

        public async Task<string> SubmitAsync(SpiderEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            using var dbTrans = await _dbContext.Database.BeginTransactionAsync();
            var dbModel = await _dbContext.Spiders.FirstOrDefaultAsync(x => x.Id == model.Id);
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

            await _dbContext.SaveChangesAsync();

            _dbContext.SpiderTemplates.RemoveRange(_dbContext.SpiderTemplates.Where(x => x.SpiderId == dbModel.Id));
            _dbContext.SpiderTemplates.AddRange(model.Templates.Select(x => new DB_SpiderTemplate { SpiderId = dbModel.Id, TemplateId = x }));
            await _dbContext.SaveChangesAsync();
            dbTrans.Commit();
            return StatusMessage.Success;
        }
    }
}
