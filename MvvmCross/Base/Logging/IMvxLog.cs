// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Base.Logging
{
    public interface IMvxLog
    {
        bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters);

        bool IsLogLevelEnabled(MvxLogLevel logLevel);
    }
}
