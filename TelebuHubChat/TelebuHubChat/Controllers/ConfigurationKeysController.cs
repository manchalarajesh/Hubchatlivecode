using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TelebuHubChat.LogClasses;

namespace TelebuHubChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationKeysController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;

        public ConfigurationKeysController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        [HttpGet]
        public JObject Get()
        {

            JObject Config = new JObject();
            try
            {
                String SocketUrl = _configuration.GetValue<string>("RequestUrls:WebSocketSubscribe");

                Config.Add(new JProperty("Success", true));
                Config.Add(new JProperty("Message", "Success"));
                Config.Add(new JProperty("SocketSubscribeUrl", SocketUrl));

            }
            catch (Exception ex)
            {
                LogProperties.error("Exception at configuration keys: " + ex.ToString());

                Config.Add(new JProperty("Success", false));
                Config.Add(new JProperty("Message", ex.ToString()));
            }




            return Config;


            //return new string[] { "value1", "value2" };
        }

    }
}