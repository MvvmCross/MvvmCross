using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetTypes(this Assembly assembly)
        {
            return assembly.DefinedTypes.Select(t => t.AsType());
        }

        public static EventInfo GetEvent(this Type type, string name)
        {
            return type.GetRuntimeEvent(name);
        }

        public static IEnumerable<Type> GetInterfaces(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces;
        }

        public static bool IsAssignableFrom(this Type type, Type otherType)
        {
            return type.GetTypeInfo().IsAssignableFrom(otherType.GetTypeInfo());
        }

        public static Attribute[] GetCustomAttributes(this Type type, Type attributeType, bool inherit)
        {
            return type.GetTypeInfo().GetCustomAttributes(attributeType, inherit).ToArray();
        }

        public static IEnumerable<ConstructorInfo> GetConstructors(this Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors.Where(c => c.IsPublic);
        }

        public static bool IsInstanceOfType(this Type type, object obj)
        {
            return type.IsAssignableFrom(obj.GetType());
        }

        public static MethodInfo GetAddMethod(this EventInfo eventInfo, bool nonPublic = false)
        {
            if (eventInfo.AddMethod == null || (!nonPublic && !eventInfo.AddMethod.IsPublic))
            {
                return null;
            }

            return eventInfo.AddMethod;
        }

        public static MethodInfo GetRemoveMethod(this EventInfo eventInfo, bool nonPublic = false)
        {
            if (eventInfo.RemoveMethod == null || (!nonPublic && !eventInfo.RemoveMethod.IsPublic))
            {
                return null;
            }

            return eventInfo.RemoveMethod;
        }

        public static MethodInfo GetGetMethod(this PropertyInfo property, bool nonPublic = false)
        {
            if (property.GetMethod == null || (!nonPublic && !property.GetMethod.IsPublic))
            {
                return null;
            }

            return property.GetMethod;
        }

        public static MethodInfo GetSetMethod(this PropertyInfo property, bool nonPublic = false)
        {
            if (property.SetMethod == null || (!nonPublic && !property.SetMethod.IsPublic))
            {
                return null;
            }

            return property.SetMethod;
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            return GetProperties(type, BindingFlags.FlattenHierarchy | BindingFlags.Public);
        }

        private static bool NullSafeIsPublic(this MethodInfo info)
        {
            if (info == null) 
                return false;
            return info.IsPublic;
        }

        private static bool NullSafeIsStatic(this MethodInfo info)
        {
            if (info == null)
                return false;
            return info.IsStatic;
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type type, BindingFlags flags)
        {
            var properties = type.GetTypeInfo().DeclaredProperties;
            if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
            {
                properties = type.GetRuntimeProperties();
            }

            return from property in properties
                   let getMethod = property.GetMethod
                   let setMethod = property.SetMethod
                   where (getMethod != null || setMethod != null)
                   where (flags & BindingFlags.Public) != BindingFlags.Public || getMethod.NullSafeIsPublic() || setMethod.NullSafeIsPublic()
                   where (flags & BindingFlags.Instance) != BindingFlags.Instance || !getMethod.NullSafeIsStatic() || !setMethod.NullSafeIsStatic()
                   where (flags & BindingFlags.Static) != BindingFlags.Static || getMethod.NullSafeIsStatic() || setMethod.NullSafeIsStatic()
                   select property;
        }

        public static PropertyInfo GetProperty(this Type type, string name, BindingFlags flags)
        {
            return GetProperties(type, flags).FirstOrDefault(p => p.Name == name);
        }

        public static PropertyInfo GetProperty(this Type type, string name)
        {
            return GetProperties(type, BindingFlags.Public | BindingFlags.FlattenHierarchy).FirstOrDefault(p => p.Name == name);
        }

        public static IEnumerable<MethodInfo> GetMethods(this Type type)
        {
            return GetMethods(type, BindingFlags.FlattenHierarchy | BindingFlags.Public);
        }

        public static IEnumerable<MethodInfo> GetMethods(this Type type, BindingFlags flags)
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

        public static MethodInfo GetMethod(this Type type, string name, BindingFlags flags)
        {
            return GetMethods(type, flags).FirstOrDefault(m => m.Name == name);
        }

        public static MethodInfo GetMethod(this Type type, string name)
        {
            return GetMethods(type, BindingFlags.Public | BindingFlags.FlattenHierarchy)
                   .FirstOrDefault(m => m.Name == name);
        }

        public static IEnumerable<ConstructorInfo> GetConstructors(this Type type, BindingFlags flags)
        {
            return type.GetConstructors()
                .Where(m => (flags & BindingFlags.Public) != BindingFlags.Public || m.IsPublic)
                .Where(m => (flags & BindingFlags.Instance) != BindingFlags.Instance || !m.IsStatic)
                .Where(m => (flags & BindingFlags.Static) != BindingFlags.Static || m.IsStatic);
        }

        public static IEnumerable<FieldInfo> GetFields(this Type type)
        {
            return GetFields(type, BindingFlags.Public | BindingFlags.FlattenHierarchy);
        }

        public static IEnumerable<FieldInfo> GetFields(this Type type, BindingFlags flags)
        {
            var fields = type.GetTypeInfo().DeclaredFields;
            if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
            {
                fields = type.GetRuntimeFields();
            }

            return fields
                .Where(f => (flags & BindingFlags.Public) != BindingFlags.Public || f.IsPublic)
                .Where(f => (flags & BindingFlags.Instance) != BindingFlags.Instance || !f.IsStatic)
                .Where(f => (flags & BindingFlags.Static) != BindingFlags.Static || f.IsStatic);
        }

        public static FieldInfo GetField(this Type type, string name, BindingFlags flags)
        {
            return GetFields(type, flags).FirstOrDefault(p => p.Name == name);
        }

        public static FieldInfo GetField(this Type type, string name)
        {
            return GetFields(type, BindingFlags.Public | BindingFlags.FlattenHierarchy).FirstOrDefault(p => p.Name == name);
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GenericTypeArguments;
        }
    }
}
