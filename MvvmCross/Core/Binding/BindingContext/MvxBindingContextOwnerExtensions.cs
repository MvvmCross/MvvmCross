// MvxBindingContextOwnerExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.BindingContext
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Binding.Bindings;

    public static partial class MvxBindingContextOwnerExtensions
    {
        public static void CreateBindingContext(this IMvxBindingContextOwner view)
        {
            view.BindingContext = new MvxBindingContext();
        }

        public static void CreateBindingContext(this IMvxBindingContextOwner view, string bindingText)
        {
            view.BindingContext = new MvxBindingContext(null, new Dictionary<object, string> { { view, bindingText } });
        }

        public static void CreateBindingContext(this IMvxBindingContextOwner view,
                                                IEnumerable<MvxBindingDescription> bindings)
        {
            view.BindingContext = new MvxBindingContext(null,
                                                        new Dictionary<object, IEnumerable<MvxBindingDescription>>
                                                            {
                                                                {view, bindings}
                                                            });
        }

        /*
		 * This overload removed at present - it caused confusion on iOS
		 * because it could lead to the bindings being described before
		 * the table cells were fully available
        public static void DelayBind(this IMvxBindingContextOwner view, params IMvxApplicable[] applicables)
        {
            view.BindingContext.DelayBind(() => applicables.Apply());
        }
        */

        public static void DelayBind(this IMvxBindingContextOwner view, Action bindingAction)
        {
            view.BindingContext.DelayBind(bindingAction);
        }

        public static void AddBinding(this IMvxBindingContextOwner view, object target, IMvxUpdateableBinding binding, object clearKey = null)
        {
            if (clearKey == null)
            {
                view.BindingContext.RegisterBinding(target, binding);
            }
            else
            {
                view.BindingContext.RegisterBindingWithClearKey(clearKey, target, binding);
            }
        }

        public static void AddBindings(this IMvxBindingContextOwner view, object target, IEnumerable<IMvxUpdateableBinding> bindings, object clearKey = null)
        {
            if (bindings == null)
                return;

            foreach (var binding in bindings)
                view.AddBinding(target, binding, clearKey);
        }

        public static void AddBindings(this IMvxBindingContextOwner view, object target, string bindingText, object clearKey = null)
        {
            var bindings = Binder.Bind(view.BindingContext.DataContext, target, bindingText);
            view.AddBindings(target, bindings, clearKey);
        }

        public static void AddBinding(this IMvxBindingContextOwner view, object target,
                                      MvxBindingDescription bindingDescription, object clearKey = null)
        {
            var descriptions = new[] { bindingDescription };
            view.AddBindings(target, descriptions, clearKey);
        }

        public static void AddBindings(this IMvxBindingContextOwner view, object target,
                                       IEnumerable<MvxBindingDescription> bindingDescriptions, object clearKey = null)
        {
            var bindings = Binder.Bind(view.BindingContext.DataContext, target, bindingDescriptions);
            view.AddBindings(target, bindings, clearKey);
        }

        public static void AddBindings(this IMvxBindingContextOwner view,
                                       IDictionary<object, string> bindingMap,
                                       object clearKey = null)
        {
            if (bindingMap == null)
                return;

            foreach (var kvp in bindingMap)
            {
                view.AddBindings(kvp.Key, kvp.Value, clearKey);
            }
        }

        public static void AddBindings(this IMvxBindingContextOwner view,
                                       IDictionary<object, IEnumerable<MvxBindingDescription>> bindingMap,
                                       object clearKey = null)
        {
            if (bindingMap == null)
                return;

            foreach (var kvp in bindingMap)
            {
                view.AddBindings(kvp.Key, kvp.Value, clearKey);
            }
        }
    }
}