using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelebuHubChat.Models
{
    [Table("WidgetThemes")]
    public class WidgetThemes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        public string Color { get; set; }

        public ICollection<Widgets> widgets { get; set; }
    }
}
