using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Interfaces
{
    public interface IMvxBinder
    {
        IMvxUpdateableBinding Bind(MvxBindingRequest bindingRequest);
        IEnumerable<IMvxUpdateableBinding> Bind(object source, object target, string bindingText);
        IEnumerable<IMvxUpdateableBinding> Bind(object source, object target,
                                                IEnumerable<MvxBindingDescription> bindingDescriptions);
    }
}