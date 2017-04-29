using MvvmCross.Core.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Core
{
    public class MvxContentPage : ContentPage, IMvxContentPage
    {
        public object DataContext
        {
            get => BindingContext;
            set => BindingContext = value;
        }

        public IMvxViewModel ViewModel
        {
            get => DataContext as IMvxViewModel;
            set => DataContext = value;
        }

        public MvxViewModelRequest Request { get; set; }
    }

    public class MvxContentPage<TViewModel>
        : MvxContentPage
            , IMvxContentPage<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}