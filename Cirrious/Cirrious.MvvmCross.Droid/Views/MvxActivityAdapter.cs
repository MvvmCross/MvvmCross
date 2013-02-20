using System;
using Android.Content;
using Android.OS;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxActivityAdapter : MvxBaseActivityAdapter
    {
        protected IMvxAndroidView AndroidView
        {
            get { return Activity as IMvxAndroidView; }
        }

        public MvxActivityAdapter(IMvxActivityEventSource eventSource)
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

        protected override void EventSourceOnStartActivityForResultCalled(object sender, MvxTypedEventArgs<MvxStartActivityForResultParameters> mvxTypedEventArgs)
        {
            var requestCode = mvxTypedEventArgs.Value.RequestCode;
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

        protected override void EventSourceOnNewIntentCalled(object sender, MvxTypedEventArgs<Intent> mvxTypedEventArgs)
        {
            AndroidView.OnViewNewIntent();
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewDestroy();
        }

        protected override void EventSourceOnCreateCalled(object sender, MvxTypedEventArgs<Bundle> mvxTypedEventArgs)
        {
            AndroidView.IsVisible = true;
            AndroidView.OnViewCreate();
        }

        protected override void EventSourceOnActivityResultCalled(object sender, MvxTypedEventArgs<MvxActivityResultParameters> mvxTypedEventArgs)
        {
            var sink = MvxServiceProviderExtensions.GetService<IMvxIntentResultSink>();
            var args = mvxTypedEventArgs.Value;
            var intentResult = new MvxIntentResultEventArgs(args.RequestCode, args.ResultCode, args.Data);
            sink.OnResult(intentResult);
        }
    }
}