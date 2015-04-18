// MvxEventSourceDialogFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.OS;
using Android.Views;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.Droid.FullFragging.Fragments.EventSource
{
    public class MvxEventSourceDialogFragment
        : DialogFragment
        , IMvxEventSourceFragment
    {
        public event EventHandler<MvxValueEventArgs<Activity>> AttachCalled;
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

        public override void OnAttach(Activity activity)
        {
            AttachCalled.Raise(this, Activity);
            base.OnAttach(activity);
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