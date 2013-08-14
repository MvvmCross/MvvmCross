// MvxSourceStep.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Source;

namespace Cirrious.MvvmCross.Binding.Bindings.SourceSteps
{
    public abstract class MvxSourceStep
        : IMvxSourceStep
    {
        private readonly MvxSourceStepDescription _description;
        private object _dataContext;

        protected MvxSourceStepDescription Description
        {
            get { return _description; }
        }

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

        public virtual Type SourceType
        {
            get { return typeof (object); }
        }

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

        private bool TryApplyValueConverterSourceToTarget(object value, out object result)
        {
            if (_description.Converter == null)
            {
                result = value;
                return true;
            }

            try
            {
                result =
                    _description.Converter.Convert(value,
                                                   TargetType,
                                                   _description.ConverterParameter,
                                                   CultureInfo.CurrentUICulture);
                return true;
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

            result = null;
            return false;
        }

        protected abstract void SetSourceValue(object sourceValue);

        protected virtual void SendSourcePropertyChanged(bool isAvailable, object value)
        {
            var handler = _changed;
            if (handler == null)
                return;

            var valueToSend = ConvertSourceToTarget(isAvailable, value);
            handler.Invoke(this, new MvxSourcePropertyBindingEventArgs(isAvailable, valueToSend));
        }

        private object ConvertSourceToTarget(bool isAvailable, object value)
        {
            if (isAvailable)
            {
                if (TryApplyValueConverterSourceToTarget(value, out value))
                {
                    return value;
                }
            }

            if (_description.FallbackValue != null)
                return _description.FallbackValue;

            return TargetType.CreateDefault();
        }

        private event EventHandler<MvxSourcePropertyBindingEventArgs> _changed;

        public event EventHandler<MvxSourcePropertyBindingEventArgs> Changed
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

        public bool TryGetValue(out object value)
        {
            object sourceValue;
            var exists = TryGetSourceValue(out sourceValue);
            value = ConvertSourceToTarget(exists, sourceValue);
            return exists;
        }

        protected abstract bool TryGetSourceValue(out object value);
    }

    public abstract class MvxSourceStep<T> : MvxSourceStep
        where T : MvxSourceStepDescription
    {
        protected new T Description
        {
            get { return (T) base.Description; }
        }

        protected MvxSourceStep(T description)
            : base(description)
        {
        }
    }
}