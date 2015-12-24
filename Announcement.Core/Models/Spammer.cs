using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class Spammer 
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("phoneNumber")]
        public String PhoneNumber { get; set; }

        [JsonProperty("reportsCount")]
        public int Complaints { get; set; }

        [JsonProperty("audioRecord")]
        public String AudioRecord { get; set; }
    }
}

