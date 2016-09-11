namespace MvvmCross.iOS.Support.Sidebar.Views
{
    using MvvmCross.Core.ViewModels;
    using XamarinSidebar;

    public class BaseMenuViewController<TViewModel> : BaseViewController<TViewModel>, IMvxSidebarMenu where TViewModel : class, IMvxViewModel
    {
        public bool HasShadowing => true;
        public int MenuWidth => 320;
    }
}