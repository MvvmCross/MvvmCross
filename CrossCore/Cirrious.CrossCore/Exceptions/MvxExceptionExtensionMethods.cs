// MvxExceptionExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Exceptions
{
    public static class MvxExceptionExtensionMethods
    {
        public static string ToLongString(this Exception exception)
        {
            if (exception == null)
                return "null exception";

            if (exception.InnerException != null)
            {
                var innerExceptionText = exception.InnerException.ToLongString();
                return string.Format("{0}: {1}\n\t{2}\nInnerException was {3}",
                                     exception.GetType().Name,
                                     exception.Message ?? "-",
                                     exception.StackTrace,
                                     innerExceptionText);
            }
            else
            {
                return string.Format("{0}: {1}\n\t{2}",
                                     exception.GetType().Name,
                                     exception.Message ?? "-",
                                     exception.StackTrace);
            }
        }

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