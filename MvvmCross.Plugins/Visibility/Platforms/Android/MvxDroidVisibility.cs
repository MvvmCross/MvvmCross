// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Visibility.Platforms.Android
{
    [Preserve(AllMembers = true)]
    public class MvxDroidVisibility : IMvxNativeVisibility
    {
        public object ToNative(MvxVisibility visibility)
        {
            switch (visibility)
            {
                case MvxVisibility.Collapsed:
                    return ViewStates.Gone;
                case MvxVisibility.Hidden:
                    return ViewStates.Invisible;
                default:
                    return ViewStates.Visible;
            }
        }
    }
}
