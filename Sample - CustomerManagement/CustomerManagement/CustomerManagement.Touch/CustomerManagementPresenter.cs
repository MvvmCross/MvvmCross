using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.Core.Interfaces;
using MonoTouch.UIKit;

namespace CustomerManagement.Touch
{
    public class CustomerManagementPresenter 
        : MvxTouchViewPresenter
          , IViewModelCloser
    {
        public CustomerManagementPresenter(UIApplicationDelegate applicationDelegate, UIWindow window) 
            : base(applicationDelegate, window)
        {
        }

        public void RequestClose(IMvxViewModel viewModel)
        {
            var nav = MasterNavigationController;
            var top = nav.TopViewController;
            var view = top as IMvxTouchView;

            if (view == null)
            {
                MvxTrace.Trace("request close ignored for {0} - no current view controller", viewModel.GetType().Name);
                return;
            }

            if (view.ViewModel != viewModel)
            {
                MvxTrace.Trace("request close ignored for {0} - current view controller is registered for a different viewmodel of type {1}", viewModel.GetType().Name, view.ViewModel.GetType().Name);
                return;
            }

            MvxTrace.Trace("request close for {0} - will close current view controller {1}", viewModel.GetType().Name, view.GetType().Name);
            nav.PopViewControllerAnimated(true);
        }
    }
}