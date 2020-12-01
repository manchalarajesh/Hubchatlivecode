using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelebuHubChat.Models
{
    [Table("ConversationHistory")]
    public class ConversationHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public Int32 AccountId { get; set; }

        public Int32 WidgetId { get; set; }

        [ForeignKey("WidgetId")]
        public Widgets Widgets { get; set; }
        public Int32 ConversationId { get; set; }

        [ForeignKey("ConversationId")]
        public Conversations Conversations { get; set; }

        public DateTime CreatedTimeUTC { get; set; } = DateTime.UtcNow;

        public Int32 FromStatus { get; set; }

        public Int32 ToStatus { get; set; }

        public string UserId { get; set; }
    }
}
