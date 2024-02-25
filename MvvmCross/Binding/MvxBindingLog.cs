// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.Binding;

internal static class MvxBindingLog
{
    public static ILogger? Instance { get; } = MvxLogHost.GetLog("MvxBind");
}
