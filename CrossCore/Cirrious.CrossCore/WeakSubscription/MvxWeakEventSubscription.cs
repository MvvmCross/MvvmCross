// MvxWeakEventSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;
using System.Reflection;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.CrossCore.WeakSubscription
{
    public class MvxWeakEventSubscription<TSource, TEventArgs>
        : IDisposable
        where TSource : class
        where TEventArgs : EventArgs
    {
        private readonly WeakReference _targetReference;
        private readonly WeakReference _sourceReference;

        private readonly MethodInfo _eventHandlerMethodInfo;

        private readonly EventInfo _sourceEventInfo;

        private bool _subscribed;

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
            if (!_subscribed)
                return;

            var source = (TSource) _sourceReference.Target;
            if (source != null)
            {
                _sourceEventInfo.GetRemoveMethod().Invoke(source, new object[] {CreateEventHandler()});
                _subscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (_subscribed)
                throw new MvxException("Should not call _subscribed twice");

            var source = (TSource) _sourceReference.Target;
            if (source != null)
            {
                _sourceEventInfo.GetAddMethod().Invoke(source, new object[] {CreateEventHandler()});
                _subscribed = true;
            }
        }
    }
}