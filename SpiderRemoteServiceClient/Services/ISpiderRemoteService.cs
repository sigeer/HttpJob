

using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderRemoteServiceClient.Services
{
    public interface ISpiderRemoteService
    {
        List<ResourceHistoryDto> GetResourceHistoryDtoList();

        string SubmitResouceHistory(ResourceHistorySetter model);
        string DeleteResource(ResourceHistorySetter model);

        List<SpiderDtoSetter> GetSpiderDtoList();
        string SubmitSpider(SpiderDtoSetter model);
        string DeleteSpider(SpiderDtoSetter model);

        List<TemplateDto> GetTemplateDtoList();
        Task<List<TemplateDto>> GetTemplateDtoListAsync();
        string SubmitTemplate(TemplateDto model);
        string DeleteTemplate(TemplateDto model);

        List<TaskDto> GetTaskList();
        int AddTask(TaskSetter model);
        void UpdateTask(TaskSetter model);
        void SetTaskStatus(int taskId, int taskStatus);


        SpiderDto? GetSpider(int id);
    }
}
