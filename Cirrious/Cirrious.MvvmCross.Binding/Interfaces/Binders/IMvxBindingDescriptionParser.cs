using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Interfaces.Binders
{
    public interface IMvxBindingDescriptionParser
    {
        IEnumerable<MvxBindingDescription> Parse(string text);
    }
}