// MvxChainedSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Chained
{
    public abstract class MvxChainedSourceBinding
        : MvxPropertyInfoSourceBinding
    {
        private readonly IList<MvxPropertyToken> _childTokens;
        private IMvxSourceBinding _currentChildBinding;

        protected MvxChainedSourceBinding(
            object source,
            string propertyName,
            IList<MvxPropertyToken> childTokens)
            : base(source, propertyName)
        {
            _childTokens = childTokens;
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
            get { return MvxBindingSingletonCache.Instance.SourceBindingFactory; }
        }

        public override Type SourceType
        {
            get
            {
                if (_currentChildBinding == null)
                    return typeof (object);

                return _currentChildBinding.SourceType;
            }
        }

        protected void UpdateChildBinding()
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

            var currentValue = PropertyInfo.GetValue(Source, PropertyIndexParameters());
            if (currentValue == null)
            {
                // value will be missing... so end consumer will need to use fallback values
                return;
            }
            else
            {
                _currentChildBinding = SourceBindingFactory.CreateBinding(currentValue, _childTokens);
                _currentChildBinding.Changed += ChildSourceBindingChanged;
            }
        }

        protected abstract object[] PropertyIndexParameters();

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