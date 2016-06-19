using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Support.Core.ViewModels
{
    public class LeftPanelViewModel : BaseViewModel
    {      
        private MvxCommand showExampleMenuItemCommand;

        public MvxCommand ShowExampleMenuItemCommand
        {
            get
            {
                showExampleMenuItemCommand = showExampleMenuItemCommand ?? new MvxCommand(DoShowExampleMenuItem);
                return showExampleMenuItemCommand;
            }
        }

        private void DoShowExampleMenuItem()
        {
            ShowViewModel<ExampleMenuItemViewModel>();
        }

        private MvxCommand showCenterPanelCommand;

        public MvxCommand ShowCenterPanelCommand
        {
            get
            {
                showCenterPanelCommand = showCenterPanelCommand ?? new MvxCommand(DoShowCenterPanel);
                return showCenterPanelCommand;
            }
        }

        private void DoShowCenterPanel()
        {
            ShowViewModel<CenterPanelViewModel>();
        }
    }
}