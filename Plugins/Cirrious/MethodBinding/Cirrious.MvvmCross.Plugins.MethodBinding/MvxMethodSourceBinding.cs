﻿// MvxMethodSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.Bindings.Source;

namespace Cirrious.MvvmCross.Plugins.MethodBinding
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
                throw  new ArgumentNullException("source");
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");
            _methodInfo = methodInfo;
        }

        public override void SetValue(object value)
        {
            // not possible
        }

        public override Type SourceType
        {
            get { return typeof(ICommand); }
        }

        public override bool TryGetValue(out object value)
        {
            throw new NotImplementedException();
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