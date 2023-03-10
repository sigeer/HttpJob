using FreeSql;
using SpiderTool.Data.Constants;
using SpiderTool.Data.DataBase;
using SpiderTool.Data.Dto.Spider;
using SpiderTool.IDomain;

namespace SpiderTool.FreeSql.Domain
{
    public class SpiderDomain : ISpiderDomain
    {
        readonly IFreeSql _freeSql;

        public SpiderDomain(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }

        public string Delete(SpiderEditDto model)
        {
            _freeSql.Ado.Transaction(() =>
            {
                _freeSql.Delete<DB_SpiderTemplate>(new { SpiderId = model.Id });
                _freeSql.Delete<DB_Spider>(model.Id);
            });
            return StatusMessage.Success;
        }

        public async Task<string> DeleteAsync(SpiderEditDto model)
        {
            return await Task.FromResult(Delete(model));
        }

        public SpiderDetailViewModel? GetSpiderDto(int id)
        {
            var dbModel = _freeSql.Select<DB_Spider>().Where(x => x.Id == id).ToOne();
            if (dbModel == null)
                return null;
            var nextPage = _freeSql.Select<DB_Template>().Where(x => x.Id == dbModel.NextPageTemplateId).ToOne();

            var data = new SpiderDetailViewModel()
            {
                Id = dbModel.Id,
                Description = dbModel.Description,
                Name = dbModel.Name,
                Method = dbModel.Method,
                PostObjStr = dbModel.PostObjStr,
                HeaderStr = dbModel.Headers,
                NextPageTemplate = nextPage == null ? new TemplateDetailViewModel() : new TemplateDetailViewModel(nextPage, isNextPage: true)
            };

            var templateIdList = _freeSql.Select<DB_SpiderTemplate>().Where(x => x.SpiderId == id).ToList(x => x.TemplateId);
            var templateReplaceList = _freeSql.Select<DB_ReplacementRule>().Where(x => templateIdList.Contains(x.TemplateId)).ToList();
            var templateList = _freeSql.Select<DB_Template>().Where(x => templateIdList.Contains(x.Id)).ToList().Select(b => new TemplateDetailViewModel(b, templateReplaceList)).ToList();

            data.TemplateList = templateList;
            return data;
        }

        public async Task<SpiderDetailViewModel?> GetSpiderDtoAsync(int id)
        {
            var dbModel = await _freeSql.Select<DB_Spider>().Where(x => x.Id == id).ToOneAsync();
            if (dbModel == null)
                return null;

            var nextPage = await _freeSql.Select<DB_Template>().Where(x => x.Id == dbModel.NextPageTemplateId).ToOneAsync();

            var data = new SpiderDetailViewModel()
            {
                Id = dbModel.Id,
                Description = dbModel.Description,
                Name = dbModel.Name,
                Method = dbModel.Method,
                PostObjStr = dbModel.PostObjStr,
                HeaderStr = dbModel.Headers,
                NextPageTemplate = nextPage == null ? new TemplateDetailViewModel() : new TemplateDetailViewModel(nextPage, isNextPage: true)
            };

            var templateIdList = await _freeSql.Select<DB_SpiderTemplate>().Where(x => x.SpiderId == id).ToListAsync(x => x.TemplateId);
            var templateReplaceList = await _freeSql.Select<DB_ReplacementRule>().Where(x => templateIdList.Contains(x.TemplateId)).ToListAsync();
            var templateList = (await _freeSql.Select<DB_Template>().Where(x => templateIdList.Contains(x.Id)).ToListAsync()).Select(b => new TemplateDetailViewModel(b, templateReplaceList)).ToList();

            data.TemplateList = templateList;
            return data;
        }
        public List<SpiderListItemViewModel> GetSpiderDtoList()
        {
            return _freeSql.Select<DB_Spider>().ToList(x => new SpiderListItemViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public async Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync()
        {
            return await _freeSql.Select<DB_Spider>().ToListAsync(x => new SpiderListItemViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public string Submit(SpiderEditDto model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            _freeSql.Transaction(() =>
            {
                var dbModel = _freeSql.Select<DB_Spider>().Where(x => x.Id == model.Id).ToOne();
                if (dbModel == null)
                {
                    dbModel = new DB_Spider
                    {
                        CreateTime = DateTime.Now,
                        LastUpdatedTime = DateTime.Now
                    };
                    dbModel.Id = (int)_freeSql.Insert<DB_Spider>(dbModel).ExecuteIdentity();
                }

                dbModel.Name = model.Name;
                dbModel.Method = model.Method;
                dbModel.Description = model.Description;
                dbModel.Headers = model.Headers;
                dbModel.PostObjStr = model.PostObjStr;
                dbModel.NextPageTemplateId = model.NextPageTemplateId;
                dbModel.LastUpdatedTime = DateTime.Now;
                _freeSql.Update<DB_Spider>().SetSource(dbModel).IgnoreColumns(x => x.CreateTime).Where(x => x.Id == dbModel.Id).ExecuteAffrows();

                _freeSql.Delete<DB_SpiderTemplate>().Where(x => x.SpiderId == dbModel.Id).ExecuteAffrows();
                var data = model.Templates.Select(x => new DB_SpiderTemplate { SpiderId = dbModel.Id, TemplateId = x }).ToList();
                _freeSql.Insert<DB_SpiderTemplate>(data).ExecuteAffrows();
            });
            return StatusMessage.Success;
        }

        public async Task<string> SubmitAsync(SpiderEditDto model)
        {
            return await Task.FromResult(Submit(model));
        }
    }
}
