// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;

namespace MvvmCross.Base
{
#nullable enable
    public static class MvxReflectionExtensions
    {
        public static Attribute[] GetCustomAttributes(this Type type, Type attributeType, bool inherit)
        {
            return type.GetTypeInfo().GetCustomAttributes(attributeType, inherit).OfType<Attribute>().ToArray();
        }

        public static bool IsInstanceOfType(this Type type, object obj)
        {
            return obj != null && (type?.IsAssignableFrom(obj.GetType()) == true || obj.IsMarshalByRefObject());
        }

        private static bool IsMarshalByRefObject(this object obj)
        {
            return obj != null && obj.GetType().FullName == "System.MarshalByRefObject";
        }

        public static MethodInfo? GetAddMethod(this EventInfo eventInfo, bool nonPublic = false)
        {
            if (eventInfo?.AddMethod == null || (!nonPublic && !eventInfo.AddMethod.IsPublic))
            {
                return null;
            }

            return eventInfo.AddMethod;
        }

        public static MethodInfo? GetRemoveMethod(this EventInfo eventInfo, bool nonPublic = false)
        {
            if (eventInfo?.RemoveMethod == null || (!nonPublic && !eventInfo.RemoveMethod.IsPublic))
            {
                return null;
            }

            return eventInfo.RemoveMethod;
        }

        public static MethodInfo? GetGetMethod(this PropertyInfo property, bool nonPublic = false)
        {
            if (property?.GetMethod == null || (!nonPublic && !property.GetMethod.IsPublic))
            {
                return null;
            }

            return property.GetMethod;
        }
    }
#nullable restore
}
