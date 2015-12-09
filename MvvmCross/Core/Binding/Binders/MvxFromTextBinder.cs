// MvxFromTextBinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings;
using System.Collections.Generic;
using System.Linq;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxFromTextBinder
        : IMvxBinder
    {
        public IEnumerable<IMvxUpdateableBinding> Bind(object source, object target, string bindingText)
        {
            var bindingDescriptions = MvxBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingText);
            return Bind(source, target, bindingDescriptions);
        }

        public IEnumerable<IMvxUpdateableBinding> LanguageBind(object source, object target, string bindingText)
        {
            var bindingDescriptions =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.LanguageParse(bindingText);
            return Bind(source, target, bindingDescriptions);
        }

        public IEnumerable<IMvxUpdateableBinding> Bind(object source, object target,
                                                       IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            if (bindingDescriptions == null)
                return new IMvxUpdateableBinding[0];

            return
                bindingDescriptions.Select(description => BindSingle(new MvxBindingRequest(source, target, description)));
        }

        public IMvxUpdateableBinding BindSingle(object source, object target, string targetPropertyName,
                                                string partialBindingDescription)
        {
            var bindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(partialBindingDescription);
            if (bindingDescription == null)
                return null;

            bindingDescription.TargetName = targetPropertyName;
            var request = new MvxBindingRequest(source, target, bindingDescription);
            return BindSingle(request);
        }

        public IMvxUpdateableBinding BindSingle(MvxBindingRequest bindingRequest)
        {
            return new MvxFullBinding(bindingRequest);
        }
    }
}