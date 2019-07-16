using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwarmSim.Classes.Entities;
using SwarmSim.Interfaces;

namespace SwarmSim
{
    public static class NewFrame
    {
        [FunctionName("NextFrame")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //Read request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<Space[,]>(requestBody);

            //Validate is a valid map
            if(!Frame.IsValidMap(data)) throw new InvalidDataException ("Invalid data");

            //Create next map from incoming map
            var returnData = Frame.NextStep(data);
            
            //Return created map
            var responseJson = JsonConvert.SerializeObject(returnData);
            return new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };
        }
    }
}
