﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using AppKit;
using Foundation;

namespace MvvmCross.Platform.Mac.Views
{
    public interface IMvxMacViewSegue
    {
        object PrepareViewModelParametersForSegue(NSStoryboardSegue segue, NSObject sender);
    }
}
