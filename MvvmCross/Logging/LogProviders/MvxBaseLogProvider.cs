// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MvvmCross.Tests")]
[assembly: InternalsVisibleTo("MvvmCross.Forms")]
[assembly: InternalsVisibleTo("MvvmCross.Platforms.Wpf")]
[assembly: InternalsVisibleTo("MvvmCross.Forms.Platforms.Wpf")]

namespace MvvmCross.Logging.LogProviders
{
    internal delegate bool Logger(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters);

    internal abstract class MvxBaseLogProvider : IMvxLogProvider
    {
        protected delegate IDisposable OpenNdc(string message);
        protected delegate IDisposable OpenMdc(string key, string value);

        private readonly Lazy<OpenNdc> _lazyOpenNdcMethod;
        private readonly Lazy<OpenMdc> _lazyOpenMdcMethod;
        private static readonly IDisposable NoopDisposableInstance = new DisposableAction();

        protected MvxBaseLogProvider()
        {
            _lazyOpenNdcMethod
                = new Lazy<OpenNdc>(GetOpenNdcMethod);

            _lazyOpenMdcMethod
               = new Lazy<OpenMdc>(GetOpenMdcMethod);
        }

        public IMvxLog GetLogFor(Type type)
            => GetLogFor(type.FullName);

        public IMvxLog GetLogFor<T>()
            => GetLogFor(typeof(T));

        public IMvxLog GetLogFor(string name)
            => new MvxLog(GetLogger(name));

        protected abstract Logger GetLogger(string name);

        public IDisposable OpenNestedContext(string message)
            => _lazyOpenNdcMethod.Value(message);

        public IDisposable OpenMappedContext(string key, string value)
            => _lazyOpenMdcMethod.Value(key, value);

        protected virtual OpenNdc GetOpenNdcMethod()
            => _ => NoopDisposableInstance;

        protected virtual OpenMdc GetOpenMdcMethod()
            => (_, __) => NoopDisposableInstance;
    }
}
