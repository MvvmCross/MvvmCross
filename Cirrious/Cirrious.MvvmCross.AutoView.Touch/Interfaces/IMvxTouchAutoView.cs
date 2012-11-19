using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Interfaces;

namespace Cirrious.MvvmCross.AutoView.Touch.Interfaces
{
    public interface IMvxTouchAutoView<TViewModel>
        : IMvxTouchView<TViewModel>
          , IMvxAutoView
          , IMvxBindingViewController
        where TViewModel : class, IMvxViewModel
    {
    }
}