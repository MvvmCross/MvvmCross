using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.AutoView.Interfaces
{
    public interface IMvxAutoView
        : IMvxView
        , IMvxServiceConsumer
    {
    }
}