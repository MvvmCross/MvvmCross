// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Extensions;
using MvvmCross.WeakSubscription;
using Xamarin.Forms;

namespace MvvmCross.Forms.Bindings.Target
{
    public class MvxBindablePropertyTargetBinding : MvxConvertingTargetBinding
    {
        private readonly BindableProperty _targetBindableProperty;
        private readonly Type _actualPropertyType;
        private readonly Xamarin.Forms.TypeConverter _typeConverter;
        private MvxNotifyPropertyChangedEventSubscription _propertyChangedSubscription;

        public MvxBindablePropertyTargetBinding(object target, BindableProperty targetBindableProperty, Type actualPropertyType)
            : base(target)
        {
            _targetBindableProperty = targetBindableProperty;
            _actualPropertyType = actualPropertyType;
            _typeConverter = _actualPropertyType.TypeConverter();
        }

        public override void SubscribeToEvents()
        {
            var bindableObject = Target as BindableObject;
            if (bindableObject == null)
                return;

            _propertyChangedSubscription = bindableObject.WeakSubscribe(OnBindableObjectPropertyChanged);
        }

        public override Type TargetValueType => _actualPropertyType;

        private MvxBindingMode _defaultBindingMode = MvxBindingMode.TwoWay;
        public override MvxBindingMode DefaultMode => _defaultBindingMode;

        protected virtual object GetValueByReflection()
        {
            var target = Target as BindableObject;
            if (target == null)
            {
                MvxBindingLog.Warning("Weak Target is null in {0} - skipping Get", GetType().Name);
                return null;
            }

            return target.GetValue(_targetBindableProperty);
        }

        protected override void SetValueImpl(object target, object value)
        {
            MvxBindingLog.Trace("Receiving setValue to " + (value ?? ""));
            var bindableObject = target as BindableObject;
            if (bindableObject == null)
            {
                MvxBindingLog.Warning("Weak Target is null in {0} - skipping set", GetType().Name);
                return;
            }

            bindableObject.SetValue(_targetBindableProperty, value);
        }

        protected override object MakeSafeValue(object value)
        {
            if (_actualPropertyType.IsInstanceOfType(value))
                return value;

            if (_typeConverter == null
                || value == null)
                // TODO - is this correct? Do we need to do more here? See #297
                return _actualPropertyType.MakeSafeValue(value);

            if (!_typeConverter.CanConvertFrom(value.GetType()))
                return null; // TODO - is this correct? Do we need to do more here? See #297

            if (value is string stringValue)
                return _typeConverter.ConvertFromInvariantString(stringValue);
            else
                return null;
        }

        private void OnBindableObjectPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var bindableObject = Target as BindableObject;

            if (args.PropertyName == _targetBindableProperty.PropertyName)
            {
                FireValueChanged(bindableObject.GetValue(_targetBindableProperty));
            }
        }
    }
}
