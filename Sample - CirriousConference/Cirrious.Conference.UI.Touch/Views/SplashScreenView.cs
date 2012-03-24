using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.Conference.UI.Touch.Views
{
    public class SplashScreenView
        : MvxBindingTouchTableViewController<SplashScreenViewModel>
    {
        public SplashScreenView(MvxShowViewModelRequest request)
            : base(request)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
#warning TODO!
            ViewModel.SplashScreenComplete = true;
        }
    }
}