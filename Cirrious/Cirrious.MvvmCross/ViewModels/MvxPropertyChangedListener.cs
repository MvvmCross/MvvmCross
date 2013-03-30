// <copyright file="MvxPropertyChangedListener.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.Generic;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxPropertyChangedListener
        : IDisposable
    {
        private readonly Dictionary<string, List<PropertyChangedEventHandler>> _handlersLookup = new Dictionary<string, List<PropertyChangedEventHandler>>();
        private readonly INotifyPropertyChanged _notificationObject;

        public MvxPropertyChangedListener(INotifyPropertyChanged notificationObject)
        {
            if (notificationObject == null)
                throw new ArgumentNullException("notificationObject");

            _notificationObject = notificationObject;
            _notificationObject.PropertyChanged += NotificationObjectOnPropertyChanged;
        }

        private void NotificationObjectOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var whichProperty = propertyChangedEventArgs.PropertyName;

            List<PropertyChangedEventHandler> handlers = null;
            if (!_handlersLookup.TryGetValue(whichProperty, out handlers))
                return;

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
                _notificationObject.PropertyChanged -= NotificationObjectOnPropertyChanged;
                Clear();
            }
        }

        public void Clear()
        {
            _handlersLookup.Clear();
        }

        public MvxPropertyChangedListener Listen<TProperty>(Expression<Func<TProperty>> property, Action handler)
        {
            return Listen<TProperty>(property, new PropertyChangedEventHandler((s, e) => handler()));
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
