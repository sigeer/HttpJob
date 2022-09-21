using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.IService
{
    public interface ISpiderService
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
