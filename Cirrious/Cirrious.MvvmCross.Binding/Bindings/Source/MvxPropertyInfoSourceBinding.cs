// MvxPropertyInfoSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding.ExtensionMethods;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public abstract class MvxPropertyInfoSourceBinding : MvxSourceBinding
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly string _propertyName;
        private IDisposable _subscription;

        protected MvxPropertyInfoSourceBinding(object source, string propertyName)
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
                _subscription = sourceNotify.WeakSubscribe(SourcePropertyChanged);
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
                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
            }
        }

        // Note - this is public because we use it in weak referenced situations
        public void SourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // we test for null or empty here - this means all properties have changed
            // - fix for https://github.com/slodge/MvvmCross/issues/280 
            if (string.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName == PropertyName)
                OnBoundPropertyChanged();
        }

        protected abstract void OnBoundPropertyChanged();
    }
}