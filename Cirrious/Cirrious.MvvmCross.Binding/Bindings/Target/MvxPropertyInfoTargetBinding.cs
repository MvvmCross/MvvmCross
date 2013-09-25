// MvxPropertyInfoTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.ExtensionMethods;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public class MvxPropertyInfoTargetBinding : MvxNoFeedbackTargetBinding
    {
        private readonly PropertyInfo _targetPropertyInfo;

        public MvxPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target)
        {
            _targetPropertyInfo = targetPropertyInfo;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // if the target property should be set to NULL on dispose then we clear it here
                // this is a fix for the possible memory leaks discussion started https://github.com/slodge/MvvmCross/issues/17#issuecomment-8527392
                var setToNullAttribute = Attribute.GetCustomAttribute(_targetPropertyInfo,
                                                                      typeof(MvxSetToNullAfterBindingAttribute), true);
                if (setToNullAttribute != null)
                {
                    SetValue(null);
                }
            }

            base.Dispose(isDisposing);
        }

        public override Type TargetType
        {
            get { return _targetPropertyInfo.PropertyType; }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        protected virtual object GetValueByReflection()
        {
            var target = Target;
            if (target == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Weak Target is null in {0} - skipping Get", GetType().Name);
                return null;
            }
            var getMethod = _targetPropertyInfo.GetGetMethod();
            return getMethod.Invoke(target, null);
        }


        protected override void TargetSetValue(object value)
        {
            var target = Target;
            _targetPropertyInfo.SetValue(target, value, null);
        }
        
        public override sealed void SetValue(object value)
        {
            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Receiving setValue to " + (value ?? ""));
            var target = Target;
            if (target == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Weak Target is null in {0} - skipping set", GetType().Name);
                return;
            }

            if (ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(value))
                return;

            var safeValue = MakeSafeValue(value);

            base.SetValue(safeValue);
        }

        protected virtual bool ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(object value)
        {
            if (TargetType == typeof(string)
                && value != null)
            {
                // specifically for double, float and decimal we do some special comparisons
                // to prevent the user losing trailing periods, leading minus signs and trailing zeros
                var valueType = value.GetType();
                if (valueType == typeof(double) ||
                    valueType == typeof(float) ||
                    valueType == typeof(decimal))
                {
                    var currentValue = (string)GetValueByReflection();
                    if (currentValue == null)
                        return false;

                    try
                    {
                        var equivalentCurrentValue = valueType.MakeSafeValue(currentValue);
                        if (equivalentCurrentValue.Equals(value))
                            return true;
                    }
                    catch (FormatException)
                    {
                        // format problem - so they are definitely not equivalent
                        return false;
                    }
                }
            }
            return false;
        }

        protected virtual object MakeSafeValue(object value)
        {
            var safeValue = _targetPropertyInfo.PropertyType.MakeSafeValue(value);
            return safeValue;
        }
    }

    public class MvxPropertyInfoTargetBinding<T> : MvxPropertyInfoTargetBinding
        where T : class
    {
        public MvxPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected T View
        {
            get { return base.Target as T; }
        }
    }
}