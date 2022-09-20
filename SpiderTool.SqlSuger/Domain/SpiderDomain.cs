using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
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

        public string Delete(SpiderDtoSetter model)
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

        public SpiderDto? GetSpiderDto(int id)
        {
            var dbModel = _dbContext.Queryable<DB_Spider>().First(x => x.Id == id);
            if (dbModel == null)
                return null;
            var nextPage = _dbContext.Queryable<DB_Template>().First(x => x.Id == dbModel.NextPageTemplateId);

            var data = new SpiderDto()
            {
                Id = dbModel.Id,
                Description = dbModel.Description,
                Name = dbModel.Name,
                Method = dbModel.Method,
                PostObjStr = dbModel.PostObjStr,
                Headers = dbModel.Headers,
                NextPageTemplateId = dbModel.NextPageTemplateId,
                NextPageTemplate = nextPage == null ? new TemplateDto() : new TemplateDto
                {
                    Id = nextPage.Id,
                    LinkedSpiderId = nextPage.LinkedSpiderId,
                    Name = nextPage.Name,
                    TemplateStr = nextPage.TemplateStr,
                    Type = nextPage.Type
                }
            };

            var templateIdList = _dbContext.Queryable<DB_SpiderTemplate>().Where(x => x.SpiderId == id).Select(x => x.TemplateId).ToList();
            var templateList = _dbContext.Queryable<DB_Template>().Where(x => templateIdList.Contains(x.Id)).Select(b => new TemplateDto()
            {
                Id = b.Id,
                LinkedSpiderId = b.LinkedSpiderId,
                Name = b.Name,
                TemplateStr = b.TemplateStr,
                Type = b.Type
            }).ToList();

            data.TemplateList = templateList;
            return data;
        }

        public List<SpiderDtoSetter> GetSpiderDtoList()
        {
            //var data=  _dbContext.Queryable<DB_Spider>().InnerJoin<DB_SpiderTemplate>((a, b) => a.Id == b.SpiderId).Select(x => new SpiderDtoSetter
            //{
            //    Description = x.Description,
            //    Headers = x.Headers,
            //    Method = x.Method,
            //    Name = x.Name,
            //    Id = x.Id,
            //    NextPageTemplateId = x.NextPageTemplateId,
            //    PostObjStr = x.PostObjStr,
            //});
            var spiderList = _dbContext.Queryable<DB_Spider>().ToList();
            var spiderTemplate = _dbContext.Queryable<DB_SpiderTemplate>().ToList();
            var data = (from x in spiderList
                        let b = spiderTemplate
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
                        });
            return data.ToList();
        }

        public List<SpiderTemplateDto> GetSpiderTemplatesDto(int spiderId)
        {
            return new List<SpiderTemplateDto>();
        }

        public string Submit(SpiderDtoSetter model)
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
            _dbContext.Updateable<DB_Spider>(dbModel).Where(x => x.Id == model.Id).ExecuteCommand();

            _dbContext.Deleteable<DB_SpiderTemplate>(x => x.SpiderId == dbModel.Id).ExecuteCommand();
            var data = model.Templates.Select(x => new DB_SpiderTemplate { SpiderId = dbModel.Id, TemplateId = x }).ToList();
            _dbContext.Insertable<DB_SpiderTemplate>(data).ExecuteCommand();

            _dbContext.Ado.CommitTran();
            return StatusMessage.Success;
        }
    }
}
