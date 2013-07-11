// MvxValueConverterRegistryFiller.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.IoC;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxValueConverterRegistryFiller : IMvxValueConverterRegistryFiller
    {
        public virtual void FillFrom(IMvxValueConverterRegistry registry, Type type)
        {
            if (type.IsAbstract)
            {
                FillFromStatic(registry, type);
            }
            else
            {
                FillFromInstance(registry, type);
            }
        }

        protected virtual void FillFromInstance(IMvxValueConverterRegistry registry, Type type)
        {
            var instance = Activator.CreateInstance(type);

            var pairs = from field in type.GetFields()
                        where !field.IsStatic
                        where field.IsPublic
                        where typeof (IMvxValueConverter).IsAssignableFrom(field.FieldType)
                        let converter = field.GetValue(instance) as IMvxValueConverter
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

        protected virtual void FillFromStatic(IMvxValueConverterRegistry registry, Type type)
        {
            var pairs = from field in type.GetFields()
                        where field.IsStatic
                        where field.IsPublic
                        where typeof (IMvxValueConverter).IsAssignableFrom(field.FieldType)
                        let converter = field.GetValue(null) as IMvxValueConverter
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

        public virtual void FillFrom(IMvxValueConverterRegistry registry, Assembly assembly)
        {
            var pairs = from type in assembly.ExceptionSafeGetTypes()
                        where type.IsPublic
                        where !type.IsAbstract
                        where typeof (IMvxValueConverter).IsAssignableFrom(type)
                        let name = FindValueConverterName(type)
                        where !string.IsNullOrEmpty(name)
                        where type.IsConventional()
                        select new {Name = name, Type = type};

            foreach (var pair in pairs)
            {
                try
                {
                    var converter = Activator.CreateInstance(pair.Type) as IMvxValueConverter;
                    MvxBindingTrace.Trace("Registering value converter {0}:{1}", pair.Name, pair.Type.Name);
                    registry.AddOrOverwrite(pair.Name, converter);
                }
                catch (Exception)
                {
                    // ignore this
                }
            }
        }

        protected virtual string FindValueConverterName(Type type)
        {
            var name = type.Name;
            name = RemoveTail(name, "ValueConverter");
            name = RemoveTail(name, "Converter");
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