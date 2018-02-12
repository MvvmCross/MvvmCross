﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace MvvmCross.Forms.Bindings
{
    public static class MvxBindablePropertyExtensions
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
