// MvxAndroidSetupSingleton.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using System.Threading;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Droid.Views;

namespace Cirrious.MvvmCross.Droid.Platform
{
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
                if (_initialized)
                    return;

                if (_initializationStarted)
                {
                    Mvx.Warning("Multiple Initialize calls made for MvxAndroidSetupSingleton singleton");
                    throw new MvxException("Multiple initialize calls made");
                }

                _initializationStarted = true;
            }

            _setup.Initialize();

            lock (LockObject)
            {
                _initialized = true;
                if (_currentSplashScreen != null)
                {
                    Mvx.Warning("Current splash screen not null during direct initialization - not sure this should ever happen!");
                    _currentSplashScreen.InitializationComplete();
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

                if (_initializationStarted)
                {
                    if (_initialized)
                    {
                        _currentSplashScreen.InitializationComplete();
                        return;
                    }

                    return;
                }

                _initializationStarted = true;
            }

            _setup.InitializePrimary();
            ThreadPool.QueueUserWorkItem(ignored =>
            {
                _setup.InitializeSecondary();
                lock (LockObject)
                {
                    _initialized = true;
                    _currentSplashScreen?.InitializationComplete();
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
                        where typeof (MvxAndroidSetup).IsAssignableFrom(type)
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