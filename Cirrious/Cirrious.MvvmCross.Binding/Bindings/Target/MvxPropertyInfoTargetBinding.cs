using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public class MvxPropertyInfoTargetBinding : MvxBaseTargetBinding
    {
        private readonly object _target;
        private readonly PropertyInfo _targetPropertyInfo;

        private enum UpdatingState
        {
            None,
            UpdatingSource,
            UpdatingTarget
        }

        private UpdatingState _updatingState = UpdatingState.None;

        protected object Target { get { return _target; } }

        public MvxPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
        {
            _target = target;
            _targetPropertyInfo = targetPropertyInfo;
        }

        public override Type TargetType
        {
            get { return _targetPropertyInfo.PropertyType; }
        }

        sealed public override void SetValue(object value)
        {
            if (_updatingState != UpdatingState.None)
                return;

            MvxBindingTrace.Trace(MvxBindingTraceLevel.Diagnostic,"Receiving setValue to " + (value ?? "").ToString());
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

            MvxBindingTrace.Trace(MvxBindingTraceLevel.Diagnostic, "Firing changed to " + (newValue ?? "").ToString());
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

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
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