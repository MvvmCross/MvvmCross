// MvxDependencyPropertyExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Cirrious.MvvmCross.BindingEx.WindowsPhone
{
    public static class MvxDependencyPropertyExtensionMethods
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
            if (converter == null)
                return null;

            return converter;
        }

        public static PropertyInfo FindActualProperty(this Type type, string name)
        {
            var property = type.GetProperty(name, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
            return property;
        }

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
    }
}