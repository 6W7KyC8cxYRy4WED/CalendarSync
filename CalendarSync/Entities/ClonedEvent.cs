using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarSync.Entities
{
    public class ClonedEvent
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey("RootCalendar")]
        public string RootCalendarId { get; set; }

        public Calendar RootCalendar { get; set; }

        [Required]
        public string ChangeKey { get; set; }
    }
}
