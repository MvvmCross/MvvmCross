// MvxBindingCreator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.WindowsStore.Converters;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Binders;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Cirrious.MvvmCross.BindingEx.WindowsStore
{
    public class MvxBindingCreator : IMvxBindingCreator
    {
        public void CreateBindings(object sender, DependencyPropertyChangedEventArgs args,
                                          Func<string, IEnumerable<MvxBindingDescription>> parseBindingDescriptions)
        {
            var attachedObject = sender as FrameworkElement;
            if (attachedObject == null)
            {
                Mvx.Warning("Null attached FrameworkElement seen in B.ind binding");
                return;
            }

            var text = args.NewValue as string;
            if (String.IsNullOrEmpty(text))
                return;

            var bindingDescriptions = parseBindingDescriptions(text);
            if (bindingDescriptions == null)
                return;

            var actualType = attachedObject.GetType();
            foreach (var bindingDescription in bindingDescriptions)
            {
                var dependencyPropertyName = bindingDescription.TargetName + "Property";
                var propertyInfo = FindProperty(actualType.GetTypeInfo(), dependencyPropertyName);
                if (propertyInfo == null)
                {
                    continue;
                }
                
                var dependencyProperty = propertyInfo.GetMethod.Invoke(null, new object[0]);
                if (dependencyProperty == null)
                {
                    Mvx.Warning("DependencyProperty not returned {0}", dependencyPropertyName);
                    continue;
                }

                var newBinding = new Windows.UI.Xaml.Data.Binding
                {
                    Path = new PropertyPath(bindingDescription.SourcePropertyPath),
                    Mode = ConvertMode(bindingDescription.Mode),
                    Converter = GetConverter(bindingDescription.Converter),
                    ConverterParameter = bindingDescription.ConverterParameter,
                };
                BindingOperations.SetBinding(attachedObject, (DependencyProperty)dependencyProperty, newBinding);
            }
        }

        private static PropertyInfo FindProperty(TypeInfo typeInfo, string dependencyPropertyName)
        {
            var propertyInfo = typeInfo.GetDeclaredProperty(dependencyPropertyName);
            if (propertyInfo != null)
            {
                return propertyInfo;
            }

            if (typeInfo.BaseType == null)
            {
                return null;
            }

            return FindProperty(typeInfo.BaseType.GetTypeInfo(), dependencyPropertyName);
        }

        private static IValueConverter GetConverter(IMvxValueConverter converter)
        {
            if (converter == null)
                return null;

            // TODO - consider caching this wrapper - it is a tiny bit wasteful creating a wrapper for each binding
            return new MvxNativeValueConverter(converter);
        }

        private static BindingMode ConvertMode(MvxBindingMode mode)
        {
            switch (mode)
            {
                case MvxBindingMode.Default:
                    return BindingMode.TwoWay;

                case MvxBindingMode.TwoWay:
                    return BindingMode.TwoWay;

                case MvxBindingMode.OneWay:
                    return BindingMode.OneWay;

                case MvxBindingMode.OneTime:
                    return BindingMode.OneTime;

                case MvxBindingMode.OneWayToSource:
                    Mvx.Warning("WinRT doesn't support OneWayToSource");
                    return BindingMode.TwoWay;

                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }
    }
}