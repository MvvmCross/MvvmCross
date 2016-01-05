// MvxFullBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings
{
    using System;

    using MvvmCross.Binding.Bindings.SourceSteps;
    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.IoC;
    using MvvmCross.Platform.Platform;

    public class MvxFullBinding
        : MvxBinding
          , IMvxUpdateableBinding
    {
        private IMvxSourceStepFactory SourceStepFactory => MvxBindingSingletonCache.Instance.SourceStepFactory;

        private IMvxTargetBindingFactory TargetBindingFactory => MvxBindingSingletonCache.Instance.TargetBindingFactory;

        private readonly MvxBindingDescription _bindingDescription;
        private IMvxSourceStep _sourceStep;
        private IMvxTargetBinding _targetBinding;

        private object _dataContext;
        private EventHandler _sourceBindingOnChanged;
        private EventHandler<MvxTargetChangedEventArgs> _targetBindingOnValueChanged;

        public object DataContext
        {
            get { return this._dataContext; }
            set
            {
                if (this._dataContext == value)
                    return;

                this._dataContext = value;
                if (this._sourceStep != null)
                    this._sourceStep.DataContext = value;
                this.UpdateTargetOnBind();
            }
        }

        public MvxFullBinding(MvxBindingRequest bindingRequest)
        {
            this._bindingDescription = bindingRequest.Description;
            this.CreateTargetBinding(bindingRequest.Target);
            this.CreateSourceBinding(bindingRequest.Source);
        }

        protected virtual void ClearSourceBinding()
        {
            if (this._sourceStep != null)
            {
                if (this._sourceBindingOnChanged != null)
                {
                    this._sourceStep.Changed -= this._sourceBindingOnChanged;
                    this._sourceBindingOnChanged = null;
                }

                this._sourceStep.Dispose();
                this._sourceStep = null;
            }
        }

        private void CreateSourceBinding(object source)
        {
            this._dataContext = source;
            this._sourceStep = this.SourceStepFactory.Create(this._bindingDescription.Source);
            this._sourceStep.TargetType = this._targetBinding.TargetType;
            this._sourceStep.DataContext = source;

            if (this.NeedToObserveSourceChanges)
            {
                this._sourceBindingOnChanged = (sender, args) =>
                    {
                        var value = this._sourceStep.GetValue();
                        this.UpdateTargetFromSource(value);
                    };
                this._sourceStep.Changed += this._sourceBindingOnChanged;
            }

            this.UpdateTargetOnBind();
        }

        private void UpdateTargetOnBind()
        {
            if (this.NeedToUpdateTargetOnBind && this._sourceStep != null)
            {
                // note that we expect Bind to be called on the UI thread - so no need to use RunOnUIThread here

                try
                {
                    var currentValue = this._sourceStep.GetValue();
                    this.UpdateTargetFromSource(currentValue);
                }
                catch (Exception exception)
                {
                    MvxBindingTrace.Trace("Exception masked in UpdateTargetOnBind {0}", exception.ToLongString());
                }
            }
        }

        protected virtual void ClearTargetBinding()
        {
            if (this._targetBinding != null)
            {
                if (this._targetBindingOnValueChanged != null)
                {
                    this._targetBinding.ValueChanged -= this._targetBindingOnValueChanged;
                    this._targetBindingOnValueChanged = null;
                }

                this._targetBinding.Dispose();
                this._targetBinding = null;
            }
        }

        private void CreateTargetBinding(object target)
        {
            this._targetBinding = this.TargetBindingFactory.CreateBinding(target, this._bindingDescription.TargetName);

            if (this._targetBinding == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Failed to create target binding for {0}", this._bindingDescription.ToString());
                this._targetBinding = new MvxNullTargetBinding();
            }

            if (this.NeedToObserveTargetChanges)
            {
                this._targetBinding.SubscribeToEvents();
                this._targetBindingOnValueChanged = (sender, args) => this.UpdateSourceFromTarget(args.Value);
                this._targetBinding.ValueChanged += this._targetBindingOnValueChanged;
            }
        }

        private void UpdateTargetFromSource(
            object value)
        {
            if (value == MvxBindingConstant.DoNothing)
                return;

            if (value == MvxBindingConstant.UnsetValue)
                value = this._targetBinding.TargetType.CreateDefault();

            try
            {
                this._targetBinding.SetValue(value);
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Error,
                    "Problem seen during binding execution for {0} - problem {1}",
                    this._bindingDescription.ToString(),
                    exception.ToLongString());
            }
        }

        private void UpdateSourceFromTarget(
            object value)
        {
            if (value == MvxBindingConstant.DoNothing)
                return;

            if (value == MvxBindingConstant.UnsetValue)
                return;

            try
            {
                this._sourceStep.SetValue(value);
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Error,
                    "Problem seen during binding execution for {0} - problem {1}",
                    this._bindingDescription.ToString(),
                    exception.ToLongString());
            }
        }

        protected bool NeedToObserveSourceChanges
        {
            get
            {
                var mode = this.ActualBindingMode;
                return mode.RequireSourceObservation();
            }
        }

        protected bool NeedToObserveTargetChanges
        {
            get
            {
                var mode = this.ActualBindingMode;
                return mode.RequiresTargetObservation();
            }
        }

        protected bool NeedToUpdateTargetOnBind
        {
            get
            {
                var bindingMode = this.ActualBindingMode;
                return bindingMode.RequireTargetUpdateOnFirstBind();
            }
        }

        protected MvxBindingMode ActualBindingMode
        {
            get
            {
                var mode = this._bindingDescription.Mode;
                if (mode == MvxBindingMode.Default)
                    mode = this._targetBinding.DefaultMode;
                return mode;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                this.ClearTargetBinding();
                this.ClearSourceBinding();
            }
        }
    }
}