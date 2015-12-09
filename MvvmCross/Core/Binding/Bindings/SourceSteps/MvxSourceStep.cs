// MvxSourceStep.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using System;
using System.Globalization;

namespace Cirrious.MvvmCross.Binding.Bindings.SourceSteps
{
    public abstract class MvxSourceStep
        : IMvxSourceStep
    {
        private readonly MvxSourceStepDescription _description;
        private object _dataContext;

        protected MvxSourceStepDescription Description => _description;

        protected MvxSourceStep(MvxSourceStepDescription description)
        {
            _description = description;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            // nothing to do in the base class
        }

        public virtual Type TargetType { get; set; }

        public virtual Type SourceType => typeof(object);

        public object DataContext
        {
            get { return _dataContext; }
            set
            {
                _dataContext = value;
                OnDataContextChanged();
            }
        }

        protected virtual void OnDataContextChanged()
        {
            // nothing to do in the base class
        }

        public void SetValue(object value)
        {
            var sourceValue = ApplyValueConverterTargetToSource(value);

            if (sourceValue == MvxBindingConstant.DoNothing)
                return;

            if (sourceValue == MvxBindingConstant.UnsetValue)
                return;

            SetSourceValue(sourceValue);
        }

        private object ApplyValueConverterTargetToSource(object value)
        {
            if (_description.Converter == null)
                return value;

            return _description.Converter.ConvertBack(value,
                                                      SourceType,
                                                      _description.ConverterParameter,
                                                      CultureInfo.CurrentUICulture);
        }

        private object ApplyValueConverterSourceToTarget(object value)
        {
            if (_description.Converter == null)
            {
                return value;
            }

            try
            {
                return
                    _description.Converter.Convert(value,
                                                   TargetType,
                                                   _description.ConverterParameter,
                                                   CultureInfo.CurrentUICulture);
            }
            catch (Exception exception)
            {
                // pokemon exception - force the use of Fallback in this case
                // we expect this exception to occur sometimes - so only "Diagnostic" level logging here
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Diagnostic,
                    "Problem seen during binding execution for {0} - problem {1}",
                    _description.ToString(),
                    exception.ToLongString());
            }

            return MvxBindingConstant.UnsetValue;
        }

        protected abstract void SetSourceValue(object sourceValue);

        protected virtual void SendSourcePropertyChanged()
        {
            var handler = _changed;

            handler?.Invoke(this, EventArgs.Empty);
        }

        private object ConvertSourceToTarget(object value)
        {
            if (value == MvxBindingConstant.DoNothing)
                return value;

            if (value != MvxBindingConstant.UnsetValue)
            {
                value = ApplyValueConverterSourceToTarget(value);
            }

            if (value != MvxBindingConstant.UnsetValue)
            {
                return value;
            }

            if (_description.FallbackValue != null)
                return _description.FallbackValue;

            return MvxBindingConstant.UnsetValue;
        }

        private event EventHandler _changed;

        public event EventHandler Changed
        {
            add
            {
                var alreadyHasListeners = _changed != null;
                _changed += value;
                if (!alreadyHasListeners)
                    OnFirstChangeListenerAdded();
            }
            remove
            {
                _changed -= value;
                if (_changed == null)
                    OnLastChangeListenerRemoved();
            }
        }

        protected virtual void OnLastChangeListenerRemoved()
        {
            // base class does nothing by default
        }

        protected virtual void OnFirstChangeListenerAdded()
        {
            // base class does nothing by default
        }

        public object GetValue()
        {
            var sourceValue = GetSourceValue();
            var value = ConvertSourceToTarget(sourceValue);
            return value;
        }

        protected abstract object GetSourceValue();
    }

    public abstract class MvxSourceStep<T> : MvxSourceStep
        where T : MvxSourceStepDescription
    {
        protected new T Description => (T)base.Description;

        protected MvxSourceStep(T description)
            : base(description)
        {
        }
    }
}