using System;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxViewForAttribute : Attribute
    {
        public Type ViewModel { get; set; }

        public MvxViewForAttribute(Type viewModel)
        {
            ViewModel = viewModel;
        }
    }
}