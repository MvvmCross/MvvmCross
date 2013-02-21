using Cirrious.CrossCore.Interfaces.Platform;

namespace Cirrious.MvvmCross.Interfaces.ViewModels
{
    public interface IMvxNavigationRequestSerializer
    {
        IMvxTextSerializer Serializer { get; }
    }
}