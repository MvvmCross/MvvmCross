using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Navigation.EventArguments
{
    public class NavigateEventArgs : EventArgs
    {
        public NavigateEventArgs()
        {
        }

        public NavigateEventArgs(IMvxViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public IMvxViewModel ViewModel { get; set; }
    }
}
