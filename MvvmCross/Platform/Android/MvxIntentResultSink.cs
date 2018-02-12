﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platform.Android.Views.Base;

namespace MvvmCross.Platform.Android
{
    public class MvxIntentResultSink : IMvxIntentResultSink, IMvxIntentResultSource
    {
        public void OnResult(MvxIntentResultEventArgs result)
        {
            Result?.Invoke(this, result);
        }

        public event EventHandler<MvxIntentResultEventArgs> Result;
    }
}
