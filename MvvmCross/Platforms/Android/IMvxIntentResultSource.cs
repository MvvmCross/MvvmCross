// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platforms.Android.Views.Base;

namespace MvvmCross.Platforms.Android
{
    public interface IMvxIntentResultSource
    {
        event EventHandler<MvxIntentResultEventArgs> Result;
    }
}
