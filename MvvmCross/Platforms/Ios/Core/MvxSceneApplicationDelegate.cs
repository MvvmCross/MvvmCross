// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using MvvmCross.Core;

namespace MvvmCross.Platforms.Ios.Core;

public abstract class MvxSceneApplicationDelegate : UIApplicationDelegate, IMvxLifetime
{
    public event EventHandler<MvxLifetimeEventArgs>? LifetimeChanged;
    public virtual string SceneConfigurationName { get; } = "MvxSceneConfiguration";
    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        return true;
    }

    public override UISceneConfiguration GetConfiguration(UIApplication application,
        UISceneSession connectingSceneSession, UISceneConnectionOptions options) =>
        new(SceneConfigurationName, connectingSceneSession.Role);
}
