using System;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class Report
    {
        public string Id { get; set; }

        public string PhoneNumber { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public List<string> Photos { get; set; }
    }
}

