using Cirrious.MvvmCross.ViewModels;

namespace Example.Core.ViewModels
{
    public class MainViewModel
        : MvxViewModel
    {
        public void Init()
        {
        }

        public void ShowMenu()
        {
            ShowViewModel<MenuViewModel>();
            ShowViewModel<ExamplesViewModel>();
        }
    }
}