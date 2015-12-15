﻿using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class User 
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public String Username { get; set; }
    }
}

