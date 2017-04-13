
using Xamarin.Forms;
using MvvmCross.Core.ViewModels;
using MvvmCross.Binding.BindingContext;

namespace MvvmCross.Forms.Presenter.Core
{
    public class MvxContentPage : ContentPage, IMvxContentPage
    {
        public object DataContext
        {
            get { return BindingContext; }
            set { BindingContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }
    }

    public class MvxContentPage<TViewModel>
        : MvxContentPage
    , IMvxContentPage<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}


