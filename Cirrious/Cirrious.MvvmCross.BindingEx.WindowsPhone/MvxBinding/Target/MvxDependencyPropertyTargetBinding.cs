// MvxDependencyPropertyTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;

namespace Cirrious.MvvmCross.BindingEx.WindowsPhone.MvxBinding.Target
{
    public class MvxDependencyPropertyTargetBinding : MvxTargetBinding
    {
        private readonly DependencyProperty _targetDependencyProperty;

        private bool _isUpdatingSource;
        private bool _isUpdatingTarget;
        private object _updatingSourceWith;

        public MvxDependencyPropertyTargetBinding(object target, string targetName,
                                                  DependencyProperty targetDependencyProperty)
            : base(target)
        {
            _targetDependencyProperty = targetDependencyProperty;
            SubscribeToChanges(targetName);
        }

        private void SubscribeToChanges(string targetName)
        {
            var frameworkElement = Target as FrameworkElement;
            if (frameworkElement == null)
                return;

            var listenerBinding = new System.Windows.Data.Binding(targetName)
                {Source = frameworkElement};
            var attachedProperty = DependencyProperty.RegisterAttached(
                "ListenAttached" + targetName + Guid.NewGuid().ToString("N")
                , typeof (object)
                , typeof (FrameworkElement)
                , new PropertyMetadata((s, e) => FireValueChanged(e.NewValue)));
            frameworkElement.SetBinding(attachedProperty, listenerBinding);
        }

        public override Type TargetType
        {
            get { return typeof (object); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
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
            // TODO - can we get the type - not sure this is possible here?
            //var safeValue = _targetDependencyProperty.PropertyType.MakeSafeValue(value);
            return value;
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