// IMvxBinder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Binders
{
    using System.Collections.Generic;

    using MvvmCross.Binding.Bindings;

    public interface IMvxBinder
    {
        IEnumerable<IMvxUpdateableBinding> Bind(object source, object target, string bindingText);

        IEnumerable<IMvxUpdateableBinding> LanguageBind(object source, object target, string bindingText);

        IEnumerable<IMvxUpdateableBinding> Bind(object source, object target,
                                                IEnumerable<MvxBindingDescription> bindingDescriptions);

        IMvxUpdateableBinding BindSingle(object source, object target, string targetPropertyName,
                                         string partialBindingDescription);

        IMvxUpdateableBinding BindSingle(MvxBindingRequest bindingRequest);
    }
}