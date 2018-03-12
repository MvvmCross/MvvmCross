// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platform.Android.Views.Base;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platform.Android.Binding.BindingContext;
using MvvmCross.Platform.Android.Binding.Views;
using MvvmCross.Platform.Android.Core;
using MvvmCross.Platform.Android.Views;
using MvvmCross.ViewModels;
using Application = Xamarin.Forms.Application;

namespace MvvmCross.Forms.Platform.Android.Views
{
    public abstract class MvxFormsApplicationActivity : MvxEventSourceFormsApplicationActivity, IMvxAndroidView
    {
        protected View _view;

        protected MvxFormsApplicationActivity()
        {
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
                if (_formsApplication == null) {
                    var formsPresenter = Mvx.Resolve<IMvxFormsViewPresenter>();
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
            if (this is IMvxAndroidSplashScreenActivity) {
                // Do not attach our inflater to splash screens.
                base.AttachBaseContext(@base);
                return;
            }
            base.AttachBaseContext(MvxContextWrapper.Wrap(@base, this));
        }

        protected override void OnCreate(Bundle bundle)
        {
            // Required for proper Push notifications handling      
            var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            setupSingleton.EnsureInitialized();

            base.OnCreate(bundle);
            ViewModel?.ViewCreated();
            RunAppStart(bundle);
        }

        protected virtual void RunAppStart(Bundle bundle)
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start();

            InitializeForms(bundle);
        }

        public virtual void InitializeForms(Bundle bundle)
        {
            if (!Xamarin.Forms.Forms.IsInitialized) {
                global::Xamarin.Forms.Forms.Init(this, bundle, GetResourceAssembly());
            }

            if (Xamarin.Forms.Application.Current != FormsApplication) {
                Xamarin.Forms.Application.Current = FormsApplication;
            }

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
}
