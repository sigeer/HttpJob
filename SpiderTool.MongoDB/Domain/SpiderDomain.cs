using AutoMapper;
using MongoDB.Driver;
using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
using SpiderTool.IDomain;
using Utility.GuidHelper;

namespace SpiderTool.MongoDB.Domain
{
    public class SpiderDomain : ISpiderDomain
    {
        readonly IMongoDatabase _db;
        readonly IMapper _mapper;
        readonly Snowflake _guidGenerator;

        public SpiderDomain(IMongoClient client, IMapper mapper)
        {
            _db = client.GetDatabase(Constants.Constants.DBName);
            _mapper = mapper;
            _guidGenerator = Snowflake.GetInstance(1);

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
            var dbModel = table.Find<DB_Spider>(x => x.Id == id).FirstOrDefault();
            if (dbModel == null)
                return null;

            var relatedTable = _db.GetCollection<DB_SpiderTemplate>(nameof(DB_SpiderTemplate));
            var relatedTemplateIds = relatedTable.Find(x => x.SpiderId == id).ToList().Select(x => x.TemplateId).ToList();

            var detailTable = _db.GetCollection<DB_Template>(nameof(DB_Template));
            var detailList = detailTable.Find(x => relatedTemplateIds.Contains(x.Id)).ToList();
            var nextPage = detailTable.Find(x => x.Id == dbModel.NextPageTemplateId).FirstOrDefault();

            var replaceRuleTable = _db.GetCollection<DB_ReplacementRule>(nameof(DB_ReplacementRule));
            var replaceRules = replaceRuleTable.Find(x => relatedTemplateIds.Contains(x.TemplateId)).ToList();

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
            data.TemplateList = detailList.Select(b => new TemplateDetailViewModel()
            {
                Id = b.Id,
                LinkedSpiderId = b.LinkedSpiderId,
                Name = b.Name,
                TemplateStr = b.TemplateStr,
                Type = b.Type,
                ReplacementRules = replaceRules.Where(x => x.TemplateId == b.Id).Select(x => new ReplacementRuleDto()
                {
                    Id = x.Id,
                    ReplacementNewlyStr = x.ReplacementNewlyStr,
                    ReplacementOldStr = x.ReplacementOldStr,
                }).ToList()
            }).ToList();
            return data;
        }

        public async Task<SpiderDetailViewModel?> GetSpiderDtoAsync(int id)
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));
            var dbModel = await table.Find<DB_Spider>(x => x.Id == id).FirstOrDefaultAsync();
            if (dbModel == null)
                return null;

            var relatedTable = _db.GetCollection<DB_SpiderTemplate>(nameof(DB_SpiderTemplate));
            var relatedTemplateIds = relatedTable.Find(x => x.SpiderId == id).ToList().Select(x => x.TemplateId).ToList();

            var detailTable = _db.GetCollection<DB_Template>(nameof(DB_Template));
            var detailList = await detailTable.Find(x => relatedTemplateIds.Contains(x.Id)).ToListAsync();
            var nextPage = await detailTable.Find(x => x.Id == dbModel.NextPageTemplateId).FirstOrDefaultAsync();

            var replaceRuleTable = _db.GetCollection<DB_ReplacementRule>(nameof(DB_ReplacementRule));
            var replaceRules = await replaceRuleTable.Find(x => relatedTemplateIds.Contains(x.TemplateId)).ToListAsync();

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
            data.TemplateList = detailList.Select(b => new TemplateDetailViewModel()
            {
                Id = b.Id,
                LinkedSpiderId = b.LinkedSpiderId,
                Name = b.Name,
                TemplateStr = b.TemplateStr,
                Type = b.Type,
                ReplacementRules = replaceRules.Where(x => x.TemplateId == b.Id).Select(x => new ReplacementRuleDto()
                {
                    Id = x.Id,
                    ReplacementNewlyStr = x.ReplacementNewlyStr,
                    ReplacementOldStr = x.ReplacementOldStr,
                }).ToList()
            }).ToList();
            data.NextPageTemplate = _mapper.Map<TemplateDetailViewModel>(nextPage);
            return data;
        }

        public List<SpiderListItemViewModel> GetSpiderDtoList()
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));
            return table.Find(Builders<DB_Spider>.Filter.Empty)
                .Project<SpiderListItemViewModel>(Builders<DB_Spider>.Projection.Include(x => x.Id).Include(x => x.Name)).ToList();
        }

        public async Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync()
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));
            return await table.Find(Builders<DB_Spider>.Filter.Empty)
                .Project<SpiderListItemViewModel>(Builders<DB_Spider>.Projection.Include(x => x.Id).Include(x => x.Name)).ToListAsync();
        }

        public string Submit(SpiderEditDto model)
        {
            try
            {
                if (!model.FormValid())
                    return StatusMessage.FormInvalid;

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
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> SubmitAsync(SpiderEditDto model)
        {
            try
            {
                if (!model.FormValid())
                    return StatusMessage.FormInvalid;

                var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));

                var template = _db.GetCollection<DB_SpiderTemplate>(nameof(DB_SpiderTemplate));
                if (model.Id == 0)
                {
                    var maxId = (await table.Find(x => x.Id > 0).SortByDescending(x => x.Id).Skip(0).Limit(1).FirstOrDefaultAsync())?.Id ?? 0;
                    var dbModel = _mapper.Map<DB_Spider>(model);
                    dbModel.Id = maxId + 1;
                    await table.InsertOneAsync(dbModel);

                    if (model.Templates.Count > 0)
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

                        await table.ReplaceOneAsync(x => x.Id == dbModel.Id, dbModel);

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
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
