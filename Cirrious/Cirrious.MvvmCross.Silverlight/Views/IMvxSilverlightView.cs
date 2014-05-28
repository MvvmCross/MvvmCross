using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Silverlight.Views
{
    public interface IMvxSilverlightView
        : IMvxView
    {
        void ClearBackStack();
    }
}