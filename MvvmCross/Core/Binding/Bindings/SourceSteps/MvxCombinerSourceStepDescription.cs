// MvxCombinerSourceStepDescription.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    using System.Collections.Generic;

    using MvvmCross.Binding.Combiners;

    public class MvxCombinerSourceStepDescription : MvxSourceStepDescription
    {
        public IMvxValueCombiner Combiner { get; set; }
        public List<MvxSourceStepDescription> InnerSteps { get; set; }

        public override string ToString()
        {
            return this.Combiner == null ? "-null-" : this.Combiner.GetType().Name + " combiner-operation";
        }
    }
}