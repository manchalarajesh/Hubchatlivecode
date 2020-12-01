using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelebuHubChat.Models
{
    [Table("ConversationMessageTypes")]
    public class ConversationMessageTypes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public string MessageType { get; set; }

        public ICollection<ConversationMessages> conversationMessages { get; set; }
    }
}
