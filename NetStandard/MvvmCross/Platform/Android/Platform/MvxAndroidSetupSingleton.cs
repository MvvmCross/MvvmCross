﻿// MvxAndroidSetupSingleton.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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
        private static readonly object LockObject = new object();
        private static TaskCompletionSource<bool> IsInitialisedTaskCompletionSource;
        private MvxAndroidSetup _setup;
        private bool _initialized;
        private IMvxAndroidSplashScreenActivity _currentSplashScreen;

        public virtual void EnsureInitialized()
        {
            lock (LockObject)
            {
                if (_initialized)
                    return;

                if (IsInitialisedTaskCompletionSource != null)
                {
                    MvxLog.Instance.Trace("EnsureInitialized has already been called so now waiting for completion");
                    IsInitialisedTaskCompletionSource.Task.Wait();
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

        public virtual void RemoveSplashScreen(IMvxAndroidSplashScreenActivity splashScreen)
        {
            lock (LockObject)
            {
                _currentSplashScreen = null;
            }
        }

        public virtual void InitializeFromSplashScreen(IMvxAndroidSplashScreenActivity splashScreen)
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

        public static MvxAndroidSetupSingleton EnsureSingletonAvailable(Context applicationContext)
        {
            if (Instance != null)
                return Instance;

            lock (LockObject)
            {
                if (Instance != null)
                    return Instance;

                var instance = new MvxAndroidSetupSingleton();
                instance.CreateSetup(applicationContext);
                return Instance;
            }
        }

        protected MvxAndroidSetupSingleton()
        {
        }

        protected virtual void CreateSetup(Context applicationContext)
        {
            var setupType = FindSetupType();
            if (setupType == null)
            {
                throw new MvxException("Could not find a Setup class for application");
            }

            try
            {
                _setup = (MvxAndroidSetup)Activator.CreateInstance(setupType, applicationContext);
            }
            catch (Exception exception)
            {
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
