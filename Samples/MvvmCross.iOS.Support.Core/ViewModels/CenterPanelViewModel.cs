namespace MvvmCross.iOS.Support.Core.ViewModels
{
    using MvvmCross.Core.ViewModels;

    public class CenterPanelViewModel : BaseViewModel
    {
        public CenterPanelViewModel()
        {
            ExampleValue = "Center Panel";

            // show the other panels
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

        private void ShowMasterCommandExecuted()
        {
            ShowViewModel<MasterViewModel>();
        }
    }
}
