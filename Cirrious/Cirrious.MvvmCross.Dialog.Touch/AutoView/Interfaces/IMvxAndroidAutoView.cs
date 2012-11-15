using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Interfaces;

namespace Cirrious.MvvmCross.Dialog.Touch.AutoView.Interfaces
{
    public interface IMvxBindingViewController
    {
        // TODO    
        void RegisterBinding(IMvxUpdateableBinding binding);
    }

    public interface IMvxTouchAutoView<TViewModel>
        : IMvxTouchView<TViewModel>
        , IMvxAutoView
        , IMvxBindingViewController
        where TViewModel : class, IMvxViewModel
    {
    }
}