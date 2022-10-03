using Microsoft.AspNetCore.Mvc;

namespace MineServer.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [Route("/health")]
        [Route("/")]
        [HttpGet]
        public string Index()
        {
            return $"web health check, {System.Net.Dns.GetHostName()}";
        }
    }
}
