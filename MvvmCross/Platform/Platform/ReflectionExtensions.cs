namespace MvvmCross.Platform
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetTypes(this Assembly assembly)
        {
            foreach (var type in assembly.DefinedTypes)
                yield return type.AsType();
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
            var ctors = type.GetTypeInfo().DeclaredConstructors;

            foreach(var ctor in ctors)
                if (ctor.IsPublic)
                    yield return ctor;
        }

        public static bool IsInstanceOfType(this Type type, object obj)
        {
            return type.IsAssignableFrom(obj.GetType()) || obj.IsMarshalByRefObject();
        }

        private static bool IsMarshalByRefObject(this object obj)
        {
            return obj != null && obj.GetType().FullName == "System.MarshalByRefObject";
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
            IEnumerable<PropertyInfo> properties;
            if ((flags & BindingFlags.FlattenHierarchy) == 0)
            {
                properties = type.GetTypeInfo().DeclaredProperties;
            }
            else
            {
                properties = type.GetRuntimeProperties();
            }

            foreach (var property in properties)
            {
                var getMethod = property.GetMethod;
                var setMethod = property.SetMethod;
                if (getMethod == null && setMethod == null) continue;

                var publicTest = (flags & BindingFlags.Public) != BindingFlags.Public || 
                    getMethod.NullSafeIsPublic() || setMethod.NullSafeIsPublic();

                var instanceTest = (flags & BindingFlags.Instance) != BindingFlags.Instance ||
                    !getMethod.NullSafeIsStatic() || !setMethod.NullSafeIsStatic();

                var staticTest = (flags & BindingFlags.Static) != BindingFlags.Static || 
                    getMethod.NullSafeIsStatic() || setMethod.NullSafeIsStatic();

                if (publicTest && instanceTest && staticTest)
                {
                    yield return property;
                }
            }
        }

        public static PropertyInfo GetProperty(this Type type, string name, BindingFlags flags)
        {
            var properties = GetProperties(type, flags);

            foreach(var property in properties)
                if (property.Name == name)
                    return property;

            return default(PropertyInfo);
        }

        public static PropertyInfo GetProperty(this Type type, string name)
        {
            var properties = GetProperties(type, BindingFlags.Public | BindingFlags.FlattenHierarchy);

            foreach(var property in properties)
                if (property.Name == name)
                    return property;

            return default(PropertyInfo);
        }

        public static IEnumerable<MethodInfo> GetMethods(this Type type)
        {
            return GetMethods(type, BindingFlags.FlattenHierarchy | BindingFlags.Public);
        }

        public static IEnumerable<MethodInfo> GetMethods(this Type type, BindingFlags flags)
        {
            var methods = type.GetTypeInfo().DeclaredMethods;
            if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
            {
                methods = type.GetRuntimeMethods();
            }

            foreach (var method in methods)
            {
                var publicTest = (flags & BindingFlags.Public) != BindingFlags.Public || method.IsPublic;
                var instanceTest = (flags & BindingFlags.Instance) != BindingFlags.Instance || !method.IsStatic;
                var staticTest = (flags & BindingFlags.Static) != BindingFlags.Static || method.IsStatic;

                if (publicTest && instanceTest && staticTest)
                    yield return method;
            }
        }

        public static MethodInfo GetMethod(this Type type, string name, BindingFlags flags)
        {
            var methods = GetMethods(type, flags);

            foreach(var method in methods)
            {
                if (method.Name == name)
                    return method;
            }

            return default(MethodInfo);
        }

        public static MethodInfo GetMethod(this Type type, string name)
        {
            var methods = GetMethods(type, BindingFlags.Public | BindingFlags.FlattenHierarchy);
            foreach (var method in methods)
            {
                if (method.Name == name)
                    return method;
            }

            return default(MethodInfo);
        }

        public static IEnumerable<ConstructorInfo> GetConstructors(this Type type, BindingFlags flags)
        {
            var ctors = type.GetConstructors();

            foreach (var ctor in ctors)
            {
                var publicTest = (flags & BindingFlags.Public) != BindingFlags.Public || ctor.IsPublic;
                var instanceTest = (flags & BindingFlags.Instance) != BindingFlags.Instance || !ctor.IsStatic;
                var staticTest = (flags & BindingFlags.Static) != BindingFlags.Static || ctor.IsStatic;

                if (publicTest && instanceTest && staticTest)
                    yield return ctor;
            }
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

            foreach (var field in fields)
            {
                var publicTest = (flags & BindingFlags.Public) != BindingFlags.Public || field.IsPublic;
                var instanceTest = (flags & BindingFlags.Instance) != BindingFlags.Instance || !field.IsStatic;
                var staticTest = (flags & BindingFlags.Static) != BindingFlags.Static || field.IsStatic;

                if (publicTest && instanceTest && staticTest)
                    yield return field;
            }
        }

        public static FieldInfo GetField(this Type type, string name, BindingFlags flags)
        {
            var fields = GetFields(type, flags);
            foreach (var field in fields)
            {
                if (field.Name == name)
                    return field;
            }

            return default(FieldInfo);
        }

        public static FieldInfo GetField(this Type type, string name)
        {
            var fields = GetFields(type, BindingFlags.Public | BindingFlags.FlattenHierarchy);
            foreach (var field in fields)
            {
                if (field.Name == name)
                    return field;
            }

            return default(FieldInfo);
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GenericTypeArguments;
        }
    }
}