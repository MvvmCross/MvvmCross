using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Platform.Core;

namespace MvvmCross.Binding.BindingContext
{
#warning TODO - this is in the wrong namespace (blame R#)
    public interface IMvxEnhancedDataConsumer
        : IMvxDataConsumer
    {
        IMvxEnhancedDataContext EnhancedDataContext { get; set; }
    }
}