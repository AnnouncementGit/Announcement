using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class User 
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public String Username { get; set; }

        [JsonProperty("reports")]
        public int Reports { get; set; }

        [JsonProperty("confirmedReports")]
        public int ConfirmedReports { get; set; }
    }
}

