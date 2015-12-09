// MvxNamedInstanceRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;
using System.Collections.Generic;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxNamedInstanceRegistry<T>
        : IMvxNamedInstanceLookup<T>
          , IMvxNamedInstanceRegistry<T>
        where T : class
    {
        private readonly Dictionary<string, T> _converters =
            new Dictionary<string, T>();

        public T Find(string converterName)
        {
            if (string.IsNullOrEmpty(converterName))
                return null;

            T toReturn;
            if (!_converters.TryGetValue(converterName, out toReturn))
            {
                // no trace here - this is expected to fail sometimes - e.g. in the case where we look for first combiner, then converter
                // MvxBindingTrace.Trace("Could not find named {0} for {1}", converterName,
                //                      typeof(T).Name);
            }
            return toReturn;
        }

        public void AddOrOverwrite(string name, T converter)
        {
            _converters[name] = converter;
        }

        public void AddOrOverwriteFrom(Assembly assembly)
        {
            this.Fill(assembly);
        }
    }
}