// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
namespace MvvmCross.Exceptions;

public static class MvxExceptionExtensions
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

    public static Exception MvxWrap(this Exception exception, string messageFormat, params object?[] formatArguments)
    {
        return new MvxException(exception, messageFormat, formatArguments);
    }
}
