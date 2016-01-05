// MvxCombinerSourceStep.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Exceptions;

    public class MvxCombinerSourceStep : MvxSourceStep<MvxCombinerSourceStepDescription>
    {
        private readonly List<IMvxSourceStep> _subSteps;

        public MvxCombinerSourceStep(MvxCombinerSourceStepDescription description)
            : base(description)
        {
            var sourceStepFactory = MvxBindingSingletonCache.Instance.SourceStepFactory;
            this._subSteps = description.InnerSteps
                                   .Select(d => sourceStepFactory.Create(d))
                                   .ToList();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                this.UnsubscribeFromChangedEvents();
                foreach (var step in this._subSteps)
                {
                    step.Dispose();
                }
            }

            base.Dispose(isDisposing);
        }

        protected override void OnFirstChangeListenerAdded()
        {
            this.SubscribeToChangedEvents();
            base.OnFirstChangeListenerAdded();
        }

        public override Type TargetType
        {
            get { return base.TargetType; }
            set
            {
                base.TargetType = value;
                this.SetSubTypeTargetTypes();
            }
        }

        private void SetSubTypeTargetTypes()
        {
            var targetTypes = this.Description.Combiner.SubStepTargetTypes(this._subSteps, this.TargetType);
            var targetTypeList = targetTypes.ToList();
            if (targetTypeList.Count != this._subSteps.Count)
                throw new MvxException("Description.Combiner provided incorrect length TargetType list");

            for (var i = 0; i < targetTypeList.Count; i++)
            {
                this._subSteps[i].TargetType = targetTypeList[i];
            }
        }

        private bool _isSubscribeToChangedEvents;

        private void SubscribeToChangedEvents()
        {
            if (this._isSubscribeToChangedEvents)
                return;

            foreach (var subStep in this._subSteps)
            {
                subStep.Changed += this.SubStepOnChanged;
            }
            this._isSubscribeToChangedEvents = true;
        }

        protected override void OnLastChangeListenerRemoved()
        {
            this.UnsubscribeFromChangedEvents();
            base.OnLastChangeListenerRemoved();
        }

        private void UnsubscribeFromChangedEvents()
        {
            if (!this._isSubscribeToChangedEvents)
                return;

            foreach (var subStep in this._subSteps)
            {
                subStep.Changed -= this.SubStepOnChanged;
            }
            this._isSubscribeToChangedEvents = false;
        }

        private void SubStepOnChanged(object sender, EventArgs args)
        {
            this.SendSourcePropertyChanged();
        }

        protected override void OnDataContextChanged()
        {
            foreach (var step in this._subSteps)
            {
                step.DataContext = this.DataContext;
            }
            base.OnDataContextChanged();
        }

        public override Type SourceType => this.Description.Combiner.SourceType(this._subSteps);

        protected override void SetSourceValue(object sourceValue)
        {
            if (sourceValue == MvxBindingConstant.UnsetValue)
                return;

            if (sourceValue == MvxBindingConstant.DoNothing)
                return;

            this.Description.Combiner.SetValue(this._subSteps, sourceValue);
        }

        protected override object GetSourceValue()
        {
            object value;
            if (!this.Description.Combiner.TryGetValue(this._subSteps, out value))
                value = MvxBindingConstant.UnsetValue;

            return value;
        }
    }
}