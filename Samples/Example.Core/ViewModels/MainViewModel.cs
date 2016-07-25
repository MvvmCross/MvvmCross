using MvvmCross.Core.ViewModels;

namespace Example.Core.ViewModels
{
    public class MainViewModel
        : MvxViewModel
    {
        public MainViewModel()
        {
        }

        public void ShowMenu()
        {
            ShowViewModel<MenuViewModel>();
        }
    }
}