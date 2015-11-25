// MvxChainedNotifyChangeFieldSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Source;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using System;
using System.Collections.Generic;

namespace MvvmCross.Plugins.FieldBinding
{
    public class MvxChainedNotifyChangeFieldSourceBinding
        : MvxNotifyChangeFieldSourceBinding
    {
        public static bool DisableWarnIndexedValueBindingWarning = false;

        private readonly List<MvxPropertyToken> _childTokens;
        private IMvxSourceBinding _currentChildBinding;

        public MvxChainedNotifyChangeFieldSourceBinding(object source, INotifyChange notifyChange,
                                                        List<MvxPropertyToken> childTokens)
            : base(source, notifyChange)
        {
            _childTokens = childTokens;
            if (!DisableWarnIndexedValueBindingWarning)
                WarnIfChildTokensSuspiciousOfIndexedValueBinding();
            UpdateChildBinding();
        }

        private void WarnIfChildTokensSuspiciousOfIndexedValueBinding()
        {
            if (_childTokens == null || _childTokens.Count < 2)
                return;

            var firstAsName = _childTokens[0] as MvxPropertyNamePropertyToken;
            if (firstAsName == null || firstAsName.PropertyName != "Value")
                return;

            var secondAsIndexed = _childTokens[1] as MvxIndexerPropertyToken;
            if (secondAsIndexed == null)
                return;

            MvxBindingTrace.Warning("Suspicious indexed binding seen to Value[] within INC binding - this may be OK, but is often a result of FluentBinding used on INC<T> - consider using INCList<TValue> or INCDictionary<TKey,TValue> instead - see https://github.com/slodge/MvvmCross/issues/353. This message can be disabled using DisableWarnIndexedValueBindingWarning");
        }

        protected override void NotifyChangeOnChanged(object sender, EventArgs eventArgs)
        {
            UpdateChildBinding();
            FireChanged();
        }

        private IMvxSourceBindingFactory SourceBindingFactory => MvxBindingSingletonCache.Instance.SourceBindingFactory;

        public override Type SourceType
        {
            get
            {
                if (_currentChildBinding == null)
                    return typeof(object);

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

            if (NotifyChange == null)
            {
                return;
            }

            var currentValue = NotifyChange.Value;
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

        private void ChildSourceBindingChanged(object sender, EventArgs e)
        {
            FireChanged();
        }

        public override object GetValue()
        {
            if (_currentChildBinding == null)
            {
                return MvxBindingConstant.UnsetValue;
            }

            return _currentChildBinding.GetValue();
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
    }
}