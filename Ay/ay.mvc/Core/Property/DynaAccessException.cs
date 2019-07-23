using System;
using System.Runtime.Serialization;

namespace Ay.MvcFramework
{
    public class DynaAccessException : System.ApplicationException
    {
        public DynaAccessException()
            : base("A property reflection error has occurred.")
        {
        }

        public DynaAccessException(Exception ex)
            : base(ex.Message, ex)
        {
        }

        public DynaAccessException(string message)
            : base(message)
        {
        }

        public DynaAccessException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DynaAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
