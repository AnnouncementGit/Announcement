using System;
using Newtonsoft.Json;

namespace Announcement.Core
{

    public enum UserRoles : int
    {
        Admin = 0,
        Moderator = 1,
        User = 2,
    }
}
