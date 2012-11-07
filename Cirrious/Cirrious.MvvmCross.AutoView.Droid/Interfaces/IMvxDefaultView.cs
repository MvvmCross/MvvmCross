using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.AutoView.Droid.Interfaces
{
    public interface IMvxDefaultView<TViewModel>
        : IMvxView<TViewModel>
          , IMvxServiceConsumer
        where TViewModel : class, IMvxViewModel
    {
    }
}