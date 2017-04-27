// MvxBindablePropertyExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenter.Binding
{
    public static class MvxBindablePropertyExtensionMethods
    {
        public static PropertyInfo FindActualProperty(this Type type, string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var property = type.GetRuntimeProperty(name);
            return property;
        }

        private static PropertyInfo FindBindablePropertyInfo(Type type, string dependencyPropertyName)
        {
            if (string.IsNullOrEmpty(dependencyPropertyName))
                return null;

            if (!EnsureIsBindablePropertyName(ref dependencyPropertyName))
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

        public static BindableProperty FindBindableProperty(this Type type, string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var propertyInfo = FindBindablePropertyInfo(type, name);

            return propertyInfo?.GetValue(null) as BindableProperty;
        }

        private static bool EnsureIsBindablePropertyName(ref string bindablePropertyName)
        {
            if (string.IsNullOrEmpty(bindablePropertyName))
                return false;

            if (!bindablePropertyName.EndsWith("Property"))
                bindablePropertyName += "Property";
            return true;
        }
    }
}