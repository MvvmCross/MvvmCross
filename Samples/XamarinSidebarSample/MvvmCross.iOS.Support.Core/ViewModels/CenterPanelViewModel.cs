namespace MvvmCross.iOS.Support.XamarinSidebarSample.Core.ViewModels
{
    using MvvmCross.Core.ViewModels;

    public class CenterPanelViewModel : BaseViewModel
    {
        public CenterPanelViewModel()
        {
            ExampleValue = "Center Panel";
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