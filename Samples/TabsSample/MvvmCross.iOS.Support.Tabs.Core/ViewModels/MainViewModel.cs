using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Support.Tabs.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        public MainViewModel()
        {
            ShowInitialViewModelsCommand = new MvxCommand(ShowInitialViewModels);
        }

        public ICommand ShowInitialViewModelsCommand { get; private set; }

        private void ShowInitialViewModels()
        {
            ShowViewModel<Tab1ViewModel>();
            ShowViewModel<Tab2ViewModel>(presentationBundle: new MvxBundle(new Dictionary<string, string> { { "icon_name", "ic_tabbar_menu" }, { "title", "Tab 2" } }));
        }
    }
}

