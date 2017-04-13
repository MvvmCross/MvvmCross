using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS.Views
{
    using MvvmCross.Core.ViewModels;
    using XamarinSidebar;

    public class BaseMenuViewController<TViewModel> : BaseViewController<TViewModel>, IMvxSidebarMenu where TViewModel : class, IMvxViewModel
    {
        public virtual UIImage MenuButtonImage => UIImage.FromBundle("threelines");

        public virtual bool AnimateMenu => true;
        public virtual float DarkOverlayAlpha => 0;
        public virtual bool HasDarkOverlay => false;
        public virtual bool HasShadowing => true;
        public virtual bool DisablePanGesture => false;
        public virtual bool ReopenOnRotate => true;

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