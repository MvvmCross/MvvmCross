// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.ViewModels;

namespace MvvmCross.Base
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

                Mvx.IoCProvider.TryResolve<IMvxInpcInterceptor>(out _inpcInterceptor);
                _inpcInterceptorResolveAttempted = true;
                return _inpcInterceptor;
            }
        }

        private IMvxStringToTypeParser _parser;

        public IMvxStringToTypeParser Parser
        {
            get
            {
                _parser = _parser ?? Mvx.IoCProvider.Resolve<IMvxStringToTypeParser>();
                return _parser;
            }
        }

        private IMvxSettings _settings;

        public IMvxSettings Settings
        {
            get
            {
                _settings = _settings ?? Mvx.IoCProvider.Resolve<IMvxSettings>();
                return _settings;
            }
        }
    }
}
