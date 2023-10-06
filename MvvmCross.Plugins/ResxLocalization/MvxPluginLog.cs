// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.Plugin.ResxLocalization
{
    internal static class MvxPluginLog
    {
        internal static ILogger Instance { get; } = MvxLogHost.GetLog("MvxPlugin.ResxLocalization");
    }
}
