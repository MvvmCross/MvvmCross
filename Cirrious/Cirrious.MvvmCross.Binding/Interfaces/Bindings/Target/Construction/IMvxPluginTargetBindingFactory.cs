using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction
{
    public interface IMvxPluginTargetBindingFactory : IMvxTargetBindingFactory
    {
        IEnumerable<MvxTypeAndNamePair> SupportedTypes { get; }
    }
}