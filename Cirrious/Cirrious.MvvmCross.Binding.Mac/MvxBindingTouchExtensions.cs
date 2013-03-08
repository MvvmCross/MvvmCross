// MvxBindingTouchExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
using Cirrious.MvvmCross.Binding.Touch.Interfaces.BindingContext;
using Cirrious.MvvmCross.Binding.Interfaces;
using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Touch
{
    public static class MvxBindingTouchExtensions
    {
		/*
        public static void ClearBindings(this IMvxBindingContextOwner view)
        {
			view.BindingContext.ClearAllBindings();
        }
		*/

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
            var binder = Mvx.Resolve<IMvxBinder>();
            view.AddBindings(binder.Bind(view.BindingContext.DataContext, target, bindingText));
        }

        public static void AddBindings(this IMvxBindingContextOwner view, object target,
                                       IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            var binder = Mvx.Resolve<IMvxBinder>();
            view.AddBindings(binder.Bind(view.BindingContext.DataContext, target, bindingDescriptions));
        }

        public static void AddBindings(this IMvxBindingContextOwner view, string targetPropertyName,
                                       IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
			object target;
            if (!view.TryGetPropertyValue(targetPropertyName, out target))
                return;

			view.AddBindings(target, bindingDescriptions);
        }

        public static void AddBindings(this IMvxBindingContextOwner view, string targetPropertyName,
                                       string bindingText)
        {
            object target;
            if (!view.TryGetPropertyValue(targetPropertyName, out target))
                return;

            view.AddBindings(target, bindingText);
        }

        public static void AddBindings(this IMvxBindingContextOwner view,
                                       IDictionary<string, string> bindingMap)
        {
            foreach (var kvp in bindingMap)
            {
                view.AddBindings(kvp.Key, kvp.Value);
            }
        }

#warning What were these two overloads used for?
        /*
		public static void AddBindings(this IMvxBindingContextOwner view, object bindingObject)
        {
            view.AddBindings(view.DataContext, bindingObject);
        }

		public static void AddBindings(this IMvxBindingContextOwner view, object source, object bindingObject)
        {
            var bindingMap = bindingObject.ToSimplePropertyDictionary();
            view.AddBindings(source, bindingMap);
        }
        */

        public static void AddBindings(this IMvxBindingContextOwner view,
                                       IDictionary<object, string> bindingMap)
        {
            foreach (var kvp in bindingMap)
            {
                var candidatePropertyName = kvp.Key as string;
                if (candidatePropertyName == null)
                    view.AddBindings(kvp.Key, kvp.Value);
                else
                    view.AddBindings(candidatePropertyName, kvp.Value);
            }
        }

        public static void AddBindings(this IMvxBindingContextOwner view,
                                       IDictionary<object, IEnumerable<MvxBindingDescription>> bindingMap)
        {
            if (bindingMap == null)
                return;

            foreach (var kvp in bindingMap)
            {
                var candidatePropertyName = kvp.Key as string;
                if (candidatePropertyName == null)
                    view.AddBindings(kvp.Key, kvp.Value);
                else
                    view.AddBindings(candidatePropertyName, kvp.Value);
            }
        }

        private static bool TryGetPropertyValue(this object host, string targetPropertyName, out object value)
        {
            if (host == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Unable to bind to null host object - property {0}",
                                      targetPropertyName);
                value = null;
                return false;
            }

            var propertyInfo = host.GetType().GetProperty(targetPropertyName);
            if (propertyInfo == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Unable to find property for binding - property {0}",
                                      targetPropertyName);
                value = null;
                return false;
            }
            value = propertyInfo.GetValue(host, null);
            if (value == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "property for binding is null - property {0}",
                                      targetPropertyName);
                return false;
            }

            return true;
        }
    }
}