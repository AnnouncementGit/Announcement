using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class SpammerShort 
    {
        [JsonProperty("phoneNumber")]
        public String PhoneNumber { get; set; }

        [JsonProperty("spamCount")]
        public int SpamCount { get; set; }

		[JsonProperty("latitude")]
		public float Latitude { get; set; }

		[JsonProperty("longitude")]
		public float Longitude { get; set; }
    }
}

