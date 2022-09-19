using Microsoft.EntityFrameworkCore;
using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Resource;
using SpiderTool.EntityFrameworkCore.ContextModel;
using SpiderTool.IDomain;

namespace SpiderTool.EntityFrameworkCore.Domain
{
    public class ResourceDomain : IResourceDomain
    {
        readonly SpiderDbContext _dbContext;

        public ResourceDomain(SpiderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ResourceHistoryDto> GetResourceDtoList()
        {
            return _dbContext.Resources.AsNoTracking().Select(x => new ResourceHistoryDto()
            {
                Id = x.Id,
                Name = x.Name,
                Url = x.Url
            }).ToList();
        }

        public string Submit(ResourceHistorySetter model)
        {
            if (!model.FormValid())
                return StatusMessage.FormInvalid;
            var dbModel = _dbContext.Resources.FirstOrDefault(x => x.Url == model.Url && x.SpiderId == model.SpiderId);
            if (dbModel == null)
            {
                dbModel = new DB_ResourceHistory
                {
                    CreateTime = DateTime.Now
                };
                _dbContext.Resources.Add(dbModel);
            }

            dbModel.Description = model.Description;
            dbModel.Name = model.Name;
            dbModel.Url = model.Url;
            dbModel.SpiderId = model.SpiderId;
            dbModel.LastUpdatedTime = DateTime.Now;
            _dbContext.SaveChanges();

            return StatusMessage.Success;
        }
        public string Delete(ResourceHistorySetter model)
        {
            var dbModel = new DB_ResourceHistory() { Id = model.Id };
            _dbContext.Resources.Attach(dbModel).State = EntityState.Deleted;
            _dbContext.SaveChanges();
            return StatusMessage.Success;
        }

    }
}
