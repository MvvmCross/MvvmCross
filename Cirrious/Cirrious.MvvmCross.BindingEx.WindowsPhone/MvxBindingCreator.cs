// MvxBindingCreator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.WindowsPhone.Converters;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Binders;

namespace Cirrious.MvvmCross.BindingEx.WindowsPhone
{
    public class MvxBindingCreator : IMvxBindingCreator
    {
        public void CreateBindings(object sender, DependencyPropertyChangedEventArgs args,
                                          Func<string, IEnumerable<MvxBindingDescription>> parseBindingDescriptions)
        {
            var attachedObject = sender as FrameworkElement;
            if (attachedObject == null)
            {
                Mvx.Warning("Null attached FrameworkElement seen in Bi.nd binding");
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
                var propertyInfo = FindProperty(actualType, dependencyPropertyName);
                if (propertyInfo == null)
                {
                    continue;
                }

                var dependencyProperty = propertyInfo.GetValue(null);
                if (dependencyProperty == null)
                {
                    Mvx.Warning("DependencyProperty not returned {0}", dependencyPropertyName);
                    continue;
                }

                var property = actualType.GetProperty(bindingDescription.TargetName);
                if (property == null)
                {
                    Mvx.Warning("Property not returned {0} - may cause issues", bindingDescription.TargetName);
                }

                var newBinding = new System.Windows.Data.Binding()
                    {
                        Path = new PropertyPath(bindingDescription.SourcePropertyPath),
                        Mode = ConvertMode(bindingDescription.Mode, property.PropertyType),
                        Converter = GetConverter(bindingDescription.Converter),
                        ConverterParameter = bindingDescription.ConverterParameter,
                        FallbackValue = bindingDescription.FallbackValue
                    };

                BindingOperations.SetBinding(attachedObject, (DependencyProperty) dependencyProperty, newBinding);
            }
        }

        private static FieldInfo FindProperty(Type type, string dependencyPropertyName)
        {
            var fieldInfo = type.GetField(dependencyPropertyName, BindingFlags.Static | BindingFlags.Public);
            return fieldInfo;
        }

        private static IValueConverter GetConverter(IMvxValueConverter converter)
        {
            if (converter == null)
                return null;

            // TODO - consider caching this wrapper - it is a tiny bit wasteful creating a wrapper for each binding
            return new MvxNativeValueConverter(converter);
        }

        private static BindingMode ConvertMode(MvxBindingMode mode, Type propertyType)
        {
            switch (mode)
            {
                case MvxBindingMode.Default:
                    // if we return TwoWay for ImageSource then we end up in 
                    // problems with WP7 not doing the auto-conversion
                    // see some of my angst in http://stackoverflow.com/questions/16752242/how-does-xaml-create-the-string-to-bitmapimage-value-conversion-when-binding-to/16753488#16753488
                    // Note: if we discover other issues here, then we should make a more flexible solution
                    if (propertyType == typeof(ImageSource))
                        return BindingMode.OneWay;

                    return BindingMode.TwoWay;

                case MvxBindingMode.TwoWay:
                    return BindingMode.TwoWay;

                case MvxBindingMode.OneWay:
                    return BindingMode.OneWay;

                case MvxBindingMode.OneTime:
                    return BindingMode.OneTime;

                case MvxBindingMode.OneWayToSource:
                    Mvx.Warning("WinPhone doesn't support OneWayToSource");
                    return BindingMode.TwoWay;

                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }
    }
}