using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public interface IMvxModalIosView : IMvxView
    {
    }

    public interface IMvxModalIosView<TViewModel>
        : IMvxModalIosView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}