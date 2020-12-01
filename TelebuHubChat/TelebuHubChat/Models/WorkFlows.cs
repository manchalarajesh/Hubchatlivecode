using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TelebuHubChat.Models
{
    [Table("WorkFlows")]
    public class WorkFlows
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public string WorkFlow { get; set; }

        public ICollection<Widgets> widgets { get; set; }
    }
}
