// IMvxValueCombiner.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Combiners
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Binding.Bindings.SourceSteps;

    public interface IMvxValueCombiner
    {
        Type SourceType(IEnumerable<IMvxSourceStep> steps);

        void SetValue(IEnumerable<IMvxSourceStep> steps, object value);

        bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value);

        IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, Type overallTargetType);
    }
}