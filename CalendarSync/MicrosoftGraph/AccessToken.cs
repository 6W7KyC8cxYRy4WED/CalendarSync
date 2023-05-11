using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarSync.MicrosoftGraph
{
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string Access { get; set; }

        [JsonProperty("refresh_token")]
        public string Refresh { get; set; }

        public bool Error { get; set; }
    }
}
