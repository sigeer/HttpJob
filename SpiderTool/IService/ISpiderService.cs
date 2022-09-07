using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.IService
{
    public interface ISpiderService
    {
        List<ResourceDto> GetResourceDtoList();

        string SubmitResouce(ResourceSetter model);
        string DeleteResource(ResourceSetter model);

        List<SpiderDtoSetter> GetSpiderDtoList();
        string SubmitSpider(SpiderDtoSetter model);
        string DeleteSpider(SpiderDtoSetter model);

        List<TemplateDto> GetTemplateDtoList();
        Task<List<TemplateDto>> GetTemplateDtoListAsync();
        string SubmitTemplate(TemplateDto model);
        string DeleteTemplate(TemplateDto model);


        SpiderDto? GetSpider(int id);
        Task<string> Crawling(string url, int spiderId);
        Task<string> Crawling(int resourceId, int spiderId);
    }
}
