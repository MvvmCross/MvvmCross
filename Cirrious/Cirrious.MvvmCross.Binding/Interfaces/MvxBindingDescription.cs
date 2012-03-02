#region Copyright
// <copyright file="MvxBindingDescription.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.Converters;

namespace Cirrious.MvvmCross.Binding.Interfaces
{
    public class MvxBindingDescription
    {
        public MvxBindingDescription()
        {            
        }

        public MvxBindingDescription(string targetName, string sourcePropertyPath, IMvxValueConverter converter, object converterParameter, object fallbackValue, MvxBindingMode mode)
        {
            TargetName = targetName;
            SourcePropertyPath = sourcePropertyPath;
            Converter = converter;
            ConverterParameter = converterParameter;
            FallbackValue = fallbackValue;
            Mode = mode;
        }

        public string TargetName { get; set; }
        public string SourcePropertyPath { get; set; }
        public IMvxValueConverter Converter { get; set; }
        public object ConverterParameter { get; set; }
        public object FallbackValue { get; set; }
        public MvxBindingMode Mode { get; set; }

        public override string ToString()
        {
            return string.Format("from {0} to {1}", SourcePropertyPath, TargetName);
        }
    }
}