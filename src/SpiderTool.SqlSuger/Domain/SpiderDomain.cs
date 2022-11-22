using SpiderTool.Data.Constants;
using SpiderTool.Data.DataBase;
using SpiderTool.Data.Dto.Spider;
using SpiderTool.IDomain;
using SqlSugar;

namespace SpiderTool.SqlSugar.Domain
{
    public class SpiderDomain : ISpiderDomain
    {
        readonly ISqlSugarClient _dbContext;

        public SpiderDomain(ISqlSugarClient dbContext)
        {
            _dbContext = dbContext;
        }

        public string Delete(SpiderEditDto model)
        {
            try
            {
                _dbContext.Ado.BeginTran();
                _dbContext.Deleteable<DB_SpiderTemplate>(x => x.SpiderId == model.Id).ExecuteCommand();
                _dbContext.Deleteable<DB_Spider>(x => x.Id == model.Id).ExecuteCommand();
                _dbContext.Ado.CommitTran();
                return StatusMessage.Success;
            }
            catch (Exception ex)
            {
                _dbContext.Ado.RollbackTran();
                return ex.Message;
            }
        }

        public async Task<string> DeleteAsync(SpiderEditDto model)
        {
            try
            {
                _dbContext.Ado.BeginTran();
                await _dbContext.Deleteable<DB_SpiderTemplate>(x => x.SpiderId == model.Id).ExecuteCommandAsync();
                await _dbContext.Deleteable<DB_Spider>(x => x.Id == model.Id).ExecuteCommandAsync();
                _dbContext.Ado.CommitTran();
                return StatusMessage.Success;
            }
            catch (Exception ex)
            {
                _dbContext.Ado.RollbackTran();
                return ex.Message;
            }
        }

        public SpiderDetailViewModel? GetSpiderDto(int id)
        {
            var dbModel = _dbContext.Queryable<DB_Spider>().First(x => x.Id == id);
            if (dbModel == null)
                return null;
            var nextPage = _dbContext.Queryable<DB_Template>().First(x => x.Id == dbModel.NextPageTemplateId);

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

            var templateIdList = _dbContext.Queryable<DB_SpiderTemplate>().Where(x => x.SpiderId == id).Select(x => x.TemplateId).ToList();
            var templateReplaceList = _dbContext.Queryable<DB_ReplacementRule>().Where(x => templateIdList.Contains(x.TemplateId)).ToList();
            var templateList = _dbContext.Queryable<DB_Template>().Where(x => templateIdList.Contains(x.Id)).ToList().Select(b => new TemplateDetailViewModel()
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
            var dbModel = await _dbContext.Queryable<DB_Spider>().FirstAsync(x => x.Id == id);
            if (dbModel == null)
                return null;

            var nextPage = await _dbContext.Queryable<DB_Template>().FirstAsync(x => x.Id == dbModel.NextPageTemplateId);

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

            var templateIdList = await _dbContext.Queryable<DB_SpiderTemplate>().Where(x => x.SpiderId == id).Select(x => x.TemplateId).ToListAsync();
            var templateReplaceList = await _dbContext.Queryable<DB_ReplacementRule>().Where(x => templateIdList.Contains(x.TemplateId)).ToListAsync();
            var templateList = (await _dbContext.Queryable<DB_Template>().Where(x => templateIdList.Contains(x.Id)).ToListAsync()).Select(b => new TemplateDetailViewModel()
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
            return _dbContext.Queryable<DB_Spider>().Select(x => new SpiderListItemViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public async Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync()
        {
            return await _dbContext.Queryable<DB_Spider>().Select(x => new SpiderListItemViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
        }

        public string Submit(SpiderEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            _dbContext.Ado.BeginTran();
            var dbModel = _dbContext.Queryable<DB_Spider>().First(x => x.Id == model.Id);
            if (dbModel == null)
            {
                dbModel = new DB_Spider
                {
                    CreateTime = DateTime.Now,
                    LastUpdatedTime = DateTime.Now
                };
                dbModel.Id = _dbContext.Insertable<DB_Spider>(dbModel).ExecuteReturnIdentity();
            }

            dbModel.Name = model.Name;
            dbModel.Method = model.Method;
            dbModel.Description = model.Description;
            dbModel.Headers = model.Headers;
            dbModel.PostObjStr = model.PostObjStr;
            dbModel.NextPageTemplateId = model.NextPageTemplateId;
            dbModel.LastUpdatedTime = DateTime.Now;
            _dbContext.Updateable<DB_Spider>(dbModel).Where(x => x.Id == dbModel.Id).ExecuteCommand();

            _dbContext.Deleteable<DB_SpiderTemplate>(x => x.SpiderId == dbModel.Id).ExecuteCommand();
            var data = model.Templates.Select(x => new DB_SpiderTemplate { SpiderId = dbModel.Id, TemplateId = x }).ToList();
            _dbContext.Insertable<DB_SpiderTemplate>(data).ExecuteCommand();

            _dbContext.Ado.CommitTran();
            return StatusMessage.Success;
        }

        public async Task<string> SubmitAsync(SpiderEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            _dbContext.Ado.BeginTran();
            var dbModel = await _dbContext.Queryable<DB_Spider>().FirstAsync(x => x.Id == model.Id);
            if (dbModel == null)
            {
                dbModel = new DB_Spider
                {
                    CreateTime = DateTime.Now,
                    LastUpdatedTime = DateTime.Now
                };
                dbModel.Id = await _dbContext.Insertable<DB_Spider>(dbModel).ExecuteReturnIdentityAsync();
            }

            dbModel.Name = model.Name;
            dbModel.Method = model.Method;
            dbModel.Description = model.Description;
            dbModel.Headers = model.Headers;
            dbModel.PostObjStr = model.PostObjStr;
            dbModel.NextPageTemplateId = model.NextPageTemplateId;
            dbModel.LastUpdatedTime = DateTime.Now;
            await _dbContext.Updateable<DB_Spider>(dbModel).Where(x => x.Id == dbModel.Id).ExecuteCommandAsync();

            await _dbContext.Deleteable<DB_SpiderTemplate>(x => x.SpiderId == dbModel.Id).ExecuteCommandAsync();
            var data = model.Templates.Select(x => new DB_SpiderTemplate { SpiderId = dbModel.Id, TemplateId = x }).ToList();
            await _dbContext.Insertable<DB_SpiderTemplate>(data).ExecuteCommandAsync();

            _dbContext.Ado.CommitTran();
            return StatusMessage.Success;
        }
    }
}
