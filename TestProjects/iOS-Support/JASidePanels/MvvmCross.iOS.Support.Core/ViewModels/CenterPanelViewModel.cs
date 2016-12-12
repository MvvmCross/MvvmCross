namespace MvvmCross.iOS.Support.JASidePanelsSample.Core.ViewModels
{
    using MvvmCross.Core.ViewModels;

    public class CenterPanelViewModel : BaseViewModel
    {
        public CenterPanelViewModel()
        {
            ExampleValue = "Center Panel";

            // Show the various available side panels
            // Only required by the iOS demo application, not required for SideBar demo but
            // has been re-introduced to ensure both demos function correctly
            ShowViewModel<LeftPanelViewModel>();
            ShowViewModel<RightPanelViewModel>();
        }

        public string RightPanelInstructions
        {
            get { return "Drag from the right hand side to show the right hand panel!  To see the SplitView feature, launch the app on an iPad simulator!"; }
        }

        public IMvxCommand ShowMasterCommand
        {
            get
            {
                return new MvxCommand(ShowMasterCommandExecuted);
            }
        }

        /// <summary>
        /// Used to show the menu in the SideBar demo application.
        /// </summary>
        public void ShowMenu()
        {
            // Loads the flyout menu on the left
            ShowViewModel<LeftPanelViewModel>();
        }

        /// <summary>
        /// Shows the master view.
        /// </summary>
        /// <remarks>
        /// When the iOS demo application is launched on a large screen device this loads a splitview
        /// controller with master/detail view locations.
        /// </remarks>
        private void ShowMasterCommandExecuted()
        {
            ShowViewModel<MasterViewModel>();
        }

        public IMvxCommand ShowKeyboardHandlingCommand
        {
            get
            {
                return new MvxCommand(ShowKeyboardHandlingCommandExecuted);
            }
        }

        /// <summary>
        /// Shows the keyboard handling view.
        /// </summary>
        private void ShowKeyboardHandlingCommandExecuted()
        {
            ShowViewModel<KeyboardHandlingViewModel>();
        }
    }
}