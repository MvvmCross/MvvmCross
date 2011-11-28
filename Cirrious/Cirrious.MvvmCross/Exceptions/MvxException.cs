using System;

namespace Cirrious.MvvmCross.Exceptions
{
    // TODO - does this need serialisation on it? (not for wp7, but maybe for MT and MD)
    public class MvxException : Exception
    {
        public MvxException()
        {            
        }

        public MvxException(string message)
            : base(message)
        {
            
        }

        public MvxException(string messageFormat, params object[] messageFormatArguments)
            : base(string.Format(messageFormat, messageFormatArguments))
        {

        }

        // the order of parameters here is slightly different to that normally expected in an exception
        // - but this order allows us to put string.Format in place 
        public MvxException(Exception innerException, string messageFormat, params object[] formatArguments)
            : base(string.Format(messageFormat, formatArguments), innerException)
        {

        }
    }
}
