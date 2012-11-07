using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.AutoView.Droid.Interfaces
{
    public interface IMvxDefaultAndroidView<TViewModel>
        : IMvxDefaultView<TViewModel>
          , IMvxAndroidView<TViewModel>
          , IMvxBindingActivity
        where TViewModel : class, IMvxViewModel
    {
    }
}