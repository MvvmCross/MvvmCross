// MvxNamedNotifyPropertyChangedEventSubscription.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using MvvmCross.Platform.Core;

namespace MvvmCross.Platform.WeakSubscription
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