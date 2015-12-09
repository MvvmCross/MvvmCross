// MvxSingletonCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core
{
    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Exceptions;

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
                if (this._inpcInterceptorResolveAttempted)
                    return this._inpcInterceptor;

                Mvx.TryResolve<IMvxInpcInterceptor>(out this._inpcInterceptor);
                this._inpcInterceptorResolveAttempted = true;
                return this._inpcInterceptor;
            }
        }

        private IMvxStringToTypeParser _parser;

        public IMvxStringToTypeParser Parser
        {
            get
            {
                this._parser = this._parser ?? Mvx.Resolve<IMvxStringToTypeParser>();
                return this._parser;
            }
        }

        private IMvxSettings _settings;

        public IMvxSettings Settings
        {
            get
            {
                this._settings = this._settings ?? Mvx.Resolve<IMvxSettings>();
                return this._settings;
            }
        }
    }
}