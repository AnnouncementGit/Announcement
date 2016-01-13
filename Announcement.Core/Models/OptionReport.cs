using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class OptionReport : UserCredentials
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        [JsonProperty("photo")]
        public byte[] Photo { get; set; }

        public OptionReport()
        {
        }

        public OptionReport(UserCredentials credentials)
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

