// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

namespace MvvmCross.ViewModels;

/// <summary>
/// Selects a connected window for navigation or a presentation hint without referencing platform UI types.
/// </summary>
public class MvxWindowPresentationBundle : MvxBundle
{
    public const string WindowIdKey = "MvxWindowId";

    public MvxWindowPresentationBundle(string windowId)
    {
        ArgumentException.ThrowIfNullOrEmpty(windowId);
        Data[WindowIdKey] = windowId;
    }
}
