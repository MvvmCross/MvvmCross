using System;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
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
#warning This code is twinkle in Stuart's eye code only!
    public abstract class MvxFragment<TViewModel>
        : Fragment
        , IMvxAndroidFragmentView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxFragment()
        {
        }

        #region Common code across all android views - one case for multiple inheritance?

        private TViewModel _viewModel;

        public Type ViewModelType
        {
            get { return typeof (TViewModel); }
        }
        
        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnViewModelSet();
            }
        }

        protected abstract void OnViewModelSet();

        public override void OnCreate(Bundle p0)
        {
            // TODO - consider telling the ViewModel here
            // ViewModel.
            // probably via an extension method
            base.OnCreate(p0);
        }

        public override void OnDestroy()
        {
            // TODO - consider telling the ViewModel here
            // ViewModel.
            // probably via an extension method
            base.OnDestroy();
        }

        #endregion
    }

    public interface IMvxAndroidFragmentView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
    }
}
