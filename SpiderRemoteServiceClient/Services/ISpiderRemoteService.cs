

using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderRemoteServiceClient.Services
{
    public interface ISpiderRemoteService
    {
        Task<bool> Ping();

        Task<List<SpiderDtoSetter>> GetSpiderDtoListAsync();
        Task<string> SubmitSpiderAsync(SpiderDtoSetter model);
        Task<string> DeleteSpiderAsync(SpiderDtoSetter model);

        Task<List<TemplateDto>> GetTemplateDtoListAsync();
        Task<string> SubmitTemplateAsync(TemplateDto model);
        Task<string> DeleteTemplateAsync(TemplateDto model);

        Task<List<TaskDto>> GetTaskListAsync();
        Task<int> AddTaskAsync(TaskSetter model);
        Task UpdateTaskAsync(TaskSetter model);
        Task SetTaskStatusAsync(int taskId, int taskStatus);


        Task<SpiderDto?> GetSpiderAsync(int id);
    }
}
