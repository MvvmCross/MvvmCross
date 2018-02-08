// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Base.Platform;

namespace MvvmCross.Binding.Binders
{
    public class MvxNamedInstanceRegistry<T>
        : IMvxNamedInstanceLookup<T>, IMvxNamedInstanceRegistry<T>
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
                // MvxBindingLog.Trace("Could not find named {0} for {1}", converterName,
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