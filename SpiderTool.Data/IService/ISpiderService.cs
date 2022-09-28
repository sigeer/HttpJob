using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderTool.IService
{
    public interface ISpiderService
    {
        List<ResourceHistoryDto> GetResourceHistoryDtoList();

        string SubmitResouceHistory(ResourceHistorySetter model);
        string DeleteResource(ResourceHistorySetter model);

        List<SpiderDtoSetter> GetSpiderDtoList();
        Task<List<SpiderDtoSetter>> GetSpiderDtoListAsync();
        string SubmitSpider(SpiderDtoSetter model);
        Task<string> SubmitSpiderAsync(SpiderDtoSetter model);
        string DeleteSpider(SpiderDtoSetter model);
        Task<string> DeleteSpiderAsync(SpiderDtoSetter model);

        List<TemplateDto> GetTemplateDtoList();
        Task<List<TemplateDto>> GetTemplateDtoListAsync();
        string SubmitTemplate(TemplateDto model);
        Task<string> SubmitTemplateAsync(TemplateDto model);
        string DeleteTemplate(TemplateDto model);
        Task<string> DeleteTemplateAsync(TemplateDto model);

        List<TaskDto> GetTaskList();
        Task<List<TaskDto>> GetTaskListAsync();
        int AddTask(TaskSetter model);
        Task<int> AddTaskAsync(TaskSetter model);
        void UpdateTask(TaskSetter model);
        Task UpdateTaskAsync(TaskSetter model);
        void SetTaskStatus(int taskId, int taskStatus);
        Task SetTaskStatusAsync(int taskId, int taskStatus);


        SpiderDto? GetSpider(int id);
        Task<SpiderDto?> GetSpiderAsync(int id);
    }
}
