// IMvxEventSourceFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.Droid.FullFragging.Fragments.EventSource
{
    public interface IMvxEventSourceFragment : IMvxDisposeSource
    {
        event EventHandler<MvxValueEventArgs<MvxCreateViewParameters>> OnCreateViewCalled;
        event EventHandler OnDestroyViewCalled;
        event EventHandler<MvxValueEventArgs<Activity>> OnAttachCalled;
    }
}