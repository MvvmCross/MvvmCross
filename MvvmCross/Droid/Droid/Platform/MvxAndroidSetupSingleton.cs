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

    public class MvxAndroidSetupSingleton
        : MvxSingleton<MvxAndroidSetupSingleton>
    {
        private static readonly object LockObject = new object();
        private MvxAndroidSetup _setup;
        private bool _initialized;
        private bool _initializationStarted;
        private IMvxAndroidSplashScreenActivity _currentSplashScreen;

        public virtual void EnsureInitialized()
        {
            lock (LockObject)
            {
                if (this._initialized)
                    return;

                if (this._initializationStarted)
                {
                    Mvx.Warning("Multiple Initialize calls made for MvxAndroidSetupSingleton singleton");
                    throw new MvxException("Multiple initialize calls made");
                }

                this._initializationStarted = true;
            }

            this._setup.Initialize();

            lock (LockObject)
            {
                this._initialized = true;
                if (this._currentSplashScreen != null)
                {
                    Mvx.Warning("Current splash screen not null during direct initialization - not sure this should ever happen!");
                    this._currentSplashScreen.InitializationComplete();
                }
            }
        }

        public virtual void RemoveSplashScreen(IMvxAndroidSplashScreenActivity splashScreen)
        {
            lock (LockObject)
            {
                this._currentSplashScreen = null;
            }
        }

        public virtual void InitializeFromSplashScreen(IMvxAndroidSplashScreenActivity splashScreen)
        {
            lock (LockObject)
            {
                this._currentSplashScreen = splashScreen;

                if (this._initializationStarted)
                {
                    if (this._initialized)
                    {
                        this._currentSplashScreen.InitializationComplete();
                        return;
                    }

                    return;
                }

                this._initializationStarted = true;
            }

            this._setup.InitializePrimary();
            ThreadPool.QueueUserWorkItem(ignored =>
            {
                this._setup.InitializeSecondary();
                lock (LockObject)
                {
                    this._initialized = true;
                    this._currentSplashScreen?.InitializationComplete();
                }
            });
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

        private MvxAndroidSetupSingleton()
        {
        }

        protected virtual void CreateSetup(Context applicationContext)
        {
            var setupType = this.FindSetupType();
            if (setupType == null)
            {
                throw new MvxException("Could not find a Setup class for application");
            }

            try
            {
                this._setup = (MvxAndroidSetup)Activator.CreateInstance(setupType, applicationContext);
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
                    this._currentSplashScreen = null;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}