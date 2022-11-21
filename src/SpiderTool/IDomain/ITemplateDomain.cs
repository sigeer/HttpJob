using SpiderTool.Dto.Spider;

namespace SpiderTool.IDomain
{
    public interface ITemplateDomain
    {
        List<TemplateDetailViewModel> GetTemplateDtoList();
        Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync();
        string Submit(TemplateEditDto model);
        Task<string> SubmitAsync(TemplateEditDto model);
        string Delete(TemplateEditDto model);
        Task<string> DeleteAsync(TemplateEditDto model);
    }
}
