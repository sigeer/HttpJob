using SpiderTool.Dto.Resource;

namespace SpiderTool.IDomain
{
    public interface IResourceDomain
    {
        List<ResourceHistoryDto> GetResourceDtoList();
        Task<List<ResourceHistoryDto>> GetResourceDtoListAsync();

        string Submit(ResourceHistorySetter model);
        string Delete(ResourceHistorySetter model);
    }
}
