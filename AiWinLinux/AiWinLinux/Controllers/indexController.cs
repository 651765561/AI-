using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AiWinLinux.Controllers
{
    
    [Route("api/[controller]")]
    public class IndexController : Controller
    {
        private readonly IOptions<AppSettingsModel> _appSettingsModel;

        public IndexController(IOptions<AppSettingsModel> appSettingsModel)
        {
            _appSettingsModel = appSettingsModel;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var item = new { Name = "WebApiService is Ok!" ,IsRun=true};
            return Ok(item);
        }

    }
  
}
