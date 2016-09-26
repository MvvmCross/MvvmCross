using UIKit;

namespace MvvmCross.iOS.Support.Sidebar.Views
{
    using MvvmCross.Core.ViewModels;
    using XamarinSidebar;

    public class BaseMenuViewController<TViewModel> : BaseViewController<TViewModel>, IMvxSidebarMenu where TViewModel : class, IMvxViewModel
    {
        public virtual UIImage MenuButtonImage => UIImage.FromBundle("threelines");

        public virtual bool HasShadowing => true;
        public virtual int MenuWidth => 320;
    }
}