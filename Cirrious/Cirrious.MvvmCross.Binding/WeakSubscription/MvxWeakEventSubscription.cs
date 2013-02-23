// MvxWeakEventSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.WeakSubscription
{
    public class MvxWeakEventSubscription<TSource, TEventArgs>
        : IDisposable
        where TSource : class
        where TEventArgs : EventArgs
    {
        private readonly WeakReference _targetReference; //A weak reference to the target object.
        private readonly WeakReference _sourceReference; //A weak reference to the source object.

        private readonly MethodInfo _eventHandlerMethodInfo;
                                    //The metadata of the method on the target that will handle the event.

        private readonly EventInfo _sourceEventInfo; // The event on the source to use

        public MvxWeakEventSubscription(
            TSource source,
            Expression<Func<TSource>> sourceEventInfoFinder,
            EventHandler<TEventArgs> targetEventHandler)
            : this(source, sourceEventInfoFinder.GetEventInfo(), targetEventHandler)
        {
        }

        public MvxWeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler<TEventArgs> targetEventHandler)
            : this(source, typeof (TSource).GetEvent(sourceEventName), targetEventHandler)
        {
        }

        public MvxWeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler<TEventArgs> targetEventHandler)
        {
            if (source == null)
                throw new ArgumentNullException();

            if (sourceEventInfo == null)
                throw new ArgumentNullException("sourceEventInfo",
                                                "missing source event info in MvxWeakEventSubscription");

            _eventHandlerMethodInfo = targetEventHandler.Method;
            _targetReference = new WeakReference(targetEventHandler.Target);
            _sourceReference = new WeakReference(source);
            _sourceEventInfo = sourceEventInfo;

            AddEventHandler();
        }

        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(OnSourceEvent);
        }

        //This is the method that will handle the event of source.
        protected void OnSourceEvent(object sender, TEventArgs e)
        {
            var target = _targetReference.Target;
            if (target != null)
            {
                _eventHandlerMethodInfo.Invoke(target, new[] {sender, e});
            }
            else
            {
                RemoveEventHandler();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandler();
            }
        }

        private void RemoveEventHandler()
        {
            var source = (TSource) _sourceReference.Target;
            if (source != null)
            {
                _sourceEventInfo.GetRemoveMethod().Invoke(source, new object[] {CreateEventHandler()});
            }
        }

        private void AddEventHandler()
        {
            var source = (TSource) _sourceReference.Target;
            if (source != null)
            {
                _sourceEventInfo.GetAddMethod().Invoke(source, new object[] {CreateEventHandler()});
            }
        }
    }
}