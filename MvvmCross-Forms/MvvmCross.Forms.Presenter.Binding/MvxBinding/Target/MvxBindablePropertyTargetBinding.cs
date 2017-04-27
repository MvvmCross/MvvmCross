// MvxBindablePropertyTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenter.Binding
{
    public class MvxBindablePropertyTargetBinding : MvxConvertingTargetBinding
    {
        private readonly string _targetName;
        private readonly BindableProperty _targetDependencyProperty;
        private readonly Type _actualPropertyType;
        private readonly TypeConverter _typeConverter;

        public MvxBindablePropertyTargetBinding(object target, string targetName, BindableProperty targetBindableProperty, Type actualPropertyType)
            : base(target)
        {
            _targetDependencyProperty = targetBindableProperty;
            _actualPropertyType = actualPropertyType;
            _targetName = targetName;
            _typeConverter = _actualPropertyType.TypeConverter();
        }

        public override void SubscribeToEvents()
        {
            var formsElement = Target as Element;
            if (formsElement == null)
                return;

            var listenerBinding = new Xamarin.Forms.Binding
            {
                Path = _targetName,
                Source = formsElement
            };
            var attachedProperty = BindableProperty.CreateAttached("ListenAttached" + _targetName + Guid.NewGuid()
                                                                                                        .ToString("N"),
                                                                   typeof(object),
                                                                   typeof(Element),
                                                                   null,
                                                                   BindingMode.OneWay,
                                                                   null,
                                                                   (s, o, n) => FireValueChanged(n));
            formsElement.SetBinding(attachedProperty, listenerBinding);
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

            return target.GetValue(_targetDependencyProperty);
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

            frameworkElement.SetValue(_targetDependencyProperty, value);
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
    }
}