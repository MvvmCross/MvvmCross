namespace MvvmCross.Binding.Combiners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MvvmCross.Binding.Bindings.SourceSteps;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.IoC;

    [MvxUnconventional]
    public class MvxValueConverterValueCombiner : MvxValueCombiner
    {
        private readonly IMvxValueConverter _valueConverter;

        public MvxValueConverterValueCombiner(IMvxValueConverter valueConverter)
        {
            this._valueConverter = valueConverter;
        }

        public override void SetValue(IEnumerable<IMvxSourceStep> steps, object value)
        {
            var sourceStep = steps.First();
            var parameter = this.GetParameterValue(steps);

            if (this._valueConverter == null)
            {
                // null value converter always fails
                return;
            }
            var converted = this._valueConverter.ConvertBack(value, sourceStep.SourceType, parameter,
                                                        System.Globalization.CultureInfo.CurrentUICulture);
            sourceStep.SetValue(converted);
        }

        private Type _targetType = typeof(object);

        public override IEnumerable<System.Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, System.Type overallTargetType)
        {
            this._targetType = overallTargetType;
            return base.SubStepTargetTypes(subSteps, overallTargetType);
        }

        private object GetParameterValue(IEnumerable<IMvxSourceStep> steps)
        {
            var parameterStep = steps.Skip(1).FirstOrDefault();
            object parameter = null;
            if (parameterStep != null)
            {
                parameter = parameterStep.GetValue();
            }
            return parameter;
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var sourceStep = steps.First();
            var parameter = this.GetParameterValue(steps);

            object sourceValue = sourceStep.GetValue();
            if (sourceValue == MvxBindingConstant.DoNothing)
            {
                value = MvxBindingConstant.DoNothing;
                return true;
            }

            if (sourceValue == MvxBindingConstant.UnsetValue)
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            if (this._valueConverter == null)
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            value = this._valueConverter.Convert(sourceValue, this._targetType, parameter, System.Globalization.CultureInfo.CurrentUICulture);
            return true;
        }
    }
}