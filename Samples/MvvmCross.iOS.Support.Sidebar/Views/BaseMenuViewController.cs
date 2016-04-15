using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Support.XamarinSidebar;

namespace MvvmCross.iOS.Support.iOS.Views
{
    public class BaseMenuViewController<TViewModel> : BaseViewController<TViewModel>, IXamarinSidebarMenu where TViewModel : class, IMvxViewModel
    {
        public bool HasShadowing { get { return true; } }
        public int MenuWidth { get { return 320; } }
    }
}

