// MvxMvvmCrossBindingCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings;

#if WINDOWS_COMMON
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MvvmCross.BindingEx.WindowsCommon.MvxBinding
#endif

#if WINDOWS_WPF
using System.Windows;
using System.Windows.Data;

namespace MvvmCross.BindingEx.Wpf.MvxBinding
#endif
{


    public class MvxMvvmCrossBindingCreator : MvxBindingCreator
    {
        protected override void ApplyBindings(FrameworkElement attachedObject,
                                              IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            var binder = MvxBindingSingletonCache.Instance.Binder;
            var bindingDescriptionList = bindingDescriptions.ToList();
            var bindings = binder.Bind(attachedObject.DataContext, attachedObject, bindingDescriptionList);
            RegisterBindingsForUpdates(attachedObject, bindings);
        }

        private void RegisterBindingsForUpdates(FrameworkElement attachedObject,
                                                IEnumerable<IMvxUpdateableBinding> bindings)
        {
            if (bindings == null)
                return;

            var bindingsList = GetOrCreateBindingsList(attachedObject);
            foreach (var binding in bindings)
            {
                bindingsList.Add(binding);
            }
        }

        private IList<IMvxUpdateableBinding> GetOrCreateBindingsList(FrameworkElement attachedObject)
        {
            var existing = attachedObject.GetValue(BindingsListProperty) as IList<IMvxUpdateableBinding>;
            if (existing != null)
                return existing;

            // attach the list
            var newList = new List<IMvxUpdateableBinding>();
            attachedObject.SetValue(BindingsListProperty, newList);

            // create a binding watcher for the list
#if WINDOWS_WPF
            var binding = new System.Windows.Data.Binding();
#endif
#if WINDOWS_COMMON
            var binding = new Windows.UI.Xaml.Data.Binding();
#endif
            bool attached = false;
            Action attachAction = () =>
            {
                if (attached)
                    return;
                BindingOperations.SetBinding(attachedObject, DataContextWatcherProperty, binding);
                attached = true;
            };

            Action detachAction = () =>
            {
                if (!attached)
                    return;
#if WINDOWS_COMMON
                attachedObject.ClearValue(DataContextWatcherProperty);
#else
                BindingOperations.ClearBinding(attachedObject, DataContextWatcherProperty);
#endif
                attached = false;
            };
            attachAction();
            attachedObject.Loaded += (o, args) =>
            {
                attachAction();
            };
            attachedObject.Unloaded += (o, args) =>
            {
                detachAction();
            };

            return newList;
        }

        public static readonly DependencyProperty DataContextWatcherProperty = DependencyProperty.Register(
            "DataContextWatcher",
            typeof(object),
            typeof(FrameworkElement),
            new PropertyMetadata(null, DataContext_Changed));

        public static object GetDataContextWatcher(DependencyObject d)
        {
            return d.GetValue(DataContextWatcherProperty);
        }

        public static void SetDataContextWatcher(DependencyObject d, string value)
        {
            d.SetValue(DataContextWatcherProperty, value);
        }

        public static readonly DependencyProperty BindingsListProperty = DependencyProperty.Register(
            "BindingsList",
            typeof(IList<IMvxUpdateableBinding>),
            typeof(FrameworkElement),
            new PropertyMetadata(null));

        public static IList<IMvxUpdateableBinding> GetBindingsList(DependencyObject d)
        {
            return d.GetValue(BindingsListProperty) as IList<IMvxUpdateableBinding>;
        }

        public static void SetBindingsList(DependencyObject d, string value)
        {
            d.SetValue(BindingsListProperty, value);
        }

        private static void DataContext_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;

            var bindings = frameworkElement?.GetValue(BindingsListProperty) as IList<IMvxUpdateableBinding>;
            if (bindings == null)
                return;

            foreach (var binding in bindings)
            {
                binding.DataContext = e.NewValue;
            }
        }
    }
}