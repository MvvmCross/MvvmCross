// MvxPathSourceStep.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.Bindings.Source;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;

namespace Cirrious.MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxPathSourceStep : MvxSourceStep<MvxPathSourceStepDescription>
    {
        private IMvxSourceBinding _sourceBinding;

        public MvxPathSourceStep(MvxPathSourceStepDescription description)
            : base(description)
        {
        }

        private IMvxSourceBindingFactory SourceBindingFactory
        {
            get { return MvxBindingSingletonCache.Instance.SourceBindingFactory; }
        }

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
                    return typeof (object);

                return _sourceBinding.SourceType;
            }
        }

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

        private void SourceBindingOnChanged(object sender, MvxSourcePropertyBindingEventArgs args)
        {
            base.SendSourcePropertyChanged(args.IsAvailable, args.Value);
        }

        protected override void SetSourceValue(object sourceValue)
        {
            if (_sourceBinding == null)
                return;

            _sourceBinding.SetValue(sourceValue);
        }

        protected override bool TryGetSourceValue(out object value)
        {
            if (_sourceBinding == null)
            {
                value = null;
                return false;
            }

            return _sourceBinding.TryGetValue(out value);
        }
    }
}