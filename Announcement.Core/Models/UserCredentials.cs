﻿using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class UserCredentials
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("role")]
        public int Role { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}

