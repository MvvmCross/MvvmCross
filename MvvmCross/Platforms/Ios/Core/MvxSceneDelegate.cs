// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Ios.Core;

[RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
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
        if (scene is UIWindowScene windowScene)
        {
            RegisterSetup();
            Window = new UIWindow(windowScene);
            MvxIosSetupSingleton
                .EnsureSingletonAvailable(this, Window)
                .EnsureInitialized();
            RunAppStart();
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
        }
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

    protected virtual void RunAppStart()
    {
        if (Mvx.IoCProvider?.TryResolve(out IMvxAppStart? startup) == true &&
            startup is { IsStarted: false })
        {
            startup.Start();
        }

        Window?.MakeKeyAndVisible();
    }

    protected abstract void RegisterSetup();

    private void FireLifetimeChanged(MvxLifetimeEvent which)
    {
        var handler = LifetimeChanged;
        handler?.Invoke(this, new MvxLifetimeEventArgs(which));
    }
}

[RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
public abstract class MvxSceneDelegate<TMvxIosSetup, TApplication> : MvxSceneDelegate
    where TMvxIosSetup : MvxIosSetup<TApplication>, new()
    where TApplication : class, IMvxApplication, new()
{
    protected override void RegisterSetup()
    {
        this.RegisterSetupType<TMvxIosSetup>();
    }
}
