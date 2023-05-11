using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarSync.Entities
{
    public class MicrosoftUser
    {
        [Key]
        public string Email { get; set; }

        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string TenantId { get; set; }

        public List<Calendar> Calendars { get; set; }

        public int LastAccessTokenStatus { get; set; }
    }
}
