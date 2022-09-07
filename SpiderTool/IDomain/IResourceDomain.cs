using SpiderTool.Dto.Resource;

namespace SpiderTool.IDomain
{
    public interface IResourceDomain
    {
        List<ResourceDto> GetResourceDtoList();

        string Submit(ResourceSetter model);
        string Delete(ResourceSetter model);
    }
}
