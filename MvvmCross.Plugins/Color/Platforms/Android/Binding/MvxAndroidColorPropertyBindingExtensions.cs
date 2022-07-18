// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using Android.Widget;

namespace MvvmCross.Plugin.Color.Platforms.Android.Binding
{
    public static class MvxAndroidColorPropertyBindingExtensions
    {
        public static string BindBackgroundColor(this View view)
           => MvxAndroidColorPropertyBinding.View_BackgroundColor;

        public static string BindTextColor(this TextView view)
           => MvxAndroidColorPropertyBinding.TextView_TextColor;
    }
}
