using CommonUtility.Constants;
using Microsoft.AspNetCore.Mvc;
using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;

namespace MineServer.Controllers
{
    public class SpiderController: BaseApiController
    {
        readonly ISpiderService _spiderService;

        public SpiderController(ISpiderService spiderService)
        {
            _spiderService = spiderService;
        }
        #region
        [HttpGet]
        public ResponseModel<List<ResourceDto>> GetResourceList()
        {
            return new ResponseModel<List<ResourceDto>>(_spiderService.GetResourceDtoList());
        }

        [HttpPost]
        public ResponseModel<string> SubmitResource([FromBody] ResourceSetter model)
        {
            return new ResponseModel<string>(_spiderService.SubmitResouce(model));
        }

        [HttpPost]
        public ResponseModel<string> DeleteResource([FromBody] ResourceSetter model)
        {
            return new ResponseModel<string>(_spiderService.DeleteResource(model));
        }
        #endregion

        #region
        [HttpGet]
        public ResponseModel<List<SpiderDtoSetter>> GetSpiderList()
        {
            return new ResponseModel<List<SpiderDtoSetter>>(_spiderService.GetSpiderDtoList());
        }

        [HttpPost]
        public ResponseModel<string> SubmitSpider([FromBody] SpiderDto model)
        {
            return new ResponseModel<string>(_spiderService.SubmitSpider(model));
        }

        [HttpPost]
        public ResponseModel<string> DeleteSpider([FromBody] SpiderDto model)
        {
            return new ResponseModel<string>(_spiderService.DeleteSpider(model));
        }
        #endregion

        #region
        [HttpGet]
        public ResponseModel<List<TemplateDto>> GetTemplateList()
        {
            return new ResponseModel<List<TemplateDto>>(_spiderService.GetTemplateDtoList());
        }

        [HttpPost]
        public ResponseModel<string> SubmitTemplate([FromBody] TemplateDto model)
        {
            return new ResponseModel<string>(_spiderService.SubmitTemplate(model));
        }

        [HttpPost]
        public ResponseModel<string> DeleteTemplate([FromBody] TemplateDto model)
        {
            return new ResponseModel<string>(_spiderService.DeleteTemplate(model));
        }
        #endregion

        [HttpGet]
        public async Task<ResponseModel<string>> Run(int resourceId, int spiderId)
        {
            return new ResponseModel<string>(await _spiderService.Crawling(resourceId, spiderId));
        }
    }
}
