using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TelebuHubChat.Models
{

    [Table("WidgetAuthkeys")]
    public class WidgetAuthkeys
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public Int32 WidgetId { get; set; }

        public Int32 WidgetChannelId { get; set; }

        public string AuthKey { get; set; }
        public string AuthToken { get; set; }

        public byte IsActive { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
