using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class SingleReport 
    {
        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        [JsonProperty("photo")]
        public byte[] Photo { get; set; }
    }
}

