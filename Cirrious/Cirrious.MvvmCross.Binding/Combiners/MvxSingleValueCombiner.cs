// MvxSingleValueCombiner.cs
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
    public class MvxSingleValueCombiner : MvxValueCombiner
    {
        public override Type SourceType(IEnumerable<IMvxSourceStep> steps)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
                return typeof (object);

            return firstStep.SourceType;
        }

        public override void SetValue(IEnumerable<IMvxSourceStep> steps, object value)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
                return;

            firstStep.SetValue(value);
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
            {
                value = null;
                return false;
            }

            return firstStep.TryGetValue(out value);
        }
    }
}