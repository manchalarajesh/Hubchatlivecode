
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using RestSharp;
using TelebuHubChat.DbContexts;
using TelebuHubChat.Helpers;
using TelebuHubChat.LogClasses;
using TelebuHubChat.Models;

namespace TelebuHubChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppHubController : ControllerBase
    {
        private readonly TelebuHubChatContext _context;
        private readonly IConfiguration _configuration;
        public WhatsAppHubController(TelebuHubChatContext context, IConfiguration Configuration)
        {
            _context = context;
            _configuration = Configuration;
        }
        // GET: api/WhatsAppHub
        [HttpGet]
        public IEnumerable<string> Get()
        {
            LogProperties.info("Whats api get called");
            return new string[] { "value1", "value2" };
        }

        // GET: api/WhatsAppHub/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/WhatsAppHub
        [HttpPost]
        public async Task PostAsync([FromBody] JObject MessageJobj)
        {


            bool outgoing;
            string ChannelUUId = "";

            string SocketPublishUrl = _configuration.GetValue<string>("RequestUrls:WebSocketPublish");

            string MdeiaType = "0";
            LogProperties.info("Whats api called");


            try

            {
                outgoing = (bool)MessageJobj["outgoing"];
                //               "sender": "919966226645",
                //"recipient": "2df4b63c-135a-4206-91d3-8132dfe64f22",
                if (!outgoing)
                {

                    ChannelUUId = Convert.ToString(MessageJobj["recipient"]);
                }

				WidgetChannels widgetChannel = null;
				Widgets widget = null;
			    try
			    {
						widgetChannel = _context.WidgetChannels.Where(wc => wc.ChanneUUID == ChannelUUId && wc.ConversationTypeId == 2).FirstOrDefault();
				        LogProperties.info("widgetChannel for message: " + widgetChannel.WidgetId );
						
						widget = _context.Widgets.Where(w => w.Id == widgetChannel.WidgetId).FirstOrDefault();
						LogProperties.info("widget for message: " + widgetChannel.WidgetId );			
				}
				catch(Exception dbEx)
				{
						widgetChannel = _context.WidgetChannels.Where(wc => wc.Id == 2).FirstOrDefault();
				        LogProperties.info("widgetChannel for message catch: " + widgetChannel.WidgetId );
						
						 widget = _context.Widgets.Where(w => w.Id == 6).FirstOrDefault();
						LogProperties.info("widget for message catch: " + widget.Id );	
					
				}
                // var widgetChannel = _context.WidgetChannels.Where(wc => wc.ChanneUUID == ChannelUUId && wc.ConversationTypeId == 2).FirstOrDefault();
				// LogProperties.info("widgetChannel for message: " + widgetChannel.WidgetId );
				
                

                //[HttpPost("{account_id}/widgets/{widget_id}/conversations/{conversation_id}/{is_endchat}/replies")]
                if (widget != null)
                {
                    if (!outgoing)
                    {
                        JObject jObject = new JObject();
                        jObject = JObject.Parse(MessageJobj.ToString());
                        //    await ProcessIncomingMessageAsync(widget, MessageJobj);

						LogProperties.info("Whats api ougoing enter");
                        string CustomerMobile = "";
                        string CustomerName = "";
                        string CustomerProfileImage = "";

                        int AccountId = widget.AccountId;
                        int WidgetId = widget.Id;
                        int ConverstationId = 0;
                        long ConverstationMessageId = 0;
                        int CustomerId = 0;
                        JObject CustJobj = new JObject();

                        CustomerMobile = Convert.ToString(MessageJobj["sender"]);
                        ChannelUUId = Convert.ToString(MessageJobj["recipient"]);
                        CustomerName = Convert.ToString(MessageJobj["payload"]["user"]["name"]);
                        CustomerProfileImage = Convert.ToString(MessageJobj["payload"]["user"]["image"]);

                        ConversationsController conController = new ConversationsController(_context, _configuration);



                        CustJobj = GetCustomerId(AccountId, CustomerName, CustomerMobile, CustomerProfileImage);
                        CustomerId = Convert.ToBoolean(CustJobj["Success"]) ? Convert.ToInt32(CustJobj["CustomerId"]) : -1;
                        LogProperties.info("Whats api after get customer");
                       var PrevConverstations = (from Con in _context.Conversations
                                           where Con.WidgetId == WidgetId && Con.CustomerId == CustomerId
                                           orderby Con.Id descending
                                           select new { 
                                               Con.Id ,
                                               Con.AgentId}).FirstOrDefault();

		            	if (PrevConverstations != null)
                        {
                            ConverstationId = PrevConverstations.Id;
                        }
						LogProperties.info("Whats api after Conversations");
                        var PrevConMessages = (from ConMess in _context.ConversationMessages
                                               where ConMess.ConversationId == ConverstationId 
                                               orderby ConMess.Id descending
                                               select new
                                               {
                                                   ConMess.Id,
                                                   ConMess.CreatedTimeUTC,
                                                   ConMess.AgentId
                                               }).FirstOrDefault();

                        DateTime RecentMsgTime;
                        if (PrevConMessages != null && PrevConMessages.Id > 0)
                        {
                            ConverstationMessageId = PrevConMessages.Id;
                            RecentMsgTime = PrevConMessages.CreatedTimeUTC;
                        }
                        else
                        {

                            RecentMsgTime = DateTime.Today.AddDays(-10);
                        }

						LogProperties.info("Whats api after Conversation messages");

                        DateTime CurrentUtcNow = DateTime.UtcNow;
                        TimeSpan MessageSpan = CurrentUtcNow - RecentMsgTime;


                        double totalSecForDay = 3 * 60 * 60; // Static condiftion
                        bool IsNewChat = false;
                        if (MessageSpan.TotalSeconds > totalSecForDay)  // create new conversations after 24 hours
                        {

                            Conversations conversations = new Conversations()
                            {
                                StatusId = 1,
                                CustomerId = CustomerId,
                                Mobile = CustomerMobile,
                                ConversationTypeId = 2
                            };


                            ActionResult<Conversations> newCon = await conController.PostConversations(AccountId, WidgetId, conversations);

                            // JObject Con = await CreateConversation(AccountId, WidgetId, conversations);

                            OkObjectResult okResult = newCon.Result as OkObjectResult;

                            if (okResult.StatusCode == 200)
                            {
                                Conversations NewConversation = okResult.Value as Conversations;
                                ConverstationId = NewConversation.Id;
                            }



                            IsNewChat = true;
                        }
						LogProperties.info("Whats api after Conversation save");
                        ConversationMessages conversationMessages = new ConversationMessages();
                        int IsBotEnd = 0;
                        //"MessageTypeId": "1",
                        //    "Message":msg
                        conversationMessages.ConversationId = ConverstationId;
                        conversationMessages.MessageTypeId = 1;
                        conversationMessages.ConversationTypeId = 2;

                        if (IsNewChat == false)
                        {
                            conversationMessages.AgentId = PrevConverstations.AgentId;
                        }
                        //message payload table
                        conversationMessages.Message = Convert.ToString(MessageJobj["payload"]["text"]);
                        string CustomerMessage = Convert.ToString(MessageJobj["payload"]["text"]);

                        string MessageType = Convert.ToString(MessageJobj["payload"]["type"]);


                        if (MessageType != "text")
                        {
                            //attachment
                                
                            conversationMessages.AttachmentUrl = Convert.ToString(MessageJobj["payload"]["attachment"]);

                            conversationMessages.Message = ConstructMediaMessage(conversationMessages.AttachmentUrl, MessageType, conversationMessages.Message, ref MdeiaType);
                        }


                        var ConInfo = _context.ConversationInfo.Where(w => w.ConversationId == ConverstationId).FirstOrDefault();
                        if (ConInfo != null)
                        {
                            IsBotEnd = ConInfo.IsBotEnd;
                        }
						if (IsBotEnd > 0)
                        {
							conversationMessages.MessageTypeId = 3;
						}
                        ActionResult<ConversationMessages> newConMsg = await conController.PostConversationMessages(AccountId, WidgetId, ConverstationId, IsBotEnd, conversationMessages);



                        OkObjectResult okResultMesg = newConMsg.Result as OkObjectResult;

                        if (okResultMesg.StatusCode == 200)
                        {
                            ConversationMessages NewConvMsg = okResultMesg.Value as ConversationMessages;
                        }

						LogProperties.info("Whats api after Conversation message save");

                        if (IsBotEnd > 0)
                        {
                            // table for botend

                            //if (IsPingAgent == "true")
                            //{
                            //    cust_jsonObj = { "conversationId": conversationId, "from": conversationId, "to": agentId, "messageType": "0", "message": "Customer OnBoard" };
                            //}
                            //else
                            //{
                            //    cust_jsonObj = { "conversationId": conversationId, "from": fromId, "to": agentId, "messageType": "0", "message": "Customer OnBoard" };
                            //}
                            if (IsNewChat == false && IsBotEnd > 0) // send message to agent through socket
                            {


                                //JObject jobj = new JObject();
                                //jobj.Add(new JProperty("conversationId", conversationMessages.ConversationId));
                                //jobj.Add(new JProperty("from", "WhatsApp_"+ conversationMessages.ConversationId));
                                //jobj.Add(new JProperty("to", "Hub_" + conversationMessages.AgentId));
                                //jobj.Add(new JProperty("message", conversationMessages.Message));
                                //jobj.Add(new JProperty("messageType", "0"));

                                //WebSocketSubscriber.Send(jobj);

                                JObject jobjs = new JObject();
                                JObject wsMessageObj = new JObject();


                                JObject CustomerDetails = new JObject();

                                //                "data":{"fromName" :"" ,"fromMobile",""}

                                CustomerDetails.Add(new JProperty("fromName", CustomerName));
                                CustomerDetails.Add(new JProperty("fromMobile", CustomerMobile));

                                jobjs.Add(new JProperty("conversationId", Convert.ToString(ConverstationId)));
                                jobjs.Add(new JProperty("from", "WhatsApp_5f2d3a8e31197a445686653b"));
                                jobjs.Add(new JProperty("to", "Hub_" + PrevConverstations.AgentId));
                                jobjs.Add(new JProperty("message", CustomerMessage));
                                jobjs.Add(new JProperty("mediaUrl", conversationMessages.AttachmentUrl));
                                jobjs.Add(new JProperty("type", MdeiaType));
                                jobjs.Add(new JProperty("data", CustomerDetails));
                                jobjs.Add(new JProperty("messageType", "0"));
                                jobjs.Add(new JProperty("ConversationTypeId", 2));

                                wsMessageObj = new JObject(new JProperty("Module", "Chat"),
                                                new JProperty("Event", "NewChat"),
                                                new JProperty("Channel_Name", "WhatsApp_Hub_" + PrevConverstations.AgentId),
                                                new JProperty("Data", jobjs));


                                // WebSocketSubscriber wb = new WebSocketSubscriber();
                                WebSocketSubscriber.pushSocket(SocketPublishUrl, wsMessageObj);
								
								LogProperties.info("Whats api after websocket publish");
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
		LogProperties.error("expection at WhatsApp Handler: " + ex.ToString());
            }
        }

        
        

      

        public JObject GetCustomerId(int AccountId, string callerName, string callerMobile,string ProfilePic)
        {
            JObject res = new JObject();
            int CustomerId = 0;
            try
            {
                CustomerId = (from Cust in _context.MasterCustomers
                              where Cust.AccountId == AccountId && Cust.Mobile == callerMobile
                              select Cust.AccountCustomerId).FirstOrDefault();

                if (CustomerId == 0)
                {

                    JObject cust = new JObject();
                    cust.Add(new JProperty("Name", callerName));
                    cust.Add(new JProperty("Mobile", callerMobile));
                    //cust.Add(new JProperty("ProfilePic", ProfilePic));


                    string url = _configuration.GetValue<string>("RequestUrls:CustomerApiUrl");
                    url += Convert.ToString(AccountId) + "&Name=" + callerName + "&Mobile=" + callerMobile  + "&Details=" + Convert.ToString(cust);
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    var response = (HttpWebResponse)request.GetResponse();
                    string responseString;

                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            responseString = reader.ReadToEnd();
                        }
                    }
                    if (responseString != "")
                    {
                        res = JObject.Parse(responseString);
                    }

                    MasterCustomers Mc = new MasterCustomers();

                    Mc.AccountId = AccountId;
                    Mc.CustomerName = callerName;
                    Mc.Mobile = callerMobile;
                    Mc.AccountCustomerId = Convert.ToInt32(res["customerId"]);


                    CustomerId = Mc.AccountCustomerId;

                    _context.MasterCustomers.Add(Mc);
                    _context.SaveChanges();
                }

                res.Add(new JProperty("Success", true));
                res.Add(new JProperty("Message", "Succes"));
                res.Add(new JProperty("CustomerId", CustomerId));
            }
            catch (Exception ex)
            {

                res.Add(new JProperty("Success", false));
                res.Add(new JProperty("Message", ex.ToString()));
                LogProperties.error("expection at get Customer: " + ex.ToString());
				
            }

            return res;
        }



        public string ConstructMediaMessage(string AttachmentUrl, string type, string Message, ref string MdeiaType)
        {
            string MediaMessage = "";
            try
            {
                JArray MediaArray = new JArray();
                JObject MediaJobj = new JObject();
                JObject PayloadJobj = new JObject();

                 MdeiaType = "1";

                string FileType = type;
                string FileExtension = "";
                string[] FilePartArray = AttachmentUrl.Split('.');

                FileExtension = FilePartArray[FilePartArray.Length - 1];

                //if (jsonData.type == "1") image
                //else if (jsonData.type == "2") video
                //else if (jsonData.type == "6") doc
                //else if (jsonData.type == "0") text

                //if (jsonData.type == "1" || jsonData.type == "6" || jsonData.type == "2")
                //{

                //         jsonObj = {
                //             "conversationId": conversationId, "from": fromId, "to": agentId, "messageType": msgType,
                //"message": "", "mediaUrl": filePath };
                if (type == "image")
                {
                    MdeiaType = "1";
                    FileType = "image";
                }
                else if (type == "video" || type == "audio")
                {
                    MdeiaType = "2";
                    FileType = type;
                }
                //else if (type == "audio")
                //{
                //    MdeiaType = "2";
                //    FileType = "audio";
                //}
                else if (type == "document")
                {
                    MdeiaType = "6";

                    if (FileExtension == "xls" || FileExtension == "xlsx" || FileExtension == "csv")
                    {

                        FileType = "excel";
                    }
                    else if (FileExtension == "pdf" || FileExtension == "docx" || FileExtension == "doc")
                    {
                        FileType = "pdf";
                    }
                    else
                    {
                        FileType = "excel";
                    }
                }
                else
                {
                    MdeiaType = "0";
                }
                
                if (type != "video")
                {
                    JArray MediaJobjArray = new JArray();



                    PayloadJobj.Add(new JProperty("payload", FileType));
                    PayloadJobj.Add(new JProperty("text", AttachmentUrl));
                    PayloadJobj.Add(new JProperty("message", Message));

                    MediaJobjArray.Add(PayloadJobj);


                    MediaJobj.Add(new JProperty("custom", MediaJobjArray));

                    
                    //PayloadJobj["payload"] = type;
                    //MediaJobj["text"] = AttachmentUrl;
                    //MediaJobj["message"] = Message;
                    //MediaMessage = @"[{"custom":[{"payload":"image","text":"' + filePath + '"}]}]";
                }
                else 
                {

                   // MessageType = "2";

                    JObject SrcJobj = new JObject();
                   
                    
                    SrcJobj.Add(new JProperty("src", AttachmentUrl));

                    PayloadJobj.Add(new JProperty("payload", SrcJobj));
                    PayloadJobj.Add(new JProperty("type", FileType));

                    MediaJobj.Add(new JProperty("attachment", PayloadJobj));
                }

                MediaArray.Add(MediaJobj);

                MediaMessage = MediaArray.ToString();

                //   [{"attachment":{"payload":{"src":"http://staging-ping.telebu.com/uploads/chat/3180-1601296120820.mp3"},"type":"video"}}]


                //if (msgType == "1")
                //    hitMsg = '[{"custom":[{"payload":"image","text":"' + filePath + '"}]}]';
                //else if (msgType == "2")
                //    hitMsg = '[{"attachment":{"payload":{"src":"' + filePath + '"},"type":"video"}}]';
                //else if (msgType == "6" && (extnsn == "xls" || extnsn == "xlsx"))
                //    hitMsg = '[{"custom":[{"payload":"excel","text":"' + filePath + '"}]}]';
                //else if (msgType == "6" && extnsn == "pdf")
                //    hitMsg = '[{"custom":[{"payload":"pdf","text":"' + filePath + '"}]}]';

            }
            catch (Exception ex)
            {
                LogProperties.error("Exception at ConstructMediaMessage " + ex.ToString());
            }

            return MediaMessage;


        }

        // PUT: api/WhatsAppHub/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
