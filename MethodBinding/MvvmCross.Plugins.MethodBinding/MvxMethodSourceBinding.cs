// MvxMethodSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings.Source;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace MvvmCross.Plugins.MethodBinding
{
    public class MvxMethodSourceBinding
        : MvxSourceBinding
          , ICommand
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