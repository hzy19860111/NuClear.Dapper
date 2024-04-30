using System;

namespace NuClear.Dapper.Exceptions
{
    public class SqlParseException : Exception
    {
        public SqlParseException()
        {
        }
        public SqlParseException(string message)
            : base(message)
        {
        }
        public SqlParseException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
        public SqlParseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
