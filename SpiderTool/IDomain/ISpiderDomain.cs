using SpiderTool.Dto.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.IDomain
{
    public interface ISpiderDomain
    {
        List<SpiderDtoSetter> GetSpiderDtoList();

        List<SpiderTemplateDto> GetSpiderTemplatesDto(int spiderId);
        string Delete(SpiderDtoSetter model);
        string Submit(SpiderDtoSetter model);
        SpiderDto? GetSpiderDto(int id);
    }
}
