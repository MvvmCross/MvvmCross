using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Preferences;
using MvvmCross.Platform.Core;
using MvvmCross.Droid.Shared;
using MvvmCross.Droid.Shared.Fragments.EventSource;
using Android.App;

namespace MvvmCross.Droid.FullFragging.Fragments.EventSource
{
    [Register("mvvmcross.droid.fullfragging.fragments.eventsource.MvxEventSourcePreferenceFragment")]
    public abstract class MvxEventSourcePreferenceFragment : PreferenceFragment
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

        public MvxEventSourcePreferenceFragment()
        {

        }

        public MvxEventSourcePreferenceFragment(IntPtr javaReference, JniHandleOwnership transfer)
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

		public override void OnAttach(Activity activity)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
			{
				AttachCalled.Raise(this, activity);
			}

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