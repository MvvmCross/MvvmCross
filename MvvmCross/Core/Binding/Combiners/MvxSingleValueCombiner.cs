// MvxSingleValueCombiner.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Combiners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MvvmCross.Binding.Bindings.SourceSteps;
    using MvvmCross.Platform.Converters;

    public class MvxSingleValueCombiner : MvxValueCombiner
    {
        public override Type SourceType(IEnumerable<IMvxSourceStep> steps)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
                return typeof(object);

            return firstStep.SourceType;
        }

        public override void SetValue(IEnumerable<IMvxSourceStep> steps, object value)
        {
            var firstStep = steps.FirstOrDefault();

            firstStep?.SetValue(value);
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            value = firstStep.GetValue();
            return true;
        }
    }
}