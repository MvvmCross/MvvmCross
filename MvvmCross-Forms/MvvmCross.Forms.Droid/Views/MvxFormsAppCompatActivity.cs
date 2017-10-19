using System.Reflection;
using Android.Content;
using Android.OS;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Xamarin.Forms.Platform.Android;

namespace MvvmCross.Forms.Droid.Views
{
    public class MvxFormsAppCompatActivity : FormsAppCompatActivity, IMvxAndroidView
    {
        public IMvxBindingContext BindingContext { get; set; }

        private IMvxAndroidActivityLifetimeListener _lifetimeListener;
        private IMvxAndroidActivityLifetimeListener LifetimeListener
        {
            get
            {
                if (_lifetimeListener == null)
                {
                    _lifetimeListener = Mvx.Resolve<IMvxAndroidActivityLifetimeListener>();
                }

                return _lifetimeListener;
            }
        }

        private MvxFormsApplication _formsApplication;
        protected MvxFormsApplication FormsApplication
        {
            get
            {
                if (_formsApplication == null)
                {
                    var formsPresenter = Mvx.Resolve<IMvxFormsViewPresenter>();
                    _formsApplication = formsPresenter.FormsApplication;
                }
                return _formsApplication;
            }
        }

        public object DataContext
        {
            get => BindingContext?.DataContext;
            set => BindingContext.DataContext = value;
        }

        public IMvxViewModel ViewModel
        {
            get => DataContext as IMvxViewModel;
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        protected virtual void OnViewModelSet()
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            InitializeForms(bundle);
        }

        protected virtual void InitializeForms(Bundle bundle)
        {
            // Required for proper Push notifications handling
            var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            setupSingleton.EnsureInitialized();
            LifetimeListener.OnCreate(this, bundle);

            var resourceAssembly = GetResourceAssembly();
            global::Xamarin.Forms.Forms.Init(this, bundle, resourceAssembly);
            LoadApplication(FormsApplication);
        }

        protected virtual Assembly GetResourceAssembly()
        {
            return Assembly.GetCallingAssembly();
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResult(intent, requestCode);
        }

        protected override void OnStart()
        {
            base.OnStart();

            LifetimeListener.OnStart(this);
        }

        protected override void OnStop()
        {
            base.OnStop();

            LifetimeListener.OnStop(this);
        }

        protected override void OnResume()
        {
            base.OnResume();

            LifetimeListener.OnResume(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            LifetimeListener.OnDestroy(this);
        }

        protected override void OnPause()
        {
            base.OnPause();

            LifetimeListener.OnPause(this);
        }

        protected override void OnRestart()
        {
            base.OnRestart();

            LifetimeListener.OnRestart(this);
        }
    }

    public class MvxFormsAppCompatActivity<TViewModel>
        : MvxFormsAppCompatActivity
    , IMvxAndroidView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
