﻿// MvxWindowsBindingCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#if WINDOWS_COMMON
using MvvmCross.Platform.WindowsCommon.Converters;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MvvmCross.BindingEx.WindowsCommon.WindowsBinding
#endif

#if WINDOWS_WPF
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MvvmCross.Binding.Bindings;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Wpf.Converters;

namespace MvvmCross.Binding.Wpf.WindowsBinding
#endif
{
    public class MvxWindowsBindingCreator : MvxBindingCreator
    {
        protected virtual void ApplyBinding(MvxBindingDescription bindingDescription, Type actualType,
                                            FrameworkElement attachedObject)
        {
            DependencyProperty dependencyProperty = actualType.FindDependencyProperty(bindingDescription.TargetName);
            if (dependencyProperty == null)
            {
                MvxLog.Instance.Warn("Dependency property not found for {0}", bindingDescription.TargetName);
                return;
            }

            var property = actualType.FindActualProperty(bindingDescription.TargetName);
            if (property == null)
            {
                MvxLog.Instance.Warn("Property not returned {0} - may cause issues", bindingDescription.TargetName);
            }

            var sourceStep = bindingDescription.Source as MvxPathSourceStepDescription;
            if (sourceStep == null)
            {
                MvxLog.Instance.Warn("Binding description for {0} is not a simple path - Windows Binding cannot cope with this", bindingDescription.TargetName);
                return;
            }

#if WINDOWS_WPF
            var newBinding = new System.Windows.Data.Binding
#endif
#if WINDOWS_COMMON
            var newBinding = new Windows.UI.Xaml.Data.Binding
#endif
            {
                Path = new PropertyPath(sourceStep.SourcePropertyPath),
                Mode = ConvertMode(bindingDescription.Mode, property?.PropertyType ?? typeof(object)),
                Converter = GetConverter(sourceStep.Converter),
                ConverterParameter = sourceStep.ConverterParameter,
#if WINDOWS_COMMON
                FallbackValue = sourceStep.FallbackValue
#endif
            };

            BindingOperations.SetBinding(attachedObject, dependencyProperty, newBinding);
        }

        protected override void ApplyBindings(FrameworkElement attachedObject,
                                              IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            var actualType = attachedObject.GetType();
            foreach (var bindingDescription in bindingDescriptions)
            {
                ApplyBinding(bindingDescription, actualType, attachedObject);
            }
        }

        protected static IValueConverter GetConverter(IMvxValueConverter converter)
        {
            if (converter == null)
                return null;

            // TODO - consider caching this wrapper - it is a tiny bit wasteful creating a wrapper for each binding
            return new MvxNativeValueConverter(converter);
        }

        protected static BindingMode ConvertMode(MvxBindingMode mode, Type propertyType)
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
                    MvxLog.Instance.Warn("WinPhone doesn't support OneWayToSource");
                    return BindingMode.TwoWay;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }
        }
    }
}