#region Copyright
// <copyright file="MvxBindingTouchExtensions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Touch.ExtensionMethods
{
    public static class MvxBindingTouchExtensions
    {
        public static void ClearBindings(this IMvxBindingTouchView view)
        {
            view.Bindings.ForEach(x => x.Dispose());
            view.Bindings.Clear();
        }

        public static void AddBinding(this IMvxBindingTouchView view, IMvxUpdateableBinding binding)
        {
            view.Bindings.Add(binding);
        }

        public static void AddBindings(this IMvxBindingTouchView view, IEnumerable<IMvxUpdateableBinding> bindings)
        {
            if (bindings == null)
                return;

            view.Bindings.AddRange(bindings);
        }

        public static void AddBindings(this IMvxBindingTouchView view, object target, string bindingText)
        {
            view.AddBindings(view.DefaultBindingSource, target, bindingText);
        }

        public static void AddBindings(this IMvxBindingTouchView view, object source, object target, string bindingText)
        {
            var binder = view.GetService<IMvxBinder>();
            view.AddBindings(binder.Bind(source, target, bindingText));
        }

        public static void AddBindings(this IMvxBindingTouchView view, object target, IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            view.AddBindings(view.DefaultBindingSource, target, bindingDescriptions);
        }

        public static void AddBindings(this IMvxBindingTouchView view, object source, object target, IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            var binder = view.GetService<IMvxBinder>();
            view.AddBindings(binder.Bind(source, target, bindingDescriptions));
        }

        public static void AddBindings(this IMvxBindingTouchView view, string targetPropertyName, IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            view.AddBindings(view.DefaultBindingSource, targetPropertyName, bindingDescriptions);
        }

        public static void AddBindings(this IMvxBindingTouchView view, object source, string targetPropertyName, IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            object target;
            if (!view.TryGetPropertyValue(targetPropertyName, out target))
                return;

            view.AddBindings(source, target, bindingDescriptions);
        }

        public static void AddBindings(this IMvxBindingTouchView view, object source, string targetPropertyName, string bindingText)
        {
            object target;
            if (!view.TryGetPropertyValue(targetPropertyName, out target))
                return;

            view.AddBindings(source, target, bindingText);
        }

        public static void AddBindings(this IMvxBindingTouchView view, string targetPropertyName, string bindingText)
        {
            view.AddBindings(view.DefaultBindingSource, targetPropertyName, bindingText);
        }

        public static void AddBindings(this IMvxBindingTouchView view, IDictionary<string, string> bindingMap)
        {
            view.AddBindings(view.DefaultBindingSource, bindingMap);
        }

        public static void AddBindings(this IMvxBindingTouchView view, object source, IDictionary<string, string> bindingMap)
        {
            foreach (var kvp in bindingMap)
            {
                view.AddBindings(source, kvp.Key, kvp.Value);
            }
        }

        public static void AddBindings(this IMvxBindingTouchView view, object bindingObject)
        {
            view.AddBindings(view.DefaultBindingSource, bindingObject);
        }

        public static void AddBindings(this IMvxBindingTouchView view, object source, object bindingObject)
        {
            var bindingMap = bindingObject.ToSimplePropertyDictionary();
            view.AddBindings(source, bindingMap);
        }

        public static void AddBindings(this IMvxBindingTouchView view, IDictionary<object, string> bindingMap)
        {
            view.AddBindings(view.DefaultBindingSource, bindingMap);
        }

        public static void AddBindings(this IMvxBindingTouchView view, object source, IDictionary<object, string> bindingMap)
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

        public static void AddBindings(this IMvxBindingTouchView view, IDictionary<object, IEnumerable<MvxBindingDescription>> bindingMap)
        {
            view.AddBindings(view.DefaultBindingSource, bindingMap);
        }

        public static void AddBindings(this IMvxBindingTouchView view, object source, IDictionary<object, IEnumerable<MvxBindingDescription>> bindingMap)
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