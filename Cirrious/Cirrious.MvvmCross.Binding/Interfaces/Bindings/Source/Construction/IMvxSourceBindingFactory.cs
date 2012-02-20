using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction
{
    public interface IMvxSourceBindingFactory
    {
        IMvxSourceBinding CreateBinding(object source, string combinedPropertyName);
        IMvxSourceBinding CreateBinding(object source, IEnumerable<string> childPropertyNames);
    }
}