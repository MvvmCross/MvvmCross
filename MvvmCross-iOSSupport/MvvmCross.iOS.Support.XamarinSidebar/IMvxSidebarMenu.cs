using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public interface IMvxSidebarMenu
    {
        UIImage MenuButtonImage { get; }
        bool HasShadowing { get; }
        bool DisablePanGesture { get; }
        bool AnimateMenu { get; }
        int MenuWidth { get; }
    }
}

