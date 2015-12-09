// MvxNotifyPropertyChangedEventSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.WeakSubscription
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public class MvxNotifyPropertyChangedEventSubscription
        : MvxWeakEventSubscription<INotifyPropertyChanged, PropertyChangedEventArgs>
    {
        private static readonly EventInfo PropertyChangedEventInfo = typeof(INotifyPropertyChanged).GetEvent("PropertyChanged");

        // This code ensures the PropertyChanged event is not stripped by Xamarin linker
        // see https://github.com/MvvmCross/MvvmCross/pull/453
        public static void LinkerPleaseInclude(INotifyPropertyChanged iNotifyPropertyChanged)
        {
            iNotifyPropertyChanged.PropertyChanged += (sender, e) => { };
        }

        public MvxNotifyPropertyChangedEventSubscription(INotifyPropertyChanged source,
                                                         EventHandler<PropertyChangedEventArgs> targetEventHandler)
            : base(source, PropertyChangedEventInfo, targetEventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new PropertyChangedEventHandler(this.OnSourceEvent);
        }
    }
}