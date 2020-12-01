using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TelebuHubChat.Models
{
    [Table("NavigationHistory")]
    public class NavigationHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Int32 Id { get; set; }

        public Int32 ConversationId { get; set; }

        [ForeignKey("ConversationId")]
        public Conversations Conversations { get; set; }

        public string Url { get; set; }

        public string Referrer { get; set; }

        private DateTime VisitingTime { get; set; }

        public DateTime LeavingTime { get; set; }

        public string Ip { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
