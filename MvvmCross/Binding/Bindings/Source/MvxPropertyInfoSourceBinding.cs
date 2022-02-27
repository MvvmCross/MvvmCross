// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Binding.Bindings.Source
{
    public abstract class MvxPropertyInfoSourceBinding : MvxSourceBinding
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly string _propertyName;
        private IDisposable _subscription;

        protected MvxPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
            : base(source)
        {
            _propertyInfo = propertyInfo;
            _propertyName = propertyInfo.Name;

            if (Source == null)
            {
                MvxBindingLog.Trace(
                    "Unable to bind to source as it's null"
                    , _propertyName);
                return;
            }

            var sourceNotify = Source as INotifyPropertyChanged;
            if (sourceNotify != null)
                _subscription = sourceNotify.WeakSubscribe(SourcePropertyChanged);
        }

        protected string PropertyName => _propertyName;

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

        protected PropertyInfo PropertyInfo => _propertyInfo;

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
                _subscription?.Dispose();
                _subscription = null;
            }
        }

        // Note - this is public because we use it in weak referenced situations
        public void SourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // we test for null or empty here - this means all properties have changed
            // - fix for https://github.com/slodge/MvvmCross/issues/280
            if (string.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName == PropertyNameForChangedEvent)
                OnBoundPropertyChanged();
        }

        protected abstract void OnBoundPropertyChanged();
    }
}
