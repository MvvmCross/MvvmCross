// MvxBindingDescription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;

namespace Cirrious.MvvmCross.Binding.Bindings
{
    public class MvxBindingDescription
    {
        public MvxBindingDescription()
        {
        }

        public MvxBindingDescription(string targetName, string sourcePropertyPath, IMvxValueConverter converter,
                                     object converterParameter, object fallbackValue, MvxBindingMode mode)
        {
            TargetName = targetName;
            Mode = mode;
            Source = new MvxPathSourceStepDescription
                {
                    SourcePropertyPath = sourcePropertyPath,
                    Converter = converter,
                    ConverterParameter = converterParameter,
                    FallbackValue = fallbackValue,
                };
        }

        public string TargetName { get; set; }
        public MvxBindingMode Mode { get; set; }
        public MvxSourceStepDescription Source { get; set; }

        public override string ToString()
        {
            return string.Format("binding {0} for {1}", TargetName, Source == null ? "-null" : Source.ToString());
        }
    }
}