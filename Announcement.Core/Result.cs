using System;

namespace Announcement.Core
{
    public class Result
    {
        public bool HasError { get; set; }

        public string Message { get; set; }

        public ResultType Type { get; set; }
    }

    public class Result<T> : Result
    {
        public T Value { get; set; }
    }

    public enum ResultType : int
    {
        Error = 0,
        Warning = 1,
        Information = 2,
        Success = 3,
    }
}

