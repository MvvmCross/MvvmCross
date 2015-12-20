using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrossUI.Core
{
    using MvvmCross.Platform;

    internal static class ReflectionExtensions
    {
        internal static IEnumerable<Type> GetTypes(this Assembly assembly)
        {
            return assembly.DefinedTypes.Select(t => t.AsType());
        }

        internal static EventInfo GetEvent(this Type type, string name)
        {
            return type.GetRuntimeEvent(name);
        }

        internal static IEnumerable<Type> GetInterfaces(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces;
        }

        internal static bool IsAssignableFrom(this Type type, Type otherType)
        {
            return type.GetTypeInfo().IsAssignableFrom(otherType.GetTypeInfo());
        }

        internal static Attribute[] GetCustomAttributes(this Type type, Type attributeType, bool inherit)
        {
            return type.GetTypeInfo().GetCustomAttributes(attributeType, inherit).ToArray();
        }

        internal static IEnumerable<ConstructorInfo> GetConstructors(this Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors;
        }

        internal static bool IsInstanceOfType(this Type type, object obj)
        {
            return type.IsAssignableFrom(obj.GetType());
        }

        internal static MethodInfo GetAddMethod(this EventInfo eventInfo, bool nonPublic = false)
        {
            if (!nonPublic && !eventInfo.AddMethod.IsPublic)
            {
                return null;
            }

            return eventInfo.AddMethod;
        }

        internal static MethodInfo GetRemoveMethod(this EventInfo eventInfo, bool noninternal = false)
        {
            if (!noninternal && !eventInfo.RemoveMethod.IsPublic)
            {
                return null;
            }

            return eventInfo.RemoveMethod;
        }

        internal static MethodInfo GetGetMethod(this PropertyInfo property, bool noninternal = false)
        {
            if (!noninternal && !property.GetMethod.IsPublic)
            {
                return null;
            }

            return property.GetMethod;
        }

        internal static MethodInfo GetSetMethod(this PropertyInfo property, bool noninternal = false)
        {
            if (!noninternal && !property.SetMethod.IsPublic)
            {
                return null;
            }

            return property.SetMethod;
        }

        internal static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            return GetProperties(type, BindingFlags.FlattenHierarchy | BindingFlags.Public);
        }

        internal static IEnumerable<PropertyInfo> GetProperties(this Type type, BindingFlags flags)
        {
            var properties = type.GetTypeInfo().DeclaredProperties;
            if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
            {
                properties = type.GetRuntimeProperties();
            }

            return from property in properties
                   let getMethod = property.GetMethod
                   where getMethod != null
                   where (flags & BindingFlags.Public) != BindingFlags.Public || getMethod.IsPublic
                   where (flags & BindingFlags.Instance) != BindingFlags.Instance || !getMethod.IsStatic
                   where (flags & BindingFlags.Static) != BindingFlags.Static || getMethod.IsStatic
                   select property;
        }

        internal static PropertyInfo GetProperty(this Type type, string name, BindingFlags flags)
        {
            return GetProperties(type, flags).FirstOrDefault(p => p.Name == name);
        }

        internal static PropertyInfo GetProperty(this Type type, string name)
        {
            return GetProperties(type, BindingFlags.Public | BindingFlags.FlattenHierarchy).FirstOrDefault(p => p.Name == name);
        }

        internal static IEnumerable<MethodInfo> GetMethods(this Type type)
        {
            return GetMethods(type, BindingFlags.FlattenHierarchy | BindingFlags.Public);
        }

        internal static IEnumerable<MethodInfo> GetMethods(this Type type, BindingFlags flags)
        {
            var properties = type.GetTypeInfo().DeclaredMethods;
            if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
            {
                properties = type.GetRuntimeMethods();
            }

            return properties
                .Where(m => (flags & BindingFlags.Public) != BindingFlags.Public || m.IsPublic)
                .Where(m => (flags & BindingFlags.Instance) != BindingFlags.Instance || !m.IsStatic)
                .Where(m => (flags & BindingFlags.Static) != BindingFlags.Static || m.IsStatic);
        }

        internal static MethodInfo GetMethod(this Type type, string name, BindingFlags flags)
        {
            return GetMethods(type, flags).FirstOrDefault(m => m.Name == name);
        }

        internal static MethodInfo GetMethod(this Type type, string name)
        {
            return GetMethods(type, BindingFlags.Public | BindingFlags.FlattenHierarchy)
                   .FirstOrDefault(m => m.Name == name);
        }

        internal static IEnumerable<ConstructorInfo> GetConstructors(this Type type, BindingFlags flags)
        {
            return type.GetConstructors()
                .Where(m => (flags & BindingFlags.Public) != BindingFlags.Public || m.IsPublic)
                .Where(m => (flags & BindingFlags.Instance) != BindingFlags.Instance || !m.IsStatic)
                .Where(m => (flags & BindingFlags.Static) != BindingFlags.Static || m.IsStatic);
        }

        internal static IEnumerable<FieldInfo> GetFields(this Type type)
        {
            return GetFields(type, BindingFlags.Public | BindingFlags.FlattenHierarchy);
        }

        internal static IEnumerable<FieldInfo> GetFields(this Type type, BindingFlags flags)
        {
            var properties = type.GetTypeInfo().DeclaredFields;
            if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
            {
                properties = type.GetRuntimeFields();
            }

            return properties
                .Where(m => (flags & BindingFlags.Public) != BindingFlags.Public || m.IsPublic)
                .Where(m => (flags & BindingFlags.Instance) != BindingFlags.Instance || !m.IsStatic)
                .Where(m => (flags & BindingFlags.Static) != BindingFlags.Static || m.IsStatic);
        }

        internal static FieldInfo GetField(this Type type, string name, BindingFlags flags)
        {
            return GetFields(type, flags).FirstOrDefault(p => p.Name == name);
        }

        internal static FieldInfo GetField(this Type type, string name)
        {
            return GetFields(type, BindingFlags.Public | BindingFlags.FlattenHierarchy).FirstOrDefault(p => p.Name == name);
        }

        internal static Type[] GetGenericArguments(this Type type)
        {
            return type.GenericTypeArguments;
        }
    }
}