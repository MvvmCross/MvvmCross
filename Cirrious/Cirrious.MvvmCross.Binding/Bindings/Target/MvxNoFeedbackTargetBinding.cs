using Cirrious.CrossCore.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public abstract class MvxNoFeedbackTargetBinding : MvxTargetBinding
    {
        private bool _isUpdatingSource;
        private bool _isUpdatingTarget;
        private object _updatingSourceWith;

        public MvxNoFeedbackTargetBinding(object target)
            : base(target)
        {
        }
        
        protected abstract void TargetSetValue(object value);

        public override void SetValue(object value)
        {     
            // to prevent feedback loops, we don't pass on 'same value' updates from the source while we are updating it
            if (_isUpdatingSource)
            {
                if (value == null)
                {
                    if (_updatingSourceWith == null)
                        return;
                }
                else
                {
                    if (value.Equals(_updatingSourceWith))
                        return;
                }
            }

            try
            {
                _isUpdatingTarget = true;
                TargetSetValue(value);
            }
            finally
            {
                _isUpdatingTarget = false;
            }
        }

        protected override void FireValueChanged(object newValue)
        {
            // we don't allow 'reentrant' updates of any kind from target to source
            if (_isUpdatingTarget
                || _isUpdatingSource)
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
