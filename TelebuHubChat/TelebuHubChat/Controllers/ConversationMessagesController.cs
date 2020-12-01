using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TelebuHubChat.DbContexts;
using TelebuHubChat.Models;
using TelebuHubChat.Helpers;
using Microsoft.EntityFrameworkCore;

namespace TelebuHubChat.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class ConversationMessagesController : ControllerBase
    {
        private readonly TelebuHubChatContext _context;
        public ConversationMessagesController(TelebuHubChatContext context)
        {
            _context = context;
        }

        [HttpGet("{account_id}/widgets/{widget_id}/conversationmessages")]
        public async Task<ActionResult<Widgets>> GetConversationMessages(int account_id, int widget_id)
        {
            IList<ConversationMessages> conversationMessages = new List<ConversationMessages>();
            string ConversationIds = HttpContext.Request.Query["ConversationIds"].ToString();
            try
            {
                if (!string.IsNullOrEmpty(ConversationIds))
                {
                    IList<int> ConversationIdList = new List<int>();
                    string[] ConversationIdsArray = ConversationIds.Split(',');
                    foreach (string con_id in ConversationIdsArray)
                    {
                        ConversationIdList.Add(Convert.ToInt32(con_id));
                    }
                    conversationMessages = await _context.ConversationMessages.Where(cm => ConversationIdList.Contains(cm.ConversationId)).ToListAsync();
                }
                else
                    return NotFound(new { Success = false, Message = "Conversation Id Not Found" });

                if (conversationMessages == null)
                {
                    return NotFound(new { Success = false, Message = "Conversation Messages Not Found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.ToString() });
            }
            return Ok(new { Success = true, ConversationMessages = conversationMessages });
        }
    }
}