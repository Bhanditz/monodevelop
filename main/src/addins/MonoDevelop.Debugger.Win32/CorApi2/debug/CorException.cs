using System;
using System.Runtime.Serialization;

namespace CorApi2.debug
{
    [Serializable] // TODO: is this used on our side?
    public unsafe class CorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the CorException.
        /// </summary>
        public CorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the CorException with the specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CorException with the specified error message and inner Exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public CorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CorException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected CorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}