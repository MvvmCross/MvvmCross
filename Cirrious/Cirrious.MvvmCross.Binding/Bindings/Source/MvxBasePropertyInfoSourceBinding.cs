// MvxBasePropertyInfoSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public abstract class MvxBasePropertyInfoSourceBinding : MvxBaseSourceBinding
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly string _propertyName;

        protected MvxBasePropertyInfoSourceBinding(object source, string propertyName)
            : base(source)
        {
            _propertyName = propertyName;

            if (Source == null)
            {
                MvxBindingTrace.Trace(
                    // this is not a Warning - as actually using a NULL source is a fairly common occurrence!
                    MvxTraceLevel.Diagnostic,
                    "Unable to bind to source as it's null"
                    , propertyName);
                return;
            }

            _propertyInfo = source.GetType().GetProperty(propertyName);
            if (_propertyInfo == null)
            {
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Warning,
                    "Unable to bind: source property source not found {0} on {1}"
                    , propertyName,
                    source.GetType().Name);
            }

            var sourceNotify = Source as INotifyPropertyChanged;
            if (sourceNotify != null)
                sourceNotify.PropertyChanged += SourcePropertyChanged;
        }

        protected string PropertyName
        {
            get { return _propertyName; }
        }

        protected string PropertyNameForChangedEvent
        {
            get
            {
                if (IsIndexedProperty)
                    return _propertyName + "[]";
                else
                    return _propertyName;
            }
        }

        protected PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
        }

        protected bool IsIndexedProperty
        {
            get
            {
                var parameters = _propertyInfo.GetIndexParameters();
                return parameters.Any();
            }
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