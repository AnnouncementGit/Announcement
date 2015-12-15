using System;
using Newtonsoft.Json;

namespace Announcement.Core
{
    public class SingleContinueReport : SingleReport 
    {
        [JsonProperty("id")]
        public string id { get; set; }
    }
}

