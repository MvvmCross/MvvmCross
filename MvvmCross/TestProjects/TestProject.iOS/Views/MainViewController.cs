using MvvmCross.iOS.Views;
using TestProject.Core.ViewModels;

namespace TestProject.iOS
{
    public partial class MainViewController : MvxViewController<MainViewModel>
    {
        public MainViewController() : base("MainViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

