using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.WindowsPhone.Interfaces
{
    public interface IMvxWindowsPhoneView<TViewModel>
        : IMvxView<TViewModel>
        , IMvxServiceConsumer<IMvxWindowsPhoneViewModelRequestTranslator>
        , IMvxServiceConsumer<IMvxViewModelLoader>
        where TViewModel : class, IMvxViewModel
    {
        void ClearBackStack();
    }
}