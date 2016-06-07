// MvxChainedSourceBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source.Chained
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MvvmCross.Binding.Bindings.Source.Construction;
    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Platform;

    public abstract class MvxChainedSourceBinding
        : MvxPropertyInfoSourceBinding
    {
        private readonly IList<MvxPropertyToken> _childTokens;
        private IMvxSourceBinding _currentChildBinding;

        protected MvxChainedSourceBinding(
            object source,
            PropertyInfo propertyInfo,
            IList<MvxPropertyToken> childTokens)
            : base(source, propertyInfo)
        {
            this._childTokens = childTokens;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (this._currentChildBinding != null)
                {
                    this._currentChildBinding.Changed -= this.ChildSourceBindingChanged;
                    this._currentChildBinding.Dispose();
                    this._currentChildBinding = null;
                }
            }

            base.Dispose(isDisposing);
        }

        private IMvxSourceBindingFactory SourceBindingFactory => MvxBindingSingletonCache.Instance.SourceBindingFactory;

        public override Type SourceType
        {
            get
            {
                if (this._currentChildBinding == null)
                    return typeof(object);

                return this._currentChildBinding.SourceType;
            }
        }

        protected void UpdateChildBinding()
        {
            if (this._currentChildBinding != null)
            {
                this._currentChildBinding.Changed -= this.ChildSourceBindingChanged;
                this._currentChildBinding.Dispose();
                this._currentChildBinding = null;
            }

            if (this.PropertyInfo == null)
            {
                return;
            }

            var currentValue = this.PropertyInfo.GetValue(this.Source, this.PropertyIndexParameters());
            if (currentValue == null)
            {
                // value will be missing... so end consumer will need to use fallback values
                return;
            }
            else
            {
                this._currentChildBinding = this.SourceBindingFactory.CreateBinding(currentValue, this._childTokens);
                this._currentChildBinding.Changed += this.ChildSourceBindingChanged;
            }
        }

        protected abstract object[] PropertyIndexParameters();

        private void ChildSourceBindingChanged(object sender, EventArgs e)
        {
            this.FireChanged();
        }

        protected override void OnBoundPropertyChanged()
        {
            this.UpdateChildBinding();
            this.FireChanged();
        }

        public override object GetValue()
        {
            if (this._currentChildBinding == null)
            {
                return MvxBindingConstant.UnsetValue;
            }

            return this._currentChildBinding.GetValue();
        }

        public override void SetValue(object value)
        {
            if (this._currentChildBinding == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "SetValue ignored in binding - target property path missing");
                return;
            }

            this._currentChildBinding.SetValue(value);
        }
    }
}