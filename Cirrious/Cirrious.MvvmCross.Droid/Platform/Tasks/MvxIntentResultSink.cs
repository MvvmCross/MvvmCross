// MvxIntentResultSink.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Droid.Interfaces;

namespace Cirrious.MvvmCross.Droid.Platform.Tasks
{
    public class MvxIntentResultSink : IMvxIntentResultSink, IMvxIntentResultSource
    {
        #region Implementation of IMvxIntentResultSink

        public void OnResult(MvxIntentResultEventArgs result)
        {
            var handler = Result;
            if (handler != null)
                handler(this, result);
        }

        #endregion

        #region Implementation of IMvxIntentResultSource

        public event EventHandler<MvxIntentResultEventArgs> Result;

        #endregion
    }
}