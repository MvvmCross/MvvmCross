using MvvmCross.iOS.Views;
using SidebarNavigation;
using UIKit;
using MvvmCross.iOS.Support.Core.ViewModels;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public partial class RootViewController : UIViewController, IMvxCanCreateIosView
    {
        public SidebarNavigation.SidebarController SidebarController { get; set; }

		public UINavigationController ContentController;
		public UIViewController MenuController;

        private static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        private int MaxMenuWidth = 300;
        private int MinSpaceRightOfTheMenu = 55;

        public RootViewController() : base(null, null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.

			ContentController = new UINavigationController();
			MenuController = new UIViewController();

			SidebarController = new SidebarController(this, ContentController, MenuController)
            {
				MenuLocation = MenuLocations.Left,
                HasShadowing = true,
                MenuWidth = UserInterfaceIdiomIsPhone ? int.Parse(UIScreen.MainScreen.Bounds.Width.ToString()) - MinSpaceRightOfTheMenu :
                    MaxMenuWidth,
            };


        }
    }
}