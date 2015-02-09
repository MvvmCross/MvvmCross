// MvxSingletonCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross
{
    public class MvxSingletonCache
        : MvxSingleton<IMvxSingletonCache>
          , IMvxSingletonCache
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

        private IMvxSafeValueCreator _safeValueCreator;
        public IMvxSafeValueCreator SafeValueCreator
        {
            get
            {
                _safeValueCreator = _safeValueCreator ?? Mvx.Resolve<IMvxSafeValueCreator>();
                return _safeValueCreator;
            }
        }
    }
}