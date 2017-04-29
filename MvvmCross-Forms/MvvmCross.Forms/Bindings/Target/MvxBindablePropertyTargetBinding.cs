// MvxBindablePropertyTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.ComponentModel;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using Xamarin.Forms;

namespace MvvmCross.Forms.Bindings.Target
{
    public class MvxBindablePropertyTargetBinding : MvxConvertingTargetBinding
    {
        private readonly BindableProperty _targetBindableProperty;
        private readonly Type _actualPropertyType;
        private readonly TypeConverter _typeConverter;
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
            var formsElement = Target as Element;
            if (formsElement == null)
                return;

            _propertyChangedSubscription = formsElement.WeakSubscribe(OnElementPropertyChanged);
        }

        public override Type TargetType => _actualPropertyType;

        private MvxBindingMode _defaultBindingMode = MvxBindingMode.TwoWay;
        public override MvxBindingMode DefaultMode => _defaultBindingMode;

        protected virtual object GetValueByReflection()
        {
            var target = Target as Element;
            if (target == null)
            {
                MvxBindingTrace.Warning("Weak Target is null in {0} - skipping Get", GetType().Name);
                return null;
            }

            return target.GetValue(_targetBindableProperty);
        }

        protected override void SetValueImpl(object target, object value)
        {
            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Receiving setValue to " + (value ?? ""));
            var frameworkElement = target as Element;
            if (frameworkElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Weak Target is null in {0} - skipping set", GetType().Name);
                return;
            }

            frameworkElement.SetValue(_targetBindableProperty, value);
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

            return _typeConverter.ConvertFrom(value);
        }

        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var formsElement = Target as Element;

            if (args.PropertyName == _targetBindableProperty.PropertyName)
            {
                FireValueChanged(formsElement.GetValue(_targetBindableProperty));
            }
        }
    }
}