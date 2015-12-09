// MvxPropertyChangedListener.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;

    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.WeakSubscription;

    public class MvxPropertyChangedListener
        : IDisposable
    {
        private readonly Dictionary<string, List<PropertyChangedEventHandler>> _handlersLookup =
            new Dictionary<string, List<PropertyChangedEventHandler>>();

        private readonly INotifyPropertyChanged _notificationObject;
        private readonly MvxNotifyPropertyChangedEventSubscription _token;

        public MvxPropertyChangedListener(INotifyPropertyChanged notificationObject)
        {
            if (notificationObject == null)
                throw new ArgumentNullException(nameof(notificationObject));

            this._notificationObject = notificationObject;
            this._token = this._notificationObject.WeakSubscribe(this.NotificationObjectOnPropertyChanged);
        }

        // Note - this is public because we use it in weak referenced situations
        public virtual void NotificationObjectOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var whichProperty = propertyChangedEventArgs.PropertyName;

            List<PropertyChangedEventHandler> handlers = null;
            if (string.IsNullOrEmpty(whichProperty))
            {
                // if whichProperty is empty, then it means everything has changed
                handlers = this._handlersLookup.Values.SelectMany(x => x).ToList();
            }
            else
            {
                if (!this._handlersLookup.TryGetValue(whichProperty, out handlers))
                    return;
            }

            foreach (var propertyChangedEventHandler in handlers)
            {
                propertyChangedEventHandler(sender, propertyChangedEventArgs);
            }
        }

        ~MvxPropertyChangedListener()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                this._token.Dispose();
                this.Clear();
            }
        }

        public void Clear()
        {
            this._handlersLookup.Clear();
        }

        public MvxPropertyChangedListener Listen<TProperty>(Expression<Func<TProperty>> property, Action handler)
        {
            return this.Listen(property, (s, e) => handler());
        }

        //TODO - is this method in or out? All depends on JIT compilation on MonoTouch
        //public MvxPropertyChangedListener Listen<TProperty>(Expression<Func<TProperty>> property, Action<TProperty> handler)
        //{
        //    return Listen<TProperty>(property, new PropertyChangedEventHandler((s, e) => handler(property.Compile().Invoke())));
        //}

        public MvxPropertyChangedListener Listen<TProperty>(Expression<Func<TProperty>> propertyExpression,
                                                            PropertyChangedEventHandler handler)
        {
            var propertyName = this._notificationObject.GetPropertyNameFromExpression(propertyExpression);
            return this.Listen(propertyName, handler);
        }

        public MvxPropertyChangedListener Listen(string propertyName, Action handler)
        {
            return this.Listen(propertyName, (s, e) => handler());
        }

        public MvxPropertyChangedListener Listen(string propertyName, PropertyChangedEventHandler handler)
        {
            List<PropertyChangedEventHandler> handlers = null;
            if (!this._handlersLookup.TryGetValue(propertyName, out handlers))
            {
                handlers = new List<PropertyChangedEventHandler>();
                this._handlersLookup.Add(propertyName, handlers);
            }

            handlers.Add(handler);
            return this;
        }
    }
}