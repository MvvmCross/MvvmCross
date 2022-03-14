// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    public class MvxUITextFieldShouldReturnTargetBinding
        : MvxTargetBinding
    {
        private ICommand _command;

        protected UITextField View => Target as UITextField;

        public MvxUITextFieldShouldReturnTargetBinding(UITextField target)
            : base(target)
        {
            target.ShouldReturn = HandleShouldReturn;
        }

        private bool HandleShouldReturn(UITextField textField)
        {
            if (_command == null)
                return false;

            var text = textField.Text;
            if (!_command.CanExecute(text))
                return false;

            textField.ResignFirstResponder();
            _command.Execute(text);
            return true;
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override void SetValue(object value)
        {
            var command = value as ICommand;
            _command = command;
        }

        public override Type TargetValueType => typeof(ICommand);

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var editText = View;
                if (editText != null)
                {
                    editText.ShouldReturn = null;
                }
            }
        }
    }
}
