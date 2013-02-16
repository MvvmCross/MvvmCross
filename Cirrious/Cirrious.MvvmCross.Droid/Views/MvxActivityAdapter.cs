using System;
using Android.Content;
using Android.OS;
using Cirrious.MvvmCross.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxActivityAdapter : BaseActivityAdapter
    {
        protected IMvxAndroidView AndroidView
        {
            get { return Activity as IMvxAndroidView; }
        }

        public MvxActivityAdapter(IActivityEventSource eventSource)
            : base(eventSource)
        {
        }

        protected override void EventSourceOnStopCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewStop();
        }

        protected override void EventSourceOnStartCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewStart();
        }

        protected override void EventSourceOnStartActivityForResultCalled(object sender, TypedEventArgs<StartActivityForResultParameters> typedEventArgs)
        {
            var requestCode = typedEventArgs.Value.RequestCode;
            switch (requestCode)
            {
                case (int)MvxIntentRequestCode.PickFromFile:
                    MvxTrace.Trace("Warning - activity request code may clash with Mvx code for {0}",
                                   (MvxIntentRequestCode)requestCode);
                    break;
            }
        }

        protected override void EventSourceOnResumeCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.IsVisible = true;
            AndroidView.OnViewResume();
        }

        protected override void EventSourceOnRestartCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewRestart();
        }

        protected override void EventSourceOnPauseCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewPause();
            AndroidView.IsVisible = false;
        }

        protected override void EventSourceOnNewIntentCalled(object sender, TypedEventArgs<Intent> typedEventArgs)
        {
            AndroidView.OnViewNewIntent();
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewDestroy();
        }

        protected override void EventSourceOnCreateCalled(object sender, TypedEventArgs<Bundle> typedEventArgs)
        {
            AndroidView.IsVisible = true;
            AndroidView.OnViewCreate();
        }

        protected override void EventSourceOnActivityResultCalled(object sender, TypedEventArgs<ActivityResultParameters> typedEventArgs)
        {
            var sink = MvxServiceProviderExtensions.GetService<IMvxIntentResultSink>();
            var args = typedEventArgs.Value;
            var intentResult = new MvxIntentResultEventArgs(args.RequestCode, args.ResultCode, args.Data);
            sink.OnResult(intentResult);
        }
    }
}