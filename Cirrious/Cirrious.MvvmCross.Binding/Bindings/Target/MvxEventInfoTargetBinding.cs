// MvxEventInfoTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using System.Windows.Input;
using Cirrious.CrossCore;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public class MvxEventInfoTargetBinding<T> : MvxTargetBinding
        where T : EventArgs
    {
        private readonly EventInfo _targetEventInfo;

        private ICommand _currentCommand;

        public MvxEventInfoTargetBinding(object target, EventInfo targetEventInfo)
            : base(target)
        {
            _targetEventInfo = targetEventInfo;

            // 	addMethod is used because of error:
            // "Attempting to JIT compile method '(wrapper delegate-invoke) <Module>:invoke_void__this___UIControl_EventHandler (MonoTouch.UIKit.UIControl,System.EventHandler)' while running with --aot-only."
            // see https://bugzilla.xamarin.com/show_bug.cgi?id=3682

            var addMethod = _targetEventInfo.GetAddMethod();
            addMethod.Invoke(target, new object[] {new EventHandler<T>(HandleEvent)});
        }

        public override Type TargetType
        {
            get { return typeof (ICommand); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var target = Target;
                if (target != null)
                {
                    _targetEventInfo.GetRemoveMethod().Invoke(target, new object[] {new EventHandler<T>(HandleEvent)});
                }
            }
        }

        private void HandleEvent(object sender, T args)
        {
            if (_currentCommand != null)
                _currentCommand.Execute(null);
        }

        public override void SetValue(object value)
        {
            var command = value as ICommand;
            _currentCommand = command;
        }
    }
}