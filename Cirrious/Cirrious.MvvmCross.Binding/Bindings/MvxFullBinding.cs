// MvxFullBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;

namespace Cirrious.MvvmCross.Binding.Bindings
{
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
            get { return _dataContext; }
            set
            {
                if (_dataContext == value)
                    return;

                _dataContext = value;
                if (_sourceStep != null)
                    _sourceStep.DataContext = value;
                UpdateTargetOnBind();
            }
        }

        public MvxFullBinding(MvxBindingRequest bindingRequest)
        {
            _bindingDescription = bindingRequest.Description;
            CreateTargetBinding(bindingRequest.Target);
            CreateSourceBinding(bindingRequest.Source);
        }

        protected virtual void ClearSourceBinding()
        {
            if (_sourceStep != null)
            {
                if (_sourceBindingOnChanged != null)
                {
                    _sourceStep.Changed -= _sourceBindingOnChanged;
                    _sourceBindingOnChanged = null;
                }

                _sourceStep.Dispose();
                _sourceStep = null;
            }
        }

        private void CreateSourceBinding(object source)
        {
            _dataContext = source;
            _sourceStep = SourceStepFactory.Create(_bindingDescription.Source);
            _sourceStep.TargetType = _targetBinding.TargetType;
            _sourceStep.DataContext = source;

            if (NeedToObserveSourceChanges)
            {
                _sourceBindingOnChanged = (sender, args) =>
                    {
                        var value = _sourceStep.GetValue();
                        UpdateTargetFromSource(value);
                    };
                _sourceStep.Changed += _sourceBindingOnChanged;
            }

            UpdateTargetOnBind();
        }

        private void UpdateTargetOnBind()
        {
            if (NeedToUpdateTargetOnBind && _sourceStep != null)
            {
                // note that we expect Bind to be called on the UI thread - so no need to use RunOnUIThread here

                try
                {
                    var currentValue = _sourceStep.GetValue();
                    UpdateTargetFromSource(currentValue);
                }
                catch (Exception exception)
                {
                    MvxBindingTrace.Trace("Exception masked in UpdateTargetOnBind {0}", exception.ToLongString());
                }
            }
        }


        protected virtual void ClearTargetBinding()
        {
            if (_targetBinding != null)
            {
                if (_targetBindingOnValueChanged != null)
                {
                    _targetBinding.ValueChanged -= _targetBindingOnValueChanged;
                    _targetBindingOnValueChanged = null;
                }

                _targetBinding.Dispose();
                _targetBinding = null;
            }
        }

        private void CreateTargetBinding(object target)
        {
            _targetBinding = TargetBindingFactory.CreateBinding(target, _bindingDescription.TargetName);

            if (_targetBinding == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Failed to create target binding for {0}", _bindingDescription.ToString());
                _targetBinding = new MvxNullTargetBinding();
            }

            if (NeedToObserveTargetChanges)
            {
                _targetBinding.SubscribeToEvents();
                _targetBindingOnValueChanged = (sender, args) => UpdateSourceFromTarget(args.Value);
                _targetBinding.ValueChanged += _targetBindingOnValueChanged;
            }
        }

        private void UpdateTargetFromSource(
            object value)
        {
            if (value == MvxBindingConstant.DoNothing)
                return;

            if (value == MvxBindingConstant.UnsetValue)
                value = _targetBinding.TargetType.CreateDefault();

            try
            {
                _targetBinding.SetValue(value);
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Error,
                    "Problem seen during binding execution for {0} - problem {1}",
                    _bindingDescription.ToString(),
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
                _sourceStep.SetValue(value);
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Error,
                    "Problem seen during binding execution for {0} - problem {1}",
                    _bindingDescription.ToString(),
                    exception.ToLongString());
            }
        }

        protected bool NeedToObserveSourceChanges
        {
            get
            {
                var mode = ActualBindingMode;
                return mode.RequireSourceObservation();
            }
        }

        protected bool NeedToObserveTargetChanges
        {
            get
            {
                var mode = ActualBindingMode;
                return mode.RequiresTargetObservation();
            }
        }

        protected bool NeedToUpdateTargetOnBind
        {
            get
            {
                var bindingMode = ActualBindingMode;
                return bindingMode.RequireTargetUpdateOnFirstBind();
            }
        }

        protected MvxBindingMode ActualBindingMode
        {
            get
            {
                var mode = _bindingDescription.Mode;
                if (mode == MvxBindingMode.Default)
                    mode = _targetBinding.DefaultMode;
                return mode;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                ClearTargetBinding();
                ClearSourceBinding();
            }
        }
    }
}