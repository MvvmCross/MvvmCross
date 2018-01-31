// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using AppKit;
using MvvmCross.Core.Platform;

namespace MvvmCross.Mac.Platform
{
    public interface IMvxApplicationDelegate : INSApplicationDelegate, IMvxLifetime
    {
    }
}