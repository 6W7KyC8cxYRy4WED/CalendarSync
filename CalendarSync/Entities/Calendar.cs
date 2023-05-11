using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarSync.Entities
{
    public class Calendar
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey("MicrosoftUser")]
        public string MicrosoftUserEmail { get; set; }

        public MicrosoftUser MicrosoftUser { get; set; }

        [Required]
        public bool WriteOnly { get; set; }

        [Required]
        public bool Purge { get; set; }
    }
}
