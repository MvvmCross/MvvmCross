// MvxBindingContextOwnerExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public static partial class MvxBindingContextOwnerExtensions
    {
        public static void CreateBindingContext(this IMvxBindingContextOwner view)
        {
            view.BindingContext = new MvxBindingContext();
        }

        public static void CreateBindingContext(this IMvxBindingContextOwner view, string bindingText)
        {
            view.BindingContext = new MvxBindingContext(null, new Dictionary<object, string> {{view, bindingText}});
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

        public static void AddBinding(this IMvxBindingContextOwner view, IMvxUpdateableBinding binding)
        {
            view.BindingContext.RegisterBinding(binding);
        }

        public static void AddBindings(this IMvxBindingContextOwner view, IEnumerable<IMvxUpdateableBinding> bindings)
        {
            if (bindings == null)
                return;

            foreach (var binding in bindings)
                view.AddBinding(binding);
        }

        public static void AddBindings(this IMvxBindingContextOwner view, object target, string bindingText)
        {
            var bindings = Binder.Bind(view.BindingContext.DataContext, target, bindingText);
            view.AddBindings(bindings);
        }

        public static void AddBinding(this IMvxBindingContextOwner view, object target,
                                      MvxBindingDescription bindingDescription)
        {
            var descriptions = new[] {bindingDescription};
            view.AddBindings(target, descriptions);
        }

        public static void AddBindings(this IMvxBindingContextOwner view, object target,
                                       IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            var bindings = Binder.Bind(view.BindingContext.DataContext, target, bindingDescriptions);
            view.AddBindings(bindings);
        }

        public static void AddBindings(this IMvxBindingContextOwner view,
                                       IDictionary<object, string> bindingMap)
        {
            if (bindingMap == null)
                return;

            foreach (var kvp in bindingMap)
            {
                view.AddBindings(kvp.Key, kvp.Value);
            }
        }

        public static void AddBindings(this IMvxBindingContextOwner view,
                                       IDictionary<object, IEnumerable<MvxBindingDescription>> bindingMap)
        {
            if (bindingMap == null)
                return;

            foreach (var kvp in bindingMap)
            {
                view.AddBindings(kvp.Key, kvp.Value);
            }
        }

        private static bool TryGetPropertyValue(this object host, string targetPropertyName, out object value)
        {
            if (host == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Unable to bind to null host object - targetProperty {0}",
                                      targetPropertyName);
                value = null;
                return false;
            }

            var propertyInfo = host.GetType().GetProperty(targetPropertyName);
            if (propertyInfo == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Unable to find targetProperty for binding - targetProperty {0}",
                                      targetPropertyName);
                value = null;
                return false;
            }
            value = propertyInfo.GetValue(host, null);
            if (value == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "targetProperty for binding is null - targetProperty {0}",
                                      targetPropertyName);
                return false;
            }

            return true;
        }
    }
}