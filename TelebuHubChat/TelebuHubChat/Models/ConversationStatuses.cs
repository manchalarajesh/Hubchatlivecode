using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelebuHubChat.Models
{
    [Table("ConversationStatuses")]
    public class ConversationStatuses
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public string Status { get; set; }

        public ICollection<Conversations> conversations { get; set; }
    }
}
