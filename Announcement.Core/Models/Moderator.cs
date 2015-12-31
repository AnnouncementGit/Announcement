using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class Moderator
    {
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}

