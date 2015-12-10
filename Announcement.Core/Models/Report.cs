using System;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class Report
    {
        public int Id { get; set; }

        public String PhoneNumber { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public List<string> Photos { get; set; }

        public string AudioRecord { get; set; }
    }
}

