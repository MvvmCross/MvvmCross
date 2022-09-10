// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.Bindings.SourceSteps;

namespace MvvmCross.Binding.Combiners
{
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
