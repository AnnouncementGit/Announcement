using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class ModeratorRegistration
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}

