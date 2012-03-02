#region Copyright
// <copyright file="MvxExceptionExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Exceptions;

namespace Cirrious.MvvmCross.ExtensionMethods
{
    public static class MvxExceptionExtensionMethods
    {
        public static string ToLongString(this Exception exception)
        {
            if (exception == null)
                return "null exception";

            return string.Format("{0}: {1}\n\t{2}",
                                 exception.GetType().Name,
                                 exception.Message ?? "-",
                                 exception.StackTrace);
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