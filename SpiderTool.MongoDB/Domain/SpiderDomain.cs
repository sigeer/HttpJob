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

namespace SpiderTool.MongoDB.Domain
{
    public class SpiderDomain : ISpiderDomain
    {
        readonly IMongoDatabase _db;
        readonly IMapper _mapper;

        public SpiderDomain(IMongoClient client, IMapper mapper)
        {
            _db = client.GetDatabase("spider");
            _mapper = mapper;
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
            var model = table.Find<DB_Spider>(x => x.Id == id);
            return _mapper.Map<SpiderDetailViewModel>(model);
        }

        public async Task<SpiderDetailViewModel?> GetSpiderDtoAsync(int id)
        {
            var table = _db.GetCollection<DB_Spider>(nameof(DB_Spider));
            var model = await table.FindAsync<DB_Spider>(x => x.Id == id);
            return _mapper.Map<SpiderDetailViewModel>(model);
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
            return StatusMessage.Success;
        }

        public Task<string> SubmitAsync(SpiderEditDto model)
        {
            throw new NotImplementedException();
        }
    }
}
