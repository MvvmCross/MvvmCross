using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;

namespace Cirrious.MvvmCross.Binding.Combiners
{
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
            System.Collections.Generic.IEnumerable<Bindings.SourceSteps.IMvxSourceStep> steps, out object value)
        {
            var stepValues = new List<bool>();
            foreach (var step in steps)
            {
                var objectValue = step.GetValue();

                if (objectValue == MvxBindingConstant.DoNothing)
                {
                    value = MvxBindingConstant.DoNothing;
                    return false;
                }
                if (objectValue == MvxBindingConstant.UnsetValue)
                {
                    value = MvxBindingConstant.UnsetValue;
                    return false;
                }
                bool booleanValue;
                if (!TryConvertToBool(objectValue, out booleanValue))
                {
                    value = MvxBindingConstant.UnsetValue;
                    return false;
                }
                stepValues.Add(booleanValue);
            }

            return TryCombine(stepValues, out value);
        }

        protected abstract bool TryCombine(List<bool> stepValues, out object value);

        protected virtual bool TryConvertToBool(object objectValue, out bool booleanValue)
        {
#warning I believe this logic is 'almost' duplicated in at least one other part of mvx - would be nice to have a single place that does this 'toBool' conversion
            if (objectValue == null)
            {
                booleanValue = false;
                return true;
            }

            if (objectValue is string)
            {
                booleanValue = "" != (string) objectValue; // any non-empty string is true
                return true;
            }

            var objectType = objectValue.GetType();
            if (objectType.IsValueType)
            {
                var defaultValue = Activator.CreateInstance(objectType);
                booleanValue = !defaultValue.Equals(objectValue);
                return true;
            }

            // object is not null - so assume it means true
            booleanValue = true;
            return true;
        }
    }
}