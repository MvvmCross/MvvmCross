using System;
using MvvmCross.Core;
using MvvmCross.Forms.Platforms.Tizen.Core;
using MvvmCross.Platforms.Tizen.Core;
using MvvmCross.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;

namespace MvvmCross.Forms.Platforms.Tizen.Views
{
    public abstract class MvxFormsTizenApplication : FormsApplication, IMvxLifetime
    {
        Application _formsApplication;

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        public MvxFormsTizenApplication() : base()
        {
            RegisterSetup();
        }

        protected override void OnResume()
        {
            base.OnResume();
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }

        protected override void OnPause()
        {
            base.OnPause();
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }

        protected override void OnTerminate()
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
            base.OnTerminate();
        }

        protected override void OnPreCreate()
        {
            MvxTizenSetupSingleton.EnsureSingletonAvailable(this).EnsureInitialized();
            var instance = MvxTizenSetupSingleton.EnsureSingletonAvailable(this);
            _formsApplication = instance.PlatformSetup<MvxFormsTizenSetup>().FormsApplication;
            base.OnPreCreate();
        }

        protected override void OnCreate()
        {
            RunAppStart();
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            base.OnCreate();
        }

        protected virtual void RunAppStart()
        {
            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
            {
                startup.Start();
            }
            LoadFormsApplication();
        }

        protected virtual void LoadFormsApplication()
        {
            LoadApplication(_formsApplication);
        }

        protected virtual void RegisterSetup()
        {
        }

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            handler?.Invoke(this, new MvxLifetimeEventArgs(which));
        }
    }

    public abstract class MvxFormsTizenApplication<TMvxTizenSetup, TApplication, TFormsApplication> : MvxFormsTizenApplication
        where TMvxTizenSetup : MvxFormsTizenSetup<TApplication, TFormsApplication>, new()
        where TApplication : class, IMvxApplication, new()
        where TFormsApplication : Application, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxTizenSetup>();
        }
    }
}
