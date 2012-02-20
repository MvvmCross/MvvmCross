using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Interfaces
{
    public interface IMvxBinder
    {
        IEnumerable<IMvxUpdateableBinding> Bind(object source, object target, string bindingText);
        IEnumerable<IMvxUpdateableBinding> Bind(object source, object target,
                                                IEnumerable<MvxBindingDescription> bindingDescriptions);
		IMvxUpdateableBinding BindSingle(MvxBindingRequest bindingRequest);
    }
}