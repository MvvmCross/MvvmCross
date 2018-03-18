// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using MvvmCross.Core;

namespace MvvmCross.Platform.Android.Core
{
    public interface IMvxAndroidSetup : IMvxSetup
    {
        void PlatformInitialize(Context applicationContext);
    }
}
