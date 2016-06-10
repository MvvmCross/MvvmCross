// MvxSourceStep.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    using System;
    using System.Globalization;

    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public abstract class MvxSourceStep
        : IMvxSourceStep
    {
        private readonly MvxSourceStepDescription _description;
        private object _dataContext;

        protected MvxSourceStepDescription Description => this._description;

        protected MvxSourceStep(MvxSourceStepDescription description)
        {
            this._description = description;
        }

        public void Dispose()
        {
            this.Dispose(true);
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
            get { return this._dataContext; }
            set
            {
                this._dataContext = value;
                this.OnDataContextChanged();
            }
        }

        protected virtual void OnDataContextChanged()
        {
            // nothing to do in the base class
        }

        public void SetValue(object value)
        {
            var sourceValue = this.ApplyValueConverterTargetToSource(value);

            if (sourceValue == MvxBindingConstant.DoNothing)
                return;

            if (sourceValue == MvxBindingConstant.UnsetValue)
                return;

            this.SetSourceValue(sourceValue);
        }

        private object ApplyValueConverterTargetToSource(object value)
        {
            if (this._description.Converter == null)
                return value;

            return this._description.Converter.ConvertBack(value,
                                                      this.SourceType,
                                                      this._description.ConverterParameter,
                                                      CultureInfo.CurrentUICulture);
        }

        private object ApplyValueConverterSourceToTarget(object value)
        {
            if (this._description.Converter == null)
            {
                return value;
            }

            try
            {
                return
                    this._description.Converter.Convert(value,
                                                   this.TargetType,
                                                   this._description.ConverterParameter,
                                                   CultureInfo.CurrentUICulture);
            }
            catch (Exception exception)
            {
                // pokemon exception - force the use of Fallback in this case
                // we expect this exception to occur sometimes - so only "Diagnostic" level logging here
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Diagnostic,
                    "Problem seen during binding execution for {0} - problem {1}",
                    this._description.ToString(),
                    exception.ToLongString());
            }

            return MvxBindingConstant.UnsetValue;
        }

        protected abstract void SetSourceValue(object sourceValue);

        protected virtual void SendSourcePropertyChanged()
        {
            this._changed?.Invoke(this, EventArgs.Empty);
        }

        private object ConvertSourceToTarget(object value)
        {
            if (value == MvxBindingConstant.DoNothing)
                return value;

            if (value != MvxBindingConstant.UnsetValue)
            {
                value = this.ApplyValueConverterSourceToTarget(value);
            }

            if (value != MvxBindingConstant.UnsetValue)
            {
                return value;
            }

            if (this._description.FallbackValue != null)
                return this._description.FallbackValue;

            return MvxBindingConstant.UnsetValue;
        }

        private event EventHandler _changed;

        public event EventHandler Changed
        {
            add
            {
                var alreadyHasListeners = this._changed != null;
                this._changed += value;
                if (!alreadyHasListeners)
                    this.OnFirstChangeListenerAdded();
            }
            remove
            {
                this._changed -= value;
                if (this._changed == null)
                    this.OnLastChangeListenerRemoved();
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
            var sourceValue = this.GetSourceValue();
            var value = this.ConvertSourceToTarget(sourceValue);
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