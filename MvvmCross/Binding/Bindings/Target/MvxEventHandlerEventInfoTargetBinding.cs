// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Windows.Input;

namespace MvvmCross.Binding.Bindings.Target
{
    public class MvxEventHandlerEventInfoTargetBinding : MvxTargetBinding
    {
        private readonly EventInfo _targetEventInfo;

        private ICommand _currentCommand;
        private readonly object _eventHandler;

        public MvxEventHandlerEventInfoTargetBinding(object target, EventInfo targetEventInfo)
            : base(target)
        {
            _targetEventInfo = targetEventInfo;

            // 	addMethod is used because of error:
            // "Attempting to JIT compile method '(wrapper delegate-invoke) <Module>:invoke_void__this___UIControl_EventHandler (UIKit.UIControl,System.EventHandler)' while running with --aot-only."
            // see https://bugzilla.xamarin.com/show_bug.cgi?id=3682
            var addMethod = _targetEventInfo.GetAddMethod();

            // we only handle EventHandler's here
            // EventHandler<T> event types will need to be handled by custom bindings
            _eventHandler = new EventHandler(HandleEvent);

            addMethod.Invoke(target, new[] { _eventHandler });
        }

        public override Type TargetValueType => typeof(ICommand);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var target = Target;
                if (target != null)
                {
                    var removeMethod = _targetEventInfo.GetRemoveMethod();
                    removeMethod.Invoke(target, new[] { _eventHandler });
                }
            }

            base.Dispose(isDisposing);
        }

        private void HandleEvent(object sender, EventArgs args)
        {
            _currentCommand?.Execute(null);
        }

        public override void SetValue(object value)
        {
            var command = value as ICommand;
            _currentCommand = command;
        }
    }
}
