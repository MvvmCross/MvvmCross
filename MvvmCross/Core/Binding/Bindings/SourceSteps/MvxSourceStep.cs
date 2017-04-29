// MvxSourceStep.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public abstract class MvxSourceStep
        : IMvxSourceStep
    {
        private object _dataContext;

        protected MvxSourceStep(MvxSourceStepDescription description)
        {
            Description = description;
        }

        protected MvxSourceStepDescription Description { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual Type TargetType { get; set; }

        public virtual Type SourceType => typeof(object);

        public object DataContext
        {
            get => _dataContext;
            set
            {
                if (_dataContext == value)
                    return;

                _dataContext = value;
                OnDataContextChanged();
            }
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

        public object GetValue()
        {
            var sourceValue = GetSourceValue();
            var value = ConvertSourceToTarget(sourceValue);
            return value;
        }

        protected virtual void Dispose(bool isDisposing)
        {
            // nothing to do in the base class
        }

        protected virtual void OnDataContextChanged()
        {
            // nothing to do in the base class
        }

        private object ApplyValueConverterTargetToSource(object value)
        {
            if (Description.Converter == null)
                return value;

            return Description.Converter.ConvertBack(value,
                SourceType,
                Description.ConverterParameter,
                CultureInfo.CurrentUICulture);
        }

        private object ApplyValueConverterSourceToTarget(object value)
        {
            if (Description.Converter == null)
                return value;

            try
            {
                return
                    Description.Converter.Convert(value,
                        TargetType,
                        Description.ConverterParameter,
                        CultureInfo.CurrentUICulture);
            }
            catch (Exception exception)
            {
                // pokemon exception - force the use of Fallback in this case
                // we expect this exception to occur sometimes - so only "Diagnostic" level logging here
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Diagnostic,
                    "Problem seen during binding execution for {0} - problem {1}",
                    Description.ToString(),
                    exception.ToLongString());
            }

            return MvxBindingConstant.UnsetValue;
        }

        protected abstract void SetSourceValue(object sourceValue);

        protected virtual void SendSourcePropertyChanged()
        {
            _changed?.Invoke(this, EventArgs.Empty);
        }

        private object ConvertSourceToTarget(object value)
        {
            if (value == MvxBindingConstant.DoNothing)
                return value;

            if (value != MvxBindingConstant.UnsetValue)
                value = ApplyValueConverterSourceToTarget(value);

            if (value != MvxBindingConstant.UnsetValue)
                return value;

            if (Description.FallbackValue != null)
                return Description.FallbackValue;

            return MvxBindingConstant.UnsetValue;
        }

        private event EventHandler _changed;

        protected virtual void OnLastChangeListenerRemoved()
        {
            // base class does nothing by default
        }

        protected virtual void OnFirstChangeListenerAdded()
        {
            // base class does nothing by default
        }

        protected abstract object GetSourceValue();
    }

    public abstract class MvxSourceStep<T> : MvxSourceStep
        where T : MvxSourceStepDescription
    {
        protected MvxSourceStep(T description)
            : base(description)
        {
        }

        protected new T Description => (T) base.Description;
    }
}