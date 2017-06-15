// MvxEventSourceDialogFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Droid.Views;
using MvvmCross.Platform.Core;

namespace MvvmCross.Droid.FullFragging.Fragments.EventSource
{
    [Register("mvvmcross.droid.fullfragging.fragments.eventsource.MvxEventSourceDialogFragment")]
    public class MvxEventSourceDialogFragment
        : DialogFragment
        , IMvxEventSourceFragment
    {
        public event EventHandler<MvxValueEventArgs<Context>> AttachCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> CreateWillBeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> CreateCalled;

        public event EventHandler<MvxValueEventArgs<MvxCreateViewParameters>> CreateViewCalled;

        public event EventHandler StartCalled;

        public event EventHandler ResumeCalled;

        public event EventHandler PauseCalled;

        public event EventHandler StopCalled;

        public event EventHandler DestroyViewCalled;

        public event EventHandler DestroyCalled;

        public event EventHandler DetachCalled;

        public event EventHandler DisposeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> SaveInstanceStateCalled;

        protected MvxEventSourceDialogFragment()
        {
        }

        protected MvxEventSourceDialogFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnAttach(Context context)
        {
			if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
			{
				AttachCalled.Raise(this, context);
			}

            base.OnAttach(context);
        }

#pragma warning disable CS0672 // Member overrides obsolete member
        public override void OnAttach(Activity activity)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
			{
				AttachCalled.Raise(this, activity);
			}

#pragma warning disable CS0618 // Type or member is obsolete
            base.OnAttach(activity);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            CreateWillBeCalled.Raise(this, savedInstanceState);
            base.OnCreate(savedInstanceState);
            CreateCalled.Raise(this, savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            CreateViewCalled.Raise(this, new MvxCreateViewParameters(inflater, container, savedInstanceState));
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnStart()
        {
            StartCalled.Raise(this);
            base.OnStart();
        }

        public override void OnResume()
        {
            ResumeCalled.Raise(this);
            base.OnResume();
        }

        public override void OnPause()
        {
            PauseCalled.Raise(this);
            base.OnPause();
        }

        public override void OnStop()
        {
            StopCalled.Raise(this);
            base.OnStop();
        }

        public override void OnDestroyView()
        {
            DestroyViewCalled.Raise(this);
            base.OnDestroyView();
        }

        public override void OnDestroy()
        {
            DestroyCalled.Raise(this);
            base.OnDestroy();
        }

        public override void OnDetach()
        {
            DetachCalled.Raise(this);
            base.OnDetach();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            SaveInstanceStateCalled.Raise(this, outState);
            base.OnSaveInstanceState(outState);
        }
    }
}