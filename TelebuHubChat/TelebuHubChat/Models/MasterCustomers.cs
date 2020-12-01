using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TelebuHubChat.Models
{
    [Table("MasterCustomers")]
    public class MasterCustomers
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public Int32 AccountId { get; set; }

        public int AccountCustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Mobile { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
