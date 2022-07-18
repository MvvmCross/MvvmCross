// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using MvvmCross.Binding.Bindings.Source;

namespace MvvmCross.Plugin.MethodBinding
{
    [Preserve(AllMembers = true)]
    public class MvxMethodSourceBinding
        : MvxSourceBinding, ICommand
    {
        private readonly MethodInfo _methodInfo;

        public MvxMethodSourceBinding(object source, MethodInfo methodInfo)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            _methodInfo = methodInfo;
        }

        public override void SetValue(object value)
        {
            // not possible
        }

        public override Type SourceType => typeof(ICommand);

        public override object GetValue()
        {
            return this;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            object[] parameters;
            if (_methodInfo.GetParameters().Any())
            {
                parameters = new object[]
                    {
                        parameter
                    };
            }
            else
            {
                parameters = new object[0];
            }

            _methodInfo.Invoke(Source, parameters);
        }

        public event EventHandler CanExecuteChanged;
    }
}
