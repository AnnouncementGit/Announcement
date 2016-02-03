using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class Spammer 
    {
        [JsonProperty("phoneNumber")]
        public String PhoneNumber { get; set; }

        [JsonProperty("allReports")]
        public int Complaints { get; set; }

        [JsonProperty("audioRecord")]
        public String AudioRecord { get; set; }
    }
}

