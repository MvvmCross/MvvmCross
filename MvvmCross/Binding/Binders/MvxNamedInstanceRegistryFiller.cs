// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.IoC;

namespace MvvmCross.Binding.Binders
{
    public class MvxNamedInstanceRegistryFiller<T> : IMvxNamedInstanceRegistryFiller<T>
        where T : class
    {
        protected virtual void FillFromInstance(
            IMvxNamedInstanceRegistry<T> registry,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type)
        {
            var instance = Activator.CreateInstance(type);

            var pairs = from field in type.GetFields()
                        where !field.IsStatic
                        where field.IsPublic
                        where typeof(T).IsAssignableFrom(field.FieldType)
                        let converter = field.GetValue(instance) as T
                        where converter != null
                        select new
                        {
                            field.Name,
                            Converter = converter
                        };

            foreach (var pair in pairs)
            {
                registry.AddOrOverwrite(pair.Name, pair.Converter);
            }
        }

        protected virtual void FillFromStatic(
            IMvxNamedInstanceRegistry<T> registry,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type type)
        {
            var pairs = from field in type.GetFields()
                        where field.IsStatic
                        where field.IsPublic
                        where typeof(T).IsAssignableFrom(field.FieldType)
                        let converter = field.GetValue(null) as T
                        where converter != null
                        select new
                        {
                            field.Name,
                            Converter = converter
                        };

            foreach (var pair in pairs)
            {
                registry.AddOrOverwrite(pair.Name, pair.Converter);
            }
        }

        public virtual void FillFrom(
            IMvxNamedInstanceRegistry<T> registry,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type)
        {
            if (type.GetTypeInfo().IsAbstract)
            {
                FillFromStatic(registry, type);
            }
            else
            {
                FillFromInstance(registry, type);
            }
        }

        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public virtual void FillFrom(IMvxNamedInstanceRegistry<T> registry, Assembly assembly)
        {
            var pairs = from type in assembly.ExceptionSafeGetTypes()
                        where type.GetTypeInfo().IsPublic
                        where !type.GetTypeInfo().IsAbstract
                        where typeof(T).IsAssignableFrom(type)
                        let name = FindName(type)
                        where !string.IsNullOrEmpty(name)
                        where type.IsConventional()
                        select new { Name = name, Type = type };

            foreach (var pair in pairs)
            {
                try
                {
                    if (pair.Type.ContainsGenericParameters) continue;

                    var converter = Activator.CreateInstance(pair.Type) as T;
                    MvxBindingLog.Instance?.LogTrace("Registering value converter {Name}:{Type}", pair.Name, pair.Type.Name);
                    registry.AddOrOverwrite(pair.Name, converter);
                }
                catch (Exception ex)
                {
                    MvxBindingLog.Instance?.LogError(ex, "Failed to register {Name} from {Type}", pair.Name,
                        pair.Type.Name);
                }
            }
        }

        public virtual string FindName(Type type)
        {
            var name = type.Name;
            name = RemoveHead(name, "Mvx");
            return name;
        }

        protected static string RemoveHead(string name, string word)
        {
            if (name.StartsWith(word))
                name = name.Substring(word.Length);
            return name;
        }

        protected static string RemoveTail(string name, string word)
        {
            if (name.EndsWith(word))
                name = name.Substring(0, name.Length - word.Length);
            return name;
        }
    }
}
