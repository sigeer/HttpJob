using SpiderTool.Dto.Spider;

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
