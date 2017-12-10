using System;

namespace MvvmCross.Core.ViewModels.Hints
{
    //Only available on Xamarin.Forms
    public class MvxPagePresentationHint
        : MvxPresentationHint
    {
        public MvxPagePresentationHint(Type viewModel)
        {
            ViewModel = viewModel;
        }

        public Type ViewModel { get; private set; }
    }
}
