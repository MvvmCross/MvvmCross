using MvvmCross.Core.ViewModels;

namespace Example.Core.ViewModels
{
    public class MainViewModel
        : MvxViewModel
    {
        public void ShowMenu()
        {
            ShowViewModel<MenuViewModel>();
        }
    }
}