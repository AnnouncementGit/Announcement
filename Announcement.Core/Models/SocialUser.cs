using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class SocialUser
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }

        [JsonProperty("token")]
        public String Token { get; set; }
    }
}

