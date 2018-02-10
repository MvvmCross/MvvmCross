// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platform.Android.Base.Views;

namespace MvvmCross.Platform.Android.Base.Platform
{
    public interface IMvxIntentResultSource
    {
        event EventHandler<MvxIntentResultEventArgs> Result;
    }
}
