using Microsoft.AspNetCore.Mvc;
using SpiderRemoteServiceClient.Services;
using SpiderTool.Data.Dto.Spider;
using SpiderTool.Data.Dto.Tasks;
using Utility.Constants;

namespace MineServer.Controllers
{
    public class SpiderController : BaseApiController
    {
        readonly ISpiderRemoteService _spiderService;

        public SpiderController(ISpiderRemoteService spiderService)
        {
            _spiderService = spiderService;
        }

        #region
        [HttpGet]
        public ResponseModel<List<SpiderListItemViewModel>> GetSpiderList()
        {
            return new ResponseModel<List<SpiderListItemViewModel>>(_spiderService.GetSpiderDtoList());
        }

        [HttpPost]
        public ResponseModel<string> SubmitSpider([FromBody] SpiderEditDto model)
        {
            return new ResponseModel<string>(_spiderService.SubmitSpider(model));
        }

        [HttpPost]
        public ResponseModel<string> DeleteSpider([FromBody] SpiderEditDto model)
        {
            return new ResponseModel<string>(_spiderService.DeleteSpider(model));
        }
        #endregion

        #region
        [HttpGet]
        public ResponseModel<List<TemplateDetailViewModel>> GetTemplateList()
        {
            return new ResponseModel<List<TemplateDetailViewModel>>(_spiderService.GetTemplateDtoList());
        }

        [HttpPost]
        public ResponseModel<string> SubmitTemplate([FromBody] TemplateEditDto model)
        {
            return new ResponseModel<string>(_spiderService.SubmitTemplate(model));
        }

        [HttpPost]
        public ResponseModel<string> DeleteTemplate([FromBody] TemplateEditDto model)
        {
            return new ResponseModel<string>(_spiderService.DeleteTemplate(model));
        }
        #endregion

        #region
        [HttpGet]
        public async Task<ResponseModel<List<TaskListItemViewModel>>> GetTaskList()
        {
            return new ResponseModel<List<TaskListItemViewModel>>(await _spiderService.GetTaskListAsync());
        }

        [HttpGet]
        public async Task<ResponseModel<List<TaskSimpleViewModel>>> GetTaskHistoryList()
        {
            return new ResponseModel<List<TaskSimpleViewModel>>(await _spiderService.GetTaskHistoryListAsync());
        }
        #endregion

        [HttpGet]
        public async Task<ResponseModel<string>> Run(string url, int spiderId)
        {
            return new ResponseModel<string>(await _spiderService.CrawlAsync(spiderId, url));
        }

        [HttpPost]
        public async Task Stop()
        {
            _spiderService.StopAllTask();
        }
    }
}
