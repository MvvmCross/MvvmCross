// IMvxEventSourceFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.OS;
using Cirrious.CrossCore.Core;
using System;

namespace Cirrious.MvvmCross.Droid.FullFragging.Fragments.EventSource
{
    public interface IMvxEventSourceFragment : IMvxDisposeSource
    {
        //Created sate
        event EventHandler<MvxValueEventArgs<Activity>> AttachCalled;

        event EventHandler<MvxValueEventArgs<Bundle>> CreateWillBeCalled;

        event EventHandler<MvxValueEventArgs<Bundle>> CreateCalled;

        event EventHandler<MvxValueEventArgs<MvxCreateViewParameters>> CreateViewCalled;

        //Started state
        event EventHandler StartCalled;

        //Resumed state
        event EventHandler ResumeCalled;

        //Paused state
        event EventHandler PauseCalled;

        //Stopped state
        event EventHandler StopCalled;

        //Destroyed state
        event EventHandler DestroyViewCalled;

        event EventHandler DestroyCalled;

        event EventHandler DetachCalled;

        event EventHandler<MvxValueEventArgs<Bundle>> SaveInstanceStateCalled;
    }
}