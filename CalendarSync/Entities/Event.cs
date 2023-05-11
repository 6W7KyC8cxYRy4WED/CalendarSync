using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarSync.Entities
{
    public class Event
    {
        public Event()
        {
            ClonedEvents = new List<ClonedEvent>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey("RootCalendar")]
        public string RootCalendarId { get; set; }

        public Calendar RootCalendar { get; set; }

        public List<ClonedEvent> ClonedEvents { get; set; }

        [Required]
        public string ChangeKey { get; set; }
    }
}
