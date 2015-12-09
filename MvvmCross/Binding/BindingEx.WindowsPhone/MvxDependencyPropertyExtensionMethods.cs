// MvxDependencyPropertyExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

#if WINDOWS_PHONE || WINDOWS_WPF

using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;

#endif
#if NETFX_CORE

using System.Reflection;
using Windows.UI.Xaml;

#endif

// ReSharper disable CheckNamespace
namespace Cirrious.MvvmCross.BindingEx.WindowsShared
// ReSharper restore CheckNamespace
{
    public static class MvxDependencyPropertyExtensionMethods
    {
#if WINDOWS_PHONE || WINDOWS_WPF

        public static TypeConverter TypeConverter(this Type type)
        {
            var typeConverter =
                type.GetCustomAttributes(typeof(TypeConverterAttribute), true).FirstOrDefault() as
                TypeConverterAttribute;
            if (typeConverter == null)
                return null;

            var converterType = Type.GetType(typeConverter.ConverterTypeName);
            if (converterType == null)
                return null;
            var converter = Activator.CreateInstance(converterType) as TypeConverter;

            return converter;
        }

        public static PropertyInfo FindActualProperty(this Type type, string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var property = type.GetProperty(name, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
            return property;
        }

        public static FieldInfo FindDependencyPropertyInfo(this Type type, string dependencyPropertyName)
        {
            if (string.IsNullOrEmpty(dependencyPropertyName))
                return null;

            if (!EnsureIsDependencyPropertyName(ref dependencyPropertyName))
                return null;

            var candidateType = type;
            while (candidateType != null)
            {
                var fieldInfo = candidateType.GetField(dependencyPropertyName, BindingFlags.Static | BindingFlags.Public);
                if (fieldInfo != null)
                    return fieldInfo;

                candidateType = candidateType.BaseType;
            }

            return null;
        }

#endif

#if NETFX_CORE

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

#endif

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