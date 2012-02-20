using Cirrious.MvvmCross.Android.Views;
using Cirrious.MvvmCross.Binding.Android.Views;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace CustomerManagement.Droid.Views
{
    public abstract class BaseView<TViewModel>
        : MvxBindingActivityView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {        
    }
}