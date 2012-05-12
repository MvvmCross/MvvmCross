#region Copyright
// <copyright file="MvxFullBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Globalization;
using System.Threading;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public class MvxFullBinding
        : MvxBaseBinding
        , IMvxUpdateableBinding
        , IMvxServiceConsumer<IMvxTargetBindingFactory>
        , IMvxServiceConsumer<IMvxSourceBindingFactory>       
    {
        private IMvxSourceBindingFactory SourceBindingFactory
        {
            get { return this.GetService<IMvxSourceBindingFactory>(); }
        }

        private IMvxTargetBindingFactory TargetBindingFactory
        {
            get { return this.GetService<IMvxTargetBindingFactory>(); }
        }

        private MvxBindingDescription _bindingDescription;
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

        private void ClearSourceBinding()
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
            _targetBinding = TargetBindingFactory.CreateBinding(target, _bindingDescription);

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
            catch (ThreadAbortException)
            {
                throw;
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
            catch (ThreadAbortException)
            {
                throw;
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

        private static void UpdateSourceFromTarget(MvxBindingRequest bindingRequest, IMvxSourceBinding sourceBinding, object value)
        {
            try
            {
                if (bindingRequest.Description.Converter != null)
                    value =
                        bindingRequest.Description.Converter.ConvertBack(value,
                                                            sourceBinding.SourceType,
                                                            bindingRequest.Description.ConverterParameter,
                                                            CultureInfo.CurrentUICulture);
                sourceBinding.SetValue(value);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(
                                        MvxTraceLevel.Error,                 

                    "Problem seen during binding execution for {0} - problem {1}",
                    bindingRequest.ToString(),
                    exception.ToLongString());
            }
        }

        protected bool NeedToObserveSourceChanges
        {
            get
            {
                switch (ActualBindingMode)
                {
                    case MvxBindingMode.Default:
                        MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Mode of default seen for binding - assuming OneWay");
                        return true;
                    case MvxBindingMode.OneWay:
                    case MvxBindingMode.TwoWay:
                        return true;
                    case MvxBindingMode.OneTime:
                    case MvxBindingMode.OneWayToSource:
                        return false;

                    default:
                        throw new MvxException("Unexpected ActualBindingMode");
                }
            }
        }

        protected bool NeedToObserveTargetChanges
        {
            get
            {
                switch (ActualBindingMode)
                {
                    case MvxBindingMode.Default:
                        MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Mode of default seen for binding - assuming OneWay");
                        return true;
                    case MvxBindingMode.OneWay:
                    case MvxBindingMode.OneTime:
                        return false;
                    case MvxBindingMode.TwoWay:
                    case MvxBindingMode.OneWayToSource:
                        return true;

                    default:
                        throw new MvxException("Unexpected ActualBindingMode");
                }
            }
        }

        protected bool NeedToUpdateTargetOnBind
        {
            get
            {
                switch (ActualBindingMode)
                {
                    case MvxBindingMode.Default:
                        MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Mode of default seen for binding - assuming OneWay");
                        return true;
                    case MvxBindingMode.OneWay:
                    case MvxBindingMode.OneTime:
                    case MvxBindingMode.TwoWay:
                        return true;
                    case MvxBindingMode.OneWayToSource:
                        return false;

                    default:
                        throw new MvxException("Unexpected ActualBindingMode");
                }
            }
        }

        private MvxBindingMode ActualBindingMode
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