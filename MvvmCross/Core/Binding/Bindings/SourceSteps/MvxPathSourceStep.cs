// MvxPathSourceStep.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    using System;

    using MvvmCross.Binding.Bindings.Source;
    using MvvmCross.Binding.Bindings.Source.Construction;
    using MvvmCross.Platform.Converters;

    public class MvxPathSourceStep : MvxSourceStep<MvxPathSourceStepDescription>
    {
        private IMvxSourceBinding _sourceBinding;

        public MvxPathSourceStep(MvxPathSourceStepDescription description)
            : base(description)
        {
        }

        private IMvxSourceBindingFactory SourceBindingFactory => MvxBindingSingletonCache.Instance.SourceBindingFactory;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                this.ClearPathSourceBinding();
            }

            base.Dispose(isDisposing);
        }

        public override Type SourceType
        {
            get
            {
                if (this._sourceBinding == null)
                    return typeof(object);

                return this._sourceBinding.SourceType;
            }
        }

        protected override void OnDataContextChanged()
        {
            this.ClearPathSourceBinding();
            this._sourceBinding = this.SourceBindingFactory.CreateBinding(this.DataContext, this.Description.SourcePropertyPath);
            if (this._sourceBinding != null)
            {
                this._sourceBinding.Changed += this.SourceBindingOnChanged;
            }
            base.OnDataContextChanged();
        }

        private void ClearPathSourceBinding()
        {
            if (this._sourceBinding != null)
            {
                this._sourceBinding.Changed -= this.SourceBindingOnChanged;
                this._sourceBinding.Dispose();
                this._sourceBinding = null;
            }
        }

        private void SourceBindingOnChanged(object sender, EventArgs args)
        {
            base.SendSourcePropertyChanged();
        }

        protected override void SetSourceValue(object sourceValue)
        {
            if (this._sourceBinding == null)
                return;

            if (sourceValue == MvxBindingConstant.UnsetValue)
                return;

            if (sourceValue == MvxBindingConstant.DoNothing)
                return;

            this._sourceBinding.SetValue(sourceValue);
        }

        protected override object GetSourceValue()
        {
            if (this._sourceBinding == null)
            {
                return MvxBindingConstant.UnsetValue;
            }

            return this._sourceBinding.GetValue();
        }
    }
}