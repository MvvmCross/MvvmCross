using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public interface IMvxSidebarMenu
    {
        UIImage MenuButtonImage { get; }
        bool HasShadowing { get; }
        int MenuWidth { get; }
    }
}

