// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using MvvmCross.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Ios.Core;

public abstract class MvxApplicationDelegate : UIApplicationDelegate, IMvxApplicationDelegate
{
    public event EventHandler<MvxLifetimeEventArgs>? LifetimeChanged;

    public virtual UIWindow? MainWindow { get; set; }

    protected MvxApplicationDelegate()
    {
        RegisterSetup();
    }

    public override void WillEnterForeground(UIApplication application)
    {
        FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
    }

    public override void DidEnterBackground(UIApplication application)
    {
        FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
    }

    public override void WillTerminate(UIApplication application)
    {
        FireLifetimeChanged(MvxLifetimeEvent.Closing);
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        MainWindow ??= new UIWindow(UIScreen.MainScreen.Bounds);

        MvxIosSetupSingleton.EnsureSingletonAvailable(this, MainWindow).EnsureInitialized();

        RunAppStart(launchOptions);

        FireLifetimeChanged(MvxLifetimeEvent.Launching);
        return true;
    }

    protected virtual void RunAppStart(object? hint = null)
    {
        if (Mvx.IoCProvider?.TryResolve(out IMvxAppStart? startup) == true && startup is { IsStarted: false })
        {
            startup.Start(GetAppStartHint(hint));
        }

        MainWindow?.MakeKeyAndVisible();
    }

    protected virtual object? GetAppStartHint(object? hint = null)
    {
        return hint;
    }

    protected abstract void RegisterSetup();

    private void FireLifetimeChanged(MvxLifetimeEvent which)
    {
        var handler = LifetimeChanged;
        handler?.Invoke(this, new MvxLifetimeEventArgs(which));
    }
}

public abstract class MvxApplicationDelegate<TMvxIosSetup, TApplication> : MvxApplicationDelegate
    where TMvxIosSetup : MvxIosSetup<TApplication>, new()
    where TApplication : class, IMvxApplication, new()
{
    protected override void RegisterSetup()
    {
        this.RegisterSetupType<TMvxIosSetup>();
    }
}
