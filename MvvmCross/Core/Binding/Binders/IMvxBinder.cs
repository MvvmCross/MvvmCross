// IMvxBinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Binders
{
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