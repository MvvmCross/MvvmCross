---
layout: documentation
title: iOS SceneDelegate
category: Platforms
---

To start your App using SceneDelegate. MvvmCross as of version 9.4.0 supports starting an iOS app using SceneDelegates.

For now there is only support for a single Window, the same as if using AppDelegate.

To adopt SceneDelegates you will need to do the following.

## 1. Add a `UIApplicationSceneManifest` in your Info.plist

To let iOS know that your App is now using SceneDelegate you need to add a section to your Info.plist file, telling it where
to look for the SceneDelegate:

```xml
<key>UIApplicationSceneManifest</key>
<dict>
    <key>UIApplicationSupportsMultipleScenes</key>
    <false/>
    <key>UISceneConfigurations</key>
    <dict>
        <key>UIWindowSceneSessionRoleApplication</key>
        <array>
            <dict>
                <key>UISceneDelegateClassName</key>
                <string>SceneDelegate</string>
                <key>UISceneConfigurationName</key>
                <string>MvxSceneConfiguration</string>
            </dict>
        </array>
    </dict>
</dict>
```

The default scene configuration used in `MvxSceneApplicationDelegate` is the name `MvxSceneConfiguration`. Also this configuration assumes you add a class called `SceneDelegate`. [See Step 3. further down in this post][#add-a-scenedelegate-class-using-mvxscenedelegate].

> You can customize this by overriding the `UISceneConfiguration GetConfiguration(UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)` method in `MvxSceneApplicationDelegate` and return your own `UISceneConfiguration`.

## 2. Switch from `MvxApplicationDelegate` to `MvxSceneApplicationDelegate`

To help you adopting and using SceneDelegate, MvvmCross provides a `MvxSceneApplicationDelegate` class which you can switch to. This can greatly simplify how your AppDelegate looks.

```csharp
[Register("AppDelegate")]
public class AppDelegate : MvxSceneApplicationDelegate;
```

If you have to overwrite `FinishedLaunching`, you can simply return `true` or `base.FinishedLaunching()`

## 3. Add a `SceneDelegate` class using `MvxSceneDelegate`

Add a new class called `SceneDelegate` which contains:

```csharp
[Register("SceneDelegate")]
public class SceneDelegate : MvxSceneDelegate<Setup, App>;
```

Make sure to have the `[Register("SceneDelegate")]` attribute, or else iOS will not find your SceneDelegate and not launch the App.

In this class we've exposed the following methods you can override:

```csharp
void WillConnect(
        UIScene scene,
        UISceneSession session,
        UISceneConnectionOptions connectionOptions)

void DidDisconnect(UIScene scene)

void DidBecomeActive(UIScene scene)

void WillResignActive(UIScene scene)

void WillEnterForeground(UIScene scene)

void DidEnterBackground(UIScene scene)
```

You can also access the `UIWindow` which was set up at startup using the `Window` property. The `WillConnect` method, will do that setup and also is where MvvmCross is launched.