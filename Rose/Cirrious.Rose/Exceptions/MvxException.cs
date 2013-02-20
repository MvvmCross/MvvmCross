﻿// MvxException.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Exceptions
{
#warning Add Serialization!
    // Officially exception should support serialisation, but we don't add it here
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