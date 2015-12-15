using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class Moderator
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}

