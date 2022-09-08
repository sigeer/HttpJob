using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Resource;
using SpiderTool.IDomain;
using SqlSugar;

namespace SpiderTool.SqlSugar.Domain
{
    public class ResourceDomain : IResourceDomain
    {
        readonly ISqlSugarClient _dbContext;

        public ResourceDomain(ISqlSugarClient dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ResourceDto> GetResourceDtoList()
        {
            return _dbContext.Queryable<DB_Resource>().Select(x => new ResourceDto()
            {
                Id = x.Id,
                Name = x.Name,
                Url = x.Url
            }).ToList();
        }

        public string Submit(ResourceSetter model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            try
            {
                _dbContext.Ado.BeginTran();

                var dbModel = _dbContext.Queryable<DB_Resource>().InSingle(model.Id);
                if (dbModel == null)
                {
                    dbModel = new DB_Resource
                    {
                        CreateTime = DateTime.Now,
                        LastUpdatedTime = DateTime.Now
                    };
                    dbModel.Id = _dbContext.Insertable<DB_Resource>(dbModel).ExecuteReturnIdentity();
                }

                dbModel.Description = model.Description;
                dbModel.Name = model.Name;
                dbModel.Url = model.Url;
                dbModel.LastUpdatedTime = DateTime.Now;

                _dbContext.Updateable<DB_Resource>(dbModel).ExecuteCommand();
                _dbContext.Ado.CommitTran();

                return StatusMessage.Success;
            }
            catch (Exception ex)
            {
                _dbContext.Ado.RollbackTran();
                return ex.Message;
            }
        }
        public string Delete(ResourceSetter model)
        {
            _dbContext.Deleteable<DB_Resource>(x => x.Id == model.Id).ExecuteCommand();
            return StatusMessage.Success;
        }

    }
}
