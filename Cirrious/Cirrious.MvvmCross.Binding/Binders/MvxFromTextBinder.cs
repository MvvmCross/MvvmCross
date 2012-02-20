using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxFromTextBinder
        : IMvxBinder
        , IMvxServiceConsumer<IMvxBindingDescriptionParser>
    {
        public IEnumerable<IMvxUpdateableBinding> Bind(object source, object target, string bindingText)
        {
            var bindingDescriptions = this.GetService<IMvxBindingDescriptionParser>().Parse(bindingText);
            if (bindingDescriptions == null)
                return null;

            return Bind(source, target, bindingDescriptions);
        }

        public IEnumerable<IMvxUpdateableBinding> Bind(object source, object target, IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            return bindingDescriptions.Select(description => BindSingle(new MvxBindingRequest(source, target, description)));
        }

        public IMvxUpdateableBinding BindSingle(MvxBindingRequest bindingRequest)
        {
            return new MvxFullBinding(bindingRequest);
        }
    }
}