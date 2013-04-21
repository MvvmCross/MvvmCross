// MvxValueConverterRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxValueConverterRegistry
        : IMvxValueConverterLookup
          , IMvxValueConverterRegistry
    {
        private readonly Dictionary<string, IMvxValueConverter> _converters =
            new Dictionary<string, IMvxValueConverter>();

        public IMvxValueConverter Find(string converterName)
        {
            if (string.IsNullOrEmpty(converterName))
                return null;

            IMvxValueConverter toReturn;
            if (!_converters.TryGetValue(converterName, out toReturn))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Could not find named converter " + converterName);
            }
            return toReturn;
        }

        public void AddOrOverwrite(string name, IMvxValueConverter converter)
        {
            _converters[name] = converter;
        }

        public void AddOrOverwriteFrom(Assembly assembly)
        {
            this.Fill(assembly);
        }
    }
}