// MvxPathSourceStep.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.Bindings.PathSource;
using Cirrious.MvvmCross.Binding.Bindings.PathSource.Construction;

namespace Cirrious.MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxPathSourceStep : MvxSourceStep<MvxPathSourceStepDescription>
    {
        private IMvxPathSourceBinding _pathSourceBinding;

        public MvxPathSourceStep(MvxPathSourceStepDescription description)
            : base(description)
        {
        }

        private IMvxPathSourceBindingFactory PathSourceBindingFactory
        {
            get { return MvxBindingSingletonCache.Instance.PathSourceBindingFactory; }
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
                if (_pathSourceBinding == null)
                    return typeof (object);

                return _pathSourceBinding.SourceType;
            }
        }

        protected override void OnDataContextChanged()
        {
            ClearPathSourceBinding();
            _pathSourceBinding = PathSourceBindingFactory.CreateBinding(DataContext, Description.SourcePropertyPath);
            if (_pathSourceBinding != null)
            {
                _pathSourceBinding.Changed += PathSourceBindingOnChanged;
            }
            base.OnDataContextChanged();
        }

        private void ClearPathSourceBinding()
        {
            if (_pathSourceBinding != null)
            {
                _pathSourceBinding.Changed -= PathSourceBindingOnChanged;
                _pathSourceBinding.Dispose();
                _pathSourceBinding = null;
            }
        }

        private void PathSourceBindingOnChanged(object sender, MvxSourcePropertyBindingEventArgs args)
        {
            base.SendSourcePropertyChanged(args.IsAvailable, args.Value);
        }

        protected override void SetSourceValue(object sourceValue)
        {
            if (_pathSourceBinding == null)
                return;

            _pathSourceBinding.SetValue(sourceValue);
        }

        protected override bool TryGetSourceValue(out object value)
        {
            if (_pathSourceBinding == null)
            {
                value = null;
                return false;
            }

            return _pathSourceBinding.TryGetValue(out value);
        }
    }
}