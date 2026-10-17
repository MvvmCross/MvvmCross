// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using MvvmCross.Presenters;

namespace MvvmCross.Platforms.Ios.Presenters;

/// <summary>
/// Opt-in scene presenter. Each connected UIWindow retains its own navigation, modal,
/// tab, page, split and popover state in an ordinary <see cref="MvxIosViewPresenter"/>.
/// </summary>
public class MvxIosMultiWindowViewPresenter : MvxWindowViewPresenter, IMvxIosViewPresenter
{
    /// <summary>
    /// Registers a scene's window. Use the UISceneSession.PersistentIdentifier as windowId,
    /// and call UnregisterWindow from the scene delegate's DidDisconnect callback.
    /// </summary>
    public virtual void RegisterWindow(string windowId, UIWindow window)
    {
        ArgumentNullException.ThrowIfNull(window);
        var presenter = CreatePresenter(window);
        presenter.PopoverPresentationSourceProvider ??= new MvxPopoverPresentationSourceProvider();
        RegisterPresenter(windowId, presenter);
    }

    /// <summary>Override to use a custom iOS presenter for each window.</summary>
    protected virtual MvxIosViewPresenter CreatePresenter(UIWindow window) => new(window);

    /// <summary>Gets a connected window's presenter for platform-specific customization.</summary>
    public MvxIosViewPresenter? GetWindowPresenter(string windowId)
        => GetPresenter(windowId) as MvxIosViewPresenter;

    /// <summary>
    /// For single-window callers. Native popover delegates call their owning window presenter directly.
    /// With multiple windows, call GetWindowPresenter(windowId) instead.
    /// </summary>
    public void ClosedPopoverViewController()
        => (SelectPresenter(null) as IMvxIosViewPresenter)?.ClosedPopoverViewController();
}
