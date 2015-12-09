// MvxEventHandlerEventInfoTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
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

    public class MvxEventHandlerEventInfoTargetBinding : MvxTargetBinding
    {
        private readonly EventInfo _targetEventInfo;

        private ICommand _currentCommand;
        private readonly object _eventHandler;

        public MvxEventHandlerEventInfoTargetBinding(object target, EventInfo targetEventInfo)
            : base(target)
        {
            this._targetEventInfo = targetEventInfo;

            // 	addMethod is used because of error:
            // "Attempting to JIT compile method '(wrapper delegate-invoke) <Module>:invoke_void__this___UIControl_EventHandler (MonoTouch.UIKit.UIControl,System.EventHandler)' while running with --aot-only."
            // see https://bugzilla.xamarin.com/show_bug.cgi?id=3682

            var addMethod = this._targetEventInfo.GetAddMethod();

            // we only handle EventHandler's here
            // EventHandler<T> event types will need to be handled by custom bindings
            this._eventHandler = new EventHandler(this.HandleEvent);

            addMethod.Invoke(target, new[] { this._eventHandler });
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
                    var removeMethod = this._targetEventInfo.GetRemoveMethod();
                    removeMethod.Invoke(target, new[] { this._eventHandler });
                }
            }
        }

        private void HandleEvent(object sender, EventArgs args)
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