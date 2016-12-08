using UIKit;

namespace MvvmCross.iOS.Support.JASidePanelsSample.iOS.Views
{
    using MvvmCross.Core.ViewModels;

    public class BaseMenuViewController<TViewModel> : BaseViewController<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public virtual UIImage MenuButtonImage => UIImage.FromBundle("threelines");

        public virtual bool HasShadowing => true;
        private int MaxMenuWidth = 300;
        private int MinSpaceRightOfTheMenu = 55;

        public int MenuWidth => UserInterfaceIdiomIsPhone ?
        int.Parse(UIScreen.MainScreen.Bounds.Width.ToString()) - MinSpaceRightOfTheMenu : MaxMenuWidth;

        private bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

    }
}