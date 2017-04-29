// MvxBindingCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using MvvmCross.Binding.Bindings;
using MvvmCross.Platform;
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
            var attachedObject = sender as Element;

            if (attachedObject == null)
            {
                Mvx.Warning("Null attached Element seen in Bi.nd binding");
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

        protected abstract void ApplyBindings(Element attachedObject,
                                              IEnumerable<MvxBindingDescription> bindingDescriptions);
    }
}