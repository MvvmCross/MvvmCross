﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using MvvmCross.Binding.Bindings;

namespace MvvmCross.Platform.Android.Binding.Binders
{
    public interface IMvxAndroidViewBinder
    {
        void BindView(View view, Context context, IAttributeSet attrs);

        IList<KeyValuePair<object, IMvxUpdateableBinding>> CreatedBindings { get; }
    }
}
