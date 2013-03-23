using System;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxViewModelAttribute : Attribute
    {
        public Type ViewModel { get; set; }

        public MvxViewModelAttribute(Type viewModel)
        {
            ViewModel = viewModel;
        }
    }
}