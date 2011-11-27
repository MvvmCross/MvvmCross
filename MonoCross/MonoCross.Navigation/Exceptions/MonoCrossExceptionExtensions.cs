using System;

namespace MonoCross.Navigation.Exceptions
{
    public static class MonoCrossExceptionExtensionMethods
    {
        public static Exception MXWrap(this Exception exception)
        {
            if (exception is MonoCrossException)
                return exception;

            return MXWrap(exception, exception.Message);
        }

        public static Exception MXWrap(this Exception exception, string message)
        {
            return new MonoCrossException(exception, message);
        }

        public static Exception MXWrap(this Exception exception, string messageFormat, params object[] formatArguments)
        {
            return new MonoCrossException(exception, messageFormat, formatArguments);
        }
    }
}
