// MvxNamedNotifyPropertyChangedEventSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.WeakSubscription
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    using MvvmCross.Platform.Core;

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
            this._propertyName = propertyName;
        }

        protected override Delegate CreateEventHandler()
        {
            return new PropertyChangedEventHandler((sender, e) =>
                {
                    if (string.IsNullOrEmpty(e.PropertyName)
                        || e.PropertyName == this._propertyName)
                    {
                        this.OnSourceEvent(sender, e);
                    }
                });
        }
    }
}