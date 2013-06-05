// MvxWindowsBindingCreator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Binders;

namespace Cirrious.MvvmCross.BindingEx.WindowsPhone.WindowsBinding
{
    public class MvxWindowsBindingCreator : MvxBindingCreator
    {
        protected virtual void ApplyBinding(MvxBindingDescription bindingDescription, Type actualType,
                                            FrameworkElement attachedObject)
        {
            DependencyProperty dependencyProperty = actualType.FindDependencyProperty(bindingDescription.TargetName);
            if (dependencyProperty == null)
            {
                Mvx.Warning("Dependency property not found for {0}", bindingDescription.TargetName);
                return;
            }

            var property = actualType.GetProperty(bindingDescription.TargetName);
            if (property == null)
            {
                Mvx.Warning("Property not returned {0} - may cause issues", bindingDescription.TargetName);
            }

            var newBinding = new System.Windows.Data.Binding
                {
                    Path = new PropertyPath(bindingDescription.SourcePropertyPath),
                    Mode = ConvertMode(bindingDescription.Mode, property == null ? typeof(object) : property.PropertyType),
                    Converter = GetConverter(bindingDescription.Converter),
                    ConverterParameter = bindingDescription.ConverterParameter,
                    FallbackValue = bindingDescription.FallbackValue
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
    }
}