// MvxSingletonCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Core
{
    public class MvxSingletonCache
        : MvxSingleton<IMvxSingletonCache>
            , IMvxSingletonCache
    {
        private IMvxInpcInterceptor _inpcInterceptor;

        private bool _inpcInterceptorResolveAttempted;

        private IMvxStringToTypeParser _parser;

        private IMvxSettings _settings;

        private MvxSingletonCache()
        {
        }

        public IMvxInpcInterceptor InpcInterceptor
        {
            get
            {
                if (_inpcInterceptorResolveAttempted)
                    return _inpcInterceptor;

                Mvx.TryResolve(out _inpcInterceptor);
                _inpcInterceptorResolveAttempted = true;
                return _inpcInterceptor;
            }
        }

        public IMvxStringToTypeParser Parser
        {
            get
            {
                _parser = _parser ?? Mvx.Resolve<IMvxStringToTypeParser>();
                return _parser;
            }
        }

        public IMvxSettings Settings
        {
            get
            {
                _settings = _settings ?? Mvx.Resolve<IMvxSettings>();
                return _settings;
            }
        }

        public static MvxSingletonCache Initialize()
        {
            if (Instance != null)
                throw new MvxException("You should only initialize MvxBindingSingletonCache once");

            var instance = new MvxSingletonCache();
            return instance;
        }
    }
}