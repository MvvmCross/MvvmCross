// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MvvmCross.Base;

namespace MvvmCross.Binding.Binders
{
    public class MvxNamedInstanceRegistry<T>
        : IMvxNamedInstanceLookup<T>, IMvxNamedInstanceRegistry<T>
        where T : class
    {
        private readonly Dictionary<string, T> _converters =
            new Dictionary<string, T>();

        public T Find(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (!_converters.TryGetValue(name, out var toReturn))
            {
                // no trace here - this is expected to fail sometimes - e.g. in the case where we look for first combiner, then converter
            }
            return toReturn;
        }

        public void AddOrOverwrite(string name, T instance)
        {
            _converters[name] = instance;
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        public void AddOrOverwriteFrom(Assembly assembly)
        {
            this.Fill(assembly);
        }
    }
}
