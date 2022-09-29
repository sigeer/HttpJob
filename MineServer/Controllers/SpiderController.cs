﻿using Microsoft.AspNetCore.Mvc;
using SpiderTool;
using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using Utility.Constants;

namespace MineServer.Controllers
{
    public class SpiderController : BaseApiController
    {
        readonly ISpiderService _spiderService;

        public SpiderController(ISpiderService spiderService)
        {
            _spiderService = spiderService;
        }
        #region
        [HttpGet]
        public ResponseModel<List<ResourceHistoryDto>> GetResourceList()
        {
            return new ResponseModel<List<ResourceHistoryDto>>(_spiderService.GetResourceHistoryDtoList());
        }

        [HttpPost]
        public ResponseModel<string> SubmitResource([FromBody] ResourceHistorySetter model)
        {
            return new ResponseModel<string>(_spiderService.SubmitResouceHistory(model));
        }

        [HttpPost]
        public ResponseModel<string> DeleteResource([FromBody] ResourceHistorySetter model)
        {
            return new ResponseModel<string>(_spiderService.DeleteResource(model));
        }
        #endregion

        #region
        [HttpGet]
        public ResponseModel<List<SpiderEditDto>> GetSpiderList()
        {
            return new ResponseModel<List<SpiderEditDto>>(_spiderService.GetSpiderDtoList());
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
        public ResponseModel<List<TemplateEditDto>> GetTemplateList()
        {
            return new ResponseModel<List<TemplateEditDto>>(_spiderService.GetTemplateDtoList());
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
            await new SpiderWorker(spiderId, _spiderService, url).Start();
            return new ResponseModel<string>("success");
        }
    }
}
