using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelebuHubChat.Models
{
	[Table("TimeSlots")]
	public class TimeSlots
    {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int32 Id { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "AccountId should not be empty")]
		public Int32 AccountId { get; set; }

		public string Name { get; set; }

		public DateTime CreatedTime { get; set; }

		public Int32 IsActive { get; set; }

    }
}
