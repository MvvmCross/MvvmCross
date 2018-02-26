// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Droid.Platform
{
    public class MvxAndroidSetupSingleton
        : MvxSingleton<MvxAndroidSetupSingleton>
    {
        private static readonly SemaphoreSlim InitializationLock = new SemaphoreSlim(1);
        private static TaskCompletionSource<bool> IsInitialisedTaskCompletionSource;
        private MvxAndroidSetup _setup;
        private bool _initialized;
        private IMvxAndroidSplashScreenActivity _currentSplashScreen;

        public async virtual Task EnsureInitialized()
        {
            await InitializationLock.WaitAsync();
            try {
                if (_initialized)
                    return;

                if (IsInitialisedTaskCompletionSource != null) {
                    MvxLog.Instance.Trace("EnsureInitialized has already been called so now waiting for completion");
                    IsInitialisedTaskCompletionSource.Task.Wait();
                } else {
                    IsInitialisedTaskCompletionSource = new TaskCompletionSource<bool>();
                    _setup.Initialize();
                    _initialized = true;

                    if (_currentSplashScreen != null) {
                        MvxLog.Instance.Warn("Current splash screen not null during direct initialization - not sure this should ever happen!");
                        var dispatcher = Mvx.GetSingleton<IMvxMainThreadDispatcher>();
                        dispatcher.RequestMainThreadAction(async () => {
                            await _currentSplashScreen?.InitializationComplete();
                        }, false);
                    }

                    IsInitialisedTaskCompletionSource.SetResult(true);
                }
            } finally {
                InitializationLock.Release();
            }
        }

        public async virtual Task RemoveSplashScreen(IMvxAndroidSplashScreenActivity splashScreen)
        {
            await InitializationLock.WaitAsync();
            try {
                _currentSplashScreen = null;
            } finally {
                InitializationLock.Release();
            }
        }

        public async virtual Task InitializeFromSplashScreen(IMvxAndroidSplashScreenActivity splashScreen)
        {
            await InitializationLock.WaitAsync();
            try {
                _currentSplashScreen = splashScreen;
                if (_initialized) {
                    await _currentSplashScreen?.InitializationComplete();
                    return;
                }

                if (IsInitialisedTaskCompletionSource != null) {
                    return;
                } else {
                    IsInitialisedTaskCompletionSource = new TaskCompletionSource<bool>();
                    _setup.InitializePrimary();
                    ThreadPool.QueueUserWorkItem(async ignored => {
                        _setup.InitializeSecondary();
                        await InitializationLock.WaitAsync();
                        try {
                            IsInitialisedTaskCompletionSource.SetResult(true);
                            _initialized = true;
                            var dispatcher = Mvx.GetSingleton<IMvxMainThreadDispatcher>();
                            dispatcher.RequestMainThreadAction(() => {
                                _currentSplashScreen?.InitializationComplete();
                            });
                        } finally {
                            InitializationLock.Release();
                        }
                    });
                }
            } finally {
                InitializationLock.Release();
            }
        }

        public async static Task<MvxAndroidSetupSingleton> EnsureSingletonAvailable(Context applicationContext)
        {
            if (Instance != null)
                return Instance;

            await InitializationLock.WaitAsync();
            try {
                if (Instance != null)
                    return Instance;

                var instance = new MvxAndroidSetupSingleton();
                instance.CreateSetup(applicationContext);
                return Instance;
            } finally {
                InitializationLock.Release();
            }
        }

        protected MvxAndroidSetupSingleton()
        {
        }

        protected virtual void CreateSetup(Context applicationContext)
        {
            var setupType = FindSetupType();
            if (setupType == null) {
                throw new MvxException("Could not find a Setup class for application");
            }

            try {
                _setup = (MvxAndroidSetup)Activator.CreateInstance(setupType, applicationContext);
            } catch (Exception exception) {
                throw exception.MvxWrap("Failed to create instance of {0}", setupType.FullName);
            }
        }

        protected virtual Type FindSetupType()
        {
            var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.ExceptionSafeGetTypes()
                        where type.Name == "Setup"
                        where typeof(MvxAndroidSetup).IsAssignableFrom(type)
                        select type;

            return query.FirstOrDefault();
        }

        protected async override void Dispose(bool isDisposing)
        {
            if (isDisposing) {
                await InitializationLock.WaitAsync();
                try {
                    _currentSplashScreen = null;
                } finally {
                    InitializationLock.Release();
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
