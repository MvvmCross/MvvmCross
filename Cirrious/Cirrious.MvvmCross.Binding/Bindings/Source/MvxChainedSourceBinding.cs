#region Copyright
// <copyright file="MvxChainedSourceBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public class MvxChainedSourceBinding 
        : MvxBasePropertyInfoSourceBinding
        , IMvxServiceConsumer<IMvxSourceBindingFactory>
    {
        private readonly List<string> _childPropertyNames;
        private IMvxSourceBinding _currentChildBinding;

        public MvxChainedSourceBinding(
            object source, 
            string propertyName,
            IEnumerable<string> childPropertyNames) 
            : base(source, propertyName)
        {
            _childPropertyNames = childPropertyNames.ToList();

            UpdateChildBinding();
        }

        private IMvxSourceBindingFactory SourceBindingFactory
        {
            get { return this.GetService<IMvxSourceBindingFactory>(); }
        }

        public override Type SourceType
        {
            get { throw new NotImplementedException(); }
        }

        private void UpdateChildBinding()
        {
            if (_currentChildBinding != null)
            {
                _currentChildBinding.Changed -= ChildSourceBindingChanged;
                _currentChildBinding = null;
            }

            if (PropertyInfo == null)
            {
                return;
            }

            var currentValue = PropertyInfo.GetValue(Source, null);
            if (currentValue == null)
            {
                // value will be missing... so end consumer will need to use fallback values
                return;
            }
            else
            {
                _currentChildBinding = SourceBindingFactory.CreateBinding(currentValue, _childPropertyNames);
                _currentChildBinding.Changed += ChildSourceBindingChanged;
            }
        }

        void ChildSourceBindingChanged(object sender, MvxSourcePropertyBindingEventArgs e)
        {
            FireChanged(e);
        }

        protected override void OnBoundPropertyChanged()
        {
            UpdateChildBinding();
            FireChanged(new MvxSourcePropertyBindingEventArgs(this));
        }

        public override bool TryGetValue(out object value)
        {
            if (_currentChildBinding == null)
            {
                value = null;
                return false;
            }

            return _currentChildBinding.TryGetValue(out value);
        }

        public override void SetValue(object value)
        {
            if (_currentChildBinding == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "SetValue ignored in binding - target property path missing");
                return;
            }

            _currentChildBinding.SetValue(value);
        }
    }
}