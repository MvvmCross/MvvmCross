namespace MvvmCross.Binding.Combiners
{
    using System.Collections.Generic;
    using System.Linq;

    using MvvmCross.Binding.ExtensionMethods;
    using MvvmCross.Platform.Converters;

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
                    return true;
                }
                if (objectValue == MvxBindingConstant.UnsetValue)
                {
                    value = MvxBindingConstant.UnsetValue;
                    return true;
                }
                bool booleanValue;
                if (!this.TryConvertToBool(objectValue, out booleanValue))
                {
                    value = MvxBindingConstant.UnsetValue;
                    return true;
                }
                stepValues.Add(booleanValue);
            }

            return this.TryCombine(stepValues, out value);
        }

        protected abstract bool TryCombine(List<bool> stepValues, out object value);

        protected virtual bool TryConvertToBool(object objectValue, out bool booleanValue)
        {
            booleanValue = objectValue.ConvertToBoolean();
            return true;
        }
    }
}