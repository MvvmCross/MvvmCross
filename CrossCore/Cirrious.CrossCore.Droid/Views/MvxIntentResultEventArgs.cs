// MvxIntentResultEventArgs.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Content;
using System;

namespace Cirrious.CrossCore.Droid.Views
{
    public class MvxIntentResultEventArgs
        : EventArgs
    {
        public MvxIntentResultEventArgs(int requestCode, Result resultCode, Intent data)
        {
            Data = data;
            ResultCode = resultCode;
            RequestCode = requestCode;
        }

        public int RequestCode { get; private set; }
        public Result ResultCode { get; private set; }
        public Intent Data { get; private set; }
    }
}