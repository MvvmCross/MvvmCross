// MvxDependencyPropertyExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Windows.UI.Xaml;

namespace MvvmCross.Binding.Uwp
{
    public static class MvxDependencyPropertyExtensionMethods
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