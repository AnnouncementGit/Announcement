using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class OptionId : UserCredentials
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public OptionId()
        {
            
        }

        public OptionId(UserCredentials credentials)
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

