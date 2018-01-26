using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MvvmCross
{
    public static partial class ReflectionExtensions
    {
        internal static IEnumerable<ConstructorInfo> GetDeclaredConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
        }

        internal static PropertyInfo GetDeclaredProperty(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
        }

        internal static PropertyInfo GetFlattenedProperty(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        }

        internal static IEnumerable<PropertyInfo> GetFlattenedProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        }

        internal static IEnumerable<MethodInfo> GetFlattenedMethods(this Type type)
        {
            return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        }

        internal static FieldInfo GetStaticField(this Type type, string fieldName)
        {
            return type.GetField(fieldName, BindingFlags.Static | BindingFlags.Public);
        }

        internal static Type GetBaseType(this Type type)
            => type.BaseType;
    }
}
