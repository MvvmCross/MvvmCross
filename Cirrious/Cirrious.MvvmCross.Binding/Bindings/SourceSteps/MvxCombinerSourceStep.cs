// MvxCombinerSourceStep.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Binding.Bindings.Source;

namespace Cirrious.MvvmCross.Binding.Bindings.SourceSteps
{
    public class MvxCombinerSourceStep : MvxSourceStep<MvxCombinerSourceStepDescription>
    {
        private readonly List<IMvxSourceStep> _subSteps;

        public MvxCombinerSourceStep(MvxCombinerSourceStepDescription description)
            : base(description)
        {
            var sourceStepFactory = MvxBindingSingletonCache.Instance.SourceStepFactory;
            _subSteps = description.InnerSteps
                                   .Select(d => sourceStepFactory.Create(d))
                                   .ToList();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                UnsubscribeFromChangedEvents();
                foreach (var step in _subSteps)
                {
                    step.Dispose();
                }
            }

            base.Dispose(isDisposing);
        }

        protected override void OnFirstChangeListenerAdded()
        {
            SubscribeToChangedEvents();
            base.OnFirstChangeListenerAdded();
        }

        public override Type TargetType
        {
            get { return base.TargetType; }
            set
            {
                base.TargetType = value;
                SetSubTypeTargetTypes();
            }
        }

        private void SetSubTypeTargetTypes()
        {
            var targetTypes = Description.Combiner.SubStepTargetTypes(_subSteps, TargetType);
            var targetTypeList = targetTypes.ToList();
            if (targetTypeList.Count != _subSteps.Count)
                throw new MvxException("Description.Combiner provided incorrect length TargetType list");

            for (var i = 0; i < targetTypeList.Count; i++)
            {
                _subSteps[i].TargetType = targetTypeList[i];
            }
        }

        private bool _isSubscribeToChangedEvents;

        private void SubscribeToChangedEvents()
        {
            if (_isSubscribeToChangedEvents)
                return;

            foreach (var subStep in _subSteps)
            {
                subStep.Changed += SubStepOnChanged;
            }
            _isSubscribeToChangedEvents = true;
        }

        protected override void OnLastChangeListenerRemoved()
        {
            UnsubscribeFromChangedEvents();
            base.OnLastChangeListenerRemoved();
        }

        private void UnsubscribeFromChangedEvents()
        {
            if (!_isSubscribeToChangedEvents)
                return;

            foreach (var subStep in _subSteps)
            {
                subStep.Changed -= SubStepOnChanged;
            }
            _isSubscribeToChangedEvents = false;
        }

        private void SubStepOnChanged(object sender, EventArgs args)
        {
            SendSourcePropertyChanged();
        }

        protected override void OnDataContextChanged()
        {
            foreach (var step in _subSteps)
            {
                step.DataContext = DataContext;
            }
            base.OnDataContextChanged();
        }

        public override Type SourceType => Description.Combiner.SourceType(_subSteps);

        protected override void SetSourceValue(object sourceValue)
        {
            if (sourceValue == MvxBindingConstant.UnsetValue)
                return;

            if (sourceValue == MvxBindingConstant.DoNothing)
                return;

            Description.Combiner.SetValue(_subSteps, sourceValue);
        }

        protected override object GetSourceValue()
        {
            object value;
            if (!Description.Combiner.TryGetValue(_subSteps, out value))
                value = MvxBindingConstant.UnsetValue;

            return value;
        }
    }
}