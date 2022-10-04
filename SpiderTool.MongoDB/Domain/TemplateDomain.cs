using AutoMapper;
using MongoDB.Driver;
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
            throw new NotImplementedException();
        }

        public Task<string> DeleteAsync(TemplateEditDto model)
        {
            throw new NotImplementedException();
        }

        public List<TemplateDetailViewModel> GetTemplateDtoList()
        {
            throw new NotImplementedException();
        }

        public Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync()
        {
            throw new NotImplementedException();
        }

        public string Submit(TemplateEditDto model)
        {
            throw new NotImplementedException();
        }

        public Task<string> SubmitAsync(TemplateEditDto model)
        {
            throw new NotImplementedException();
        }
    }
}
