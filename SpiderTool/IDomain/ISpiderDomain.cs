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
        List<SpiderListItemViewModel> GetSpiderDtoList();
        Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync();
        SpiderDetailViewModel? GetSpiderDto(int id);
        Task<SpiderDetailViewModel?> GetSpiderDtoAsync(int id);
        string Delete(SpiderEditDto model);
        Task<string> DeleteAsync(SpiderEditDto model);
        string Submit(SpiderEditDto model);
        Task<string> SubmitAsync(SpiderEditDto model);
    }
}
