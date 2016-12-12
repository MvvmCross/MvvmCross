using MvvmCross.Core.ViewModels;
using System;

namespace Example.Core.ViewModels
{
    public class MenuViewModel
        : MvxViewModel
    {
        public MenuViewModel()
        {
        }

        public void ShowViewModelAndroid(Type viewModel)
        {
            ShowViewModel(viewModel);
        }
    }
}