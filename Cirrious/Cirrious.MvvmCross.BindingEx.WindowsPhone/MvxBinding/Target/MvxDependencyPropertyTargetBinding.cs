// MvxDependencyPropertyTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
#if WINDOWS_PHONE || WINDOWS_WPF
using System.ComponentModel;
#endif
using System.Reflection;
#if WINDOWS_PHONE || WINDOWS_WPF
using System.Windows;
using System.Windows.Media;
#endif
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

// ReSharper disable CheckNamespace
namespace Cirrious.MvvmCross.BindingEx.WindowsShared.MvxBinding.Target
// ReSharper restore CheckNamespace
{
    public class MvxDependencyPropertyTargetBinding : MvxTargetBinding
    {
        private readonly DependencyProperty _targetDependencyProperty;
        private readonly Type _actualPropertyType;
#if WINDOWS_PHONE || WINDOWS_WPF
        private readonly TypeConverter _typeConverter;
#endif

        private bool _isUpdatingSource;
        private bool _isUpdatingTarget;
        private object _updatingSourceWith;

        public MvxDependencyPropertyTargetBinding(object target, string targetName, DependencyProperty targetDependencyProperty, Type actualPropertyType)
            : base(target)
        {
            _targetDependencyProperty = targetDependencyProperty;
            _actualPropertyType = actualPropertyType;
#if WINDOWS_PHONE || WINDOWS_WPF
            _typeConverter = _actualPropertyType.TypeConverter();
#endif
            // if we return TwoWay for ImageSource then we end up in 
            // problems with WP7 not doing the auto-conversion
            // see some of my angst in http://stackoverflow.com/questions/16752242/how-does-xaml-create-the-string-to-bitmapimage-value-conversion-when-binding-to/16753488#16753488
            // Note: if we discover other issues here, then we should make a more flexible solution
            if (_actualPropertyType == typeof (ImageSource))
            {
                _defaultMode = MvxBindingMode.OneWay;
            }

            SubscribeToChanges(targetName);
        }

        private void SubscribeToChanges(string targetName)
        {
            var frameworkElement = Target as FrameworkElement;
            if (frameworkElement == null)
                return;

#if WINDOWS_PHONE || WINDOWS_WPF
            var listenerBinding = new System.Windows.Data.Binding(targetName)
                {Source = frameworkElement};
#endif
#if NETFX_CORE
            var listenerBinding = new Windows.UI.Xaml.Data.Binding()
                {
                    Path = new PropertyPath(targetName),
                    Source = frameworkElement
                };
#endif
            var attachedProperty = DependencyProperty.RegisterAttached(
                "ListenAttached" + targetName + Guid.NewGuid().ToString("N")
                , typeof (object)
                , typeof (FrameworkElement)
                , new PropertyMetadata(null, (s, e) => FireValueChanged(e.NewValue)));
            frameworkElement.SetBinding(attachedProperty, listenerBinding);
        }

        public override Type TargetType
        {
            get { return _actualPropertyType; }
        }

        private MvxBindingMode _defaultMode = MvxBindingMode.TwoWay;
        public override MvxBindingMode DefaultMode
        {
            get { return _defaultMode; }
        }

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

        public override void SetValue(object value)
        {
            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Receiving setValue to " + (value ?? ""));
            var target = Target as FrameworkElement;
            if (target == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Weak Target is null in {0} - skipping set", GetType().Name);
                return;
            }

            var safeValue = MakeSafeValue(value);

            // to prevent feedback loops, we don't pass on 'same value' updates from the source while we are updating it
            if (_isUpdatingSource
                && safeValue.Equals(_updatingSourceWith))
                return;

            try
            {
                _isUpdatingTarget = true;
                target.SetValue(_targetDependencyProperty, safeValue);
            }
            finally
            {
                _isUpdatingTarget = false;
            }
        }

        protected virtual object MakeSafeValue(object value)
        {
#if WINDOWS_PHONE || WINDOWS_WPF
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
#if NETFX_CORE
            return _actualPropertyType.MakeSafeValue(value);
#endif
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