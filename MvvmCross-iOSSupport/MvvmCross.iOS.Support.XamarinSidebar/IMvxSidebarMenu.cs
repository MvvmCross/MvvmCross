using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public interface IMvxSidebarMenu
    {
        bool AnimateMenu { get; }
        bool DisablePanGesture { get; }
        bool HasShadowing { get; }
        UIImage MenuButtonImage { get; }
        
        int MenuWidth { get; }
    }
}

