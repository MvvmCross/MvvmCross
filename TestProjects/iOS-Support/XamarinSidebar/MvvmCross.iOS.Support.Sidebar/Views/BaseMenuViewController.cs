using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Support.XamarinSidebar.Views;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS.Views
{
    public class BaseMenuViewController<TViewModel> : BaseViewController<TViewModel>, IMvxSidebarMenu
        where TViewModel : class, IMvxViewModel
    {
        private readonly int MaxMenuWidth = 300;
        private readonly int MinSpaceRightOfTheMenu = 55;

        private bool UserInterfaceIdiomIsPhone => UIDevice.CurrentDevice.UserInterfaceIdiom ==
                                                  UIUserInterfaceIdiom.Phone;

        public virtual UIImage MenuButtonImage => UIImage.FromBundle("threelines");

        public virtual bool AnimateMenu => true;
        public virtual float DarkOverlayAlpha => 0;
        public virtual bool HasDarkOverlay => false;
        public virtual bool HasShadowing => true;
        public virtual bool DisablePanGesture => false;
        public virtual bool ReopenOnRotate => true;

        public int MenuWidth => UserInterfaceIdiomIsPhone
            ? int.Parse(UIScreen.MainScreen.Bounds.Width.ToString()) - MinSpaceRightOfTheMenu
            : MaxMenuWidth;
    }
}