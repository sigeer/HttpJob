using Microsoft.AspNetCore.Mvc;
using SpiderRemoteServiceClient.Services;
using SpiderTool.Dto.Spider;
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

        [HttpGet]
        public async Task<ResponseModel<string>> Run(string url, int spiderId)
        {
            return new ResponseModel<string>(await _spiderService.CrawlAsync(spiderId, url));
        }
    }
}
