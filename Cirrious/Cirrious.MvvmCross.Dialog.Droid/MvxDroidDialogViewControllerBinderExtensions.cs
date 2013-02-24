// MvxDroidDialogViewControllerBinderExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Dialog.Droid.Interfaces;

namespace Cirrious.MvvmCross.Dialog.Droid
{
    public static class MvxDroidDialogViewControllerBinderExtensions
    {
        public static T Bind<T>(this T element, IMvxBindingDialogActivityView droidView, string descriptionText)
        {
            droidView.AddBindings(element, droidView.DefaultBindingSource, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingDialogActivityView droidView,
                                IEnumerable<MvxBindingDescription> descriptions)
        {
            droidView.AddBindings(element, droidView.DefaultBindingSource, descriptions);
            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingDialogActivityView droidView, object source,
                                string descriptionText)
        {
            droidView.AddBindings(source, element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingDialogActivityView droidView, object source,
                                IEnumerable<MvxBindingDescription> descriptions)
        {
            droidView.AddBindings(source, element, descriptions);
            return element;
        }

        public static void AddBindings<T>(this IMvxBindingDialogActivityView droidView, T element, object source,
                                          string bindingText)
        {
            var binder = Mvx.Resolve<IMvxBinder>();
            var bindings = binder.Bind(source, element, bindingText);
            foreach (var binding in bindings)
            {
                droidView.RegisterBinding(binding);
            }
        }

        public static void AddBindings<T>(this IMvxBindingDialogActivityView droidView, T element, object source,
                                          IEnumerable<MvxBindingDescription> descriptions)
        {
            var binder = Mvx.Resolve<IMvxBinder>();
            var bindings = binder.Bind(source, element, descriptions);
            foreach (var binding in bindings)
            {
                droidView.RegisterBinding(binding);
            }
        }
    }
}