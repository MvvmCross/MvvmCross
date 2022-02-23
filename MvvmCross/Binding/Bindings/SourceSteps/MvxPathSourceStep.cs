// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding.Bindings.Source;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Converters;

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxPathSourceStep : MvxSourceStep<MvxPathSourceStepDescription>
    {
        private IMvxSourceBinding _sourceBinding;

        private readonly object _sourceLocker = new object();

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
            lock (_sourceLocker)
            {
                if (_sourceBinding != null)
                {
                    _sourceBinding.Changed -= SourceBindingOnChanged;
                    _sourceBinding.Dispose();
                    _sourceBinding = null;
                }
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
