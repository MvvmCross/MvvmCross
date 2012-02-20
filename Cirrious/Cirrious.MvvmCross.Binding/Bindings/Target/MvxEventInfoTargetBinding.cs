using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public class MvxEventInfoTargetBinding<T> : MvxBaseTargetBinding
        where T : EventArgs
    {
        private readonly object _target;
        private readonly EventInfo _targetEventInfo;

        private IMvxCommand _currentCommand;

        public MvxEventInfoTargetBinding(object target, EventInfo targetEventInfo)
        {
            _target = target;
            _targetEventInfo = targetEventInfo;
            _targetEventInfo.AddEventHandler(_target, new EventHandler<T>(HandleEvent));
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
                _targetEventInfo.RemoveEventHandler(_target, new EventHandler<T>(HandleEvent));
        }

        private void HandleEvent(object sender, T args)
        {
            if (_currentCommand != null)
                _currentCommand.Execute();
        }

        public override Type TargetType
        {
            get { return typeof (IMvxCommand); }
        }

        public override void SetValue(object value)
        {
            var command = value as IMvxCommand;
            _currentCommand = command;
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }
    }
}