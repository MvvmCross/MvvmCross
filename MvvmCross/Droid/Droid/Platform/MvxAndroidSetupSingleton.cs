// MvxAndroidSetupSingleton.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Platform
{
    using System;
    using System.Linq;
    using System.Threading;

    using Android.Content;

    using MvvmCross.Droid.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.IoC;
    using System.Threading.Tasks;

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
                    Mvx.Trace("EnsureInitialized has already been called so now waiting for completion");
                    IsInitialisedTaskCompletionSource.Task.Wait();
                }
                else
                {
                    IsInitialisedTaskCompletionSource = new TaskCompletionSource<bool>();
                    this._setup.Initialize();
                    this._initialized = true;

                    if (this._currentSplashScreen != null)
                    {
                        Mvx.Warning("Current splash screen not null during direct initialization - not sure this should ever happen!");
                        var dispatcher = Mvx.GetSingleton<IMvxMainThreadDispatcher>();
                        dispatcher.RequestMainThreadAction(() =>
                        {
                            this._currentSplashScreen?.InitializationComplete();
                        });
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
                this._currentSplashScreen = splashScreen;
                if (_initialized)
                {
                    this._currentSplashScreen?.InitializationComplete();
                    return;
                }

                if (IsInitialisedTaskCompletionSource != null)
                {
                    return;
                }
                else
                {
                    IsInitialisedTaskCompletionSource = new TaskCompletionSource<bool>();
                    this._setup.InitializePrimary();
                    ThreadPool.QueueUserWorkItem(ignored =>
                    {
                        this._setup.InitializeSecondary();
                        lock (LockObject)
                        {
                            IsInitialisedTaskCompletionSource.SetResult(true);
                            this._initialized = true;
                            var dispatcher = Mvx.GetSingleton<IMvxMainThreadDispatcher>();
                            dispatcher.RequestMainThreadAction(() =>
                            {
                                this._currentSplashScreen?.InitializationComplete();
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
                throw new MvxException("Could not find a Setup class for application");

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
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembly.ExceptionSafeGetTypes())
				{
					if (type.Name != "Setup") continue;
					if (!typeof(MvxAndroidSetup).IsAssignableFrom(type)) continue;

					return type;
				}
			}

			return default(Type);
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
