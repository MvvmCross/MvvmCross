using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Support.XamarinSidebar;

namespace MvvmCross.iOS.Support.iOS.Views
{
    public class BaseMenuViewController<TViewModel> : BaseViewController<TViewModel>, IMvxSidebarMenu where TViewModel : class, IMvxViewModel
    {
        public bool HasShadowing => true;
        public int MenuWidth => 320;
    }
}

