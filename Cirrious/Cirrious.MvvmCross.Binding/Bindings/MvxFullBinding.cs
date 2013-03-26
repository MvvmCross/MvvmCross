// MvxFullBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings.Source;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;

namespace Cirrious.MvvmCross.Binding.Bindings
{
    public class MvxFullBinding
        : MvxBinding
          , IMvxUpdateableBinding
    {
        private IMvxSourceBindingFactory SourceBindingFactory
        {
            get { return MvxBindingSingletonCache.Instance.SourceBindingFactory; }
        }

        private IMvxTargetBindingFactory TargetBindingFactory
        {
            get { return MvxBindingSingletonCache.Instance.TargetBindingFactory; }
        }

        private readonly MvxBindingDescription _bindingDescription;
        private IMvxSourceBinding _sourceBinding;
        private IMvxTargetBinding _targetBinding;

        private object _dataContext;

        public object DataContext
        {
            get { return _dataContext; }
            set
            {
                if (_dataContext == value)
                    return;

                ClearSourceBinding();
                CreateSourceBinding(value);
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
            if (_sourceBinding != null)
            {
                _sourceBinding.Dispose();
                _sourceBinding = null;
            }
        }

        private void CreateSourceBinding(object source)
        {
            _dataContext = source;
            _sourceBinding = SourceBindingFactory.CreateBinding(source, _bindingDescription.SourcePropertyPath);

            if (NeedToObserveSourceChanges)
                _sourceBinding.Changed += (sender, args) => UpdateTargetFromSource(args.IsAvailable, args.Value);

            if (NeedToUpdateTargetOnBind)
            {
                // note that we expect Bind to be called on the UI thread - so no need to use RunOnUIThread here
                object currentValue;
                bool currentIsAvailable = _sourceBinding.TryGetValue(out currentValue);
                UpdateTargetFromSource(currentIsAvailable, currentValue);
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
                _targetBinding.ValueChanged += (sender, args) => UpdateSourceFromTarget(args.Value);
            }
        }

        private void UpdateTargetFromSource(
            bool isAvailable,
            object value)
        {
            try
            {
                if (isAvailable)
                {
                    if (_bindingDescription.Converter != null)
                        value =
                            _bindingDescription.Converter.Convert(value,
                                                                  _targetBinding.TargetType,
                                                                  _bindingDescription.ConverterParameter,
                                                                  CultureInfo.CurrentUICulture);
                }
                else
                {
                    value = _bindingDescription.FallbackValue;
                }
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
            try
            {
                if (_bindingDescription.Converter != null)
                    value =
                        _bindingDescription.Converter.ConvertBack(value,
                                                                  _sourceBinding.SourceType,
                                                                  _bindingDescription.ConverterParameter,
                                                                  CultureInfo.CurrentUICulture);
                _sourceBinding.SetValue(value);
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
                if (_targetBinding != null)
                    _targetBinding.Dispose();
                if (_sourceBinding != null)
                    _sourceBinding.Dispose();
            }
        }
    }
}