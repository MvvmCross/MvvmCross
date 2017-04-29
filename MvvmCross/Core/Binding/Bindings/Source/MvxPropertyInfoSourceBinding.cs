// MvxPropertyInfoSourceBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Bindings.Source
{
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
                MvxBindingTrace.Trace(
                    // this is not a Warning - as actually using a NULL source is a fairly common occurrence!
                    MvxTraceLevel.Diagnostic,
                    "Unable to bind to source as it's null"
                    , PropertyName);
                return;
            }

            var sourceNotify = Source as INotifyPropertyChanged;
            if (sourceNotify != null)
                _subscription = sourceNotify.WeakSubscribe(SourcePropertyChanged);
        }

        protected string PropertyName { get; }

        protected string PropertyNameForChangedEvent
        {
            get
            {
                if (IsIndexedProperty)
                    return PropertyName + "[]";
                return PropertyName;
            }
        }

        protected PropertyInfo PropertyInfo { get; }

        protected bool IsIndexedProperty
        {
            get
            {
                var parameters = PropertyInfo.GetIndexParameters();
                return parameters.Any();
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
                if (_subscription != null)
                {
                    _subscription.Dispose();
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