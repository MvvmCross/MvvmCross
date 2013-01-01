// MvxPropertyInfoTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public class MvxPropertyInfoTargetBinding : MvxBaseTargetBinding
    {
        private readonly object _target;
        private readonly PropertyInfo _targetPropertyInfo;

        private UpdatingState _updatingState = UpdatingState.None;

        public MvxPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
        {
            _target = target;
            _targetPropertyInfo = targetPropertyInfo;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // if the target property should be set to NULL on dispose then we clear it here
                // this is a fix for the possible memory leaks discussion started https://github.com/slodge/MvvmCross/issues/17#issuecomment-8527392
                var setToNullAttribute = Attribute.GetCustomAttribute(_targetPropertyInfo,
                                                                      typeof (MvxSetToNullAfterBindingAttribute), true);
                if (setToNullAttribute != null)
                {
                    SetValue(null);
                }
            }

            base.Dispose(isDisposing);
        }

        protected object Target
        {
            get { return _target; }
        }

        public override Type TargetType
        {
            get { return _targetPropertyInfo.PropertyType; }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override sealed void SetValue(object value)
        {
            if (_updatingState != UpdatingState.None)
                return;

            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Receiving setValue to " + (value ?? ""));
            try
            {
                _updatingState = UpdatingState.UpdatingTarget;
                var safeValue = _targetPropertyInfo.PropertyType.MakeSafeValue(value);
                _targetPropertyInfo.SetValue(_target, safeValue, null);
            }
            finally
            {
                _updatingState = UpdatingState.None;
            }
        }

        protected override sealed void FireValueChanged(object newValue)
        {
            if (_updatingState != UpdatingState.None)
                return;

            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Firing changed to " + (newValue ?? ""));
            try
            {
                _updatingState = UpdatingState.UpdatingSource;
                base.FireValueChanged(newValue);
            }
            finally
            {
                _updatingState = UpdatingState.None;
            }
        }

        #region Nested type: UpdatingState

        private enum UpdatingState
        {
            None,
            UpdatingSource,
            UpdatingTarget
        }

        #endregion
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