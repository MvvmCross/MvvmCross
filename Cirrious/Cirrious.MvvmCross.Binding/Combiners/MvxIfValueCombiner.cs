// MvxIfValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using System.Linq;

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
            var result = testStep.GetValue();
            if (result == MvxBindingConstant.DoNothing)
            {
                value = MvxBindingConstant.DoNothing;
                return true;
            }

            if (result == MvxBindingConstant.UnsetValue)
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            if (IsTrue(result))
            {
                value = ReturnSubStepResult(ifStep);
                return true;
            }

            value = ReturnSubStepResult(elseStep);
            return true;
        }

        protected virtual bool IsTrue(object result)
        {
            return result.ConvertToBoolean();
        }

        protected virtual object ReturnSubStepResult(IMvxSourceStep subStep)
        {
            if (subStep == null)
            {
                return MvxBindingConstant.UnsetValue;
            }
            return subStep.GetValue();
        }
    }
}