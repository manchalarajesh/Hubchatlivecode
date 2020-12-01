using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TelebuHubChat.DbContexts;
using TelebuHubChat.Models;


namespace TelebuHubChat.Controllers
{
    
    public class ChatController : Controller
    {
        public TelebuHubChatContext _context;

      
        public ChatController(TelebuHubChatContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [EnableCors("AllowMyOrigin","*","*")]
        public IActionResult Chat(string widgetUUID,int isAgent,int isPing, string httpReferrer)
        {
            string curURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";


            //string B = widgetUUID; 
            var widget = _context.Widgets.Where(w => String.Equals(w.UUID, widgetUUID)).FirstOrDefault();
            Chat rec = new Chat
            {
                AccountId = widget.AccountId,
                WidgetId = widget.Id,
                url = curURL,
                IsAgent = isAgent,
                IsPing = isPing,
                HttpReferrer = httpReferrer,
		WidgetUuid =widget.UUID

            };
            HttpContext.Session.SetInt32("AccountId", widget.AccountId);
            HttpContext.Session.SetInt32("WidgetId", widget.Id);
	    HttpContext.Session.SetString("WidgetUuid", widget.UUID);


            if (widget != null)
            {
                ViewBag.Message = rec;
                return View(rec);
            }
            else
            {
                return NotFound();
            }
        }
    }
}