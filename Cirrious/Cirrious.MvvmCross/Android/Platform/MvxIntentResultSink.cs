#region Copyright
// <copyright file="MvxIntentResultSink.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Android.Interfaces;

namespace Cirrious.MvvmCross.Android.Platform
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