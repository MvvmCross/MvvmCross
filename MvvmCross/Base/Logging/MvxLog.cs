// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platform.Logging.LogProviders;

namespace MvvmCross.Base.Logging
{
    internal class MvxLog : IMvxLog
    {
        internal static IMvxLog Instance { get; set; }

        internal const string FailedToGenerateLogMessage = "Failed to generate log message";

        private readonly Logger _logger;

        internal MvxLog(Logger logger)
        {
            _logger = logger;
        }

        public bool IsLogLevelEnabled(MvxLogLevel logLevel) => _logger(logLevel, null);

        public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
        {
            if (messageFunc == null)
                return _logger(logLevel, null);

            Func<string> wrappedMessageFunc = () =>
            {
                try
                {
                    return messageFunc();
                }
                catch (Exception ex)
                {
                    Log(MvxLogLevel.Error, () => FailedToGenerateLogMessage, ex);
                }

                return null;
            };

            return _logger(logLevel, wrappedMessageFunc, exception, formatParameters);
        }
    }
}
