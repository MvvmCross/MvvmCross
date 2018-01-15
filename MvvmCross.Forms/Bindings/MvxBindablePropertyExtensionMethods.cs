// MvxBindablePropertyExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using System.Reflection;
using MvvmCross.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Bindings
{
    public static class MvxBindablePropertyExtensionMethods
    {
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

        public static FieldInfo FindBindablePropertyInfo(this Type type, string dependencyPropertyName)
        {
            if (string.IsNullOrEmpty(dependencyPropertyName))
                return null;

            if (!EnsureIsBindablePropertyName(ref dependencyPropertyName))
                return null;

            var candidateType = type;
            while (candidateType != null)
            {
                var fieldInfo = candidateType.GetField(dependencyPropertyName, BindingFlags.Static | BindingFlags.Public);
                if (fieldInfo != null)
                    return fieldInfo;

                candidateType = candidateType.GetTypeInfo().BaseType;
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