using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace CustomerManagement.AutoViews.Droid.Views
{
    public abstract class BaseView<TViewModel>
        : MvxBindingActivityView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {        
    }
}