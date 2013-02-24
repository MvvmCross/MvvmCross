// MvxBindingTouchExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;

namespace Cirrious.MvvmCross.Binding.Touch
{
    public static class MvxBindingTouchExtensions
    {
        public static void ClearBindings(this IMvxBindingOwner view)
        {
            view.Bindings.ForEach(x => x.Dispose());
            view.Bindings.Clear();
        }

        public static void AddBinding(this IMvxBindingOwner view, IMvxUpdateableBinding binding)
        {
            view.Bindings.Add(binding);
        }

        public static void AddBindings(this IMvxBindingOwner view, IEnumerable<IMvxUpdateableBinding> bindings)
        {
            if (bindings == null)
                return;

            view.Bindings.AddRange(bindings);
        }

        public static void AddBindings(this IMvxBindingOwner view, object target, string bindingText)
        {
            view.AddBindings(view.DataContext, target, bindingText);
        }

        public static void AddBindings(this IMvxBindingOwner view, object source, object target, string bindingText)
        {
            var binder = MvxIoCExtensions.Resolve<IMvxBinder>();
            view.AddBindings(binder.Bind(source, target, bindingText));
        }

        public static void AddBindings(this IMvxBindingOwner view, object target,
                                       IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            view.AddBindings(view.DataContext, target, bindingDescriptions);
        }

        public static void AddBindings(this IMvxBindingOwner view, object source, object target,
                                       IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            var binder = MvxIoCExtensions.Resolve<IMvxBinder>();
            view.AddBindings(binder.Bind(source, target, bindingDescriptions));
        }

        public static void AddBindings(this IMvxBindingOwner view, string targetPropertyName,
                                       IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            view.AddBindings(view.DataContext, targetPropertyName, bindingDescriptions);
        }

        public static void AddBindings(this IMvxBindingOwner view, object source, string targetPropertyName,
                                       IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            object target;
            if (!view.TryGetPropertyValue(targetPropertyName, out target))
                return;

            view.AddBindings(source, target, bindingDescriptions);
        }

        public static void AddBindings(this IMvxBindingOwner view, object source, string targetPropertyName,
                                       string bindingText)
        {
            object target;
            if (!view.TryGetPropertyValue(targetPropertyName, out target))
                return;

            view.AddBindings(source, target, bindingText);
        }

        public static void AddBindings(this IMvxBindingOwner view, string targetPropertyName, string bindingText)
        {
            view.AddBindings(view.DataContext, targetPropertyName, bindingText);
        }

        public static void AddBindings(this IMvxBindingOwner view, IDictionary<string, string> bindingMap)
        {
            view.AddBindings(view.DataContext, bindingMap);
        }

        public static void AddBindings(this IMvxBindingOwner view, object source,
                                       IDictionary<string, string> bindingMap)
        {
            foreach (var kvp in bindingMap)
            {
                view.AddBindings(source, kvp.Key, kvp.Value);
            }
        }

#warning What were these two overloads used for?
        /*
		public static void AddBindings(this IMvxBindingOwner view, object bindingObject)
        {
            view.AddBindings(view.DataContext, bindingObject);
        }

		public static void AddBindings(this IMvxBindingOwner view, object source, object bindingObject)
        {
            var bindingMap = bindingObject.ToSimplePropertyDictionary();
            view.AddBindings(source, bindingMap);
        }
        */

        public static void AddBindings(this IMvxBindingOwner view, IDictionary<object, string> bindingMap)
        {
            view.AddBindings(view.DataContext, bindingMap);
        }

        public static void AddBindings(this IMvxBindingOwner view, object source,
                                       IDictionary<object, string> bindingMap)
        {
            foreach (var kvp in bindingMap)
            {
                var candidatePropertyName = kvp.Key as string;
                if (candidatePropertyName == null)
                    view.AddBindings(source, kvp.Key, kvp.Value);
                else
                    view.AddBindings(source, candidatePropertyName, kvp.Value);
            }
        }

        public static void AddBindings(this IMvxBindingOwner view,
                                       IDictionary<object, IEnumerable<MvxBindingDescription>> bindingMap)
        {
            view.AddBindings(view.DataContext, bindingMap);
        }

        public static void AddBindings(this IMvxBindingOwner view, object source,
                                       IDictionary<object, IEnumerable<MvxBindingDescription>> bindingMap)
        {
            if (bindingMap == null)
                return;

            foreach (var kvp in bindingMap)
            {
                var candidatePropertyName = kvp.Key as string;
                if (candidatePropertyName == null)
                    view.AddBindings(source, kvp.Key, kvp.Value);
                else
                    view.AddBindings(source, candidatePropertyName, kvp.Value);
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