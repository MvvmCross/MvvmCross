// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Binding.Bindings.Source
{
    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public abstract class MvxPropertyInfoSourceBinding : MvxSourceBinding
    {
        private IDisposable _subscription;

        protected MvxPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
            : base(source)
        {
            PropertyInfo = propertyInfo;
            PropertyName = propertyInfo.Name;

            if (Source == null)
            {
                MvxBindingLog.Instance?.LogTrace(
                    "Unable to bind to source as it's null. PropertyName: {PropertyName}", PropertyName);
                return;
            }

            if (Source is INotifyPropertyChanged sourceNotify)
                _subscription = sourceNotify.WeakSubscribe(SourcePropertyChanged);
        }

        protected string PropertyName { get; }
        protected PropertyInfo PropertyInfo { get; }

        protected string PropertyNameForChangedEvent
        {
            get
            {
                if (IsIndexedProperty)
                    return PropertyName + "[]";
                else
                    return PropertyName;
            }
        }

        protected bool IsIndexedProperty
        {
            get
            {
                var parameters = PropertyInfo.GetIndexParameters();
                return parameters.Length != 0;
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
            {
                OnBoundPropertyChanged();
            }
        }

        protected abstract void OnBoundPropertyChanged();
    }
}
