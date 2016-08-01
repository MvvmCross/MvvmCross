// MvxConvertingTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target
{
    using MvvmCross.Binding.ExtensionMethods;
    using MvvmCross.Platform.Platform;

    public abstract class MvxConvertingTargetBinding : MvxTargetBinding
    {
        private bool _isUpdatingSource;
        private bool _isUpdatingTarget;
        private object _updatingSourceWith;

        protected MvxConvertingTargetBinding(object target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected abstract void SetValueImpl(object target, object value);

        public override void SetValue(object value)
        {
            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Receiving setValue to " + (value ?? ""));
            var target = Target;
            if (target == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Weak Target is null in {0} - skipping set", GetType().Name);
                return;
            }

            if (ShouldSkipSetValueForViewSpecificReasons(target, value))
                return;

            var safeValue = MakeSafeValue(value);

            // to prevent feedback loops, we don't pass on 'same value' updates from the source while we are updating it
            if (_isUpdatingSource)
            {
                if (safeValue == null)
                {
                    if (_updatingSourceWith == null)
                        return;
                }
                else
                {
                    if (safeValue.Equals(_updatingSourceWith))
                        return;
                }
            }

            try
            {
                _isUpdatingTarget = true;
                SetValueImpl(target, safeValue);
            }
            finally
            {
                _isUpdatingTarget = false;
            }
        }

        protected virtual bool ShouldSkipSetValueForViewSpecificReasons(object target, object value)
        {
            return false;
        }

        protected virtual object MakeSafeValue(object value)
        {
            var safeValue = TargetType.MakeSafeValue(value);
            return safeValue;
        }

        protected sealed override void FireValueChanged(object newValue)
        {
            // we don't allow 'reentrant' updates of any kind from target to source
            if (_isUpdatingTarget || _isUpdatingSource)
                return;

            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Firing changed to " + (newValue ?? ""));
            try
            {
                _isUpdatingSource = true;
                _updatingSourceWith = newValue;

                base.FireValueChanged(newValue);
            }
            finally
            {
                _isUpdatingSource = false;
                _updatingSourceWith = null;
            }
        }
    }
}