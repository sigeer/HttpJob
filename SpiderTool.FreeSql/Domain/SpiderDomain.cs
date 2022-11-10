using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
using SpiderTool.IDomain;
using FreeSql;

namespace SpiderTool.FreeSql.Domain
{
    public class SpiderDomain : ISpiderDomain
    {
        readonly IFreeSql _dbContext;

        public SpiderDomain(IFreeSql dbContext)
        {
            _dbContext = dbContext;
        }

        public string Delete(SpiderEditDto model)
        {
            _dbContext.Ado.Transaction(() =>
            {
                _dbContext.Delete<DB_SpiderTemplate>(new { SpiderId = model.Id });
                _dbContext.Delete<DB_Spider>(model.Id);
            });
            return StatusMessage.Success;
        }

        public async Task<string> DeleteAsync(SpiderEditDto model)
        {
            return await Task.FromResult(Delete(model));
        }

        public SpiderDetailViewModel? GetSpiderDto(int id)
        {
            var dbModel = _dbContext.Select<DB_Spider>().Where(x => x.Id == id).First();
            if (dbModel == null)
                return null;
            var nextPage = _dbContext.Select<DB_Template>().Where(x => x.Id == dbModel.NextPageTemplateId).First();

            var data = new SpiderDetailViewModel()
            {
                Id = dbModel.Id,
                Description = dbModel.Description,
                Name = dbModel.Name,
                Method = dbModel.Method,
                PostObjStr = dbModel.PostObjStr,
                HeaderStr = dbModel.Headers,
                NextPageTemplate = nextPage == null ? new TemplateDetailViewModel() : new TemplateDetailViewModel
                {
                    Id = nextPage.Id,
                    LinkedSpiderId = nextPage.LinkedSpiderId,
                    Name = nextPage.Name,
                    TemplateStr = nextPage.TemplateStr,
                    Type = nextPage.Type
                }
            };

            var templateIdList = _dbContext.Select<DB_SpiderTemplate>().Where(x => x.SpiderId == id).ToList(x => x.TemplateId);
            var templateReplaceList = _dbContext.Select<DB_ReplacementRule>().Where(x => templateIdList.Contains(x.TemplateId)).ToList();
            var templateList = _dbContext.Select<DB_Template>().Where(x => templateIdList.Contains(x.Id)).ToList().Select(b => new TemplateDetailViewModel()
            {
                Id = b.Id,
                LinkedSpiderId = b.LinkedSpiderId,
                Name = b.Name,
                TemplateStr = b.TemplateStr,
                Type = b.Type,
                ReplacementRules = templateReplaceList.Where(x => x.TemplateId == b.Id).Select(x => new ReplacementRuleDto()
                {
                    Id = x.Id,
                    ReplacementNewlyStr = x.ReplacementNewlyStr,
                    ReplacementOldStr = x.ReplacementOldStr,
                }).ToList()
            }).ToList();

            data.TemplateList = templateList;
            return data;
        }

        public async Task<SpiderDetailViewModel?> GetSpiderDtoAsync(int id)
        {
            var dbModel = await _dbContext.Select<DB_Spider>().Where(x => x.Id == id).FirstAsync();
            if (dbModel == null)
                return null;

            var nextPage = await _dbContext.Select<DB_Template>().Where(x => x.Id == dbModel.NextPageTemplateId).FirstAsync();

            var data = new SpiderDetailViewModel()
            {
                Id = dbModel.Id,
                Description = dbModel.Description,
                Name = dbModel.Name,
                Method = dbModel.Method,
                PostObjStr = dbModel.PostObjStr,
                HeaderStr = dbModel.Headers,
                NextPageTemplate = nextPage == null ? new TemplateDetailViewModel() : new TemplateDetailViewModel
                {
                    Id = nextPage.Id,
                    LinkedSpiderId = nextPage.LinkedSpiderId,
                    Name = nextPage.Name,
                    TemplateStr = nextPage.TemplateStr,
                    Type = nextPage.Type
                }
            };

            var templateIdList = await _dbContext.Select<DB_SpiderTemplate>().Where(x => x.SpiderId == id).ToListAsync(x => x.TemplateId);
            var templateReplaceList = await _dbContext.Select<DB_ReplacementRule>().Where(x => templateIdList.Contains(x.TemplateId)).ToListAsync();
            var templateList = (await _dbContext.Select<DB_Template>().Where(x => templateIdList.Contains(x.Id)).ToListAsync()).Select(b => new TemplateDetailViewModel()
            {
                Id = b.Id,
                LinkedSpiderId = b.LinkedSpiderId,
                Name = b.Name,
                TemplateStr = b.TemplateStr,
                Type = b.Type,
                ReplacementRules = templateReplaceList.Where(x => x.TemplateId == b.Id).Select(x => new ReplacementRuleDto()
                {
                    Id = x.Id,
                    ReplacementNewlyStr = x.ReplacementNewlyStr,
                    ReplacementOldStr = x.ReplacementOldStr,
                }).ToList()
            }).ToList();

            data.TemplateList = templateList;
            return data;
        }
        public List<SpiderListItemViewModel> GetSpiderDtoList()
        {
            return _dbContext.Select<DB_Spider>().ToList(x => new SpiderListItemViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public async Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync()
        {
            return await _dbContext.Select<DB_Spider>().ToListAsync(x => new SpiderListItemViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public string Submit(SpiderEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            _dbContext.Transaction(() =>
            {
                var dbModel = _dbContext.Select<DB_Spider>().Where(x => x.Id == model.Id).First();
                if (dbModel == null)
                {
                    dbModel = new DB_Spider
                    {
                        CreateTime = DateTime.Now,
                        LastUpdatedTime = DateTime.Now
                    };
                    dbModel.Id = (int)_dbContext.Insert<DB_Spider>(dbModel).ExecuteIdentity();
                }

                dbModel.Name = model.Name;
                dbModel.Method = model.Method;
                dbModel.Description = model.Description;
                dbModel.Headers = model.Headers;
                dbModel.PostObjStr = model.PostObjStr;
                dbModel.NextPageTemplateId = model.NextPageTemplateId;
                dbModel.LastUpdatedTime = DateTime.Now;
                _dbContext.Update<DB_Spider>(dbModel).Where(x => x.Id == dbModel.Id).ExecuteAffrows();

                _dbContext.Delete<DB_SpiderTemplate>().Where(x => x.SpiderId == dbModel.Id).ExecuteAffrows();
                var data = model.Templates.Select(x => new DB_SpiderTemplate { SpiderId = dbModel.Id, TemplateId = x }).ToList();
                _dbContext.Insert<DB_SpiderTemplate>(data).ExecuteAffrows();
            });
            return StatusMessage.Success;
        }

        public async Task<string> SubmitAsync(SpiderEditDto model)
        {
            return await Task.FromResult(Submit(model));
        }
    }
}
