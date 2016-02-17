using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class User 
    {       
        [JsonProperty("username")]
        public String Username { get; set; }

		[JsonProperty("displayName")]
		public string DisplayName { get; set; }

        [JsonProperty("password")]
        public String Password { get; set; }

        [JsonProperty("reports")]
        public int Reports { get; set; }

        [JsonProperty("confirmedReports")]
        public int ConfirmedReports { get; set; }
    }
}

