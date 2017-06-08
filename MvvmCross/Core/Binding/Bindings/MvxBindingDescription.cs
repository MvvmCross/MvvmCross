// MvxBindingDescription.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Binding.Bindings
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
            return $"binding {TargetName} for {(Source == null ? "-null" : Source.ToString())}";
        }
    }
}