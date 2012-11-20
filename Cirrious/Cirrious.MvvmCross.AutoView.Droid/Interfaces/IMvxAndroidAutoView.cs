using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.AutoView.Droid.Interfaces
{
    public interface IMvxAndroidAutoView<TViewModel>
        : IMvxAndroidView<TViewModel>
        , IMvxAutoView
        , IMvxBindingActivity
        where TViewModel : class, IMvxViewModel
    {
    }
}