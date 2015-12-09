using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cirrious.MvvmCross.Binding.Combiners
{
    [MvxUnconventional]
    public class MvxValueConverterValueCombiner : MvxValueCombiner
    {
        private readonly IMvxValueConverter _valueConverter;

        public MvxValueConverterValueCombiner(IMvxValueConverter valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public override void SetValue(IEnumerable<IMvxSourceStep> steps, object value)
        {
            var sourceStep = steps.First();
            var parameter = GetParameterValue(steps);

            if (_valueConverter == null)
            {
                // null value converter always fails
                return;
            }
            var converted = _valueConverter.ConvertBack(value, sourceStep.SourceType, parameter,
                                                        System.Globalization.CultureInfo.CurrentUICulture);
            sourceStep.SetValue(converted);
        }

        private Type _targetType = typeof(object);

        public override IEnumerable<System.Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, System.Type overallTargetType)
        {
            _targetType = overallTargetType;
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
            var parameter = GetParameterValue(steps);

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

            if (_valueConverter == null)
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            value = _valueConverter.Convert(sourceValue, _targetType, parameter, System.Globalization.CultureInfo.CurrentUICulture);
            return true;
        }
    }
}