// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Source;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using MvvmCross.Converters;

namespace MvvmCross.Plugin.FieldBinding
{
    [Preserve(AllMembers = true)]
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

        private IMvxSourceBindingFactory SourceBindingFactory => MvxBindingSingletonCache.Instance.SourceBindingFactory;

        private void ChildSourceBindingChanged(object sender, EventArgs e)
        {
            FireChanged();
        }

        public override void SetValue(object value)
        {
            _currentChildBinding?.SetValue(value);
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
