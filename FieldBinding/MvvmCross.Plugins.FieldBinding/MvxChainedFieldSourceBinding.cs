// MvxChainedFieldSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Source;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MvvmCross.Plugins.FieldBinding
{
    public class MvxChainedFieldSourceBinding
        : MvxFieldSourceBinding
    {
        private readonly IList<MvxPropertyToken> _childTokens;
        private IMvxSourceBinding _currentChildBinding;

        public MvxChainedFieldSourceBinding(object source, FieldInfo fieldInfo, IList<MvxPropertyToken> childTokens)
            : base(source, fieldInfo)
        {
            _childTokens = childTokens;
            UpdateChildBinding();
        }

        private void UpdateChildBinding()
        {
            if (_currentChildBinding != null)
            {
                _currentChildBinding.Changed -= ChildSourceBindingChanged;
                _currentChildBinding.Dispose();
                _currentChildBinding = null;
            }
            var currentValue = FieldInfo.GetValue(Source);
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

        private IMvxSourceBindingFactory SourceBindingFactory
        {
            get { return MvxBindingSingletonCache.Instance.SourceBindingFactory; }
        }

        private void ChildSourceBindingChanged(object sender, EventArgs e)
        {
            FireChanged();
        }

        public override void SetValue(object value)
        {
            if (_currentChildBinding == null)
                return;

            _currentChildBinding.SetValue(value);
        }

        public override Type SourceType
        {
            get
            {
                if (_currentChildBinding == null)
                    return typeof(object);

                return _currentChildBinding.SourceType;
            }
        }

        public override object GetValue()
        {
            if (_currentChildBinding == null)
            {
                return MvxBindingConstant.UnsetValue;
            }
            return _currentChildBinding.GetValue();
        }
    }
}