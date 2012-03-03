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
using System.Reflection;
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
                var safeValue = MakeValueSafeForTarget(value);
                _targetPropertyInfo.SetValue(_target, safeValue, null);
            }
            finally 
            {
                _updatingState = UpdatingState.None;
            }
        }

        private object MakeValueSafeForTarget(object value)
        {
            object toReturn = value;
#warning not sure about value type hack here + could also add enum checking/parsing (Enum.ToObject())?
            if (_targetPropertyInfo.PropertyType.IsValueType)
            {
                if (toReturn == null)
                {
                    toReturn = Activator.CreateInstance(_targetPropertyInfo.PropertyType);
                    return toReturn;
                }
            }

            if (_targetPropertyInfo.PropertyType == typeof (string))
            {
                if (!(toReturn is string))
                {
                    if (toReturn != null)
                    {
                        toReturn = toReturn.ToString();
                    }
                    else
                    {
#warning not sure about string.empty here
                        toReturn = string.Empty;
                    }
                }
            }
            return toReturn;
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