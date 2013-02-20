using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Views
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