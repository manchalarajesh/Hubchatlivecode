using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelebuHubChat.Models
{
    [Table("Conversations")]
    public class Conversations
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public Int32 WidgetId { get; set; }

        [ForeignKey("WidgetId")]
        public Widgets Widgets { get; set; }

        public Int32 StatusId { get; set; }

        [ForeignKey("StatusId")]
        public ConversationStatuses ConversationStatuses { get; set; }

        public string BrowserUserAgent { get; set; }

        public string CustomerIPAddress { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTimeUTC { get; set; } = DateTime.UtcNow;

        public DateTime? AgentRequestedTimeUTC { get; set; } 

        public DateTime? AgentAllocatedTimeUTC { get; set; } 

        public DateTime? AgentAcceptedTimeUTC { get; set; } 

        public DateTime? FirstAgentMessageTimeUTC { get; set; } 

        public DateTime? ClosingTimeUTC { get; set; }

        public bool? ClosedByCustomer { get; set; }

        public ICollection<ConversationMessages> conversationMessages { get; set; }

        public ICollection<NavigationHistory> navigationHistories { get; set; }

        public ICollection<ConversationHistory> conversationHistories { get; set; }

        public Int64 CustomerId { get; set; }

        public string UserId { get; set; }

        public byte IsClosed { get; set; }
	
	 public string AssignedAgentName { get; set; }

	  public string Mobile { get; set; }
        public Int32 ConversationTypeId { get; set; }
        public Int32 AgentId { get; set; }
	 public Int32 BotFlowCompleted { get; set; }
    }
}
