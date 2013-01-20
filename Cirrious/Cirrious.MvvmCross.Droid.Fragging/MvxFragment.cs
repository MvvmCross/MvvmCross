using System;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Cirrious.MvvmCross.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;

/*
 * This is not finished code - in fact it's hardly even started code!
 * 
 * Things to consider:
 * 1. Would be nice to remove the lifecycle thing that doesn't really work anyway!
 * 2. Would be lovely to avoice the way that the GetActivityForResult works - how does AzureMoileServices do that?
 * 3. Would be cool to improve memory use - e.g. when bindings get torn down 
 * 4. Essential that the fragment stuff makes sense
 * 5. Take a look at the Android Google IO conference sample from Google - treat that as best practice?
 * 6. Maybe just try porting the conference sample across? 
 */ 
    

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public abstract class MvxFragment<TViewModel>
        : Fragment
        , IMvxAndroidView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxFragment()
        {
            IsVisible = true;
        }

        #region Common code across all android views - one case for multiple inheritance?

        private TViewModel _viewModel;

        public Type ViewModelType
        {
            get { return typeof (TViewModel); }
        }
        
        public bool IsVisible { get; private set; }

        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnViewModelSet();
            }
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            base.StartActivityForResult(intent, requestCode);
        }

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.OnViewCreate();
        }

        public override void OnDestroy()
        {
            this.OnViewDestroy();
            base.OnDestroy();
        }

        /*
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            this.OnViewNewIntent();
        }
         */

        protected abstract void OnViewModelSet();

        public override void OnResume()
        {
            base.OnResume();
            IsVisible = true;
            this.OnViewResume();
        }

        public override void OnPause()
        {
            this.OnViewPause();
            IsVisible = false;
            base.OnPause();
        }

        public override void OnStart()
        {
            base.OnStart();
            this.OnViewStart();
        }

#warning OnRestart is missing
        /*
        protected override void OnRestart()
        {
            base.OnRestart();
            this.OnViewRestart();
        }
         */

        public override void OnStop()
        {
            this.OnViewStop();
            base.OnStop();
        }

        /*
        public override void StartActivityForResult(Intent intent, int requestCode)
        {
            switch (requestCode)
            {
                case (int) MvxIntentRequestCode.PickFromFile:
                    MvxTrace.Trace("Warning - activity request code may clash with Mvx code for {0}",
                                   (MvxIntentRequestCode) requestCode);
                    break;
                default:
                    // ok...
                    break;
            }
            base.StartActivityForResult(intent, requestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            this.GetService<IMvxIntentResultSink>()
                .OnResult(new MvxIntentResultEventArgs(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }
        */

        #endregion
    }
}
