using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderTool.IService
{
    public interface ISpiderBaseService
    {
        List<TaskSimpleViewModel> GetTaskHistoryList();
        Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync();

        List<SpiderListItemViewModel> GetSpiderDtoList();
        Task<List<SpiderListItemViewModel>> GetSpiderDtoListAsync();
        string SubmitSpider(SpiderEditDto model);
        Task<string> SubmitSpiderAsync(SpiderEditDto model);
        string DeleteSpider(SpiderEditDto model);
        Task<string> DeleteSpiderAsync(SpiderEditDto model);

        List<TemplateDetailViewModel> GetTemplateDtoList();
        Task<List<TemplateDetailViewModel>> GetTemplateDtoListAsync();
        string SubmitTemplate(TemplateEditDto model);
        Task<string> SubmitTemplateAsync(TemplateEditDto model);
        string DeleteTemplate(TemplateEditDto model);
        Task<string> DeleteTemplateAsync(TemplateEditDto model);

        List<TaskListItemViewModel> GetTaskList();
        Task<List<TaskListItemViewModel>> GetTaskListAsync();
        int AddTask(TaskEditDto model);
        Task<int> AddTaskAsync(TaskEditDto model);
        void UpdateTask(TaskEditDto model);
        Task UpdateTaskAsync(TaskEditDto model);
        void SetTaskStatus(int taskId, int taskStatus);
        Task SetTaskStatusAsync(int taskId, int taskStatus);

        void SetLinkedSpider(SpiderDetailViewModel detail);
        SpiderDetailViewModel? GetSpider(int id);
        Task<SpiderDetailViewModel?> GetSpiderAsync(int id);

        void BulkUpdateTaskStatus(IEnumerable<int> tasks, int taskStatus);
        Task BulkUpdateTaskStatusAsync(IEnumerable<int> tasks, int taskStatus);
    }
    public interface ISpiderService : ISpiderBaseService
    {
        bool CanConnect();
        Task<bool> CanConnectAsync();

    }
}
