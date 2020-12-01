using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TelebuHubChat.Models
{
    [Table("ConversationInfo")]
    public class ConversationInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public Int32 ConversationId { get; set; }

        public byte IsBotEnd { get; set; }

        public DateTime CreatedTimeUtc { get; set; }
    }
}
