using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.IoC;

namespace MvvmCross.Binding.Combiners
{
    [MvxUnconventional]
    public class MvxValueConverterValueCombiner : MvxValueCombiner
    {
        private readonly IMvxValueConverter _valueConverter;

        private Type _targetType = typeof(object);

        public MvxValueConverterValueCombiner(IMvxValueConverter valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public override void SetValue(IEnumerable<IMvxSourceStep> steps, object value)
        {
            var sourceStep = steps.First();
            var parameter = GetParameterValue(steps);

            if (_valueConverter == null)
                return;
            var converted = _valueConverter.ConvertBack(value, sourceStep.SourceType, parameter,
                CultureInfo.CurrentUICulture);
            sourceStep.SetValue(converted);
        }

        public override IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps,
            Type overallTargetType)
        {
            _targetType = overallTargetType;
            return base.SubStepTargetTypes(subSteps, overallTargetType);
        }

        private static object GetParameterValue(IEnumerable<IMvxSourceStep> steps)
        {
            var parameterStep = steps.Skip(1).FirstOrDefault();
            object parameter = null;
            if (parameterStep != null)
                parameter = parameterStep.GetValue();
            return parameter;
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var sourceStep = steps.First();
            var parameter = GetParameterValue(steps);

            var sourceValue = sourceStep.GetValue();
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

            value = _valueConverter.Convert(sourceValue, _targetType, parameter, CultureInfo.CurrentUICulture);
            return true;
        }
    }
}