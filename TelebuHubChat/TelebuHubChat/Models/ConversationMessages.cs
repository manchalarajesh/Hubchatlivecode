using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelebuHubChat.Models
{
    [Table("ConversationMessages")]
    public class ConversationMessages
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public Int32 ConversationId { get; set; }

        [ForeignKey("ConversationId")]
        public Conversations Conversations { get; set; }

        public Int32 MessageTypeId { get; set; }

        [ForeignKey("MessageTypeId")]
        public ConversationMessageTypes ConversationMessageTypes { get; set; }

        public int? AgentId { get; set; }

        //[MySqlCharset("utf8")]
        public string Message { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTimeUTC { get; set; } = DateTime.UtcNow;

        public byte IsDelivered { get; set; }

	public Int32 ConversationTypeId { get; set; }

	public string AttachmentUrl { get; set; }
        
    public string AttachmentId { get; set; }
    }
}
