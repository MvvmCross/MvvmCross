// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.Views
{
    public interface IMvxAndroidViewModelRequestTranslator
    {
        Intent GetIntentFor(MvxViewModelRequest request);
        Intent GetIntentFor(IMvxViewModel existingViewModelToUse, MvxViewModelRequest request);
    }
}
