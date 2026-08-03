// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using MvvmCross.Core;

namespace MvvmCross.Platforms.Ios.Core;

/// <summary>
/// Base scene delegate that fires MvvmCross lifetime events.
/// Use <see cref="MvvmCross.Platforms.Ios.Hosting.MvxIosHostBuilder"/> in your
/// <see cref="WillConnect"/> override to initialize the framework.
/// </summary>
public abstract class MvxSceneDelegate : UIResponder, IUIWindowSceneDelegate, IMvxLifetime
{
    public event EventHandler<MvxLifetimeEventArgs>? LifetimeChanged;

    [Export("window")] public UIWindow? Window { get; set; }

    [Export("scene:willConnectToSession:options:")]
    public virtual void WillConnect(
        UIScene scene,
        UISceneSession session,
        UISceneConnectionOptions connectionOptions)
    {
    }

    [Export("sceneDidDisconnect:")]
    public virtual void DidDisconnect(UIScene scene)
    {
    }

    [Export("sceneDidBecomeActive:")]
    public virtual void DidBecomeActive(UIScene scene)
    {
        FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
    }

    [Export("sceneWillResignActive:")]
    public virtual void WillResignActive(UIScene scene)
    {
        FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
    }

    [Export("sceneWillEnterForeground:")]
    public virtual void WillEnterForeground(UIScene scene)
    {
    }

    [Export("sceneDidEnterBackground:")]
    public virtual void DidEnterBackground(UIScene scene)
    {
    }

    protected void FireLifetimeChanged(MvxLifetimeEvent which)
    {
        LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
    }
}

