// MvxFromTextBinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxFromTextBinder
        : IMvxBinder
        , IMvxServiceConsumer
    {
        #region IMvxBinder Members

        public IEnumerable<IMvxUpdateableBinding> Bind(object source, object target, string bindingText)
        {
            var bindingDescriptions = this.GetService<IMvxBindingDescriptionParser>().Parse(bindingText);
            if (bindingDescriptions == null)
                return null;

            return Bind(source, target, bindingDescriptions);
        }

        public IEnumerable<IMvxUpdateableBinding> Bind(object source, object target,
                                                       IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            return
                bindingDescriptions.Select(description => BindSingle(new MvxBindingRequest(source, target, description)));
        }

        public IMvxUpdateableBinding BindSingle(object source, object target, string targetPropertyName,
                                                string partialBindingDescription)
        {
            var bindingDescription = this.GetService<IMvxBindingDescriptionParser>().ParseSingle(partialBindingDescription);
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

        #endregion
    }
}