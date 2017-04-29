using MvvmCross.Core.ViewModels;

namespace Example.Core.ViewModels
{
    public class SecondHostViewModel
        : MvxViewModel
    {
        public void ShowMenu()
        {
            ShowViewModel<HomeViewModel>();
        }
    }
}