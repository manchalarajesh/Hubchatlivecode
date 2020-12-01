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
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using TelebuHubChat.LogClasses;
//using System.Data.Entity.SqlServer;
using System.Web.Http.ModelBinding;

namespace TelebuHubChat.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly TelebuHubChatContext _context;
        Int32 ChatsInitiated;
        Int32 ChatsAttended;
        Int32 BotFlowCompleted;
        Int32 ClosedChats;
        Int32 ReInitiatedChats;
        Int32 ChatsAssignedByAgent;
        Int32 BotFlowEndChats_grptalk;

        private readonly IConfiguration _configuration;
        public ConversationsController(TelebuHubChatContext context, IConfiguration Configuration)
        {
            _context = context;
            _configuration = Configuration;
        }

        [HttpGet("{account_id}/widgets/{widget_id}/conversations")]
        public async Task<ActionResult<Widgets>> GetConversations(int account_id, int widget_id)
        {
            IList<Conversations> conversations = new List<Conversations>();
            string StrFromDate = HttpContext.Request.Query["FromDate"].ToString();
            string StrToDate = HttpContext.Request.Query["ToDate"].ToString();
            Int32 AgentId = Convert.ToInt32(HttpContext.Request.Query["AgentId"].ToString());
            DateTime FromDate;
            DateTime ToDate;
            try
            {
                if (!string.IsNullOrEmpty(StrFromDate) && !string.IsNullOrEmpty(StrToDate))
                {
                    FromDate = DateTime.ParseExact(StrFromDate, "yyyy-MM-dd", null);
                    ToDate = DateTime.ParseExact(StrToDate, "yyyy-MM-dd", null);
                    if (AgentId != 0)
                        conversations = await _context.Conversations.Where(c => c.WidgetId == widget_id &&
                        c.CreatedTimeUTC.Date >= FromDate.Date && c.CreatedTimeUTC.Date <= ToDate.Date && c.AgentId == AgentId).ToListAsync();
                    else
                        conversations = await _context.Conversations.Where(c => c.WidgetId == widget_id &&
                    c.CreatedTimeUTC.Date >= FromDate.Date && c.CreatedTimeUTC.Date <= ToDate.Date).ToListAsync();
                }
                else
                    conversations = await _context.Conversations.Where(c => c.WidgetId == widget_id).ToListAsync();

                if (conversations == null)
                {
                    return NotFound(new { Success = false, Message = "Conversations Not Found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.ToString() });
            }
            return Ok(new { Success = true, Conversations = conversations });
        }

        [HttpPost("{account_id}/widgets/{widget_id}/conversations")]
        public async Task<ActionResult<Conversations>> PostConversations(int account_id, int widget_id, Conversations conversations)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");
            try
            {
                var widget = await _context.Widgets.FindAsync(widget_id);
                WidgetChannels widgetChannel = null;

                if (widget != null)
                {
                    if (widget.AccountId == account_id)
                    {
                        var conversation = new Conversations();
                        var response = new HttpResponseMessage();
                        var apiResponse = string.Empty;
                        //string receivedReservation
                        //string postData = "[{ \"op\": \"replace\", \"path\": \" + name + \", \"value\": \"utter_greet\"}," +
                        //      "{ \"op\": \"replace\", \"path\":\"policy\", \"value\": \"MappingPolicy\"}," +
                        //  "{ \"op\": \"replace\", \"path\":\"confidence\", \"value\": \"0.987232\"},]";

                        //  string postData = "{\"name\": \"utter_greet\",\"policy\": \"MappingPolicy\",\"confidence\":\"0.987232\"}";

                        var postData = new
                        {
                            name = @"welcome_action",
                            // policy = "MappingPolicy",
                            // confidence = "0.987232"

                        };



                        conversation.WidgetId = widget_id;
                        conversation.StatusId = conversations.StatusId;
                        conversation.BrowserUserAgent = conversations.BrowserUserAgent;
                        conversation.CustomerIPAddress = conversations.CustomerIPAddress;
                        conversation.CreatedTimeUTC = DateTime.UtcNow;
                        conversation.AgentRequestedTimeUTC = conversations.AgentRequestedTimeUTC;
                        conversation.AgentAllocatedTimeUTC = conversations.AgentAllocatedTimeUTC;
                        conversation.AgentAcceptedTimeUTC = conversations.AgentAcceptedTimeUTC;
                        conversation.FirstAgentMessageTimeUTC = conversations.FirstAgentMessageTimeUTC;
                        conversation.ClosingTimeUTC = conversations.ClosingTimeUTC;
                        conversation.ClosedByCustomer = conversations.ClosedByCustomer;
                        conversation.CustomerId = conversations.CustomerId;
                        conversation.Mobile = conversations.Mobile;
                        conversation.ConversationTypeId = conversations.ConversationTypeId;

                        _context.Conversations.Add(conversation);
                        await _context.SaveChangesAsync();

                        using (var httpClient = new HttpClient())
                        {
                            string BotUrl = "";

                            if (conversations.ConversationTypeId == 2)
                            {
                                widgetChannel = _context.WidgetChannels.Where(wc => wc.WidgetId == widget_id && wc.ConversationTypeId == 2).FirstOrDefault();
                                BotUrl = widgetChannel.RasaBotUrl;
                            }
                            else
                            {
                                BotUrl = widget.RasaBotUrl;
                            }
                            // var request = new HttpRequestMessage
                            // {
                            //  RequestUri = new Uri(BotUrl + "conversations/" + conversation.Id + "/execute"),
                            // RequestUri = new Uri("http://172.31.16.142:5009/conversations/" + conversations.Id + "/execute"),
                            // RequestUri = new Uri("http://45.249.77.142:6060/conversations/" + conversations.Id + "/execute"),
                            // Method = new HttpMethod("Post"),
                            //JsonConvert.SerializeObject(/
                            // Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json")
                            // };
                            //  response = await httpClient.SendAsync(request);

                            // apiResponse = await response.Content.ReadAsStringAsync();

                            //receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);


                        }
                        // return Ok(apiResponse);
                        return Ok(conversation);
                    }
                    else
                    {
                        return BadRequest("AccountId and WidgetId is not concerned with any widget.");
                    }
                }
                else
                {
                    return BadRequest("No such widget");
                }
            }
            catch (Exception ex)
            {
                LogProperties.error("Exception in PostConversation: " + ex.ToString());

                return BadRequest("Exception in PostConversation: " + ex.ToString());


            }


        }
        //[HttpPost("{account_id}/widgets/{widget_id}/conversations")]
        //public async Task<ActionResult<Conversations>> PostConversations(int account_id, int widget_id, Conversations conversations)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid data.");

        //    var widget = await _context.Widgets.FindAsync(widget_id);

        //    if (widget != null)
        //    {
        //        if (widget.AccountId == account_id)
        //        {
        //            var conversation = new Conversations();
        //            var response = new HttpResponseMessage();
        //            var apiResponse = string.Empty;
        //            //string receivedReservation
        //            //string postData = "[{ \"op\": \"replace\", \"path\": \" + name + \", \"value\": \"utter_greet\"}," +
        //            //      "{ \"op\": \"replace\", \"path\":\"policy\", \"value\": \"MappingPolicy\"}," +
        //            //  "{ \"op\": \"replace\", \"path\":\"confidence\", \"value\": \"0.987232\"},]";

        //            //  string postData = "{\"name\": \"utter_greet\",\"policy\": \"MappingPolicy\",\"confidence\":\"0.987232\"}";

        //            var postData = new
        //            {
        //                name = @"welcome_action",
        //               // policy = "MappingPolicy",
        //               // confidence = "0.987232"

        //            };



        //            conversation.WidgetId = widget_id;
        //            conversation.StatusId = conversations.StatusId;
        //            conversation.BrowserUserAgent = conversations.BrowserUserAgent;
        //            conversation.CustomerIPAddress = conversations.CustomerIPAddress;
        //            conversation.CreatedTimeUTC = DateTime.UtcNow;
        //            conversation.AgentRequestedTimeUTC = conversations.AgentRequestedTimeUTC;
        //            conversation.AgentAllocatedTimeUTC = conversations.AgentAllocatedTimeUTC;
        //            conversation.AgentAcceptedTimeUTC = conversations.AgentAcceptedTimeUTC;
        //            conversation.FirstAgentMessageTimeUTC = conversations.FirstAgentMessageTimeUTC;
        //            conversation.ClosingTimeUTC = conversations.ClosingTimeUTC;
        //            conversation.ClosedByCustomer = conversations.ClosedByCustomer;

        //            _context.Conversations.Add(conversation);
        //            await _context.SaveChangesAsync();

        //            using (var httpClient = new HttpClient())
        //            {
        //                var request = new HttpRequestMessage
        //                {
        //                    RequestUri = new Uri(widget.RasaBotUrl + "conversations/" + conversation.Id + "/execute"),
        //                    // RequestUri = new Uri("http://172.31.16.142:5009/conversations/" + conversations.Id + "/execute"),
        //                    // RequestUri = new Uri("http://45.249.77.142:6060/conversations/" + conversations.Id + "/execute"),
        //                    Method = new HttpMethod("Post"),
        //                    //JsonConvert.SerializeObject(/
        //                    Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json")
        //                };
        //                response = await httpClient.SendAsync(request);

        //                apiResponse = await response.Content.ReadAsStringAsync();

        //                //receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);


        //            }
        //            // return Ok(apiResponse);
        //            return Ok(conversation);
        //        }
        //        else
        //        {
        //            return BadRequest("AccountId and WidgetId is not concerned with any widget.");
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest("No such widget");
        //    }
        //}

        [HttpPost("{account_id}/widgets/{widget_id}/conversations/{conversation_id}/{is_endchat}/replies")]
        public async Task<ActionResult<ConversationMessages>> PostConversationMessages(int account_id, int widget_id, int conversation_id, int is_endchat, ConversationMessages conversationMessages)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");


            try
            {
                var conversation = await _context.Conversations.FindAsync(conversation_id);

                var widget = _context.Widgets.Where(w => w.Id == widget_id).FirstOrDefault();

                var botURL = widget.RasaBotUrl;

                WidgetChannels widgetChannel = null;

                if (widget != null)
                {

                    if (widget.AccountId == account_id)
                    {

                        if (conversation.Id == conversation_id)
                        {

                            if (conversationMessages.ConversationTypeId == 0)
                            {
                                conversationMessages.ConversationTypeId = conversation.ConversationTypeId;
                            }
                            //whatsapp
                            if (conversationMessages.ConversationTypeId == 2)
                            {
                                widgetChannel = _context.WidgetChannels.Where(wc => wc.WidgetId == widget_id && wc.ConversationTypeId == 2).FirstOrDefault();

                                LogProperties.info("getting widget channel for whatsapp" + widgetChannel.Id);
                            }
                            var conversationMessage = new ConversationMessages();

                            conversationMessage.ConversationId = conversation_id;
                            conversationMessage.MessageTypeId = conversationMessages.MessageTypeId;
                            conversationMessage.AgentId = conversationMessages.AgentId;
                            conversationMessage.Message = conversationMessages.Message;
                            conversationMessage.CreatedTimeUTC = DateTime.UtcNow;
                            conversationMessage.IsDelivered = 0;
                            conversationMessage.ConversationTypeId = conversationMessages.ConversationTypeId;
                            conversationMessage.AttachmentUrl = conversationMessages.AttachmentUrl;
                            conversationMessage.AttachmentId = conversationMessages.AttachmentId;

                            _context.ConversationMessages.Add(conversationMessage);
                            await _context.SaveChangesAsync();

                            var response = new HttpResponseMessage();
                            var apiResponce = string.Empty;
                            var postData = new
                            {
                                sender = conversation_id,
                                message = conversationMessages.Message

                            };
                            if (is_endchat == 0)
                            {

                                using (var httpClient = new HttpClient())
                                {
                                    string BotUrl = "";

                                    if (conversationMessages.ConversationTypeId == 2)
                                    {

                                        LogProperties.info("Conversations bot url came");

                                        BotUrl = widgetChannel.RasaBotUrl;

                                        LogProperties.info("Conversations bot url : " + conversation_id.ToString() + ", " + widgetChannel.RasaBotUrl);
                                    }
                                    else
                                    {
                                        BotUrl = widget.RasaBotUrl;
                                    }
                                    var request = new HttpRequestMessage
                                    {

                                        RequestUri = new Uri(BotUrl + "webhooks/rest/webhook"),
                                        // RequestUri = new Uri("http://192.168.74.51:5005/webhooks/rest/webhook"),
                                        Method = new HttpMethod("Post"),
                                        Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json")
                                    };
                                    response = await httpClient.SendAsync(request);
                                    apiResponce = await response.Content.ReadAsStringAsync();
                                }

                                if (conversation.ConversationTypeId == 2)
                                {

                                    LogProperties.info("bot response: " + apiResponce);


                                    JArray MessagesArray = JArray.Parse(apiResponce);
                                    JObject LastJObject;
                                    LastJObject = (JObject)MessagesArray[MessagesArray.Count - 1];

                                    bool IsBotEndNow = false;

                                    // updating msg to jsaon array
                                    if (LastJObject.ContainsKey("custom") && Convert.ToString(LastJObject["custom"][0]["payload"]) == "Endchat")//    Convert.ToBoolean(JArray.Parse(apiResponce)[0]["custom"]["payload"]))
                                    {
                                        
                                        IsBotEndNow = true;
                                        

                                        string LastMessage = Convert.ToString(LastJObject["custom"][0]["text"]);

                                        LastJObject.Add(new JProperty("text", LastMessage));

                                    }
                                    // send messages to customer through whatsapp
                                    SendReplyToWhatsAppCustomer(conversation.Mobile, MessagesArray, widgetChannel.ChanneUUID, widgetChannel.Id);

                                    // updating botend flow in db
                                    if (IsBotEndNow)
                                    {
                                        var ConInfo = new ConversationInfo()
                                        {

                                            ConversationId = conversation_id,
                                            IsBotEnd = 2,
                                            CreatedTimeUtc = DateTime.UtcNow
                                        };

                                        LogProperties.info("agent look up : " + conversation_id.ToString() + ", before sending agent look up");

                                        

                                        ActionResult<Conversations> AgentLookUp = await PostToAgentLookup(account_id, widget_id, conversation_id, "customer_channel", "NEWCHAT");


                                        // JObject Con = await CreateConversation(AccountId, WidgetId, conversations);

                                        OkObjectResult okResult = AgentLookUp.Result as OkObjectResult;


                                        if (okResult.StatusCode == 200)
                                        {
                                            ConInfo.IsBotEnd = 1;
                                        }
                                        LogProperties.info("agent look up : " + conversation_id.ToString() + ", after sending agent look up");
                                        _context.ConversationInfo.Add(ConInfo);
                                        await _context.SaveChangesAsync();
                                        //agent look up

                                        //UpdateBotFlowEnd(int account_id, int widget_id, int conversation_id, byte botFlowEnd)
                                        ActionResult<Conversations> UpdateBotFlow = await UpdateBotFlowEnd(account_id, widget_id, conversation_id, 1);


                                        // JObject Con = await CreateConversation(AccountId, WidgetId, conversations);

                                        OkObjectResult okUpdatBotFlowResult = UpdateBotFlow.Result as OkObjectResult;

                                    }
                                }


                                _context.ConversationMessages.Remove(conversationMessage);
                                conversationMessage.Id = conversationMessage.Id + 1;
                                conversationMessage.ConversationId = conversation_id;
                                conversationMessage.MessageTypeId = 2;
                                conversationMessage.AgentId = conversationMessages.AgentId;
                                conversationMessage.Message = apiResponce;
                                conversationMessage.CreatedTimeUTC = DateTime.UtcNow;
                                conversationMessage.IsDelivered = 0;
                                conversationMessage.ConversationTypeId = conversationMessages.ConversationTypeId;

                                _context.ConversationMessages.Add(conversationMessage);
                                await _context.SaveChangesAsync();

                            }
                            else if (conversation.ConversationTypeId == 2 && conversationMessages.MessageTypeId == 4)
                            {
                                JArray AgentMessages = new JArray();

                                JObject AgntMessage = new JObject();
                                AgntMessage.Add(new JProperty("text", String.IsNullOrEmpty(conversationMessages.AttachmentId) ? conversationMessages.Message : ""));
                                AgntMessage.Add(new JProperty("MediaId", conversationMessages.AttachmentId));

                                AgentMessages.Add(AgntMessage);


                                if (widgetChannel != null)
                                {
                                    SendReplyToWhatsAppCustomer(conversation.Mobile, AgentMessages, widgetChannel.ChanneUUID, widgetChannel.Id);
                                }

                            }


                            return Ok(conversationMessage);

                        }
                        else
                        {
                            return BadRequest("No Such Conversation Exist");
                        }
                    }
                    else
                    {
                        return BadRequest("Incorrect Parameter");
                    }
                }
                else
                {
                    return BadRequest("No such widget");
                }

            }
            catch (Exception ex)
            {
                LogProperties.error("Error at insert messages: " + ex.ToString());
                return BadRequest(ex.ToString());


            }



        }

        //[HttpPost("{account_id}/widgets/{widget_id}/conversations/{conversation_id}/{is_endchat}/replies")]
        //public async Task<ActionResult<ConversationMessages>> PostConversationMessages(int account_id, int widget_id, int conversation_id, int is_endchat, ConversationMessages conversationMessages)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid data.");

        //    var conversation = await _context.Conversations.FindAsync(conversation_id);

        //    var widget = _context.Widgets.Where(w => w.Id == widget_id).FirstOrDefault();

        //    var botURL=widget.RasaBotUrl;


        //    if (widget != null)
        //    {

        //        if (widget.AccountId == account_id)
        //        {

        //            if (conversation.Id == conversation_id)
        //            {
        //                var conversationMessage = new ConversationMessages();

        //                conversationMessage.ConversationId = conversation_id;
        //                conversationMessage.MessageTypeId = conversationMessages.MessageTypeId;
        //                conversationMessage.AgentId = conversationMessages.AgentId;
        //                conversationMessage.Message = conversationMessages.Message;
        //                conversationMessage.CreatedTimeUTC = DateTime.UtcNow;
        //                conversationMessage.IsDelivered = 0;

        //                _context.ConversationMessages.Add(conversationMessage);
        //                await _context.SaveChangesAsync();

        //                var response = new HttpResponseMessage();
        //                var apiResponce = string.Empty;
        //                var postData = new
        //                {
        //                    sender = conversation_id,
        //                    message = conversationMessages.Message

        //                };
        //                if (is_endchat == 0)
        //                {

        //                    using (var httpClient = new HttpClient())
        //                    {
        //                        var request = new HttpRequestMessage
        //                        {
        //                            RequestUri = new Uri(widget.RasaBotUrl + "webhooks/rest/webhook"),
        //                            // RequestUri = new Uri("http://192.168.74.51:5005/webhooks/rest/webhook"),
        //                            Method = new HttpMethod("Post"),
        //                            Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json")
        //                        };
        //                        response = await httpClient.SendAsync(request);
        //                        apiResponce = await response.Content.ReadAsStringAsync();
        //                    }

        //                    _context.ConversationMessages.Remove(conversationMessage);
        //                    conversationMessage.Id = conversationMessage.Id + 1;
        //                    conversationMessage.ConversationId = conversation_id;
        //                    conversationMessage.MessageTypeId = 2;
        //                    conversationMessage.AgentId = conversationMessages.AgentId;
        //                    conversationMessage.Message = apiResponce;
        //                    conversationMessage.CreatedTimeUTC = DateTime.UtcNow;
        //                    conversationMessage.IsDelivered = 0;

        //                    _context.ConversationMessages.Add(conversationMessage);
        //                    await _context.SaveChangesAsync();

        //                }
        //                return Ok(conversationMessage);

        //            }
        //            else
        //            {
        //                return BadRequest("No Such Conversation Exist");
        //            }
        //        }
        //        else
        //        {
        //            return BadRequest("Incorrect Parameter");
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest("No such widget");
        //    }
        //}

        ///
        /// 
        [HttpPost("{account_id}/widgets/{widget_id}/conversations/{conversation_id}/{conversation_message_id}/UpdateStatusOfMsg")]
        public async Task<ActionResult<ConversationMessages>> PostUpdateConversationMessages(int account_id, int widget_id, int conversation_id, int conversation_message_id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var conversationMessage = _context.ConversationMessages.FirstOrDefault(item => item.Id == conversation_message_id);

            var widget = _context.Widgets.Where(w => w.Id == widget_id).FirstOrDefault();


            if (widget != null)
            {

                if (widget.AccountId == account_id)
                {

                    if (conversationMessage.Id == conversation_message_id)
                    {
                        conversationMessage.IsDelivered = 1;

                        _context.ConversationMessages.Update(conversationMessage);
                        await _context.SaveChangesAsync();

                        //return Ok(conversationMessage);
                        return Ok(conversationMessage);
                    }
                    else
                    {
                        return BadRequest("No Such Conversation Exist");
                    }
                }
                else
                {
                    return BadRequest("Incorrect Parameter");
                }
            }
            else
            {
                return BadRequest("No such widget");
            }
        }

        [HttpPost("{account_id}/widgets/{widget_id}/conversations/{conversation_id}/{customerId}/UpdateCustomerId")]
        public async Task<ActionResult<Conversations>> PostUpdateConversationCustomerId(int account_id, int widget_id, int conversation_id, int customerId)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var conversationId = _context.Conversations.FirstOrDefault(item => item.Id == conversation_id);

            var widget = _context.Widgets.Where(w => w.Id == widget_id).FirstOrDefault();


            if (widget != null)
            {

                if (widget.AccountId == account_id)
                {

                    if (conversationId.Id == conversation_id)
                    {
                        conversationId.CustomerId = customerId;

                        _context.Conversations.Update(conversationId);
                        await _context.SaveChangesAsync();

                        //return Ok(conversationMessage);
                        return Ok(conversationId);
                    }
                    else
                    {
                        return BadRequest("No Such Conversation Exist");
                    }
                }
                else
                {
                    return BadRequest("Incorrect Parameter");
                }
            }
            else
            {
                return BadRequest("No such widget");
            }
        }

        [HttpGet("{account_id}/widgets/{widget_id}/conversations/{customerId}/GetConversationsOfCustomer")]
        public async Task<ActionResult<Conversations>> GetConversationsOfCustomer(int account_id, int widget_id, int customerId)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            //var conversations = _context.Conversations.FirstOrDefault(item => item.CustomerId == customerId);

            var widget = _context.Widgets.Where(w => w.Id == widget_id).FirstOrDefault();


            if (widget != null)
            {

                if (widget.AccountId == account_id)
                {
                    var conversations = await _context.Conversations.Where(c => c.CustomerId == customerId && c.WidgetId == widget_id).Select(convo => new { ConversationId = convo.Id, AssignedAgent = (convo.AssignedAgentName == null ? "Bot" : convo.AssignedAgentName), ConversationMessages = convo.conversationMessages }).ToListAsync();
                    return Ok(conversations); ;
                }
                else
                {
                    return BadRequest("Incorrect Parameter");
                }
            }
            else
            {
                return BadRequest("No such widget");
            }
        }
        /// 
        ///
        [HttpPost("{account_id}/widgets/{widget_id}/conversations/{conversation_id}/{userId}/{agentName}/UpdateUserId")]
        public async Task<ActionResult<Conversations>> PostUpdateConversationUserId(int account_id, int widget_id, int conversation_id, string userId, string agentName)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var conversationId = _context.Conversations.FirstOrDefault(item => item.Id == conversation_id);

            var widget = _context.Widgets.Where(w => w.Id == widget_id).FirstOrDefault();


            if (widget != null)
            {

                if (widget.AccountId == account_id)
                {

                    if (conversationId.Id == conversation_id)
                    {
                        conversationId.UserId = userId;
                        conversationId.AssignedAgentName = agentName;

                        _context.Conversations.Update(conversationId);
                        await _context.SaveChangesAsync();

                        //return Ok(conversationMessage);
                        return Ok(conversationId);
                    }
                    else
                    {
                        return BadRequest("No Such Conversation Exist");
                    }
                }
                else
                {
                    return BadRequest("Incorrect Parameter");
                }
            }
            else
            {
                return BadRequest("No such widget");
            }
        }

        [HttpGet("{account_id}/widgets/{widget_id}/conversationsStatuses/")]
        public async Task<ActionResult<Widgets>> GetConversationStatus(int account_id, int widget_id)
        {
            IList<Conversations> conversations = new List<Conversations>();
            IList<ConversationMessages> conversationMessages = new List<ConversationMessages>();
            IList<ConversationHistory> conversationHistory = new List<ConversationHistory>();
            IList<Conversations> assignedChats = new List<Conversations>();

            List<int> intCollection = new List<int>();
            string StrDate = HttpContext.Request.Query["Date"].ToString();
            // string StrToDate = HttpContext.Request.Query["ToDate"].ToString();
            DateTime FromDate;
            // Int32 ChatsInitiated;
            try
            {
                if (!string.IsNullOrEmpty(StrDate))
                {
                    FromDate = DateTime.ParseExact(StrDate, "yyyy-MM-dd", null);
                    conversations = await _context.Conversations.Where(c => c.WidgetId == widget_id && c.CreatedTimeUTC.Date == FromDate.Date).ToListAsync();
                    ChatsInitiated = conversations.Count();
                    if (ChatsInitiated > 0)
                    {
                        //conversationMessages =
                        conversationMessages = await _context.ConversationMessages.Where(w => w.CreatedTimeUTC.Date >= FromDate.Date).ToListAsync();



                        var result = (from cm in conversationMessages
                                      join c in conversations on cm.ConversationId equals c.Id
                                      where cm.MessageTypeId == 4
                                      select cm.ConversationId).Distinct().ToList();
                        ChatsAttended = result.Count();



                        var botFlowEndChats = (from cm in conversationMessages
                                               join c in conversations on cm.ConversationId equals c.Id
                                               where cm.Message.Contains("Endchat")
                                               select cm.ConversationId).Distinct().ToList();
                        BotFlowCompleted = botFlowEndChats.Count();

                        var botFlowEndChats_grptalk = (from cm in conversationMessages
                                                       join c in conversations on cm.ConversationId equals c.Id
                                                       where (cm.Message.Contains("Endchat") || cm.Message.Contains("No worries! We are available on your beck and call if you need anything. Please continue browsing and have a great day!") || cm.Message.Contains("No problem :) Please feel free to initiate a conversation at any point of time. Have a great day!") || cm.Message.Contains("We understand. Please feel free to initiate a conversation at any point of time. Have a great day :)"))
                                                       select cm.ConversationId).Distinct().ToList();

                        BotFlowEndChats_grptalk = botFlowEndChats_grptalk.Count();

                        var closedchat = (from c in conversations
                                          where c.IsClosed == 1
                                          select c.Id).Distinct().ToList();
                        ClosedChats = closedchat.Count();

                        conversationHistory = await _context.ConversationHistory.Where(c => c.WidgetId == widget_id && c.CreatedTimeUTC.Date == FromDate.Date).ToListAsync();

                        var reeInitiatedChats = (from c in conversationHistory
                                                 where c.ToStatus == 3
                                                 select c.ConversationId).Distinct().ToList();
                        ReInitiatedChats = reeInitiatedChats.Count();

                        assignedChats = await _context.Conversations.Where(c => c.WidgetId == widget_id && c.UserId != null && c.CreatedTimeUTC.Date == FromDate.Date).ToListAsync();


                        ChatsAssignedByAgent = assignedChats.Count();

                    }

                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.ToString() });
            }


            

            FromDate = DateTime.ParseExact(StrDate, "yyyy-MM-dd", null);

 

            var newCustomerDetails = (from c in conversations
									  join cm in conversationMessages on  c.Id equals cm.ConversationId
                                      where c.WidgetId == widget_id && c.CreatedTimeUTC.Date == (DateTime.ParseExact(StrDate, "yyyy-MM-dd", null).Date)
                                       select new
                                        {  
											Id=c.Id,
                                            CustomerId = c.CustomerId,
                                            AgentConnected = (c.AssignedAgentName == null ? "No" : "Yes"),
											Reintiated1 = "No",
											Botflowcompleted1=(c.BotFlowCompleted == 0 ?  "No" : "Yes"),
											AssignedAgentName =(c.AssignedAgentName == null ? "" : c.AssignedAgentName)
                                        }).Distinct().ToList();
										
			  var ReintiatedCstDetails = (from c in conversations
										  join ch in conversationHistory on c.Id equals ch.ConversationId 
										  where ch.WidgetId == widget_id && ch.CreatedTimeUTC.Date == FromDate.Date &&
										  ch.ToStatus == 3
                                           select new
                                           {  
											    Id=ch.ConversationId,
                                               CustomerId = c.CustomerId,
											    AgentConnected = (c.AssignedAgentName == null ? "No" : "Yes"),
												Reintiated1 = "Yes",
												Botflowcompleted1=(c.BotFlowCompleted == 0 ?  "No" : "Yes"),
											   AssignedAgentName =(c.AssignedAgentName == null ? "" : c.AssignedAgentName)
                                           }).Distinct().ToList();




            //oldDetails=oldDetails,	

            return Ok(new
            {
                Success = true,
                CustDetails = newCustomerDetails,
                ReintiatedCstDetails = ReintiatedCstDetails,
                chatsInitiated = ChatsInitiated,
                chatsAttended = ChatsAttended,
                botFlowCompleted = BotFlowCompleted,
                botFlowEndChats_grptalk = BotFlowEndChats_grptalk,
                closedChats = ClosedChats,
                reInitiatedChats = ReInitiatedChats,
                chatcsAssignedByAgent = ChatsAssignedByAgent
            });
			


            // return Ok(new { Success = true, chatsInitiated = ChatsInitiated, chatsAttended = ChatsAttended });
        }
        [HttpPost("{account_id}/widgets/{widget_id}/conversations/{conversation_id}/customerchannel/{customer_channel}/action/{serviceaction}")]
        public async Task<ActionResult<Conversations>> PostToAgentLookup(int account_id, int widget_id, int conversation_id, string customer_channel, string serviceaction)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var widget = await _context.Widgets.FindAsync(widget_id);

            if (widget != null)
            {
                if (widget.AccountId == account_id)
                {
                    var response = new HttpResponseMessage();
                    var apiResponse = string.Empty;

                    var postData = new
                    {
                        AccountId = @account_id,
                        Action = serviceaction,
                        Channel = customer_channel,
                        WidgetId = widget_id,
                        ConversationId = conversation_id

                    };

                    string AgentLookUpServiceUrl = _configuration.GetValue<string>("RequestUrls:AgentLookUpServiceUrl");

                    using (var httpClient = new HttpClient())
                    {
                        var request = new HttpRequestMessage
                        {
                            RequestUri = new Uri(AgentLookUpServiceUrl),
                            Method = new HttpMethod("Post"),
                            //JsonConvert.SerializeObject(/
                            Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json")
                        };
                        response = await httpClient.SendAsync(request);

                        apiResponse = await response.Content.ReadAsStringAsync();

                        //receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);


                    }
                    return Ok(apiResponse);
                }
                else
                {
                    return BadRequest("AccountId and WidgetId is not concerned with any widget.");
                }
            }
            else
            {
                return BadRequest("No such widget");
            }
        }

        [HttpPost("{account_id}/widgets/{widget_id}/conversations/{conversation_id}/{Isclosed}/ClosedCustomerChat")]
        public async Task<ActionResult<Conversations>> ClosedCustomerChat(int account_id, int widget_id, int conversation_id, byte isClosed)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var conversationId = _context.Conversations.FirstOrDefault(item => item.Id == conversation_id);

            var widget = _context.Widgets.Where(w => w.Id == widget_id).FirstOrDefault();


            if (widget != null)
            {

                if (widget.AccountId == account_id)
                {

                    if (conversationId.Id == conversation_id)
                    {
                        conversationId.IsClosed = isClosed;
                        conversationId.ClosingTimeUTC = DateTime.UtcNow;

                        _context.Conversations.Update(conversationId);
                        await _context.SaveChangesAsync();
                        //  var conversationHistory = new ConversationHistory();
                        //if (isClosed == 1)
                        //{
                        //    var convHistory = _context.ConversationHistory.FirstOrDefault(item => item.ConversationId == conversation_id);
                        //    if (convHistory.ConversationId == conversation_id)
                        //    {
                        //        conversationHistory.AccountId = account_id;
                        //        conversationHistory.WidgetId = widget_id;
                        //        conversationHistory.ConversationId = conversation_id;
                        //        conversationHistory.FromStatus = 3;
                        //        conversationHistory.ToStatus = 2;
                        //    }
                        //    else
                        //    {
                        //        conversationHistory.AccountId = account_id;
                        //        conversationHistory.WidgetId = widget_id;
                        //        conversationHistory.ConversationId = conversation_id;
                        //        conversationHistory.FromStatus = 1;
                        //        conversationHistory.ToStatus = 2;
                        //    }
                        //}
                        //else if(isClosed == 0){
                        //    conversationHistory.AccountId = account_id;
                        //    conversationHistory.WidgetId = widget_id;
                        //    conversationHistory.ConversationId = conversation_id;
                        //    conversationHistory.FromStatus = 2;
                        //    conversationHistory.ToStatus = 3;
                        //}
                        //_context.ConversationHistory.Add(conversationHistory);
                        //await _context.SaveChangesAsync();
                        //return Ok(conversationMessage);
                        return Ok(conversationId);

                    }
                    else
                    {
                        return BadRequest("No Such Conversation Exist");
                    }
                }
                else
                {
                    return BadRequest("Incorrect Parameter");
                }
            }
            else
            {
                return BadRequest("No such widget");
            }
        }

        public void SendReplyToWhatsAppCustomer(string CustomerNumber, JArray Messages, string ChannelUUId, int WidgetChannelId)
        {


            try
            {
                WidgetAuthkeys WidgetAuthKeys = (from Auth in _context.WidgetAuthkeys
                                                 where Auth.WidgetChannelId == WidgetChannelId
                                                 select Auth).FirstOrDefault();

                string AuthKey, AuthToken;
                string Authorization;

                AuthKey = WidgetAuthKeys.AuthKey;
                AuthToken = WidgetAuthKeys.AuthToken;
                var AuthBytes = System.Text.Encoding.UTF8.GetBytes(AuthKey + ":" + AuthToken);
                Authorization = "Basic " + Convert.ToBase64String(AuthBytes);

                string url = _configuration.GetValue<string>("RequestUrls:BluekiteUrl");
                url += AuthKey + "/Whatsapp/";
                url += ChannelUUId + "/Messages/";

                for (int i = 0; i < Messages.Count; i++)
                //foreach (JObject Message in Messages)
                {
                    JObject Message = (JObject)Messages[i];
                    //// send bot response as a reply to customer
                    SendWhatsAppMessage(Message, url, CustomerNumber, Authorization);
                }
            }
            catch (Exception ex)
            {
                LogProperties.error("Exception at SendReplyToWhatsAppCustomer: " + ex.ToString());

            }
        }

        [HttpGet("{account_id}/widgets/{widget_id}/conversations/{conversationId}/getConversationEndchat")]
        public async Task<ActionResult<Conversations>> getConversationEndchat(int account_id, int widget_id, int conversationId)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            //var conversations = _context.Conversations.FirstOrDefault(item => item.CustomerId == customerId);

            var widget = _context.Widgets.Where(w => w.Id == widget_id).FirstOrDefault();


            if (widget != null)
            {

                if (widget.AccountId == account_id)
                {
                    var conversations = await _context.Conversations.Where(c => c.Id == conversationId && c.WidgetId == widget_id).Select(convo => new { getConversationEndchat = convo.IsClosed,BotFlowCompleted = convo.BotFlowCompleted }).ToListAsync();
                    return Ok(conversations); ;
                }
                else
                {
                    return BadRequest("Incorrect Parameter");
                }
            }
            else
            {
                return BadRequest("No such widget");
            }
        }

        [HttpPost("{account_id}/conversations/{conversation_id}/agents/{agent_id}/{agent_name}")]
        public async Task<ActionResult<Conversations>> UpdateConversationAgent(int account_id, int conversation_id, int agent_id, string agent_name)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");


            try
            {
                var conversation = _context.Conversations.FirstOrDefault(item => item.Id == conversation_id);

                if (conversation != null)
                {
                    conversation.AgentId = agent_id;

                    _context.Conversations.Update(conversation);
                    await _context.SaveChangesAsync();

                    if (conversation.ConversationTypeId == 2)
                    {
                        //var widgetChannel = _context.WidgetChannels.Where(wc => wc.WidgetId == conversation.WidgetId && wc.ConversationTypeId == 2).FirstOrDefault();

                        //    JArray jArray = new JArray();
                        //    JObject jObject = new JObject();
                        //    jObject.Add(new JProperty("text", "you are now connected with Agent"));
                        //    jArray.Add(jObject);
                        //    SendReplyToWhatsAppCustomer(conversation.Mobile, jArray, widgetChannel.ChanneUUID, widgetChannel.Id);


                        var AccountCustomers = _context.MasterCustomers.FirstOrDefault(item => item.AccountCustomerId == conversation.CustomerId);
                        CustomerOnboardMessageToAgentThrghWhastApp(conversation, agent_name, AccountCustomers.CustomerName);


                    }
                }
            }
            catch (Exception ex)
            {

                LogProperties.error("expection at UpdateConversationAgent: " + ex.ToString());
            }


            return Ok("Success.");

        }


        public void CustomerOnboardMessageToAgentThrghWhastApp(Conversations conversation, string agent_name, string CustomerName)
        {
            try
            {
                var widgetChannel = _context.WidgetChannels.Where(wc => wc.WidgetId == conversation.WidgetId && wc.ConversationTypeId == 2).FirstOrDefault();

                //Agent connect message to customer

                JArray jArray = new JArray();
                JObject jObject = new JObject();
                jObject.Add(new JProperty("text", "you are now connected with Agent: " + agent_name));
                jArray.Add(jObject);
                SendReplyToWhatsAppCustomer(conversation.Mobile, jArray, widgetChannel.ChanneUUID, widgetChannel.Id);

                // onboard customer to agent.

                JObject Onboardjobjs = new JObject();
                JObject wsOnboardMessageObj = new JObject();
                JObject CustomerDetails = new JObject();

                //                "data":{"fromName" :"" ,"fromMobile",""}

                CustomerDetails.Add(new JProperty("fromName", CustomerName));
                CustomerDetails.Add(new JProperty("fromMobile", conversation.Mobile));

                Onboardjobjs.Add(new JProperty("conversationId", Convert.ToString(conversation.Id)));
                Onboardjobjs.Add(new JProperty("from", conversation.Mobile));
                Onboardjobjs.Add(new JProperty("to", "Hub_" + conversation.AgentId));
                Onboardjobjs.Add(new JProperty("message", "Customer On-Boarded With Number: " + conversation.Mobile));
                Onboardjobjs.Add(new JProperty("data", CustomerDetails));
                Onboardjobjs.Add(new JProperty("messageType", "0"));
                Onboardjobjs.Add(new JProperty("ConversationTypeId", 2));

                wsOnboardMessageObj = new JObject(new JProperty("Module", "Chat"),
                                new JProperty("Event", "NewChat"),
                                new JProperty("Channel_Name", "WhatsApp_Hub_" + conversation.AgentId),
                                new JProperty("Data", Onboardjobjs));

                string SocketPublishUrl = _configuration.GetValue<string>("RequestUrls:WebSocketPublish"); ;
                WebSocketSubscriber.pushSocket(SocketPublishUrl, wsOnboardMessageObj);

                //// onboard customer details to agent.
                ////"Name:" + localStorage.getItem("callerName") + ",Mobile:" + localStorage.getItem("callerMobile");

                //JObject Customerjobjs = new JObject();
                //JObject wsMessageObj = new JObject();

                //JObject CustomerDetails = new JObject();
                //string CustomerDetails = "Name:" + CustomerName + ",Mobile:" + conversation.Mobile ;

                //Customerjobjs.Add(new JProperty("conversationId", conversation.Id));
                //Customerjobjs.Add(new JProperty("from", conversation.Mobile));
                //Customerjobjs.Add(new JProperty("to", "Hub_" + conversation.AgentId));
                //Customerjobjs.Add(new JProperty("message", "Customer On-Boarded With Number: " + conversation.Mobile));
                //Customerjobjs.Add(new JProperty("messageType", "0"));
                //Customerjobjs.Add(new JProperty("ConversationTypeId", 2));

                //wsMessageObj = new JObject(new JProperty("Module", "Chat"),
                //                new JProperty("Event", "NewChat"),
                //                new JProperty("Channel_Name", "WhatsApp_Hub_" + conversation.AgentId),
                //                new JProperty("Data", Customerjobjs));


                //WebSocketSubscriber.pushSocket(SocketPublishUrl, wsMessageObj);


            }
            catch (Exception ex)
            {
                LogProperties.error("expection at CustomerOnboardMessageToAgentThrghWhastApp: " + ex.ToString());

            }


        }

        public void SendWhatsAppMessage(JObject Message, string url, string CustomerNumber, string Authorization)
        {
            try
            {
                LogProperties.info("SendWhatsAppMessage called : " + Message.ToString() + " , url " + url + " , CustomerNumber " + CustomerNumber + " , Authorization : " + Authorization);
                HttpWebRequest objWebRequest = null;
                HttpWebResponse objWebResponse = null;
                StreamWriter objStreamWriter = null;
                StreamReader objStreamReader = null;

                JObject jresponse = new JObject();

                string MediaId = Convert.ToString(Message["MediaId"]);

                jresponse.Add(new JProperty("Text", Convert.ToString(Message["text"])));
                jresponse.Add(new JProperty("Number", CustomerNumber));
                jresponse.Add(new JProperty("MediaId", String.IsNullOrEmpty(MediaId) ? "1" : MediaId));
                jresponse.Add(new JProperty("MessageType", String.IsNullOrEmpty(MediaId) ? "Text" : "Attachment"));
                jresponse.Add(new JProperty("Tool", "API"));
                jresponse.Add(new JProperty("TemplateID", ""));

                Uri uri = new Uri(url);

                //objWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.75.44:420/v0.1/Accounts/sIkXPrYqDPe6xxjZgT1z/Whatsapp/2df4b63c-135a-4206-91d3-8132dfe64f22/Messages/");
                objWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                objWebRequest.Method = "POST";
                objWebRequest.ContentType = "application/json";

                //objWebRequest.Headers.Add("authorization", "Basic c0lrWFByWXFEUGU2eHhqWmdUMXo6aGQ1VzJkMmJjQXpzOHRtUk1hd3hXSlR3MXFXcFZlNW9nMHNNMkNNVg==");
                objWebRequest.Headers.Add("authorization", Authorization);
                objStreamWriter = new StreamWriter(objWebRequest.GetRequestStream());

                objStreamWriter.Write(jresponse);
                objStreamWriter.Flush();
                objStreamWriter.Close();
                objWebResponse = (HttpWebResponse)objWebRequest.GetResponse();
                objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
                string stringResult = objStreamReader.ReadToEnd();
                LogProperties.info("SendWhatsAppMessage Response: " + stringResult);
                objStreamReader.Close();

            }
            catch (Exception ex)
            {
                LogProperties.error("expection at SendWhatsAppMessage: " + ex.ToString());

            }
        }

        [HttpPost("{account_id}/widgets/{widget_id}/conversations/{conversation_id}/{botFlowEnd}/UpdateBotFlowEnd")]
        public async Task<ActionResult<Conversations>> UpdateBotFlowEnd(int account_id, int widget_id, int conversation_id, byte botFlowEnd)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var conversationId = _context.Conversations.FirstOrDefault(item => item.Id == conversation_id);

            var widget = _context.Widgets.Where(w => w.Id == widget_id).FirstOrDefault();


            if (widget != null)
            {

                if (widget.AccountId == account_id)
                {

                    if (conversationId.Id == conversation_id)
                    {
                        conversationId.BotFlowCompleted = botFlowEnd;
                        _context.Conversations.Update(conversationId);
                        await _context.SaveChangesAsync();
                        return Ok(conversationId);

                    }
                    else
                    {
                        return BadRequest("No Such Conversation Exist");
                    }
                }
                else
                {
                    return BadRequest("Incorrect Parameter");
                }
            }
            else
            {
                return BadRequest("No such widget");
            }
        }
    }
}
