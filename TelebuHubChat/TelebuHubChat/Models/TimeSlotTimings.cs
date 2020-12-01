using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelebuHubChat.Models
{
    [Table("TimeSlotTimings")]
    public class TimeSlotTimings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public Int32 TimeSlotId { get; set; }
        
        public Int32 DayId { get; set; }
    
        public string FromTime { get; set; }

        public string ToTime { get; set; }

        public DateTime CreatedTime { get; set; }

    }
}
