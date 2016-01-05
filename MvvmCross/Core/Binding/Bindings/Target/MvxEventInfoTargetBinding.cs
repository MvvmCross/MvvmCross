// MvxEventInfoTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target
{
    using System;
    using System.Reflection;
    using System.Windows.Input;

    using MvvmCross.Platform;

    public class MvxEventInfoTargetBinding<T> : MvxTargetBinding
        where T : EventArgs
    {
        private readonly EventInfo _targetEventInfo;

        private ICommand _currentCommand;

        public MvxEventInfoTargetBinding(object target, EventInfo targetEventInfo)
            : base(target)
        {
            this._targetEventInfo = targetEventInfo;

            // 	addMethod is used because of error:
            // "Attempting to JIT compile method '(wrapper delegate-invoke) <Module>:invoke_void__this___UIControl_EventHandler (UIKit.UIControl,System.EventHandler)' while running with --aot-only."
            // see https://bugzilla.xamarin.com/show_bug.cgi?id=3682

            var addMethod = this._targetEventInfo.GetAddMethod();
            addMethod.Invoke(target, new object[] { new EventHandler<T>(this.HandleEvent) });
        }

        public override Type TargetType => typeof(ICommand);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var target = this.Target;
                if (target != null)
                {
                    this._targetEventInfo.GetRemoveMethod().Invoke(target, new object[] { new EventHandler<T>(this.HandleEvent) });
                }
            }
        }

        private void HandleEvent(object sender, T args)
        {
            this._currentCommand?.Execute(null);
        }

        public override void SetValue(object value)
        {
            var command = value as ICommand;
            this._currentCommand = command;
        }
    }
}