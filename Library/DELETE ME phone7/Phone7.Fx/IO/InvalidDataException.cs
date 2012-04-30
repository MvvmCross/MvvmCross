using System;
using System.Runtime.Serialization;

namespace Phone7.Fx.IO
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InvalidDataException:SystemException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataException"/> class.
        /// </summary>
        public InvalidDataException()
            : base("GenericInvalidData")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidDataException(string message)
            : base(message)
        {
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception.</param>
        public InvalidDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}