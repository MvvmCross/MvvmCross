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
            return new MonoCrossException(message, exception);
        }
    }
}
