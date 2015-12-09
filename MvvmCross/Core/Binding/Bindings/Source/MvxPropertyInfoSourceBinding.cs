// MvxPropertyInfoSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.WeakSubscription;

    public abstract class MvxPropertyInfoSourceBinding : MvxSourceBinding
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly string _propertyName;
        private IDisposable _subscription;

        protected MvxPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
            : base(source)
        {
            this._propertyInfo = propertyInfo;
            this._propertyName = propertyInfo.Name;

            if (this.Source == null)
            {
                MvxBindingTrace.Trace(
                    // this is not a Warning - as actually using a NULL source is a fairly common occurrence!
                    MvxTraceLevel.Diagnostic,
                    "Unable to bind to source as it's null"
                    , this._propertyName);
                return;
            }

            var sourceNotify = this.Source as INotifyPropertyChanged;
            if (sourceNotify != null)
                this._subscription = sourceNotify.WeakSubscribe(SourcePropertyChanged);
        }

        protected string PropertyName => this._propertyName;

        protected string PropertyNameForChangedEvent
        {
            get
            {
                if (this.IsIndexedProperty)
                    return this._propertyName + "[]";
                else
                    return this._propertyName;
            }
        }

        protected PropertyInfo PropertyInfo => this._propertyInfo;

        protected bool IsIndexedProperty
        {
            get
            {
                var parameters = this._propertyInfo.GetIndexParameters();
                return parameters.Any();
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (this._subscription != null)
                {
                    this._subscription.Dispose();
                    this._subscription = null;
                }
            }
        }

        // Note - this is public because we use it in weak referenced situations
        public void SourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // we test for null or empty here - this means all properties have changed
            // - fix for https://github.com/slodge/MvvmCross/issues/280
            if (string.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName == this.PropertyNameForChangedEvent)
                this.OnBoundPropertyChanged();
        }

        protected abstract void OnBoundPropertyChanged();
    }
}