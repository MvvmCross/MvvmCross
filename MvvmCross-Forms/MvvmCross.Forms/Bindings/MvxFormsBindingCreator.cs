// MvxFormsBindingCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings;
using MvvmCross.Platform.WeakSubscription;
using Xamarin.Forms;

namespace MvvmCross.Forms.Bindings
{
    public class MvxFormsBindingCreator : MvxBindingCreator
    {
        protected override void ApplyBindings(Element attachedObject,
                                              IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            var binder = MvxBindingSingletonCache.Instance.Binder;
            var bindingDescriptionList = bindingDescriptions.ToList();
            var bindings = binder.Bind(attachedObject.BindingContext, attachedObject, bindingDescriptionList);

            RegisterBindingsForUpdates(attachedObject, bindings);
        }

        private void RegisterBindingsForUpdates(Element attachedObject,
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

        private IList<IMvxUpdateableBinding> GetOrCreateBindingsList(Element attachedObject)
        {
            var existing = attachedObject.GetValue(BindingsListProperty) as IList<IMvxUpdateableBinding>;

            if (existing != null)
                return existing;

            // attach the list
            var newList = new List<IMvxUpdateableBinding>();
            attachedObject.SetValue(BindingsListProperty, newList);

            bool attached = false;
            Action attachAction = () =>
            {
                if (attached)
                    return;

                attachedObject.BindingContextChanged += DataContextChanged;
                attached = true;
            };

            Action detachAction = () =>
            {
                if (!attached)
                    return;

                attachedObject.BindingContextChanged -= DataContextChanged;
                attached = false;
            };

            attachAction();
            var subscription = attachedObject.WeakSubscribe((s, a) =>
                                                            {
                                                                if (a.PropertyName == nameof(Element.Parent))
                                                                {
                                                                    if (attachedObject.Parent != null)
                                                                    {
                                                                        attachAction();
                                                                    }
                                                                    else
                                                                    {
                                                                        detachAction();
                                                                    }
                                                                }
                                                            });
            attachedObject.SetValue(DataContextWatcherProperty, subscription);

            return newList;
        }

        private static void DataContextChanged(object obj, EventArgs args)
        {
            BindableObject d = obj as BindableObject;
            IList<IMvxUpdateableBinding> bindings = d?.GetValue(BindingsListProperty) as IList<IMvxUpdateableBinding>;

            if (bindings != null)
            {
                foreach (var binding in bindings)
                {
                    binding.DataContext = d.BindingContext;
                }
            }
        }

        public static readonly BindableProperty BindingsListProperty = BindableProperty.CreateAttached("BindingsList",
                                                                                                       typeof(IList<IMvxUpdateableBinding>),
                                                                                                       typeof(BindableObject),
                                                                                                       null);
        public static readonly BindableProperty DataContextWatcherProperty = BindableProperty.CreateAttached("DataContextWatcher",
                                                                                                       typeof(MvxNotifyPropertyChangedEventSubscription),
                                                                                                       typeof(BindableObject),
                                                                                                       null);

        public static IList<IMvxUpdateableBinding> GetBindingsList(BindableObject d)
        {
            return d.GetValue(BindingsListProperty) as IList<IMvxUpdateableBinding>;
        }

        public static void SetBindingsList(BindableObject d, IList<IMvxUpdateableBinding> value)
        {
            d.SetValue(BindingsListProperty, value);
        }

        public static MvxNotifyPropertyChangedEventSubscription GetDataContextWatcher(BindableObject d)
        {
            return d.GetValue(BindingsListProperty) as MvxNotifyPropertyChangedEventSubscription;
        }

        public static void SetDataContextWatcher(BindableObject d, MvxNotifyPropertyChangedEventSubscription value)
        {
            d.SetValue(BindingsListProperty, value);
        }
    }
}