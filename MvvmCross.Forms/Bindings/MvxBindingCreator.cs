// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Base.Logging;
using MvvmCross.Binding.Bindings;
using MvvmCross.Forms.Base;
using Xamarin.Forms;

namespace MvvmCross.Forms.Bindings
{
    public abstract class MvxBindingCreator : IMvxBindingCreator
    {
        public void CreateBindings(object sender,
                                   object oldValue,
                                   object newValue,
                                   Func<string, IEnumerable<MvxBindingDescription>> parseBindingDescriptions)
        {
            var attachedObject = sender as BindableObject;

            if (attachedObject == null)
            {
                MvxFormsLog.Instance.Warn("Null attached Element seen in Bi.nd binding");
                return;
            }

            var text = newValue as string;
            if (string.IsNullOrEmpty(text))
                return;

            var bindingDescriptions = parseBindingDescriptions(text);
            if (bindingDescriptions == null)
                return;

            ApplyBindings(attachedObject, bindingDescriptions);
        }

        protected abstract void ApplyBindings(BindableObject attachedObject,
                                              IEnumerable<MvxBindingDescription> bindingDescriptions);
    }
}
