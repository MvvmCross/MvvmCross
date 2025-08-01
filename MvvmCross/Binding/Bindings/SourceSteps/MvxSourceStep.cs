// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Extensions.Logging;
using MvvmCross.Converters;

namespace MvvmCross.Binding.Bindings.SourceSteps
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
            get
            {
                return _dataContext;
            }
            [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
            set
            {
                if (_dataContext == value)
                    return;

                _dataContext = value;
                OnDataContextChanged();
            }
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
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
                MvxBindingLog.Instance?.LogTrace(
                    exception,
                    "Problem seen during binding execution for {BindingDescription}",
                    _description.ToString());
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
