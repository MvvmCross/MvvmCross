using System;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;

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
            ShowViewModel<MenuViewModel> ();
            ShowViewModel<ExamplesViewModel> ();
        }
    }
}

