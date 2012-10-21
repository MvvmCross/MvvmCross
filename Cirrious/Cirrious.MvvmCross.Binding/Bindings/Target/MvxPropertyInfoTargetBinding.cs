#region Copyright
// <copyright file="MvxPropertyInfoTargetBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Linq;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Exceptions;
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
                var setToNullAttribute = Attribute.GetCustomAttribute(_targetPropertyInfo, typeof(MvxSetToNullAfterBindingAttribute), true);
                if (setToNullAttribute != null)
                {
                    SetValue(null);
                }
            }

            base.Dispose(isDisposing);
        }

        protected object Target { get { return _target; } }

        public override Type TargetType
        {
            get { return _targetPropertyInfo.PropertyType; }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        sealed public override void SetValue(object value)
        {
            if (_updatingState != UpdatingState.None)
                return;

            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic,"Receiving setValue to " + (value ?? "").ToString());
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

        sealed protected override void FireValueChanged(object newValue)
        {
            if (_updatingState != UpdatingState.None)
                return;

            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Firing changed to " + (newValue ?? "").ToString());
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