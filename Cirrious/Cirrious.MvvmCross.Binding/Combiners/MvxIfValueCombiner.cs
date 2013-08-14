// MvxIfValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Linq;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;

namespace Cirrious.MvvmCross.Binding.Combiners
{
    public class MvxIfValueCombiner
        : MvxValueCombiner
    {
        public override bool TryGetValue(System.Collections.Generic.IEnumerable<Bindings.SourceSteps.IMvxSourceStep> steps, out object value)
        {
            var list = steps.ToList();
            switch (list.Count)
            {
                case 2:
                    return TryEvaluateIf(list[0], list[1], null, out value);
                case 3:
                    return TryEvaluateIf(list[0], list[1], list[2], out value);

                default:
                    MvxBindingTrace.Warning("Unexpected substep count of {0} in 'If' ValueCombiner", list.Count);
                    return base.TryGetValue(list, out value);
            }
        }

        private bool TryEvaluateIf(IMvxSourceStep testStep, IMvxSourceStep ifStep, IMvxSourceStep elseStep, out object value)
        {
            object result;
            var testAvailable = testStep.TryGetValue(out result);
            if (!testAvailable)
            {
                value = null;
                return false;
            }

            if (IsTrue(result))
                return ReturnSubStepResult(ifStep, out value);

            return ReturnSubStepResult(elseStep, out value);
        }

        protected virtual bool IsTrue(object result)
        {
            if (result == null)
                return false;

            if (result is string)
                return string.IsNullOrEmpty((string)result);

            if (result is bool)
                return (bool) result;

            if (result is int)
                return (int)result != 0;

            if (result is double)
                return (double)result != 0.0;

            return true;
        }

        protected virtual bool ReturnSubStepResult(IMvxSourceStep subStep, out object value)
        {
            if (subStep == null)
            {
                value = null;
                return true;
            }
            return subStep.TryGetValue(out value);
        }
    }
}