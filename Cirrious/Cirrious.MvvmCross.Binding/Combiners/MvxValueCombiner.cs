// MvxValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;

namespace Cirrious.MvvmCross.Binding.Combiners
{
    public abstract class MvxValueCombiner
        : IMvxValueCombiner
    {
        public virtual Type SourceType(IEnumerable<IMvxSourceStep> steps, object parameter)
        {
            return typeof (object);
        }

        public virtual void SetValue(IEnumerable<IMvxSourceStep> steps, object parameter, object value)
        {
            // do nothing
        }

        public virtual bool TryGetValue(IEnumerable<IMvxSourceStep> steps, object parameter, out object value)
        {
            value = null;
            return false;
        }

        public virtual IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, object parameter,
                                                            Type overallTargetType)
        {
            return subSteps.Select(x => overallTargetType);
        }
    }
}