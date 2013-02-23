// IMvxEventSourceFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.Core;

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public interface IMvxEventSourceFragment : IMvxDisposeSource
    {
        event EventHandler<MvxValueEventArgs<MvxCreateViewParameters>> OnCreateViewCalled;
        event EventHandler OnDestroyViewCalled;
    }
}