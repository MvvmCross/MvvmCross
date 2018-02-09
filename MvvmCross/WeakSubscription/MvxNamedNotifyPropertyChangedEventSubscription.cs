// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using MvvmCross.Base;

namespace MvvmCross.WeakSubscription
{
    public class MvxNamedNotifyPropertyChangedEventSubscription<T>
        : MvxNotifyPropertyChangedEventSubscription
    {
        private readonly string _propertyName;

        public MvxNamedNotifyPropertyChangedEventSubscription(INotifyPropertyChanged source,
                                                              Expression<Func<T>> property,
                                                              EventHandler<PropertyChangedEventArgs> targetEventHandler)
            : this(source, (string)source.GetPropertyNameFromExpression(property), targetEventHandler)
        {
        }

        public MvxNamedNotifyPropertyChangedEventSubscription(INotifyPropertyChanged source,
                                                              string propertyName,
                                                              EventHandler<PropertyChangedEventArgs> targetEventHandler)
            : base(source, targetEventHandler)
        {
            _propertyName = propertyName;
        }

        protected override Delegate CreateEventHandler()
        {
            return new PropertyChangedEventHandler((sender, e) =>
                {
                    if (string.IsNullOrEmpty(e.PropertyName)
                        || e.PropertyName == _propertyName)
                    {
                        OnSourceEvent(sender, e);
                    }
                });
        }
    }
}
