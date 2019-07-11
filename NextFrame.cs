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
    public static class NewFrame
    {
        [FunctionName("NewFrame")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            Frame frame = new Frame();
            frame.Init();
            log.LogInformation($"{frame.ToString()}");

            var responseJson = JsonConvert.SerializeObject(frame.map);

            return new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };
        }
    }
}
