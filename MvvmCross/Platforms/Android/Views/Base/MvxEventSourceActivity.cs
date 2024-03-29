// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using Android.Content;
using Android.Runtime;
using MvvmCross.Base;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;

namespace MvvmCross.Platforms.Android.Views.Base
{
    [Register("mvvmcross.platforms.android.views.base.MvxEventSourceActivity")]
    public abstract class MvxEventSourceActivity
        : Activity, IMvxEventSourceActivity
    {
        protected MvxEventSourceActivity()
        {
        }

        protected MvxEventSourceActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            CreateWillBeCalled?.Raise(this, savedInstanceState);
            base.OnCreate(savedInstanceState);
            CreateCalled?.Raise(this, savedInstanceState);
        }

        protected override void OnDestroy()
        {
            DestroyCalled?.Raise(this);
            base.OnDestroy();
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            NewIntentCalled?.Raise(this, intent);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ResumeCalled?.Raise(this);
        }

        protected override void OnPause()
        {
            PauseCalled?.Raise(this);
            base.OnPause();
        }

        protected override void OnStart()
        {
            base.OnStart();
            StartCalled?.Raise(this);
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            RestartCalled?.Raise(this);
        }

        protected override void OnStop()
        {
            StopCalled?.Raise(this);
            base.OnStop();
        }

        public override void StartActivityForResult(Intent? intent, int requestCode)
        {
            StartActivityForResultCalled?.Raise(this, new MvxStartActivityForResultParameters(intent, requestCode));
            base.StartActivityForResult(intent, requestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            ActivityResultCalled?.Raise(this, new MvxActivityResultParameters(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            SaveInstanceStateCalled?.Raise(this, outState);
            base.OnSaveInstanceState(outState);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled?.Raise(this);
            }
            base.Dispose(disposing);
        }

        public event EventHandler? DisposeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle?>>? CreateWillBeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle?>>? CreateCalled;

        public event EventHandler? DestroyCalled;

        public event EventHandler<MvxValueEventArgs<Intent?>>? NewIntentCalled;

        public event EventHandler? ResumeCalled;

        public event EventHandler? PauseCalled;

        public event EventHandler? StartCalled;

        public event EventHandler? RestartCalled;

        public event EventHandler? StopCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>>? SaveInstanceStateCalled;

        public event EventHandler<MvxValueEventArgs<MvxStartActivityForResultParameters>>? StartActivityForResultCalled;

        public event EventHandler<MvxValueEventArgs<MvxActivityResultParameters>>? ActivityResultCalled;
    }
}
