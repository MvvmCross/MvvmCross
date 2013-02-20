using System;
using Android.App;
using Android.Content;
using Android.OS;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Interfaces.Core;

namespace Cirrious.CrossCore.Droid.Views
{
    public class MvxBaseActivityAdapter
    {
        private readonly IMvxActivityEventSource _eventSource;

        protected Activity Activity
        {
            get { return _eventSource as Activity; }
        }

        public MvxBaseActivityAdapter(IMvxActivityEventSource eventSource)
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

        protected virtual void EventSourceOnCreateWillBeCalled(object sender, MvxTypedEventArgs<Bundle> mvxTypedEventArgs)
        {
        }

        protected virtual void EventSourceOnStopCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnStartCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnStartActivityForResultCalled(object sender, MvxTypedEventArgs<MvxStartActivityForResultParameters> mvxTypedEventArgs)
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

        protected virtual void EventSourceOnNewIntentCalled(object sender, MvxTypedEventArgs<Intent> mvxTypedEventArgs)
        {
        }

        protected virtual void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnCreateCalled(object sender, MvxTypedEventArgs<Bundle> mvxTypedEventArgs)
        {
        }

        protected virtual void EventSourceOnActivityResultCalled(object sender, MvxTypedEventArgs<MvxActivityResultParameters> mvxTypedEventArgs)
        {
        }
    }
}