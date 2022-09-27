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
        Task<List<SpiderDtoSetter>> GetSpiderDtoListAsync();
        SpiderDto? GetSpiderDto(int id);
        Task<SpiderDto?> GetSpiderDtoAsync(int id);
        string Delete(SpiderDtoSetter model);
        Task<string> DeleteAsync(SpiderDtoSetter model);
        string Submit(SpiderDtoSetter model);
        Task<string> SubmitAsync(SpiderDtoSetter model);
    }
}
