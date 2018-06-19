// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Android.Content;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Platforms.Android.Views.Base;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using Application = Xamarin.Forms.Application;

namespace MvvmCross.Forms.Platforms.Android.Views
{
    public abstract class MvxFormsApplicationActivity : MvxEventSourceFormsApplicationActivity, IMvxAndroidView
    {
        protected View _view;

        protected MvxFormsApplicationActivity()
        {
            RegisterSetup();
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get
            {
                return DataContext as IMvxViewModel;
            }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        private Application _formsApplication;
        protected Application FormsApplication
        {
            get
            {
                if (_formsApplication == null)
                {
                    var formsPresenter = Mvx.IoCProvider.Resolve<IMvxFormsViewPresenter>();
                    _formsApplication = formsPresenter.FormsApplication;
                }
                return _formsApplication;
            }
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResult(intent, requestCode);
        }

        public IMvxBindingContext BindingContext { get; set; }

        public override void SetContentView(int layoutResId)
        {
            _view = this.BindingInflate(layoutResId, null);

            SetContentView(_view);
        }

        protected virtual void OnViewModelSet()
        {
        }

        protected override void AttachBaseContext(Context @base)
        {
            if (this is IMvxSetupMonitor)
            {
                // Do not attach our inflater to splash screens.
                base.AttachBaseContext(@base);
                return;
            }
            base.AttachBaseContext(MvxContextWrapper.Wrap(@base, this));
        }

        protected override void OnCreate(Bundle bundle)
        {
            // ensuring mvvmcross is running here is required
            // otherwise app will crash when inflating the view because of the Forms base class
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            setup.EnsureInitialized();

            base.OnCreate(bundle);
            ViewModel?.ViewCreated();
            RunAppStart(bundle);
        }

        protected virtual void RunAppStart(Bundle bundle)
        {
            InitializeForms(bundle);

            var startup = Mvx.IoCProvider.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start(GetAppStartHint(bundle));

            InitializeApplication();
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return null;
        }

        public virtual void InitializeForms(Bundle bundle)
        {
            if (!Xamarin.Forms.Forms.IsInitialized)
            {
                global::Xamarin.Forms.Forms.Init(this, bundle, GetResourceAssembly());
            }

            if (Xamarin.Forms.Application.Current != FormsApplication)
            {
                Xamarin.Forms.Application.Current = FormsApplication;
            }
        }

        public virtual void InitializeApplication()
        {
            LoadApplication(FormsApplication);
        }

        protected virtual Assembly GetResourceAssembly()
        {
            return this.GetType().Assembly;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel?.ViewDestroy(IsFinishing);
        }

        protected override void OnStart()
        {
            base.OnStart();
            ViewModel?.ViewAppearing();
        }

        protected override void OnResume()
        {
            base.OnResume();
            ViewModel?.ViewAppeared();
        }

        protected override void OnPause()
        {
            base.OnPause();
            ViewModel?.ViewDisappearing();
        }

        protected override void OnStop()
        {
            base.OnStop();
            ViewModel?.ViewDisappeared();
        }

        public override async void OnBackPressed()
        {
            var presenter = Mvx.IoCProvider.Resolve<IMvxFormsPagePresenter>();
            var pages = presenter.CurrentPageTree;

            for (var i = pages.Length - 1; i >= 0; i--)
            {
                var pg = pages[i];
                if (pg is Xamarin.Forms.NavigationPage navPage)
                {
                    if (pg.Navigation.ModalStack.Count > 0)
                    {
                        await pg.Navigation.PopModalAsync();
                        return;
                    }

                    if (pg.Navigation.NavigationStack.Count > 1)
                    {
                        var handled = pg.SendBackButtonPressed();
                        if (handled) return;
                    }
                }
                else
                {
                    var handled = pg.SendBackButtonPressed();
                    if (handled) return;
                }
            }

            MoveTaskToBack(true);
        }

        protected virtual void RegisterSetup()
        {
        }
    }

    public class MvxFormsApplicationActivity<TViewModel>
        : MvxFormsApplicationActivity
    , IMvxAndroidView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }

    public abstract class MvxFormsApplicationActivity<TMvxAndroidSetup, TApplication, TFormsApplication> : MvxFormsApplicationActivity
    where TMvxAndroidSetup : MvxFormsAndroidSetup<TApplication, TFormsApplication>, new()
    where TApplication : class, IMvxApplication, new()
    where TFormsApplication : Application, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxAndroidSetup>();
        }
    }

    public abstract class MvxFormsApplicationActivity<TMvxAndroidSetup, TApplication, TFormsApplication, TViewModel> : MvxFormsApplicationActivity<TViewModel>
    where TMvxAndroidSetup : MvxFormsAndroidSetup<TApplication, TFormsApplication>, new()
    where TApplication : class, IMvxApplication, new()
    where TFormsApplication : Application, new()
         where TViewModel : class, IMvxViewModel
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxAndroidSetup>();
        }
    }
}
