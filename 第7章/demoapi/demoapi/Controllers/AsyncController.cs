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
    public class AsyncController : ControllerBase
    {
        [HttpGet]
        public async Task<int[]> Get()
        {
            // _logger.LogInformation("Get called!");

            int[] result = null;
            await Task.Run(async () => {
                var random = new Random();
                result = new int[random.Next(1, 200)];
                for (var i = 0; i < result.Length; ++i)
                {
                    Thread.Sleep(50);
                    // await Task.Delay(50);
                    result[i] = random.Next();
                }
            });

            return result;
        }
    }
}
