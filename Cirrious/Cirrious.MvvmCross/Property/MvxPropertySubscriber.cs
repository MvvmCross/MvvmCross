#region Copyright
// <copyright file="MvxPropertySubscriber.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;

namespace Cirrious.MvvmCross.Property
{
    public class MvxPropertySubscriber<T>
        : IDisposable
        where T : INotifyPropertyChanged
    {
        private const string WrongExpressionMessage = "Wrong expression\nshould be called as\nSubscribe(() => Property);";

        private List<PropertyChangedEventHandler> handlers;

        private T notificationObject;

        public MvxPropertySubscriber(T notificationObject)
        {
            if (notificationObject == null)
                throw new ArgumentNullException("notificationObject");

            this.notificationObject = notificationObject;
            handlers = new List<PropertyChangedEventHandler>();
        }

        public MvxPropertySubscriber<T> Subscribe<R>(Expression<Func<R>> property, Action handler)
        {
            return Subscribe<R>(property, new PropertyChangedEventHandler((s, e) => handler()));
        }

        public MvxPropertySubscriber<T> Subscribe<R>(Expression<Func<R>> property, PropertyChangedEventHandler handler)
        {
            string name;

            try
            {
                name = MvxPropertyUtils.PropertyName<R>(property);
            }
            catch(Exception e)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }

            var member = (property.Body as MemberExpression).Member as PropertyInfo;

#if NETFX_CORE
            if (!member.DeclaringType.GetTypeInfo().IsAssignableFrom(notificationObject.GetType().GetTypeInfo()))
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }
#else
            if (!member.DeclaringType.IsAssignableFrom(notificationObject.GetType()))
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }
#endif

            PropertyChangedEventHandler helperHandler = (s, e) =>
            {
                if(e.PropertyName == name)
                {
                    handler(s, e);
                }
            };

            handlers.Add(helperHandler);

            notificationObject.PropertyChanged += helperHandler;

            return this;
        }
    
        public void Dispose()
        {
 	        foreach(var handler in handlers)
            {
                notificationObject.PropertyChanged -= handler;
            }
        }
    }
}