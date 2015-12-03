using System;

namespace Announcement.Core
{
    public class ActionResult
    {
        public bool HasError { get; set; }

        public string Message { get; set; }
    }

    public class ActionResult<T> : ActionResult
    {
        public T Value { get; set; }
    }
}

