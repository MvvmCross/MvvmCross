// MvxEventSourceActivity.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Droid.Views
{
    using System;

    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Runtime;

    using MvvmCross.Platform.Core;

    [Register("mvvmcross.platform.droid.views.MvxEventSourceActivity")]
    public abstract class MvxEventSourceActivity
        : Activity, IMvxEventSourceActivity
    {
        protected MvxEventSourceActivity()
        {
        }

        protected MvxEventSourceActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        protected override void OnCreate(Bundle bundle)
        {
            this.CreateWillBeCalled.Raise(this, bundle);
            base.OnCreate(bundle);
            this.CreateCalled.Raise(this, bundle);
        }

        protected override void OnDestroy()
        {
            this.DestroyCalled.Raise(this);
            base.OnDestroy();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            this.NewIntentCalled.Raise(this, intent);
        }

        protected override void OnResume()
        {
            base.OnResume();
            this.ResumeCalled.Raise(this);
        }

        protected override void OnPause()
        {
            this.PauseCalled.Raise(this);
            base.OnPause();
        }

        protected override void OnStart()
        {
            base.OnStart();
            this.StartCalled.Raise(this);
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            this.RestartCalled.Raise(this);
        }

        protected override void OnStop()
        {
            this.StopCalled.Raise(this);
            base.OnStop();
        }

        public override void StartActivityForResult(Intent intent, int requestCode)
        {
            this.StartActivityForResultCalled.Raise(this, new MvxStartActivityForResultParameters(intent, requestCode));
            base.StartActivityForResult(intent, requestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            this.ActivityResultCalled.Raise(this, new MvxActivityResultParameters(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            this.SaveInstanceStateCalled.Raise(this, outState);
            base.OnSaveInstanceState(outState);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public event EventHandler DisposeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> CreateWillBeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> CreateCalled;

        public event EventHandler DestroyCalled;

        public event EventHandler<MvxValueEventArgs<Intent>> NewIntentCalled;

        public event EventHandler ResumeCalled;

        public event EventHandler PauseCalled;

        public event EventHandler StartCalled;

        public event EventHandler RestartCalled;

        public event EventHandler StopCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> SaveInstanceStateCalled;

        public event EventHandler<MvxValueEventArgs<MvxStartActivityForResultParameters>> StartActivityForResultCalled;

        public event EventHandler<MvxValueEventArgs<MvxActivityResultParameters>> ActivityResultCalled;
    }
}