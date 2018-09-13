// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Logging;

namespace MvvmCross.Plugin.File
{
    internal static class MvxPluginLog
    {
        internal static IMvxLog Instance { get; } = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor("MvxPlugin");
    }
}
