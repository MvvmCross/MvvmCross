// MvxAndroidDialogViewControllerBinderExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Views;

namespace Cirrious.MvvmCross.Dialog.Droid
{
    public static class MvxAndroidDialogViewControllerBinderExtensions
    {
        public static T Bind<T>(this T element, IMvxAndroidView droidView, string descriptionText)
        {
            droidView.AddBindings(element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxAndroidView droidView,
                                IEnumerable<MvxBindingDescription> descriptions)
        {
            droidView.AddBindings(element, descriptions);
            return element;
        }

        public static T Bind<T>(this T element, IMvxAndroidView droidView, object source,
                                string descriptionText)
        {
            droidView.AddBindings(source, element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxAndroidView droidView, object source,
                                IEnumerable<MvxBindingDescription> descriptions)
        {
            droidView.AddBindings(source, element, descriptions);
            return element;
        }

        public static void AddBindings<T>(this IMvxAndroidView droidView, T element, object source,
                                          string bindingText)
        {
            var binder = Mvx.Resolve<IMvxBinder>();
            var bindings = binder.Bind(source, element, bindingText);
            foreach (var binding in bindings)
            {
                droidView.AddBinding(binding);
            }
        }

        public static void AddBindings<T>(this IMvxAndroidView droidView, T element, object source,
                                          IEnumerable<MvxBindingDescription> descriptions)
        {
            var binder = Mvx.Resolve<IMvxBinder>();
            var bindings = binder.Bind(source, element, descriptions);
            foreach (var binding in bindings)
            {
                droidView.AddBinding(binding);
            }
        }
    }
}