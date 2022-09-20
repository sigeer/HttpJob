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

        public List<ResourceHistoryDto> GetResourceDtoList()
        {
            return _dbContext.Queryable<DB_ResourceHistory>().Select(x => new ResourceHistoryDto()
            {
                Id = x.Id,
                Name = x.Name,
                Url = x.Url
            }).OrderByDescending(x => x.LastUpdatedTime).ToList();
        }

        public async Task<List<ResourceHistoryDto>> GetResourceDtoListAsync()
        {
            return await _dbContext.Queryable<DB_ResourceHistory>().Select(x => new ResourceHistoryDto()
            {
                Id = x.Id,
                Name = x.Name,
                Url = x.Url
            }).OrderByDescending(x => x.LastUpdatedTime).ToListAsync();
        }

        public string Submit(ResourceHistorySetter model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;

            try
            {
                _dbContext.Ado.BeginTran();

                var dbModel = _dbContext.Queryable<DB_ResourceHistory>().First(x => x.Url == model.Url && x.SpiderId == model.SpiderId);
                if (dbModel == null)
                {
                    dbModel = new DB_ResourceHistory
                    {
                        CreateTime = DateTime.Now,
                        LastUpdatedTime = DateTime.Now
                    };
                    dbModel.Id = _dbContext.Insertable<DB_ResourceHistory>(dbModel).ExecuteReturnIdentity();
                }

                dbModel.Description = model.Description;
                dbModel.Name = model.Name;
                dbModel.Url = model.Url;
                dbModel.SpiderId = model.SpiderId;
                dbModel.LastUpdatedTime = DateTime.Now;

                _dbContext.Updateable<DB_ResourceHistory>(dbModel).Where(x => x.Id == dbModel.Id).ExecuteCommand();
                _dbContext.Ado.CommitTran();

                return StatusMessage.Success;
            }
            catch (Exception ex)
            {
                _dbContext.Ado.RollbackTran();
                return ex.Message;
            }
        }
        public string Delete(ResourceHistorySetter model)
        {
            _dbContext.Deleteable<DB_ResourceHistory>(x => x.Id == model.Id).ExecuteCommand();
            return StatusMessage.Success;
        }

    }
}
