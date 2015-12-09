// MvxConvertingTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
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
            var target = this.Target;
            if (target == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Weak Target is null in {0} - skipping set", this.GetType().Name);
                return;
            }

            if (this.ShouldSkipSetValueForViewSpecificReasons(target, value))
                return;

            var safeValue = this.MakeSafeValue(value);

            // to prevent feedback loops, we don't pass on 'same value' updates from the source while we are updating it
            if (this._isUpdatingSource)
            {
                if (safeValue == null)
                {
                    if (this._updatingSourceWith == null)
                        return;
                }
                else
                {
                    if (safeValue.Equals(this._updatingSourceWith))
                        return;
                }
            }

            try
            {
                this._isUpdatingTarget = true;
                this.SetValueImpl(target, safeValue);
            }
            finally
            {
                this._isUpdatingTarget = false;
            }
        }

        protected virtual bool ShouldSkipSetValueForViewSpecificReasons(object target, object value)
        {
            return false;
        }

        protected virtual object MakeSafeValue(object value)
        {
            var safeValue = this.TargetType.MakeSafeValue(value);
            return safeValue;
        }

        protected sealed override void FireValueChanged(object newValue)
        {
            // we don't allow 'reentrant' updates of any kind from target to source
            if (this._isUpdatingTarget
                || this._isUpdatingSource)
                return;

            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Firing changed to " + (newValue ?? ""));
            try
            {
                this._isUpdatingSource = true;
                this._updatingSourceWith = newValue;

                base.FireValueChanged(newValue);
            }
            finally
            {
                this._isUpdatingSource = false;
                this._updatingSourceWith = null;
            }
        }
    }
}