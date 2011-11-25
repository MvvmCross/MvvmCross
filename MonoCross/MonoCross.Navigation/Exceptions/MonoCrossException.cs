using System;

namespace MonoCross.Navigation.Exceptions
{
    // TODO - does this need serialisation on it? (not for wp7, but maybe for MT and MD)
    public class MonoCrossException : Exception
    {
        public MonoCrossException()
        {            
        }

        public MonoCrossException(string message)
            : base(message)
        {
            
        }

        public MonoCrossException(string messageFormat, params object[] messageFormatArguments)
            : base(string.Format(messageFormat, messageFormatArguments))
        {

        }

        public MonoCrossException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
