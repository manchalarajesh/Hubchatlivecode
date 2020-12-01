using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

using TelebuHubChat.LogClasses;
using TelebuHubChat.DbContexts;
using Microsoft.Extensions.Configuration;
using TelebuHubChat.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TelebuHubChat.Controllers
{
    [Route("api/[controller]")]
    public class SocialMediaFilesController : Controller
    {
        private readonly TelebuHubChatContext _context;
        private readonly IConfiguration _configuration;


        public SocialMediaFilesController(TelebuHubChatContext context, IConfiguration Configuration)
        {
            _context = context;
            _configuration = Configuration;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post()
        {
            int WidgetId = Convert.ToInt32(HttpContext.Request.Query["WidgetId"]);
            try
            {
                WidgetChannels WidgetChannels = (from Wchannel in _context.WidgetChannels
                                                 where Wchannel.WidgetId == WidgetId &&
                                                 Wchannel.ConversationTypeId == 2 select Wchannel).FirstOrDefault();

                WidgetAuthkeys WidgetAuthKeys = (from Auth in _context.WidgetAuthkeys
                                                 where Auth.WidgetChannelId == WidgetChannels.Id
                                                 select Auth).FirstOrDefault();

                string AuthKey, AuthToken;
                string Authorization;

                AuthKey = WidgetAuthKeys.AuthKey;
                AuthToken = WidgetAuthKeys.AuthToken;
                var AuthBytes = System.Text.Encoding.UTF8.GetBytes(AuthKey + ":" + AuthToken);
                Authorization = "Basic " + Convert.ToBase64String(AuthBytes);

                string url = _configuration.GetValue<string>("RequestUrls:BluekiteUrl");
                url += AuthKey + "/SocialMediaFiles/";
                // url += ChannelUUId + "/Messages/";



                //var uri = new Uri("https://restapi.smscountry.com/v0.1/Accounts/sIkXPrYqDPe6xxjZgT1z/SocialMediaFiles/");

                var uri = new Uri(url);




                var ms = new MemoryStream();
                //var formFile = Request.Form.Files[0];
                Request.Form.Files[0].CopyTo(ms);
                var fileBytes = ms.ToArray();

                var client = new RestClient(uri);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Authorization);
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFileBytes("attach", fileBytes, Request.Form.Files[0].FileName);
                //request.AddFile("", "/C:/Users/Administrator/Pictures/Screenshots/Screenshot_2.png");




                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);


                var res = JObject.Parse(response.Content);

                return  Ok(res);
               

            }
            catch (Exception ex)
            {

                
                LogProperties.error("Exception at while uploading file to server " +ex.ToString());
                JObject res = new JObject();
                res.Add(new JProperty("Success", "False"));
                res.Add(new JProperty("Message", ex.ToString()));

               return Ok(res);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
