using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelebuHubChat.Models
{ 
     [Table("CustomerIpInformation")]
    public class CustomerIpInformation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string HttpURL { get; set; }

        public string ClientIp { get; set; }

        public Int64 CustomerId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTimeUTC { get; set; } = DateTime.UtcNow;
	
	public string HttpReferrer { get; set; }
    }
}
