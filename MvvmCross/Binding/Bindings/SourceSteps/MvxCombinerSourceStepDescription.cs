// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding.Combiners;

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxCombinerSourceStepDescription : MvxSourceStepDescription
    {
        public IMvxValueCombiner Combiner { get; set; }
        public List<MvxSourceStepDescription> InnerSteps { get; set; }

        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "This is a diagnostic ToString method and the type name is not critical for functionality")]
        public override string ToString()
        {
            return Combiner == null ? "-null-" : Combiner.GetType().Name + " combiner-operation";
        }
    }
}
