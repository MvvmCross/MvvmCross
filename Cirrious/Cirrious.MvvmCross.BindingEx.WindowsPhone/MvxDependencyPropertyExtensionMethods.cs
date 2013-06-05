// MvxDependencyPropertyExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using System.Windows;

namespace Cirrious.MvvmCross.BindingEx.WindowsPhone
{
    public static class MvxDependencyPropertyExtensionMethods
    {
        public static DependencyProperty FindDependencyProperty(this Type type, string name)
        {
            var propertyInfo = FindDependencyPropertyInfo(type, name);
            if (propertyInfo == null)
            {
                return null;
            }

            return propertyInfo.GetValue(null) as DependencyProperty;
        }

        public static FieldInfo FindDependencyPropertyInfo(this Type type, string dependencyPropertyName)
        {
            if (!dependencyPropertyName.EndsWith("Property"))
                dependencyPropertyName += "Property";

            var fieldInfo = type.GetField(dependencyPropertyName, BindingFlags.Static | BindingFlags.Public);
            return fieldInfo;
        }
    }
}