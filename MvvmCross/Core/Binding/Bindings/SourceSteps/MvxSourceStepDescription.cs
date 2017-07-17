// MvxSourceStepDescription.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Converters;

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxSourceStepDescription
    {
        public IMvxValueConverter Converter { get; set; }
        public object ConverterParameter { get; set; }
        public object FallbackValue { get; set; }
    }
}