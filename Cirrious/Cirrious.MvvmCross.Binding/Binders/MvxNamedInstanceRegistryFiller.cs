// MvxNamedInstanceRegistryFiller.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using System;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxNamedInstanceRegistryFiller<T> : IMvxNamedInstanceRegistryFiller<T>
        where T : class
    {
        public virtual void FillFrom(IMvxNamedInstanceRegistry<T> registry, Type type)
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

        protected virtual void FillFromInstance(IMvxNamedInstanceRegistry<T> registry, Type type)
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

        protected virtual void FillFromStatic(IMvxNamedInstanceRegistry<T> registry, Type type)
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
                    var converter = Activator.CreateInstance(pair.Type) as T;
                    MvxBindingTrace.Trace("Registering value converter {0}:{1}", pair.Name, pair.Type.Name);
                    registry.AddOrOverwrite(pair.Name, converter);
                }
                catch (Exception)
                {
                    // ignore this
                }
            }
        }

        protected virtual string FindName(Type type)
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