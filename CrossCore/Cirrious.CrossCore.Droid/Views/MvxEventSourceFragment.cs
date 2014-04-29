// MvxEventSourceFragmentActivity.cs
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
using Fragment = Android.Support.V4.App.Fragment;

namespace Cirrious.CrossCore.Droid.Views
{
    public abstract class MvxEventSourceFragment
        : Fragment
          , IMvxEventSourceFragment
    {	
        public override void OnCreate(Bundle bundle)
        {
            CreateWillBeCalled.Raise(this, bundle);
            base.OnCreate(bundle);
            CreateCalled.Raise(this, bundle);
        }

        public override void OnAttach(Android.App.Activity activity)
        {
            base.OnAttach(activity);
        }

        public override void OnDestroy()
        {
            DestroyCalled.Raise(this);
            base.OnDestroy();
        }            

        public override void OnResume()
        {
            base.OnResume();
            ResumeCalled.Raise(this);
        }

        public override void OnPause()
        {
            PauseCalled.Raise(this);
            base.OnPause();
        }

        public override void OnStart()
        {
            base.OnStart();
            StartCalled.Raise(this);
        }            

        public override void OnDetach()
        {
            base.OnDetach();
            // TODO DetachCalled.Raise(this);
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            // TODO OnDestroyViewCalled.Raise(this);
        }

        public override void OnStop()
        {
            StopCalled.Raise(this);
            base.OnStop();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            SaveInstanceStateCalled.Raise(this, outState);
            base.OnSaveInstanceState(outState);
        }            

        public override void StartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResultCalled.Raise(this, new MvxStartActivityForResultParameters(intent, requestCode));
            base.StartActivityForResult(intent, requestCode);
        }            

        /*public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            ActivityResultCalled.Raise(Activity, new MvxActivityResultParameters(requestCode, (Result)resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public event EventHandler DisposeCalled;
        public event EventHandler<MvxValueEventArgs<Bundle>> CreateWillBeCalled;
        public event EventHandler<MvxValueEventArgs<Bundle>> CreateCalled;
        public event EventHandler DestroyCalled;
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

