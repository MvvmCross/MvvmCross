using System;
using System.Reflection;
using System.Windows.Input;

namespace MvvmCross.Binding.Bindings.Target
{
    public class MvxEventNameTargetBinding<TEventArgs> : MvxTargetBinding
    {
        private readonly EventInfo _targetEventInfo;
        private readonly bool _useEventArgsAsCommandParameter;

        private ICommand _currentCommand;

        public MvxEventNameTargetBinding(object target, string targetEventName, bool useEventArgsAsCommandParameter = true) : base(target)
        {
            _targetEventInfo = target.GetType().GetTypeInfo().GetDeclaredEvent(targetEventName);
            _useEventArgsAsCommandParameter = useEventArgsAsCommandParameter;

            //  addMethod is used because of error:
            // "Attempting to JIT compile method '(wrapper delegate-invoke) <Module>:invoke_void__this___UIControl_EventHandler (UIKit.UIControl,System.EventHandler)' while running with --aot-only."
            // see https://bugzilla.xamarin.com/show_bug.cgi?id=3682
            var addMethod = _targetEventInfo.AddMethod;
            addMethod.Invoke(target, new object[] { new EventHandler<TEventArgs>(HandleEvent) });
        }

        public override Type TargetType { get; } = typeof(ICommand);

        public override MvxBindingMode DefaultMode { get; } = MvxBindingMode.OneWay;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var target = Target;
                if (target != null)
                {
                    var removeMethod = _targetEventInfo.RemoveMethod;
                    removeMethod.Invoke(target, new object[] { new EventHandler<TEventArgs>(HandleEvent) });
                }
            }

            base.Dispose(isDisposing);
        }

        private void HandleEvent(object sender, TEventArgs parameter)
        {
            var commandParameter = _useEventArgsAsCommandParameter ? (object)parameter : null;

            if (_currentCommand != null && _currentCommand.CanExecute(commandParameter))
                _currentCommand.Execute(commandParameter);
        }

        public override void SetValue(object value)
        {
            var command = value as ICommand;
            _currentCommand = command;
        }
    }
}
