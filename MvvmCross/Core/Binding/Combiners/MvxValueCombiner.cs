// MvxValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
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

    public abstract class MvxValueCombiner
        : IMvxValueCombiner
    {
        public virtual Type SourceType(IEnumerable<IMvxSourceStep> steps)
        {
            return typeof(object);
        }

        public virtual void SetValue(IEnumerable<IMvxSourceStep> steps, object value)
        {
            // do nothing
        }

        public virtual bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            value = null;
            return false;
        }

        public virtual IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps,
                                                            Type overallTargetType)
        {
            // by default a combiner just demand objects from its sources
            return subSteps.Select(x => typeof(object));
        }
    }
}