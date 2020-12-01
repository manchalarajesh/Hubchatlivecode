using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TelebuHubChat.Models
{
    [Table("Purposes")]
    public class Purposes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public string Purpose { get; set; }

        public ICollection<Widgets> widgets { get; set; }
    }
}
