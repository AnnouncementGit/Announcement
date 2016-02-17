using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class SocialUser
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

		[JsonProperty("displayName")]
		public string DisplayName { get; set; }

        [JsonProperty("token")]
        public String Token { get; set; }
    }
}

