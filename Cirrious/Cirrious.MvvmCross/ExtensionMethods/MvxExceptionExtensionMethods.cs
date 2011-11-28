using System;
using Cirrious.MvvmCross.Exceptions;

namespace Cirrious.MvvmCross.ExtensionMethods
{
    public static class MvxExceptionExtensionMethods
    {
        public static Exception MvxWrap(this Exception exception)
        {
            if (exception is MvxException)
                return exception;

            return MvxWrap(exception, exception.Message);
        }

        public static Exception MvxWrap(this Exception exception, string message)
        {
            return new MvxException(exception, message);
        }

        public static Exception MvxWrap(this Exception exception, string messageFormat, params object[] formatArguments)
        {
            return new MvxException(exception, messageFormat, formatArguments);
        }
    }
}
