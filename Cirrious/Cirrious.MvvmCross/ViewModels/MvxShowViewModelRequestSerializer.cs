using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Views
{
    public class MvxShowViewModelRequestSerializer
        : IMvxShowViewModelRequestSerializer
    {
        public MvxShowViewModelRequestSerializer(IMvxTextSerializer serializer)
        {
            Serializer = serializer;
        }

        public IMvxTextSerializer Serializer { get; private set; }
    }
}