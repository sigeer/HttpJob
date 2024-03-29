﻿using Microsoft.AspNetCore.Mvc;
using SpiderRemoteServiceClient.Services;

namespace MineServer.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        readonly ISpiderRemoteService _service;

        public HomeController(ISpiderRemoteService service)
        {
            _service = service;
        }

        [Route("/health")]
        [Route("/")]
        [HttpGet]
        public string Index()
        {
            return $"web health check, service CanConnect={_service.CanConnect()}";
        }

        [Route("/Ping")]
        [HttpGet]
        public async Task<string> Ping()
        {
            return $"web health check, service Ping={await _service.Ping()}";
        }
    }
}
