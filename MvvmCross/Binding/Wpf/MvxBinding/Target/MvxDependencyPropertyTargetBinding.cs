// MvxDependencyPropertyTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Platform.Platform;

#if WINDOWS_COMMON
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace MvvmCross.BindingEx.WindowsCommon.MvxBinding.Target
#endif

#if WINDOWS_WPF
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace MvvmCross.BindingEx.Wpf.MvxBinding.Target
#endif
{
    public class MvxDependencyPropertyTargetBinding : MvxConvertingTargetBinding
    {
        private readonly string _targetName;
        private readonly DependencyProperty _targetDependencyProperty;
        private readonly Type _actualPropertyType;
#if WINDOWS_WPF
        private readonly TypeConverter _typeConverter;
#endif

        public MvxDependencyPropertyTargetBinding(object target, string targetName, DependencyProperty targetDependencyProperty, Type actualPropertyType)
            : base(target)
        {
            _targetDependencyProperty = targetDependencyProperty;
            _actualPropertyType = actualPropertyType;
            _targetName = targetName;
#if WINDOWS_WPF
            _typeConverter = _actualPropertyType.TypeConverter();
#endif
            // if we return TwoWay for ImageSource then we end up in
            // problems with WP7 not doing the auto-conversion
            // see some of my angst in http://stackoverflow.com/questions/16752242/how-does-xaml-create-the-string-to-bitmapimage-value-conversion-when-binding-to/16753488#16753488
            // Note: if we discover other issues here, then we should make a more flexible solution
            if (_actualPropertyType == typeof(ImageSource))
            {
                _defaultBindingMode = MvxBindingMode.OneWay;
            }
        }

        public override void SubscribeToEvents()
        {
            var frameworkElement = Target as FrameworkElement;
            if (frameworkElement == null)
                return;

#if WINDOWS_WPF
            var listenerBinding = new System.Windows.Data.Binding(_targetName)
            { Source = frameworkElement };
#endif
#if WINDOWS_COMMON
            var listenerBinding = new Windows.UI.Xaml.Data.Binding()
            {
                Path = new PropertyPath(_targetName),
                Source = frameworkElement
            };
#endif
            var attachedProperty = DependencyProperty.RegisterAttached(
                "ListenAttached" + _targetName + Guid.NewGuid().ToString("N")
                , typeof(object)
                , typeof(FrameworkElement)
                , new PropertyMetadata(null, (s, e) => FireValueChanged(e.NewValue)));
            frameworkElement.SetBinding(attachedProperty, listenerBinding);
        }

        public override Type TargetType => _actualPropertyType;

        private MvxBindingMode _defaultBindingMode = MvxBindingMode.TwoWay;
        public override MvxBindingMode DefaultMode => _defaultBindingMode;

        protected virtual object GetValueByReflection()
        {
            var target = Target as FrameworkElement;
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
            var frameworkElement = target as FrameworkElement;
            if (frameworkElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Weak Target is null in {0} - skipping set", GetType().Name);
                return;
            }

            frameworkElement.SetValue(_targetDependencyProperty, value);
        }

        protected override object MakeSafeValue(object value)
        {
#if WINDOWS_WPF
            if (_actualPropertyType.IsInstanceOfType(value))
                return value;

            if (_typeConverter == null
                || value == null)
                // TODO - is this correct? Do we need to do more here? See #297
                return _actualPropertyType.MakeSafeValue(value);

            if (!_typeConverter.CanConvertFrom(value.GetType()))
                return null; // TODO - is this correct? Do we need to do more here? See #297

            return _typeConverter.ConvertFrom(value);
#endif
#if WINDOWS_COMMON
            return _actualPropertyType.MakeSafeValue(value);
#endif
        }
    }
}