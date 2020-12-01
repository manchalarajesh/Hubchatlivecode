using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TelebuHubChat.Models
{
    [Table("WelcomeFormsTable")]
    public class WelcomeFormsTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public Int32 WidgetId { get; set; }
        [ForeignKey("WidgetId")]
        public Widgets Widgets { get; set; }

        [MaxLength(1500)]
        public string MetaData { get; set; }

        public ICollection<Widgets> widgets { get; set; }
    }
}
