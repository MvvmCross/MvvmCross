// MvxCombinerSourceStepDescription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Combiners;

namespace Cirrious.MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxCombinerSourceStepDescription : MvxSourceStepDescription
    {
        public IMvxValueCombiner Combiner { get; set; }
        public List<MvxSourceStepDescription> InnerSteps { get; set; }

        public override string ToString()
        {
            return Combiner == null ? "-null-" : Combiner.GetType().Name + " combiner-operation";
        }
    }
}