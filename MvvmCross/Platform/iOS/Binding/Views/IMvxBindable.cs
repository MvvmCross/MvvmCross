﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Core;

namespace MvvmCross.Platform.Ios.Binding.Views
{
    public interface IMvxBindable
        : IMvxBindingContextOwner
        , IMvxDataConsumer
    {
    }
}
