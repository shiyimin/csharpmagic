using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace demoapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SequentialController : ControllerBase
    {
        // private ILogger _logger;

        // public SequetialController(ILogger<SequetialController> logger)
        // {
        //     _logger = logger;
        // }

        [HttpGet]
        public int[] Get()
        {
            // _logger.LogInformation("Get called!");
            var random = new Random();
            var result = new int[random.Next(1, 200)];
            for (var i = 0; i < result.Length; ++i)
            {
                Thread.Sleep(50);
                result[i] = random.Next();
            }
            return result;
        }
    }
}
