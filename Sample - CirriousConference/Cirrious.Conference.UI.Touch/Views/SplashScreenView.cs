using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Touch.Views;

namespace Cirrious.Conference.UI.Touch.Views
{
    public class SplashScreenView
        : MvxTableViewController
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