// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Windows.UI.Xaml;

namespace MvvmCross.Platforms.Uap.Binding
{
    public static class MvxDependencyPropertyExtensions
    {
        public static PropertyInfo FindActualProperty(this Type type, string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var property = type.GetRuntimeProperty(name);
            return property;
        }

        private static PropertyInfo FindDependencyPropertyInfo(Type type, string dependencyPropertyName)
        {
            if (string.IsNullOrEmpty(dependencyPropertyName))
                return null;

            if (!EnsureIsDependencyPropertyName(ref dependencyPropertyName))
                return null;

            var typeInfo = type.GetTypeInfo();
            while (typeInfo != null)
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

                typeInfo = typeInfo.BaseType.GetTypeInfo();
            }

            return null;
        }

        public static DependencyProperty FindDependencyProperty(this Type type, string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var propertyInfo = FindDependencyPropertyInfo(type, name);

            return propertyInfo?.GetValue(null) as DependencyProperty;
        }

        private static bool EnsureIsDependencyPropertyName(ref string dependencyPropertyName)
        {
            if (string.IsNullOrEmpty(dependencyPropertyName))
                return false;

            if (!dependencyPropertyName.EndsWith("Property"))
                dependencyPropertyName += "Property";
            return true;
        }
    }
}
