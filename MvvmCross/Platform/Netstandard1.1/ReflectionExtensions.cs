using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MvvmCross
{
    public static partial class ReflectionExtensions
    {
        public static IEnumerable<Type> GetTypes(this Assembly assembly)
        {
            return assembly.ExportedTypes;
        }

        public static bool IsAssignableFrom(this Type typeToBeAssigned, Type typeToBeAssignedTo)
        {
            return typeToBeAssigned.GetTypeInfo().IsAssignableFrom(typeToBeAssignedTo.GetTypeInfo());
        }

        public static IEnumerable<ConstructorInfo> GetDeclaredConstructors(this Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors;
        }

        public static IEnumerable<Type> GetInterfaces(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces;
        }

        public static PropertyInfo GetDeclaredProperty(this Type type, string propertyName)
        {
            return type.GetTypeInfo().GetDeclaredProperty(propertyName);
        }

        public static PropertyInfo GetFlattenedProperty(this Type type, string propertyName)
        {
            return type.GetRuntimeProperty(propertyName);
        }

        public static IEnumerable<PropertyInfo> GetFlattenedProperties(this Type type)
        {
            return type.GetRuntimeProperties();
        }

        public static IEnumerable<MethodInfo> GetFlattenedMethods(this Type type)
        {
            return type.GetRuntimeMethods();
        }

        public static EventInfo GetEvent(this Type type, string eventName)
        {
            return type.GetTypeInfo().GetDeclaredEvent(eventName);
        }

        public static FieldInfo GetStaticField(this Type type, string fieldName)
        {
            var field = type.GetTypeInfo().GetDeclaredField(fieldName);
            return field.IsStatic ? field : null;
        }

        public static MethodInfo GetSetMethod(this PropertyInfo propertyInfo)
        {
            return propertyInfo.SetMethod;
        }
    }
}
