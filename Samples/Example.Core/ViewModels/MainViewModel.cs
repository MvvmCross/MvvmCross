using Cirrious.MvvmCross.ViewModels;

namespace Example.Core.ViewModels
{
    public class MainViewModel
        : MvxViewModel
    {
        public ExamplesViewModel Examples { get; private set; }

        public MainViewModel() {
            Examples = new ExamplesViewModel();
        }

        public void ShowMenu()
        {
            ShowViewModel<MenuViewModel>();
            ShowViewModel<ExamplesViewModel>();
        }
    }
}