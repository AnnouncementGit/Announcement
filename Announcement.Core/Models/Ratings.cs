using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class Ratings 
    {
        [JsonProperty("topUsers")]
        public List<User> TopUsers { get; set; }

        [JsonProperty("topSpammers")]
        public List<Spammer> TopSpammers { get; set; }
    }
}

