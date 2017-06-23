// MvxFullBinding.cs
//
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Bindings
{
    public class MvxFullBinding
        : MvxBinding, IMvxUpdateableBinding
    {
        private IMvxSourceStepFactory SourceStepFactory => MvxBindingSingletonCache.Instance.SourceStepFactory;

        private IMvxTargetBindingFactory TargetBindingFactory => MvxBindingSingletonCache.Instance.TargetBindingFactory;

        private readonly MvxBindingDescription _bindingDescription;
        private IMvxSourceStep _sourceStep;
        private IMvxTargetBinding _targetBinding;
        private readonly object _targetLocker = new object();

        private object _dataContext;
        private EventHandler _sourceBindingOnChanged;
        private EventHandler<MvxTargetChangedEventArgs> _targetBindingOnValueChanged;

        private object _defaultTargetValue;
        private CancellationTokenSource _cancelSource = new CancellationTokenSource();
        private IMvxMainThreadDispatcher dispatcher => MvxBindingSingletonCache.Instance.MainThreadDispatcher;

        public object DataContext
        {
            get
            {
                return _dataContext;
            }
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
            // NOTE: We do not call the setter for DataContext here because we are
            // setting up the sourceStep.
            // If that method is updated we will need to make sure that this method
            // does the right thing.
            _dataContext = source;
            _sourceStep = SourceStepFactory.Create(_bindingDescription.Source);
            _sourceStep.TargetType = _targetBinding.TargetType;
            _sourceStep.DataContext = source;

            if (NeedToObserveSourceChanges)
            {
                _sourceBindingOnChanged = (sender, args) =>
                {
                    //Capture the cancel token first
                    var cancel = _cancelSource.Token;
                    //GetValue can now be executed in a worker thread. Is it the responsibility of the caller to switch threads, or ours ?
                    //As the source is the viewmodel, i suppose it is the responsibility of the caller.
                    var value = _sourceStep.GetValue();
                    UpdateTargetFromSource(value, cancel);
                };
                _sourceStep.Changed += _sourceBindingOnChanged;
            }

            UpdateTargetOnBind();
        }

        private void UpdateTargetOnBind()
        {
            if (NeedToUpdateTargetOnBind && _sourceStep != null)
            {
                _cancelSource.Cancel();
                _cancelSource = new CancellationTokenSource();
                var cancel = _cancelSource.Token;

                try
                {
                    var currentValue = _sourceStep.GetValue();
                    UpdateTargetFromSource(currentValue, cancel);
                }
                catch (Exception exception)
                {
                    MvxBindingTrace.Trace("Exception masked in UpdateTargetOnBind {0}", exception.ToLongString());
                }
            }
        }

        protected virtual void ClearTargetBinding()
        {
            lock (_targetLocker)
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
        }

        private void CreateTargetBinding(object target)
        {
            _targetBinding = TargetBindingFactory.CreateBinding(target, _bindingDescription.TargetName);

            if (_targetBinding == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Failed to create target binding for {0}", _bindingDescription.ToString());
                _targetBinding = new MvxNullTargetBinding();
            }

            if (NeedToObserveTargetChanges)
            {
                _targetBinding.SubscribeToEvents();
                _targetBindingOnValueChanged = (sender, args) => UpdateSourceFromTarget(args.Value);
                _targetBinding.ValueChanged += _targetBindingOnValueChanged;
            }

            _defaultTargetValue = _targetBinding.TargetType.CreateDefault();
        }

        private void UpdateTargetFromSource(object value, CancellationToken cancel)
        {
            if (value == MvxBindingConstant.DoNothing || cancel.IsCancellationRequested)
                return;

            if (value == MvxBindingConstant.UnsetValue)
                value = _defaultTargetValue;

            dispatcher.RequestMainThreadAction(() =>
            {
                if (cancel.IsCancellationRequested)
                    return;

                try
                {
                    lock (_targetLocker)
                    {
                        _targetBinding?.SetValue(value);
                    }
                }
                catch (Exception exception)
                {
                    MvxBindingTrace.Trace(
                        MvxTraceLevel.Error,
                        "Problem seen during binding execution for {0} - problem {1}",
                        _bindingDescription.ToString(),
                        exception.ToLongString());
                }
            });
        }

        private void UpdateSourceFromTarget(object value)
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
                if (mode == MvxBindingMode.Default && _targetBinding != null)
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