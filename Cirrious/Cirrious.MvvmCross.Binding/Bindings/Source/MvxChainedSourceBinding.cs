// MvxChainedSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_currentChildBinding != null)
                {
                    _currentChildBinding.Dispose();
                    _currentChildBinding = null;
                }
            }

            base.Dispose(isDisposing);
        }

        private IMvxSourceBindingFactory SourceBindingFactory
        {
            get { return this.GetService(); }
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
                _currentChildBinding.Dispose();
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

        private void ChildSourceBindingChanged(object sender, MvxSourcePropertyBindingEventArgs e)
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
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "SetValue ignored in binding - target property path missing");
                return;
            }

            _currentChildBinding.SetValue(value);
        }
    }
}