// MvxDependencyPropertyTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.BindingEx.WindowsPhone.MvxBinding.Target
{
    using System;

#if WINDOWS_PHONE || WINDOWS_WPF
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
#endif

#if NETFX_CORE
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;
#endif

    using MvvmCross.Binding;
    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Binding.ExtensionMethods;
    using MvvmCross.Platform.Platform;

    public class MvxDependencyPropertyTargetBinding : MvxConvertingTargetBinding
    {
        private readonly string _targetName;
        private readonly DependencyProperty _targetDependencyProperty;
        private readonly Type _actualPropertyType;
#if WINDOWS_PHONE || WINDOWS_WPF
        private readonly TypeConverter _typeConverter;
#endif

        public MvxDependencyPropertyTargetBinding(object target, string targetName, DependencyProperty targetDependencyProperty, Type actualPropertyType)
            : base(target)
        {
            this._targetDependencyProperty = targetDependencyProperty;
            this._actualPropertyType = actualPropertyType;
            this._targetName = targetName;
#if WINDOWS_PHONE || WINDOWS_WPF
            this._typeConverter = this._actualPropertyType.TypeConverter();
#endif
            // if we return TwoWay for ImageSource then we end up in
            // problems with WP7 not doing the auto-conversion
            // see some of my angst in http://stackoverflow.com/questions/16752242/how-does-xaml-create-the-string-to-bitmapimage-value-conversion-when-binding-to/16753488#16753488
            // Note: if we discover other issues here, then we should make a more flexible solution
            if (this._actualPropertyType == typeof(ImageSource))
            {
                this._defaultBindingMode = MvxBindingMode.OneWay;
            }
        }

        public override void SubscribeToEvents()
        {
            var frameworkElement = this.Target as FrameworkElement;
            if (frameworkElement == null)
                return;

#if WINDOWS_PHONE || WINDOWS_WPF
            var listenerBinding = new System.Windows.Data.Binding(this._targetName)
            { Source = frameworkElement };
#endif
#if NETFX_CORE
            var listenerBinding = new Windows.UI.Xaml.Data.Binding()
            {
                Path = new PropertyPath(_targetName),
                Source = frameworkElement
            };
#endif
            var attachedProperty = DependencyProperty.RegisterAttached(
                "ListenAttached" + this._targetName + Guid.NewGuid().ToString("N")
                , typeof(object)
                , typeof(FrameworkElement)
                , new PropertyMetadata(null, (s, e) => this.FireValueChanged(e.NewValue)));
            frameworkElement.SetBinding(attachedProperty, listenerBinding);
        }

        public override Type TargetType => this._actualPropertyType;

        private MvxBindingMode _defaultBindingMode = MvxBindingMode.TwoWay;
        public override MvxBindingMode DefaultMode => this._defaultBindingMode;

        protected virtual object GetValueByReflection()
        {
            var target = this.Target as FrameworkElement;
            if (target == null)
            {
                MvxBindingTrace.Warning("Weak Target is null in {0} - skipping Get", this.GetType().Name);
                return null;
            }

            return target.GetValue(this._targetDependencyProperty);
        }

        protected override void SetValueImpl(object target, object value)
        {
            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Receiving setValue to " + (value ?? ""));
            var frameworkElement = target as FrameworkElement;
            if (frameworkElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Weak Target is null in {0} - skipping set", this.GetType().Name);
                return;
            }

            frameworkElement.SetValue(this._targetDependencyProperty, value);
        }

        protected override object MakeSafeValue(object value)
        {
#if WINDOWS_PHONE || WINDOWS_WPF
            if (this._actualPropertyType.IsInstanceOfType(value))
                return value;

            if (this._typeConverter == null
                || value == null)
                // TODO - is this correct? Do we need to do more here? See #297
                return this._actualPropertyType.MakeSafeValue(value);

            if (!this._typeConverter.CanConvertFrom(value.GetType()))
                return null; // TODO - is this correct? Do we need to do more here? See #297

            return this._typeConverter.ConvertFrom(value);
#endif
#if NETFX_CORE
            return _actualPropertyType.MakeSafeValue(value);
#endif
        }
    }
}