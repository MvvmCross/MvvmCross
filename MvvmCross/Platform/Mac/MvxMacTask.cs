﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Foundation;
using AppKit;

namespace MvvmCross.Platform.Mac
{
    public class MvxMacTask
    {
        protected bool DoUrlOpen(NSUrl url)
        {
            var sharedWorkSpace = NSWorkspace.SharedWorkspace;
            return sharedWorkSpace.OpenUrl(url);
        }
    }
}
