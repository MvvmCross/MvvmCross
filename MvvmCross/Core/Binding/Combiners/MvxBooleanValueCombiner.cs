// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Binding.Combiners
{
    public class MvxInvertedValueCombiner
			: MvxBooleanValueCombiner
	{
		protected override bool TryCombine(List<bool> stepValues, out object value)
		{
			value = stepValues.Any(x => !x)
							  && true;
			return true;
		}
	}

    public class MvxAndValueCombiner
        : MvxBooleanValueCombiner
    {
        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.All(x => x);
            return true;
        }
    }

    public class MvxOrValueCombiner
        : MvxBooleanValueCombiner
    {
        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.Any(x => x);
            return true;
        }
    }

    public class MvxNotValueCombiner
        : MvxBooleanValueCombiner
    {
        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.All(x => !x);
            return true;
        }
    }

    public class MvxXorValueCombiner
        : MvxBooleanValueCombiner
    {
        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.Any(x => !x)
                && stepValues.Any(x => x);
            return true;
        }
    }

    public abstract class MvxBooleanValueCombiner
        : MvxValueCombiner
    {
        public override bool TryGetValue(
            IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var stepValues = new List<bool>();
            foreach (var step in steps)
            {
                var objectValue = step.GetValue();

                if (objectValue == MvxBindingConstant.DoNothing)
                {
                    value = MvxBindingConstant.DoNothing;
                    return true;
                }
                if (objectValue == MvxBindingConstant.UnsetValue)
                {
                    value = MvxBindingConstant.UnsetValue;
                    return true;
                }
                bool booleanValue;
                if (!TryConvertToBool(objectValue, out booleanValue))
                {
                    value = MvxBindingConstant.UnsetValue;
                    return true;
                }
                stepValues.Add(booleanValue);
            }

            return TryCombine(stepValues, out value);
        }

        protected abstract bool TryCombine(List<bool> stepValues, out object value);

        protected virtual bool TryConvertToBool(object objectValue, out bool booleanValue)
        {
            booleanValue = objectValue.ConvertToBoolean();
            return true;
        }
    }
}