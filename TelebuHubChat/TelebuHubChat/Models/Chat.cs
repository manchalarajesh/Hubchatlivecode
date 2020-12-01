using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelebuHubChat.Models
{
    public class Chat
    {
       
            public int WidgetId { get; set; }
            public int AccountId { get; set; }
            public int IsAgent { get; set; }
            public int IsPing { get; set; }
            public string url { get; set; }
            public string HttpReferrer { get; set; }
	 	  public string WidgetUuid {get;set;}

      
    }
}
