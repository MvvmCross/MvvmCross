#region Copyright
// <copyright file="MvxValueConverterRegistry.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Interfaces.Converters;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxValueConverterRegistry
        : IMvxValueConverterProvider
        , IMvxValueConverterRegistry
    {
        private readonly Dictionary<string, IMvxValueConverter> _converters = new Dictionary<string, IMvxValueConverter>();

        #region IMvxValueConverterProvider Members

        public IMvxValueConverter Find(string converterName)
        {
            IMvxValueConverter toReturn;
            if (!_converters.TryGetValue(converterName, out toReturn))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,"Could not find named converter " + converterName);
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