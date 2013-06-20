using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxCombinerSourceStepDescription : MvxSourceStepDescription
    {
        public IMvxValueCombiner Combiner { get; set; }
        public List<MvxSourceStepDescription> InnerSteps { get; set; }
        public object CombinerParameter { get; set; }
    }
}