using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class Report
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        [JsonProperty("photos")]
        public List<string> Photos { get; set; }
    }
}

