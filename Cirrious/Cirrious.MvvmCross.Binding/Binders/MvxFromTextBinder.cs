// MvxFromTextBinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxFromTextBinder
        : IMvxBinder
    {
        #region IMvxBinder Members

        public IEnumerable<IMvxUpdateableBinding> Bind(object source, object target, string bindingText)
        {
            var bindingDescriptions = Mvx.Resolve<IMvxBindingDescriptionParser>().Parse(bindingText);
            if (bindingDescriptions == null)
                return new IMvxUpdateableBinding[0];

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
            var bindingDescription =
                Mvx.Resolve<IMvxBindingDescriptionParser>().ParseSingle(partialBindingDescription);
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