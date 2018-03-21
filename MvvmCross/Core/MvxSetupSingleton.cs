// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.Core
{
    public abstract class MvxSetupSingleton
       : MvxSingleton<MvxSetupSingleton>
    {
        private static readonly object LockObject = new object();
        private static TaskCompletionSource<bool> IsInitialisedTaskCompletionSource;
        private IMvxSetup _setup;
        private bool _initialized;
        private IMvxSplashScreen _currentSplashScreen;

        protected virtual void EnsureInitialized()
        {
            lock (LockObject)
            {
                if (_initialized)
                    return;

                if (IsInitialisedTaskCompletionSource != null)
                {
                    MvxLog.Instance.Trace("EnsureInitialized has already been called so now waiting for completion");
                    IsInitialisedTaskCompletionSource.Task.GetAwaiter().GetResult();
                }
                else
                {
                    IsInitialisedTaskCompletionSource = new TaskCompletionSource<bool>();
                    _setup.Initialize();
                    _initialized = true;

                    if (_currentSplashScreen != null)
                    {
                        MvxLog.Instance.Warn("Current splash screen not null during direct initialization - not sure this should ever happen!");
                        var dispatcher = Mvx.GetSingleton<IMvxMainThreadDispatcher>();
                        dispatcher.RequestMainThreadAction(() =>
                        {
                            _currentSplashScreen?.InitializationComplete();
                        }, false);
                    }

                    IsInitialisedTaskCompletionSource.SetResult(true);
                }
            }
        }

        public virtual void RemoveSplashScreen(IMvxSplashScreen splashScreen)
        {
            lock (LockObject)
            {
                _currentSplashScreen = null;
            }
        }

        protected virtual void InitializeFromSplashScreen(IMvxSplashScreen splashScreen)
        {
            lock (LockObject)
            {
                _currentSplashScreen = splashScreen;
                if (_initialized)
                {
                    _currentSplashScreen?.InitializationComplete();
                    return;
                }

                if (IsInitialisedTaskCompletionSource != null)
                {
                    return;
                }
                else
                {
                    IsInitialisedTaskCompletionSource = new TaskCompletionSource<bool>();
                    _setup.InitializePrimary();
                    ThreadPool.QueueUserWorkItem(ignored =>
                    {
                        _setup.InitializeSecondary();
                        lock (LockObject)
                        {
                            IsInitialisedTaskCompletionSource.SetResult(true);
                            _initialized = true;
                            var dispatcher = Mvx.GetSingleton<IMvxMainThreadDispatcher>();
                            dispatcher.RequestMainThreadAction(() =>
                            {
                                _currentSplashScreen?.InitializationComplete();
                            });
                        }
                    });
                }
            }
        }

        public static TMvxSetupSingleton EnsureSingletonAvailable<TMvxSetupSingleton>() 
            where TMvxSetupSingleton : MvxSetupSingleton, new()
        {
            if (Instance != null)
                return Instance as TMvxSetupSingleton;

            lock (LockObject)
            {
                if (Instance != null)
                    return Instance as TMvxSetupSingleton;

                var instance = new TMvxSetupSingleton();
                instance.CreateSetup();
                return Instance as TMvxSetupSingleton;
            }
        }

        protected MvxSetupSingleton()
        {
        }

        protected virtual void CreateSetup()
        {
            try
            {
                _setup = MvxSetup.Instance;
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Failed to create setup instance");
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                lock (LockObject)
                {
                    _currentSplashScreen = null;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
