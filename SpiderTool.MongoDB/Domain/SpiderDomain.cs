using AutoMapper;
using MongoDB.Driver;
using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
using SpiderTool.IDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.GuidHelper;

namespace SpiderTool.MongoDB.Domain
{
    public class SpiderDomain : ISpiderDomain
    {
        readonly IMongoDatabase _db;
        readonly IMapper _mapper;
        readonly Snowflake _guidGenerator;

        public SpiderDomain(IMongoClient client, IMapper mapper, Snowflake snowflake)
        {
            _db = client.GetDatabase("spider");
            _mapper = mapper;
            _guidGenerator = snowflake;

        }

        public string Delete(SpiderEditDto model)
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));
            table.DeleteOne<DB_Spider>(x => x.Id == model.Id);
            return StatusMessage.Success;
        }

        public async Task<string> DeleteAsync(SpiderEditDto model)
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));
            await table.DeleteOneAsync<DB_Spider>(x => x.Id == model.Id);
            return StatusMessage.Success;
        }

        public SpiderDetailViewModel? GetSpiderDto(int id)
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));
            var model = table.Find<DB_Spider>(x => x.Id == id).FirstOrDefault();

            var relatedTable = _db.GetCollection<DB_SpiderTemplate>(nameof(DB_SpiderTemplate));
            var relatedTemplateIds = relatedTable.Find(x => x.SpiderId == id).ToList().Select(x => x.TemplateId).ToList();

            var detailTable = _db.GetCollection<DB_Template>(nameof(DB_Template));
            var detailList = detailTable.Find(x => relatedTemplateIds.Contains(x.Id)).ToList();
            var nextDetail = detailTable.Find(x => x.Id == model.NextPageTemplateId).FirstOrDefault();

            return new SpiderDetailViewModel
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                HeaderStr = model.Headers,
                Method = model.Method,
                PostObjStr = model.PostObjStr,
                TemplateList = _mapper.Map<List<TemplateDetailViewModel>>(detailList),
                NextPageTemplate = _mapper.Map<TemplateDetailViewModel>(nextDetail)
            };
        }

        public async Task<SpiderDetailViewModel?> GetSpiderDtoAsync(int id)
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));
            var model = await table.Find<DB_Spider>(x => x.Id == id).FirstOrDefaultAsync();

            var relatedTable = _db.GetCollection<DB_SpiderTemplate>(nameof(DB_SpiderTemplate));
            var relatedTemplateIds = relatedTable.Find(x => x.SpiderId == id).ToList().Select(x => x.TemplateId).ToList();

            var detailTable = _db.GetCollection<DB_Template>(nameof(DB_Template));
            var detailList = await detailTable.Find(x => relatedTemplateIds.Contains(x.Id)).ToListAsync();
            var nextDetail = await detailTable.Find(x => x.Id == model.NextPageTemplateId).FirstOrDefaultAsync();

            return new SpiderDetailViewModel
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                HeaderStr = model.Headers,
                Method = model.Method,
                PostObjStr = model.PostObjStr,
                TemplateList = _mapper.Map<List<TemplateDetailViewModel>>(detailList),
                NextPageTemplate = _mapper.Map<TemplateDetailViewModel>(nextDetail)
            };
        }

        public List<SpiderListItemViewModel> GetSpiderDtoList()
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));
            var model = (table.AsQueryable<DB_Spider>()).ToList();
            return _mapper.Map<List<SpiderListItemViewModel>>(model);
        }

        public async Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync()
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));
            var model = await table.AsQueryable<DB_Spider>().ToListAsync();
            return _mapper.Map<List<SpiderListItemViewModel>>(model);
        }

        public string Submit(SpiderEditDto model)
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));

            var template = _db.GetCollection<DB_SpiderTemplate>(nameof(DB_SpiderTemplate));
            if (model.Id == 0)
            {
                var maxId = table.Find(x => x.Id > 0).SortByDescending(x => x.Id).Skip(0).Limit(1).FirstOrDefault()?.Id ?? 0;
                var dbModel = _mapper.Map<DB_Spider>(model);
                dbModel.Id = maxId + 1;
                dbModel.CreateTime = DateTime.Now;
                dbModel.LastUpdatedTime = DateTime.Now;
                table.InsertOne(dbModel);

                if (model.Templates.Count > 0)
                    template.InsertMany(model.Templates.Select(x => new DB_SpiderTemplate
                    {
                        Id = (int)_guidGenerator.NextId(),
                        TemplateId = x,
                        SpiderId = dbModel.Id,
                    }));
            }
            else
            {
                var dbModel = table.Find(x => x.Id == model.Id).FirstOrDefault();
                if (dbModel != null)
                {
                    dbModel.Name = model.Name;
                    dbModel.Method = model.Method;
                    dbModel.Description = model.Description;
                    dbModel.Headers = model.Headers;
                    dbModel.PostObjStr = model.PostObjStr;
                    dbModel.NextPageTemplateId = model.NextPageTemplateId;
                    dbModel.LastUpdatedTime = DateTime.Now;

                    table.ReplaceOne(x => x.Id == dbModel.Id, dbModel);

                    template.DeleteMany(x => x.SpiderId == dbModel.Id);

                    if (model.Templates.Count > 0)
                        template.InsertMany(model.Templates.Select(x => new DB_SpiderTemplate
                        {
                            Id = (int)_guidGenerator.NextId(),
                            TemplateId = x,
                            SpiderId = dbModel.Id,
                        }));
                }
            }
            return StatusMessage.Success;
        }

        public async Task<string> SubmitAsync(SpiderEditDto model)
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));

            var template = _db.GetCollection<DB_SpiderTemplate>(nameof(DB_SpiderTemplate));
            if (model.Id == 0)
            {
                var maxId = (await table.Find(x => x.Id > 0).SortByDescending(x => x.Id).Skip(0).Limit(1).FirstOrDefaultAsync())?.Id ?? 0;
                var dbModel = _mapper.Map<DB_Spider>(model);
                dbModel.Id = maxId + 1;
                await table.InsertOneAsync(dbModel);

                if (model.Templates.Count>0)
                    await template.InsertManyAsync(model.Templates.Select(x => new DB_SpiderTemplate
                    {
                        Id = (int)_guidGenerator.NextId(),
                        TemplateId = x,
                        SpiderId = dbModel.Id,
                    }));
            }
            else
            {
                var dbModel = await table.Find(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    dbModel.Name = model.Name;
                    dbModel.Method = model.Method;
                    dbModel.Description = model.Description;
                    dbModel.Headers = model.Headers;
                    dbModel.PostObjStr = model.PostObjStr;
                    dbModel.NextPageTemplateId = model.NextPageTemplateId;
                    dbModel.LastUpdatedTime = DateTime.Now;

                    await table.ReplaceOneAsync(x => x.Id == dbModel.Id,  dbModel);

                    await template.DeleteManyAsync(x => x.SpiderId == dbModel.Id);
                    if (model.Templates.Count > 0)
                        await template.InsertManyAsync(model.Templates.Select(x => new DB_SpiderTemplate
                        {
                            Id = (int)_guidGenerator.NextId(),
                            TemplateId = x,
                            SpiderId = dbModel.Id,
                        }));
                }
            }
            return StatusMessage.Success;
        }
    }
}
