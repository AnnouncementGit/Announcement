using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class ModeratorRegistration : UserCredentials
    {
        [JsonProperty("moderatorUsername")]
        public String ModeratorUsername { get; set; }

        [JsonProperty("moderatorPassword")]
        public String ModeratorPassword { get; set; }

        public ModeratorRegistration()
        {

        }

        public ModeratorRegistration(UserCredentials credentials)
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

