using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelebuHubChat.Models
{
    [Table("Widgets")]
    public class Widgets
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "AccountId should not be empty")]
        public Int32 AccountId { get; set; }

        public string WidgetName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime ExpiryDateUTC { get; set; }

        public sbyte PopInAfterSeconds { get; set; } = 0;

        public Int32 ThemeId { get; set; }

        [ForeignKey("ThemeId")]
        public WidgetThemes WidgetThemes { get; set; }

        public Int32 WorkFlowId { get; set; }

        [ForeignKey("WorkFlowId")]
        public WorkFlows WorkFlows { get; set; }

        //[NotMapped]
        public string Purpose { get; set; }

        [MaxLength(1500)]
        public string MetaData { get; set; }

        //public Int32 PurposeId { get; set; }
        //[ForeignKey("PurposeId")]
        //public Purposes Purposes { get; set; }

        public string MinimizeStateText { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9]{32})$")]
        public string UUID { get; set; }

        public string DomainToLoadIn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTimeUTC { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedTimeUTC { get; set; } = DateTime.UtcNow;

        //[NotMapped]
        //public string MetaData { get; set; }

        public ICollection<Conversations> conversations { get; set; }

        [MaxLength(1500)]
        public string CustomMetaData { get; set; }

        public Int32 AgentAndCustomerWaitTimeRestrictionInSec { get; set; }

        public string TimeToDisplayWelcomeFormToCustomer { get; set; }
        public Int32 AutoCloseTimeForChatInMin { get; set; }
        [MaxLength(1500)]
        public string WhileConnectingToAnAgent { get; set; }
        [MaxLength(1500)]
        public string CustomerWaitTimeForAgentConnect { get; set; }
        [MaxLength(1500)]
        public string BotChatClosure { get; set; }
        [MaxLength(1500)]
        public string AgentChatClosure { get; set; }

        [MaxLength(1500)]
        public string NonBusinessConnect { get; set; }

        public Int32 TimeSlotId { get; set; }

        [MaxLength(1500)]
        public string CustomMessageForChatIcon { get; set; }

        //public ICollection<WelcomeFormsTable> welcomeFormsTables { get; set; }
	 [MaxLength(1500)]
        public string RasaBotUrl { get; set; }
    }
}
