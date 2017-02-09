// MvxWeakEventSubscription.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Platform.WeakSubscription
{
    public class MvxWeakEventSubscription<TSource, TEventArgs> : IDisposable
        where TSource : class
    {
        private readonly WeakReference _targetReference;
        private readonly WeakReference<TSource> _sourceReference;

        private readonly MethodInfo _eventHandlerMethodInfo;

        private readonly EventInfo _sourceEventInfo;

        // we store a copy of our Delegate/EventHandler in order to prevent it being
        // garbage collected while the `client` still has ownership of this subscription
        private readonly Delegate _ourEventHandler;

        private bool _subscribed;

        public MvxWeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler<TEventArgs> targetEventHandler)
            : this(source, typeof(TSource).GetEvent(sourceEventName), targetEventHandler)
        {
        }

        protected MvxWeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler<TEventArgs> targetEventHandler)
        {
            if (source == null)
                throw new ArgumentNullException();

            if (sourceEventInfo == null)
                throw new ArgumentNullException(nameof(sourceEventInfo),
                                                "missing source event info in MvxWeakEventSubscription");

            this._eventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
            this._targetReference = new WeakReference(targetEventHandler.Target);
            this._sourceReference = new WeakReference<TSource>(source);
            this._sourceEventInfo = sourceEventInfo;

            // TODO: need to move this virtual call out of the constructor - need to implement a separate Init() method
            this._ourEventHandler = this.CreateEventHandler();

            this.AddEventHandler();
        }

        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(this.OnSourceEvent);
        }

        protected virtual object GetTargetObject()
        {
            return _targetReference.Target;
        }

        //This is the method that will handle the event of source.
        protected void OnSourceEvent(object sender, TEventArgs e)
        {
            var target = GetTargetObject();
            if (target != null)
            {
                this._eventHandlerMethodInfo.Invoke(target, new[] { sender, e });
            }
            else
            {
                this.RemoveEventHandler();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RemoveEventHandler();
            }
        }

        private void RemoveEventHandler()
        {
            if (!this._subscribed)
                return;

            TSource source;
            if (this._sourceReference.TryGetTarget(out source))
            {
                this._sourceEventInfo.GetRemoveMethod().Invoke(source, new object[] { this._ourEventHandler });
                this._subscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (this._subscribed)
                throw new MvxException("Should not call _subscribed twice");

            TSource source;
            if (this._sourceReference.TryGetTarget(out source))
            {
                this._sourceEventInfo.GetAddMethod().Invoke(source, new object[] { this._ourEventHandler });
                this._subscribed = true;
            }
        }
    }

    public class MvxWeakEventSubscription<TSource> : IDisposable
        where TSource : class
    {
        private readonly WeakReference _targetReference;
        private readonly WeakReference<TSource> _sourceReference;

        private readonly MethodInfo _eventHandlerMethodInfo;

        private readonly EventInfo _sourceEventInfo;

        // we store a copy of our Delegate/EventHandler in order to prevent it being
        // garbage collected while the `client` still has ownership of this subscription
        private readonly Delegate _ourEventHandler;

        private bool _subscribed;

        public MvxWeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler targetEventHandler)
            : this(source, typeof(TSource).GetEvent(sourceEventName), targetEventHandler)
        {
        }

        protected MvxWeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler targetEventHandler)
        {
            if (source == null)
                throw new ArgumentNullException();

            if (sourceEventInfo == null)
                throw new ArgumentNullException(nameof(sourceEventInfo),
                                                "missing source event info in MvxWeakEventSubscription");

            this._eventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
            this._targetReference = new WeakReference(targetEventHandler.Target);
            this._sourceReference = new WeakReference<TSource>(source);
            this._sourceEventInfo = sourceEventInfo;

            // TODO: need to move this virtual call out of the constructor - need to implement a separate Init() method
            this._ourEventHandler = this.CreateEventHandler();

            this.AddEventHandler();
        }

        protected virtual object GetTargetObject()
        {
            return _targetReference.Target;
        }

        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler(this.OnSourceEvent);
        }

        //This is the method that will handle the event of source.
        protected void OnSourceEvent(object sender, EventArgs e)
        {
            var target = GetTargetObject();
            if (target != null)
            {
                this._eventHandlerMethodInfo.Invoke(target, new[] { sender, e });
            }
            else
            {
                this.RemoveEventHandler();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RemoveEventHandler();
            }
        }

        private void RemoveEventHandler()
        {
            if (!this._subscribed)
                return;

            TSource source;
            if (this._sourceReference.TryGetTarget(out source))
            {
                this._sourceEventInfo.GetRemoveMethod().Invoke(source, new object[] { this._ourEventHandler });
                this._subscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (this._subscribed)
                throw new MvxException("Should not call _subscribed twice");

            TSource source;
            if (this._sourceReference.TryGetTarget(out source))
            {
                this._sourceEventInfo.GetAddMethod().Invoke(source, new object[] { this._ourEventHandler });
                this._subscribed = true;
            }
        }
    }
}