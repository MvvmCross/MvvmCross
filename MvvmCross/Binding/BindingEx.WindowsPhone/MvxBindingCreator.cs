// MvxBindingCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#if WINDOWS_PHONE || WINDOWS_WPF
#endif

#if NETFX_CORE

using Windows.UI.Xaml;

#endif


namespace MvvmCross.BindingEx.WindowsPhone

{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using global::MvvmCross.Binding.Bindings;
    using global::MvvmCross.Platform;

    public abstract class MvxBindingCreator : IMvxBindingCreator
    {
        public void CreateBindings(object sender, DependencyPropertyChangedEventArgs args,
                                   Func<string, IEnumerable<MvxBindingDescription>> parseBindingDescriptions)
        {
            var attachedObject = sender as FrameworkElement;
            if (attachedObject == null)
            {
                Mvx.Warning("Null attached FrameworkElement seen in Bi.nd binding");
                return;
            }

            var text = args.NewValue as string;
            if (String.IsNullOrEmpty(text))
                return;

            var bindingDescriptions = parseBindingDescriptions(text);
            if (bindingDescriptions == null)
                return;

            this.ApplyBindings(attachedObject, bindingDescriptions);
        }

        protected abstract void ApplyBindings(FrameworkElement attachedObject,
                                              IEnumerable<MvxBindingDescription> bindingDescriptions);
    }
}