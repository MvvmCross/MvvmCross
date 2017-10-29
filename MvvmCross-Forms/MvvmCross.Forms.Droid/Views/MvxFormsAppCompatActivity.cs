using System.Reflection;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Droid.Views.EventSource;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Views;
using MvvmCross.Platform;

namespace MvvmCross.Forms.Droid.Views
{
    public class MvxFormsAppCompatActivity : MvxEventSourceFormsAppCompatActivity, IMvxAndroidView
    {
        private View _view;
        private readonly Assembly _resourceAssembly;

        protected MvxFormsAppCompatActivity()
        {
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
            _resourceAssembly = Assembly.GetCallingAssembly();
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

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResult(intent, requestCode);
        }

        protected virtual void OnViewModelSet()
        {
        }

        public IMvxBindingContext BindingContext { get; set; }

        public override void SetContentView(int layoutResId)
        {
            _view = this.BindingInflate(layoutResId, null);

            SetContentView(_view);
        }

        protected override void AttachBaseContext(Context @base)
        {
            if (this is IMvxAndroidSplashScreenActivity)
            {
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
            InitializeForms(bundle);
        }

        protected virtual void InitializeForms(Bundle bundle)
        {
            var resourceAssembly = GetResourceAssembly();
            global::Xamarin.Forms.Forms.Init(this, bundle, resourceAssembly);
            LoadApplication(FormsApplication);
        }

        protected virtual Assembly GetResourceAssembly()
        {
            return _resourceAssembly;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel?.ViewDestroy();
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

        public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            var view = MvxAppCompatActivityHelper.OnCreateView(parent, name, context, attrs);
            return view ?? base.OnCreateView(parent, name, context, attrs);
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
