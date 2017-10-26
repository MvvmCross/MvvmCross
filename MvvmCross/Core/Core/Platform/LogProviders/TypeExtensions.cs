using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvvmCross.Core.Platform.LogProviders
{
    internal static class TypeExtensions
    {
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
    }
}
