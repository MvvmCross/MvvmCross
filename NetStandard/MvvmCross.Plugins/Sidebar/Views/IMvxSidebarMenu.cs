using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar.Views
{
    public interface IMvxSidebarMenu
    {
        bool AnimateMenu { get; }
        bool DisablePanGesture { get; }
        float DarkOverlayAlpha { get; }
        bool HasDarkOverlay { get; }
        bool HasShadowing { get; }
        float ShadowOpacity { get; }
        float ShadowRadius { get; }
        UIColor ShadowColor { get; }
        UIImage MenuButtonImage { get; }
        int MenuWidth { get; }
        bool ReopenOnRotate { get; }
        void MenuWillOpen();
        void MenuDidOpen();
        void MenuWillClose();
        void MenuDidClose();
    }
}