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

        public List<ResourceDto> GetResourceDtoList()
        {
            return _dbContext.Resources.AsNoTracking().Select(x => new ResourceDto()
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
            var dbModel = _dbContext.Resources.FirstOrDefault(x => x.Id == model.Id);
            if (dbModel == null)
            {
                dbModel = new DB_Resource
                {
                    CreateTime = DateTime.Now
                };
                _dbContext.Resources.Add(dbModel);
            }

            dbModel.Description = model.Description;
            dbModel.Name = model.Name;
            dbModel.Url = model.Url;
            dbModel.LastUpdatedTime = DateTime.Now;
            _dbContext.SaveChanges();

            return StatusMessage.Success;
        }
        public string Delete(ResourceSetter model)
        {
            var dbModel = new DB_Resource() { Id = model.Id };
            _dbContext.Resources.Attach(dbModel).State = EntityState.Deleted;
            _dbContext.SaveChanges();
            return StatusMessage.Success;
        }

    }
}
