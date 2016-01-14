using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class OptionSpammer : UserCredentials
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("audioRecord")]
        public string AudioRecord { get; set; }

        public OptionSpammer()
        {
            
        }

        public OptionSpammer(UserCredentials credentials)
        {
            if (credentials != null)
            {
                Username = credentials.Username;

                Role = credentials.Role;

                AccessToken = credentials.AccessToken;
            }
        }
    }

}

