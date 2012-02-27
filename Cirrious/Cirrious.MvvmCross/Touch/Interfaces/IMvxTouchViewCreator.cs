using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Interfaces
{
    public interface IMvxTouchViewCreator
    {
        IMvxTouchView CreateView(MvxShowViewModelRequest request);
    }
}