using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.ViewModels;

namespace CustomerManagement.Droid.Views
{
    public abstract class BaseView<TViewModel>
        : MvxActivity
        where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}