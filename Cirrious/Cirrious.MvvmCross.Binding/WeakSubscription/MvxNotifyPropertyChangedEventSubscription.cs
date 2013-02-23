using System;
using System.ComponentModel;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.WeakSubscription
{
    public class MvxNotifyPropertyChangedEventSubscription
        : MvxWeakEventSubscription<INotifyPropertyChanged, PropertyChangedEventArgs>
    {
        private static readonly EventInfo EventInfo = typeof (INotifyPropertyChanged).GetEvent("PropertyChanged");

        public MvxNotifyPropertyChangedEventSubscription(INotifyPropertyChanged source, EventHandler<PropertyChangedEventArgs> targetEventHandler) 
            : base(source, EventInfo, targetEventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new PropertyChangedEventHandler(OnSourceEvent);
        }
    }
}