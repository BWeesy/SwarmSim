using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SwarmSim
{
    public static class InitFrame
    {
        [FunctionName("InitFrame")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            Frame frame = new Frame();
            log.LogInformation("Creating new Frame");

            return (ActionResult)new OkObjectResult($"{frame.ToString()}");
        }
    }
}
