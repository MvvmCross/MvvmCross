using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.AutoViews.Core.Interfaces;

namespace CustomerManagement.Droid
{
    public class SimpleDroidViewModelCloser
        : IViewModelCloser
    {
        public void RequestClose(IMvxViewModel viewModel)
        {
            var topActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            
            var activity = topActivity.Activity;
            var view = activity as IMvxAndroidView;

            if (view == null)
            {
                MvxTrace.Trace("request close ignored for {0} - no current activity", viewModel.GetType().Name);
                return;
            }
            
            if (view.ViewModel != viewModel)
            {
                MvxTrace.Trace("request close ignored for {0} - current activity is registered for a different viewmodel of type {1}", viewModel.GetType().Name, view.ViewModel.GetType().Name);
                return;
            }

            MvxTrace.Trace("request close for {0} - will close current activity {1}", viewModel.GetType().Name, view.GetType().Name);
            activity.Finish();
        }
    }
}