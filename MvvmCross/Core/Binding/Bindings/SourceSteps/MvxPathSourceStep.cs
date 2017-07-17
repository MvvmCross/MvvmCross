// MvxPathSourceStep.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Binding.Bindings.Source;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Binding.Bindings.SourceSteps
{
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
                ClearPathSourceBinding();
            }

            base.Dispose(isDisposing);
        }

        public override Type SourceType
        {
            get
            {
                if (_sourceBinding == null)
                    return typeof(object);

                return _sourceBinding.SourceType;
            }
        }

        //TODO: optim: dont recreate the source binding on each datacontext change, as SourcePropertyPath does not change.
        //TODO: optim: don't subscribe to the Changed event if the binding mode does not need it.
        protected override void OnDataContextChanged()
        {
            ClearPathSourceBinding();
            _sourceBinding = SourceBindingFactory.CreateBinding(DataContext, Description.SourcePropertyPath);
            if (_sourceBinding != null)
            {
                _sourceBinding.Changed += SourceBindingOnChanged;
            }
            base.OnDataContextChanged();
        }

        private void ClearPathSourceBinding()
        {
            if (_sourceBinding != null)
            {
                _sourceBinding.Changed -= SourceBindingOnChanged;
                _sourceBinding.Dispose();
                _sourceBinding = null;
            }
        }

        private void SourceBindingOnChanged(object sender, EventArgs args)
        {
            SendSourcePropertyChanged();
        }

        protected override void SetSourceValue(object sourceValue)
        {
            if (_sourceBinding == null)
                return;

            if (sourceValue == MvxBindingConstant.UnsetValue)
                return;

            if (sourceValue == MvxBindingConstant.DoNothing)
                return;

            _sourceBinding.SetValue(sourceValue);
        }

        protected override object GetSourceValue()
        {
            if (_sourceBinding == null)
            {
                return MvxBindingConstant.UnsetValue;
            }

            return _sourceBinding.GetValue();
        }
    }
}