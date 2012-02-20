using System;
using System.ComponentModel;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public abstract class MvxBasePropertyInfoSourceBinding : MvxBaseSourceBinding
    {
        private readonly string _propertyName;
        private readonly PropertyInfo _propertyInfo;

        protected string PropertyName { get { return _propertyName; } }
        protected PropertyInfo PropertyInfo { get { return _propertyInfo; } }

        protected MvxBasePropertyInfoSourceBinding(object source, string propertyName)
            : base(source)
        {
            _propertyName = propertyName;

            if (Source == null)
            {
                MvxBindingTrace.Trace(
                    MvxBindingTraceLevel.Warning,                 
                    "Unable to bind to source is null"
                                  , propertyName);
                return;
            }

            _propertyInfo = source.GetType().GetProperty(propertyName);
            if (_propertyInfo == null)
            {
                MvxBindingTrace.Trace(
                    MvxBindingTraceLevel.Warning,
                    "Unable to bind: source property source not found {0} on {1}"
                                  , propertyName,
                                  source.GetType().Name);
            }

            var sourceNotify = Source as INotifyPropertyChanged;
            if (sourceNotify != null)
                sourceNotify.PropertyChanged += new PropertyChangedEventHandler(SourcePropertyChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var sourceNotify = Source as INotifyPropertyChanged;
                if (sourceNotify != null)
                    sourceNotify.PropertyChanged -= SourcePropertyChanged;
            }
        }

        private void SourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PropertyName)
                OnBoundPropertyChanged();            
        }

        protected abstract void OnBoundPropertyChanged();
    }
}