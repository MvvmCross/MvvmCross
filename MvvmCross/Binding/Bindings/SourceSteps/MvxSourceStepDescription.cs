// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Converters;

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxSourceStepDescription
    {
        public IMvxValueConverter Converter { get; set; }
        public object ConverterParameter { get; set; }
        public object FallbackValue { get; set; }
    }
}
