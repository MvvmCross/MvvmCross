﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Extensions;

namespace MvvmCross.Platform.Uap.Binding.MvxBinding.Target
{
    public class MvxDependencyPropertyTargetBinding : MvxConvertingTargetBinding
    {
        private readonly string _targetName;
        private readonly DependencyProperty _targetDependencyProperty;
        private readonly Type _actualPropertyType;

        public MvxDependencyPropertyTargetBinding(object target, string targetName, DependencyProperty targetDependencyProperty, Type actualPropertyType)
            : base(target)
        {
            _targetDependencyProperty = targetDependencyProperty;
            _actualPropertyType = actualPropertyType;
            _targetName = targetName;

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

            var listenerBinding = new Windows.UI.Xaml.Data.Binding()
            {
                Path = new PropertyPath(_targetName),
                Source = frameworkElement
            };

            var attachedProperty = DependencyProperty.RegisterAttached(
                "ListenAttached" + _targetName + Guid.NewGuid().ToString("N"), 
                typeof(object), 
                typeof(FrameworkElement), 
                new PropertyMetadata(null, (s, e) => FireValueChanged(e.NewValue)));

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
                MvxBindingLog.Warning("Weak Target is null in {0} - skipping Get", GetType().Name);
                return null;
            }

            return target.GetValue(_targetDependencyProperty);
        }

        protected override void SetValueImpl(object target, object value)
        {
            MvxBindingLog.Trace("Receiving setValue to " + (value ?? ""));
            var frameworkElement = target as FrameworkElement;
            if (frameworkElement == null)
            {
                MvxBindingLog.Trace("Weak Target is null in {0} - skipping set", GetType().Name);
                return;
            }

            frameworkElement.SetValue(_targetDependencyProperty, value);
        }

        protected override object MakeSafeValue(object value)
        {
            return _actualPropertyType.MakeSafeValue(value);
        }
    }
}
