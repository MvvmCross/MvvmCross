using MvvmCross.Core.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Core
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.Appearing();
            ViewModel?.Appeared();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel?.Disappearing();
            ViewModel?.Disappeared();
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


