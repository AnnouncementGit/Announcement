using System;

namespace Announcement.Core
{
    public class Result
    {
        public bool HasError { get; set; }

        public string Message { get; set; }
    }
        
    public class Result<T> : Result
    {
        public bool IsSuccess { get; set; }

        public T Value { get; set; }
    }
}

