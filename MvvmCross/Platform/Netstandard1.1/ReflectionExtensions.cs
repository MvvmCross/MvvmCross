using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace MvvmCross
{
    public static partial class ReflectionExtensions
    {
        internal static IEnumerable<Type> GetTypes(this Assembly assembly)
        {
            return assembly.ExportedTypes;
        }

        internal static bool IsAssignableFrom(this Type typeToBeAssigned, Type typeToBeAssignedTo)
        {
            return typeToBeAssigned.GetTypeInfo().IsAssignableFrom(typeToBeAssignedTo.GetTypeInfo());
        }

        internal static IEnumerable<ConstructorInfo> GetDeclaredConstructors(this Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors;
        }

        internal static IEnumerable<Type> GetInterfaces(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces;
        }

        internal static PropertyInfo GetDeclaredProperty(this Type type, string propertyName)
        {
            return type.GetTypeInfo().GetDeclaredProperty(propertyName);
        }

        internal static PropertyInfo GetFlattenedProperty(this Type type, string propertyName)
        {
            return type.GetRuntimeProperty(propertyName);
        }

        internal static IEnumerable<PropertyInfo> GetFlattenedProperties(this Type type)
        {
            return type.GetRuntimeProperties();
        }

        internal static IEnumerable<MethodInfo> GetFlattenedMethods(this Type type)
        {
            return type.GetRuntimeMethods();
        }

        internal static EventInfo GetEvent(this Type type, string eventName)
        {
            return type.GetTypeInfo().GetDeclaredEvent(eventName);
        }

        internal static FieldInfo GetStaticField(this Type type, string fieldName)
        {
            var field = type.GetTypeInfo().GetDeclaredField(fieldName);
            return field.IsStatic ? field : null;
        }

        internal static ConstructorInfo GetConstructorPortable(this Type type, params Type[] types)
           => type.GetTypeInfo().DeclaredConstructors.FirstOrDefault
                      (constructor =>
                           constructor.GetParameters()
                                      .Select(parameter => parameter.ParameterType)
                                      .SequenceEqual(types));

        internal static MethodInfo GetMethodPortable(this Type type, string name)
            => type.GetRuntimeMethods().SingleOrDefault(m => m.Name == name);

        internal static MethodInfo GetMethodPortable(this Type type, string name, params Type[] types)
            => type.GetRuntimeMethod(name, types);

        internal static PropertyInfo GetPropertyPortable(this Type type, string name)
            => type.GetRuntimeProperty(name);

        internal static IEnumerable<FieldInfo> GetFieldsPortable(this Type type)
            => type.GetRuntimeFields();

        internal static Type GetBaseTypePortable(this Type type)
            => type.GetTypeInfo().BaseType;

        internal static MethodInfo GetGetMethod(this PropertyInfo propertyInfo)
            => propertyInfo.GetMethod;

        internal static MethodInfo GetSetMethod(this PropertyInfo propertyInfo)
            => propertyInfo.SetMethod;

        internal static Assembly GetAssemblyPortable(this Type type)
            => type.GetTypeInfo().Assembly;

        internal static MethodInfo GetMethod(this Type type, string name, Type[] parameters=null)
            => type.GetRuntimeMethod(name, parameters);

        internal static IEnumerable<Attribute> GetCustomAttributes(this Type type, Type attributeType, bool inherit)
            => type.GetTypeInfo().GetCustomAttributes(attributeType, inherit);

        internal static IEnumerable<FieldInfo> GetFields(this Type type)
            => type.GetTypeInfo().DeclaredFields;

        internal static PropertyInfo GetProperty(this Type type, string name)
            => type.GetTypeInfo().GetDeclaredProperty(name);

        internal static Type GetBaseType(this Type type)
            => type.GetTypeInfo().BaseType;

        internal static ConstructorInfo GetConstructor(this Type type, Type[] parameterTypes)
            => type.GetConstructorPortable(parameterTypes);
    }
}
