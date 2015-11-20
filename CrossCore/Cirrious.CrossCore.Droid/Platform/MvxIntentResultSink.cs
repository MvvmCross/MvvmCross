// MvxIntentResultSink.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Droid.Views;
using System;

namespace Cirrious.CrossCore.Droid.Platform
{
    public class MvxIntentResultSink : IMvxIntentResultSink, IMvxIntentResultSource
    {
        public void OnResult(MvxIntentResultEventArgs result)
        {
            var handler = Result;
            handler?.Invoke(this, result);
        }

        public event EventHandler<MvxIntentResultEventArgs> Result;
    }
}