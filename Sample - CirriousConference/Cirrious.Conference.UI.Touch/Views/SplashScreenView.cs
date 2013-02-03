using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.Conference.UI.Touch.Views
{
    public class SplashScreenView
        : MvxBindingTableViewController
    {
		public new SplashScreenViewModel ViewModel {
			get { return (SplashScreenViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
#warning TODO!
            ViewModel.SplashScreenComplete = true;
        }
    }
}