using SpiderTool.Dto.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.IDomain
{
    public interface ITemplateDomain
    {
        List<TemplateDto> GetTemplateDtoList();
        Task<List<TemplateDto>> GetTemplateDtoListAsync();
        string Submit(TemplateDto model);
        Task<string> SubmitAsync(TemplateDto model);
        string Delete(TemplateDto model);
        Task<string> DeleteAsync(TemplateDto model);
    }
}
