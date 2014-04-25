﻿// MvxEventSourceFragmentActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
//
// @author: Anass Bouassaba <anass.bouassaba@digitalpatrioten.com>

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Cirrious.CrossCore.Core;
using Android.Support.V4.App;

namespace Cirrious.CrossCore.Droid.Views
{
    public abstract class MvxEventSourceFragmentActivity
    	: FragmentActivity
      , IMvxEventSourceFragmentActivity
    {
    	protected override void OnCreate(Bundle bundle)
    	{
    		CreateWillBeCalled.Raise(this, bundle);
    		base.OnCreate(bundle);
    		CreateCalled.Raise(this, bundle);
    	}            

    	protected override void OnDestroy()
    	{
    		DestroyCalled.Raise(this);
    		base.OnDestroy();
    	}

    	protected override void OnNewIntent(Intent intent)
    	{
    		base.OnNewIntent(intent);
    		NewIntentCalled.Raise(this, intent);
    	}

    	protected override void OnResume()
    	{
    		base.OnResume();
    		ResumeCalled.Raise(this);
    	}

    	protected override void OnPause()
    	{
    		PauseCalled.Raise(this);
    		base.OnPause();
    	}

    	protected override void OnStart()
    	{
    		base.OnStart();
    		StartCalled.Raise(this);
    	}

    	protected override void OnRestart()
    	{
    		base.OnRestart();
    		RestartCalled.Raise(this);
    	}

    	protected override void OnStop()
    	{
    		StopCalled.Raise(this);
    		base.OnStop();
    	}

    	protected override void OnSaveInstanceState(Bundle outState)
    	{
    		SaveInstanceStateCalled.Raise(this, outState);
    		base.OnSaveInstanceState(outState);
    	}

    	public override void StartActivityForResult(Intent intent, int requestCode)
    	{
    		StartActivityForResultCalled.Raise(this, new MvxStartActivityForResultParameters(intent, requestCode));
    		base.StartActivityForResult(intent, requestCode);
    	}

    	protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    	{
    		ActivityResultCalled.Raise(this, new MvxActivityResultParameters(requestCode, resultCode, data));
    		base.OnActivityResult(requestCode, resultCode, data);
    	}

    	protected override void Dispose(bool disposing)
    	{
    		if (disposing)
    		{
    			DisposeCalled.Raise(this);
    		}
    		base.Dispose(disposing);
    	}

        /*
         * When the ActionBar home button is pressed, the bindings are not reloaded
         * on the parent activity, this override forces the ActionBar home button
         * to trigger the same lifecycle behavior as the hardware button
         */
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            OnBackPressed();
            return true;
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

