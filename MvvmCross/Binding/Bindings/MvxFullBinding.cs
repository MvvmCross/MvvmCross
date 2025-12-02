// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Converters;
using MvvmCross.IoC;

[assembly: InternalsVisibleTo("MvvmCross.UnitTest")]

namespace MvvmCross.Binding.Bindings
{
    [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    public class MvxFullBinding
        : MvxBinding, IMvxUpdateableBinding
    {
#if NET9_0_OR_GREATER
        private readonly Lock _lock = new();
#else
        private readonly object _lock = new();
#endif
        private readonly MvxBindingDescription _bindingDescription;
        private readonly object _defaultTargetValue;

        private IMvxSourceStep _sourceStep;
        private IMvxTargetBinding _targetBinding;
        private object _dataContext;
        private CancellationTokenSource _cancelSource = new();

        public object DataContext
        {
            get => _dataContext;
            set
            {
                if (_dataContext == value)
                    return;

                _dataContext = value;

                lock (_lock)
                {
                    _sourceStep?.DataContext = value;
                }

                UpdateTargetOnBind();
            }
        }

        public MvxFullBinding(MvxBindingRequest bindingRequest)
        {
            _dataContext = bindingRequest.Source;
            _bindingDescription = bindingRequest.Description;
            _targetBinding = CreateTargetBinding(bindingRequest);
            _defaultTargetValue = _targetBinding.TargetValueType.CreateDefault();
            _sourceStep = CreateSourceBinding(bindingRequest);

            UpdateTargetOnBind();
        }

        protected virtual void ClearSourceBinding()
        {
            lock (_lock)
            {
                if (_sourceStep != null)
                {
                    _sourceStep.Changed -= OnSourceBindingChanged;
                    _sourceStep.Dispose();
                }

                _sourceStep = null;
            }
        }

        private IMvxSourceStep CreateSourceBinding(MvxBindingRequest bindingRequest)
        {
            var sourceStep = MvxBindingSingletonCache.Instance.SourceStepFactory.Create(bindingRequest.Description.Source);
            sourceStep.TargetType = _targetBinding.TargetValueType;
            sourceStep.DataContext = bindingRequest.Source;

            if (NeedToObserveSourceChanges)
            {
                sourceStep.Changed += OnSourceBindingChanged;
            }

            return sourceStep;
        }

        private void OnSourceBindingChanged(object sender, EventArgs e)
        {
            var value = _sourceStep.GetValue();
            CancellationToken cancel;
            lock (_lock)
            {
                cancel = _cancelSource.Token;
            }
            UpdateTargetFromSource(value, cancel);
        }

        private void UpdateTargetOnBind()
        {
            if (NeedToUpdateTargetOnBind && _sourceStep != null)
            {
                CancellationToken cancel;
                lock (_lock)
                {
                    _cancelSource.Cancel();
                    _cancelSource.Dispose();
                    _cancelSource = new CancellationTokenSource();
                    cancel = _cancelSource.Token;
                }

                try
                {
                    var currentValue = _sourceStep.GetValue();
                    UpdateTargetFromSource(currentValue, cancel);
                }
                catch (Exception exception)
                {
                    MvxBindingLog.Instance?.LogTrace(exception, "Exception masked in UpdateTargetOnBind");
                }
            }
        }

        protected virtual void ClearTargetBinding()
        {
            lock (_lock)
            {
                if (_targetBinding != null)
                {
                    _targetBinding.ValueChanged -= UpdateSourceFromTarget;
                    _targetBinding.Dispose();
                    _targetBinding = null;
                }
            }
        }

        private IMvxTargetBinding CreateTargetBinding(MvxBindingRequest request)
        {
            var binding = MvxBindingSingletonCache.Instance.TargetBindingFactory.CreateBinding(request.Target, request.Description.TargetName);

            if (binding == null)
            {
                MvxBindingLog.Instance?.LogWarning("Failed to create target binding for {BindingDescription}", request.Description.ToString());
                binding = new MvxNullTargetBinding();
            }

            if (NeedToObserveTargetChanges)
            {
                binding.SubscribeToEvents();
                binding.ValueChanged += UpdateSourceFromTarget;
            }

            return binding;
        }

        private async void UpdateTargetFromSource(object value, CancellationToken cancel)
        {
            if (value == MvxBindingConstant.DoNothing || cancel.IsCancellationRequested)
                return;

            if (value == MvxBindingConstant.UnsetValue)
            {
                lock (_lock)
                {
                    value = _defaultTargetValue;
                }
            }

            await MvxBindingSingletonCache.Instance.MainThreadDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                if (cancel.IsCancellationRequested)
                    return;

                try
                {
                    lock (_lock)
                    {
                        _targetBinding?.SetValue(value);
                    }
                }
                catch (Exception exception)
                {
                    MvxBindingLog.Instance?.LogError(
                        exception,
                        "Problem seen during binding execution for {BindingDescription}",
                        _bindingDescription.ToString());
                }
            });
        }

        private void UpdateSourceFromTarget(object sender, MvxTargetChangedEventArgs args)
        {
            if (args.Value == MvxBindingConstant.DoNothing)
                return;

            if (args.Value == MvxBindingConstant.UnsetValue)
                return;

            try
            {
                lock (_lock)
                {
                    _sourceStep?.SetValue(args.Value);
                }
            }
            catch (Exception exception)
            {
                MvxBindingLog.Instance?.LogError(
                    exception,
                    "Problem seen during binding execution for {BindingDescription}",
                    _bindingDescription.ToString());
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

        protected internal MvxBindingMode ActualBindingMode
        {
            get
            {
                lock (_lock)
                {
                    var mode = _bindingDescription.Mode;
                    if (mode == MvxBindingMode.Default && _targetBinding != null)
                        mode = _targetBinding.DefaultMode;
                    return mode;
                }
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                ClearTargetBinding();
                ClearSourceBinding();

                lock (_lock)
                {
                    _cancelSource?.Cancel();
                    _cancelSource?.Dispose();
                    _cancelSource = null;
                }
            }

            base.Dispose(isDisposing);
        }
    }
}
