using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TelebuHubChat.Models
{
    [Table("WidgetChannels")]
    public class WidgetChannels
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public Int32 WidgetId { get; set; }

        public string ChanneUUID { get; set; }

        public Int32 ConversationTypeId { get; set; }
        public byte IsActive { get; set; }
        public string RasaBotUrl { get; set; }
        public DateTime CreatedTime { get; set; }


    }
}
