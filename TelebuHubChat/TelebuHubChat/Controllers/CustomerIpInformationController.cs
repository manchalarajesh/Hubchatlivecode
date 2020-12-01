using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TelebuHubChat.DbContexts;
using TelebuHubChat.Models;
using TelebuHubChat.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.RegularExpressions;

namespace TelebuHubChat.Controllers
{
     [Route("widgets")]
    [ApiController]
    public class CustomerIpInformationController : ControllerBase
    {
        private readonly TelebuHubChatContext _context;
        private readonly IConfiguration _configuration;
        public CustomerIpInformationController(TelebuHubChatContext context, IConfiguration Configuration)
        {
            _context = context;
            _configuration = Configuration;
        }
                 [HttpPost("/widgets/{widget_id}/HttpURL/{customerId}/InsertCustomerIpInformation")]
                 public async Task<ActionResult<CustomerIpInformation>> InsertCustomerIpInformation(int widget_id, int customerId, CustomerIpInformation customerIpInformation)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");
			
			 string curURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

        //    string ip = Response.HttpContext.Connection.RemoteIpAddress.ToString();
	// string externalIP;
           // externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
          //  externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                       //  .Matches(externalIP)[0].ToString();

          //  if (ip == "::1")
          //  {
               // ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
// ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
         //   }

            var widget = await _context.Widgets.FindAsync(widget_id);

            var customerIpInformations = new CustomerIpInformation();
            var conversationMessage = new ConversationMessages();

            if (widget != null)
            {

                customerIpInformations.HttpURL = customerIpInformation.HttpURL;
                customerIpInformations.ClientIp =customerIpInformation.ClientIp; // externalIP;
                customerIpInformations.CustomerId = customerId;
				customerIpInformations.CreatedTimeUTC = DateTime.UtcNow; 
				customerIpInformations.HttpReferrer = customerIpInformation.HttpURL;



                _context.CustomerIpInformation.Add(customerIpInformations);
                await _context.SaveChangesAsync();

                return Ok(customerIpInformations);
            }
            else
            {
                return BadRequest("No such widget");
            }
        }
    }
}