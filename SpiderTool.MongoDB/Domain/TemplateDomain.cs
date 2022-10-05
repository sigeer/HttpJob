using AutoMapper;
using MongoDB.Driver;
using SpiderTool.Constants;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
using SpiderTool.IDomain;

namespace SpiderTool.MongoDB.Domain
{
    public class TemplateDomain : ITemplateDomain
    {
        readonly IMongoDatabase _db;
        readonly IMapper _mapper;

        public TemplateDomain(IMongoClient client, IMapper mapper)
        {
            _db = client.GetDatabase("spider");
            _mapper = mapper;
        }

        public string Delete(TemplateEditDto model)
        {
            var collection = _db.GetCollection<DB_Template>(nameof(DB_Template));
            collection.DeleteMany(x => x.Id == model.Id);
            return StatusMessage.Success;
        }

        public async Task<string> DeleteAsync(TemplateEditDto model)
        {
            var collection = _db.GetCollection<DB_Template>(nameof(DB_Template));
            await collection.DeleteManyAsync(x => x.Id == model.Id);
            return StatusMessage.Success;
        }

        public List<TemplateDetailViewModel> GetTemplateDtoList()
        {
            var templateCollection = _db.GetCollection<DB_Template>(nameof(DB_Template));
            var templateReplacementRuleCollection = _db.GetCollection<DB_ReplacementRule>(nameof(DB_ReplacementRule));
            var total = templateCollection.Find(Builders<DB_Template>.Filter.Empty).ToList();
            var allRule = templateReplacementRuleCollection.Find(Builders<DB_ReplacementRule>.Filter.Empty).ToList();
            return total.Select(a => new TemplateDetailViewModel
            {
                Id = a.Id,
                Name = a.Name,
                TemplateStr = a.TemplateStr,
                LinkedSpiderId = a.LinkedSpiderId,
                Type = a.Type,
                ReplacementRules = allRule.Where(x => x.TemplateId == a.Id).Select(x => new ReplacementRuleDto
                {
                    Id = x.Id,
                    ReplacementOldStr = x.ReplacementOldStr,
                    ReplacementNewlyStr = x.ReplacementNewlyStr
                }).ToList()
            }).ToList();
        }

        public async Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync()
        {
            var templateCollection = _db.GetCollection<DB_Template>(nameof(DB_Template));
            var templateReplacementRuleCollection = _db.GetCollection<DB_ReplacementRule>(nameof(DB_ReplacementRule));
            var total = await (await templateCollection.FindAsync(Builders<DB_Template>.Filter.Empty)).ToListAsync();
            var allRule = await (await templateReplacementRuleCollection.FindAsync(Builders<DB_ReplacementRule>.Filter.Empty)).ToListAsync();
            return total.Select(a => new TemplateDetailViewModel
            {
                Id = a.Id,
                Name = a.Name,
                TemplateStr = a.TemplateStr,
                LinkedSpiderId = a.LinkedSpiderId,
                Type = a.Type,
                ReplacementRules = allRule.Where(x => x.TemplateId == a.Id).Select(x => new ReplacementRuleDto
                {
                    Id = x.Id,
                    ReplacementOldStr = x.ReplacementOldStr,
                    ReplacementNewlyStr = x.ReplacementNewlyStr
                }).ToList()
            }).ToList();
        }

        public string Submit(TemplateEditDto model)
        {
            var table = _db.GetCollection<DB_Template>(nameof(DB_Template));

            var template = _db.GetCollection<DB_ReplacementRule>(nameof(DB_ReplacementRule));
            if (model.Id == 0)
            {
                var maxId = table.Find(x => x.Id > 0).SortByDescending(x => x.Id).Skip(0).Limit(1).FirstOrDefault()?.Id ?? 0;
                var dbModel = _mapper.Map<DB_Template>(model);
                dbModel.Id = maxId + 1;
                dbModel.CreateTime = DateTime.Now;
                dbModel.LastUpdatedTime = DateTime.Now;
                table.InsertOne(dbModel);

                if (model.ReplacementRules.Count > 0)
                    template.InsertMany(model.ReplacementRules.Select(x => new DB_ReplacementRule
                    {
                        TemplateId = dbModel.Id,
                        ReplacementNewlyStr = x.ReplacementNewlyStr,
                        ReplacementOldStr = x.ReplacementOldStr
                    }));
            }
            else
            {
                var dbModel = table.Find(x => x.Id == model.Id).FirstOrDefault();
                if (dbModel != null)
                {
                    dbModel.Name = model.Name;
                    dbModel.TemplateStr = model.TemplateStr;
                    dbModel.Type = model.Type;
                    dbModel.LastUpdatedTime = DateTime.Now;
                    dbModel.LinkedSpiderId = model.LinkedSpiderId;

                    table.ReplaceOne(x => x.Id == dbModel.Id, dbModel);

                    template.DeleteMany(x => x.TemplateId == dbModel.Id);

                    if (model.ReplacementRules.Count > 0)
                        template.InsertMany(model.ReplacementRules.Select(x => new DB_ReplacementRule
                        {
                            TemplateId = dbModel.Id,
                            ReplacementNewlyStr = x.ReplacementNewlyStr,
                            ReplacementOldStr = x.ReplacementOldStr
                        }));
                }
            }
            return StatusMessage.Success;
        }

        public async Task<string> SubmitAsync(TemplateEditDto model)
        {
            var table = _db.GetCollection<DB_Template>(nameof(DB_Template));

            var template = _db.GetCollection<DB_ReplacementRule>(nameof(DB_ReplacementRule));
            if (model.Id == 0)
            {
                var maxId = (await table.Find(x => x.Id > 0).SortByDescending(x => x.Id).Skip(0).Limit(1).FirstOrDefaultAsync())?.Id ?? 0;
                var dbModel = _mapper.Map<DB_Template>(model);
                dbModel.Id = maxId + 1;
                dbModel.CreateTime = DateTime.Now;
                dbModel.LastUpdatedTime = DateTime.Now;
                await table.InsertOneAsync(dbModel);

                if (model.ReplacementRules.Count > 0)
                    await template.InsertManyAsync(model.ReplacementRules.Select(x => new DB_ReplacementRule
                    {
                        TemplateId = dbModel.Id,
                        ReplacementNewlyStr = x.ReplacementNewlyStr,
                        ReplacementOldStr = x.ReplacementOldStr
                    }));
            }
            else
            {
                var dbModel = await table.Find(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    dbModel.Name = model.Name;
                    dbModel.TemplateStr = model.TemplateStr;
                    dbModel.Type = model.Type;
                    dbModel.LastUpdatedTime = DateTime.Now;
                    dbModel.LinkedSpiderId = model.LinkedSpiderId;

                    await table.ReplaceOneAsync(x => x.Id == dbModel.Id, dbModel);

                    await template.DeleteManyAsync(x => x.TemplateId == dbModel.Id);

                    if (model.ReplacementRules.Count > 0)
                        await template.InsertManyAsync(model.ReplacementRules.Select(x => new DB_ReplacementRule
                        {
                            TemplateId = dbModel.Id,
                            ReplacementNewlyStr = x.ReplacementNewlyStr,
                            ReplacementOldStr = x.ReplacementOldStr
                        }));
                }
            }
            return StatusMessage.Success;
        }
    }
}
