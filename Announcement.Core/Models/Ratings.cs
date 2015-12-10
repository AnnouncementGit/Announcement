using System;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class Ratings 
    {
        public List<User> TopUsers { get; set; }

        public List<Spammer> TopSpammers { get; set; }
    }
}

