// MvxValueConverterRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.Converters;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxValueConverterRegistry
        : IMvxValueConverterProvider
          , IMvxValueConverterRegistry
    {
        private readonly Dictionary<string, IMvxValueConverter> _converters =
            new Dictionary<string, IMvxValueConverter>();

        #region IMvxValueConverterProvider Members

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

        #endregion

        #region IMvxValueConverterRegistry Members

        public void AddOrOverwrite(string name, IMvxValueConverter converter)
        {
            _converters[name] = converter;
        }

        #endregion
    }
}