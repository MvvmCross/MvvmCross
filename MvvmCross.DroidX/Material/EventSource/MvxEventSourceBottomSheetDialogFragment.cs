﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Google.Android.Material.BottomSheet;
using MvvmCross.Base;
using MvvmCross.Platforms.Android.Views;

namespace MvvmCross.DroidX.Material.EventSource
{
	public class MvxEventSourceBottomSheetDialogFragment
		: BottomSheetDialogFragment, IMvxEventSourceFragment
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

		protected MvxEventSourceBottomSheetDialogFragment()
		{
		}

	    protected MvxEventSourceBottomSheetDialogFragment(IntPtr javaReference, JniHandleOwnership transfer)
	        : base(javaReference, transfer)
	    {
	    }

		public override void OnAttach(Context context)
		{
			AttachCalled.Raise(this, context);
			base.OnAttach(context);
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
