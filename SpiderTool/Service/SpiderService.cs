using SpiderTool.Constants;
using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.IDomain;
using SpiderTool.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.Service
{
    public class SpiderService : ISpiderService
    {
        readonly IResourceDomain _resourceDomain;
        readonly ISpiderDomain _spiderDomain;
        readonly ITemplateDomain _templateDomain;
        readonly SpiderWorker _spiderWorker;

        public SpiderService(IResourceDomain resourceDomain, ISpiderDomain spiderDomain, ITemplateDomain templateDomain, SpiderWorker spiderWorker)
        {
            _resourceDomain = resourceDomain;
            _spiderDomain = spiderDomain;
            _templateDomain = templateDomain;
            _spiderWorker = spiderWorker;
        }

        public async Task<string> Crawling(string url, int spiderId)
        {
            var spider = GetSpider(spiderId);
            if (spider == null)
                return StatusMessage.Error;

            await _spiderWorker.Start(url, spiderId);
            return StatusMessage.Success;
        }

        public async Task<string> Crawling(int resourceId, int spiderId)
        {
            var spider = GetSpider(spiderId);
            if (spider == null)
                return StatusMessage.Error;

            var resource = _resourceDomain.GetResourceDtoList().FirstOrDefault(x => x.Id == resourceId);
            if (resource == null || string.IsNullOrEmpty(resource.Url))
                return StatusMessage.Error;

            await _spiderWorker.Start(resource.Url, spiderId);
            return StatusMessage.Success;
        }

        public string DeleteResource(ResourceSetter model)
        {
            return _resourceDomain.Delete(model);
        }

        public string DeleteSpider(SpiderDtoSetter model)
        {
            return _spiderDomain.Delete(model);
        }

        public string DeleteTemplate(TemplateDto model)
        {
            return _templateDomain.Delete(model);
        }

        public List<ResourceDto> GetResourceDtoList()
        {
            return _resourceDomain.GetResourceDtoList();
        }

        public SpiderDto? GetSpider(int id)
        {
            return _spiderDomain.GetSpiderDto(id);
        }

        public List<SpiderDtoSetter> GetSpiderDtoList()
        {
            return _spiderDomain.GetSpiderDtoList();
        }

        public List<TemplateDto> GetTemplateDtoList()
        {
            return _templateDomain.GetTemplateDtoList();
        }

        public async Task<List<TemplateDto>> GetTemplateDtoListAsync()
        {
            return await _templateDomain.GetTemplateDtoListAsync();
        }

        public string SubmitResouce(ResourceSetter model)
        {
            return _resourceDomain.Submit(model);
        }

        public string SubmitSpider(SpiderDtoSetter model)
        {
            return _spiderDomain.Submit(model);
        }

        public string SubmitTemplate(TemplateDto model)
        {
            return _templateDomain.Submit(model);
        }
    }
}
