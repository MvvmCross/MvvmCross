// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using Android.Util;
using Android.Views;

namespace MvvmCross.Binding.Droid.Binders
{
    public interface IMvxAndroidViewFactory
    {
        View CreateView(View parent, string name, Context context, IAttributeSet attrs);
    }
}