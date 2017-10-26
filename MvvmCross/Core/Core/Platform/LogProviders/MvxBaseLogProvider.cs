using System;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Core.Platform.LogProviders
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

        public IMvxLog GetLogFor<T>()
            => GetLogFor(typeof(T).FullName);

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
