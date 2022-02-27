// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using Android.Util;
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Binders
{
#nullable enable
    public interface IMvxLayoutInflaterFactory
    {
        View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs);
    }
#nullable restore
}
