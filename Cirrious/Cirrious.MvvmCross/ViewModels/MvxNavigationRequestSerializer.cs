using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxNavigationRequestSerializer
        : IMvxNavigationRequestSerializer
    {
        public MvxNavigationRequestSerializer(IMvxTextSerializer serializer)
        {
            Serializer = serializer;
        }

        public IMvxTextSerializer Serializer { get; private set; }
    }
}