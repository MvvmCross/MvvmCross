using MvvmCross.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar.Views
{
    public interface IMvxSidebarViewController
    {
        /// <summary>
        /// Closes the active menu, if none are open nothing will happen.
        /// When multiple are open, all will close.
        /// </summary>
        void CloseMenu();

        /// <summary>
        /// Opens the left or the right menu (as indicated by the parameter).
        /// </summary>
        /// <param name="panelEnum">Indicates if either the left or the right menu should be opened.</param>
        void Open(MvxPanelEnum panelEnum);

        void SetNavigationController(UINavigationController navigationController);

        bool CloseChildViewModel(IMvxViewModel viewModel);
    }
}