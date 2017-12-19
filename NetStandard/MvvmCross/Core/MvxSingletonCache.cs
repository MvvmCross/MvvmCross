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
        : MvxSingleton<IMvxSingletonCache>, IMvxSingletonCache
    {
        public static MvxSingletonCache Initialize()
        {
            if (Instance != null)
                throw new MvxException("You should only initialize MvxBindingSingletonCache once");

            var instance = new MvxSingletonCache();
            return instance;
        }

        private MvxSingletonCache()
        {
        }

        private bool _inpcInterceptorResolveAttempted;
        private IMvxInpcInterceptor _inpcInterceptor;

        public IMvxInpcInterceptor InpcInterceptor
        {
            get
            {
                if (_inpcInterceptorResolveAttempted)
                    return _inpcInterceptor;

                Mvx.TryResolve<IMvxInpcInterceptor>(out _inpcInterceptor);
                _inpcInterceptorResolveAttempted = true;
                return _inpcInterceptor;
            }
        }

        private IMvxStringToTypeParser _parser;

        public IMvxStringToTypeParser Parser
        {
            get
            {
                _parser = _parser ?? Mvx.Resolve<IMvxStringToTypeParser>();
                return _parser;
            }
        }

        private IMvxSettings _settings;

        public IMvxSettings Settings
        {
            get
            {
                _settings = _settings ?? Mvx.Resolve<IMvxSettings>();
                return _settings;
            }
        }
    }
}