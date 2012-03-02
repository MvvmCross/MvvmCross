#region Copyright
// <copyright file="MvxEventHandlerEventInfoTargetBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public class MvxEventHandlerEventInfoTargetBinding : MvxBaseTargetBinding
    {
        private readonly object _target;
        private readonly EventInfo _targetEventInfo;

        private IMvxCommand _currentCommand;

        public MvxEventHandlerEventInfoTargetBinding(object target, EventInfo targetEventInfo)
        {
            _target = target;
            _targetEventInfo = targetEventInfo;
            _targetEventInfo.AddEventHandler(_target, new EventHandler(HandleEvent));
        }

        public override Type TargetType
        {
            get { return typeof (IMvxCommand); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
                _targetEventInfo.RemoveEventHandler(_target, new EventHandler(HandleEvent));
        }

        private void HandleEvent(object sender, EventArgs args)
        {
            if (_currentCommand != null)
                _currentCommand.Execute();
        }

        public override void SetValue(object value)
        {
            var command = value as IMvxCommand;
            _currentCommand = command;
        }
    }
}