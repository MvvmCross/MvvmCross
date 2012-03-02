#region Copyright
// <copyright file="MvxFromTextBinder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

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
        #region IMvxBinder Members

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

        #endregion
    }
}