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
            var data = _dbContext.Queryable<DB_Spider>()
               .LeftJoin<DB_Template>((a, b) => a.NextPageTemplateId == b.Id)
               .Select((a, b) => new SpiderDto()
               {
                   Id = a.Id,
                   Description = a.Description,
                   Name = a.Name,
                   Method = a.Method,
                   PostObjStr = a.PostObjStr,
                   Headers = a.Headers,
                   NextPageTemplateId = a.NextPageTemplateId,
                   NextPageTemplate = b == null ? new TemplateDto() : new TemplateDto
                   {
                       Id = b.Id,
                       LinkedSpiderId = b.LinkedSpiderId,
                       Name = b.Name,
                       TemplateStr = b.TemplateStr,
                       Type = b.Type
                   }
               }).InSingle(id);

            if (data != null)
            {
                var templateIdList = _dbContext.Queryable<DB_SpiderTemplate>().Where(x => x.SpiderId == id).Select(x => x.Id).ToList();
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

            return null;
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
            var data = (from x in _dbContext.Queryable<DB_Spider>()
                        let b = _dbContext.Queryable<DB_SpiderTemplate>().Where(a => a.SpiderId == x.Id).ToList()
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
            var sql = data.ToSql();
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
            var dbModel = _dbContext.Queryable<DB_Spider>().InSingle(model.Id);
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
            _dbContext.Updateable<DB_Spider>(dbModel).ExecuteCommand();

            _dbContext.Deleteable<DB_SpiderTemplate>(x => x.SpiderId == dbModel.Id).ExecuteCommand();
            _dbContext.Insertable<DB_SpiderTemplate>(model.Templates.Select(x => new DB_SpiderTemplate { SpiderId = dbModel.Id, TemplateId = x })).ExecuteCommand();

            _dbContext.Ado.CommitTran();
            return StatusMessage.Success;
        }
    }
}
