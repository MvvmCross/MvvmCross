using System;
using Android.App;
using Android.Content;
using Android.OS;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class BaseActivityAdapter
    {
        private readonly IActivityEventSource _eventSource;

        protected Activity Activity
        {
            get { return _eventSource as Activity; }
        }

        public BaseActivityAdapter(IActivityEventSource eventSource)
        {
            _eventSource = eventSource;

            _eventSource.CreateCalled += EventSourceOnCreateCalled;
            _eventSource.CreateWillBeCalled += EventSourceOnCreateWillBeCalled;
            _eventSource.StartCalled += EventSourceOnStartCalled;
            _eventSource.RestartCalled += EventSourceOnRestartCalled;
            _eventSource.ResumeCalled += EventSourceOnResumeCalled;
            _eventSource.PauseCalled += EventSourceOnPauseCalled;
            _eventSource.StopCalled += EventSourceOnStopCalled;
            _eventSource.DestroyCalled += EventSourceOnDestroyCalled;
            _eventSource.DisposeCalled += EventSourceOnDisposeCalled;

            _eventSource.NewIntentCalled += EventSourceOnNewIntentCalled;
            
            _eventSource.ActivityResultCalled += EventSourceOnActivityResultCalled;
            _eventSource.StartActivityForResultCalled += EventSourceOnStartActivityForResultCalled;
        }

        protected virtual void EventSourceOnCreateWillBeCalled(object sender, TypedEventArgs<Bundle> typedEventArgs)
        {
        }

        protected virtual void EventSourceOnStopCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnStartCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnStartActivityForResultCalled(object sender, TypedEventArgs<StartActivityForResultParameters> typedEventArgs)
        {
        }

        protected virtual void EventSourceOnResumeCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnRestartCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnPauseCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnNewIntentCalled(object sender, TypedEventArgs<Intent> typedEventArgs)
        {
        }

        protected virtual void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnCreateCalled(object sender, TypedEventArgs<Bundle> typedEventArgs)
        {
        }

        protected virtual void EventSourceOnActivityResultCalled(object sender, TypedEventArgs<ActivityResultParameters> typedEventArgs)
        {
        }
    }
}