// MvxPropertyChangedListener.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.WeakSubscription;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.ViewModels
{
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

            _notificationObject = notificationObject;
            _token = _notificationObject.WeakSubscribe(NotificationObjectOnPropertyChanged);
        }

        // Note - this is public because we use it in weak referenced situations
        public virtual void NotificationObjectOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var whichProperty = propertyChangedEventArgs.PropertyName;

            List<PropertyChangedEventHandler> handlers = null;
            if (string.IsNullOrEmpty(whichProperty))
            {
                // if whichProperty is empty, then it means everything has changed
                handlers = _handlersLookup.Values.SelectMany(x => x).ToList();
            }
            else
            {
                if (!_handlersLookup.TryGetValue(whichProperty, out handlers))
                    return;
            }

            foreach (var propertyChangedEventHandler in handlers)
            {
                propertyChangedEventHandler(sender, propertyChangedEventArgs);
            }
        }

        ~MvxPropertyChangedListener()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _token.Dispose();
                Clear();
            }
        }

        public void Clear()
        {
            _handlersLookup.Clear();
        }

        public MvxPropertyChangedListener Listen<TProperty>(Expression<Func<TProperty>> property, Action handler)
        {
            return Listen(property, (s, e) => handler());
        }

        //TODO - is this method in or out? All depends on JIT compilation on MonoTouch
        //public MvxPropertyChangedListener Listen<TProperty>(Expression<Func<TProperty>> property, Action<TProperty> handler)
        //{
        //    return Listen<TProperty>(property, new PropertyChangedEventHandler((s, e) => handler(property.Compile().Invoke())));
        //}

        public MvxPropertyChangedListener Listen<TProperty>(Expression<Func<TProperty>> propertyExpression,
                                                            PropertyChangedEventHandler handler)
        {
            var propertyName = _notificationObject.GetPropertyNameFromExpression(propertyExpression);
            return Listen(propertyName, handler);
        }

        public MvxPropertyChangedListener Listen(string propertyName, Action handler)
        {
            return Listen(propertyName, (s, e) => handler());
        }

        public MvxPropertyChangedListener Listen(string propertyName, PropertyChangedEventHandler handler)
        {
            List<PropertyChangedEventHandler> handlers = null;
            if (!_handlersLookup.TryGetValue(propertyName, out handlers))
            {
                handlers = new List<PropertyChangedEventHandler>();
                _handlersLookup.Add(propertyName, handlers);
            }

            handlers.Add(handler);
            return this;
        }
    }
}