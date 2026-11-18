---
layout: documentation
title: iOS multiple windows
category: Platforms
---

`MvxIosMultiWindowViewPresenter` gives each scene its own presenter and navigation state.
Application services are shared through one host. Existing presentation attributes apply within
the selected window.

## Setup

Set `UIApplicationSceneManifest/UIApplicationSupportsMultipleScenes` to `true` in `Info.plist`
and configure `UIWindowSceneSessionRoleApplication` with your registered scene delegate class.

Build and start one host in `AppDelegate.FinishedLaunching` using
`MvxIosHostBuilder.CreateBuilder()`, which registers the multi-window presenter.
Omit `StartWith`; each scene navigates to its own root. See the
[Playground app delegate](https://github.com/MvvmCross/MvvmCross/blob/develop/Projects/Playground/Playground.iOS/MultiWindow/MultiWindowAppDelegate.cs)
for service registration. `CreateBuilder(window)` retains single-window behavior.

In `MvxSceneDelegate.WillConnect`, create a `UIWindow` from the `UIWindowScene` and register it
using the session's persistent identifier:

```csharp
var presenter = Host.Services.GetRequiredService<MvxIosMultiWindowViewPresenter>();
presenter.RegisterWindow(session.PersistentIdentifier, Window);
```

Navigate to the scene's root, then call `Window.MakeKeyAndVisible()` if navigation succeeded
and the scene is still connected. In `DidDisconnect`, cancel scene-specific work, call
`presenter.UnregisterWindow(scene.Session.PersistentIdentifier)` and release the window.
Registration and unregistration must run on the UI thread; connected window IDs must be unique.
The [Playground scene delegate](https://github.com/MvvmCross/MvvmCross/blob/develop/Projects/Playground/Playground.iOS/MultiWindow/MultiWindowSceneDelegate.cs)
shows the complete lifecycle, including cancellation.

## Navigation

Pass the window ID to each ViewModel and include it in every navigation request:

```csharp
await navigation.Navigate<DetailsViewModel, string>(windowId,
    new MvxWindowPresentationBundle(windowId));

// Close routes by ViewModel ownership.
await navigation.Close(this);
```

| Request | Default behavior |
| --- | --- |
| Connected window ID | Present in that window. |
| Unknown or disconnected window ID | Return `false`. |
| No window ID, exactly one connected window | Present in that window. |
| No window ID, zero or multiple connected windows | Return `false`. |
| Close a known ViewModel instance | Route to its owning window; return `false` if disconnected. |
| Close an unknown ViewModel instance | Return `false`. |

Use separate ViewModel instances for each window. An instance belongs to one window connection
and cannot move to another, including a reconnection with the same ID. Share data through
application services. A second presentation of the same instance returns `false` while the first
is pending. Close ownership is tracked for `MvxViewModelInstanceRequest`, as used by
`IMvxNavigationService`.

For presentation hints, assign a `MvxWindowPresentationBundle` to the hint's `Body`.
Close hints use ViewModel ownership.

For popovers, set the anchor on the connected window's provider before navigating:

```csharp
var windowPresenter = presenter.GetWindowPresenter(windowId)!;
windowPresenter.PopoverPresentationSourceProvider!.SourceView = button;
```

Each window has its own provider. Use `SourceBarButtonItem` for a navigation-bar button.

## Customization

Register a subclass with `MvxIosHostBuilder.CreateBuilder().UsePresenter<MyMultiWindowPresenter>()`.
Override `SelectPresenter` for custom routing, `CreatePresenter(UIWindow)` for per-window
presentation, or `GetOwningPresenter` for ownership of views created outside ViewModel-first navigation.
`GetWindowPresenter(windowId)` returns a connected window's presenter for customization.

## Sample

Build the [iOS Playground](https://github.com/MvvmCross/MvvmCross/tree/develop/Projects/Playground/Playground.iOS)
multi-window demo on macOS:

```sh
dotnet build Projects/Playground/Playground.iOS/Playground.iOS.csproj \
  -p:MvxMultiWindowDemo=true -p:RuntimeIdentifier=iossimulator-arm64
```

The demo provides a shared counter, window-specific child and modal navigation, and delayed
navigation that retains its originating window. **New window** and **Close this window** use
UIKit scene APIs and are enabled when `UIApplication.SupportsMultipleScenes` is `true`.
Omit `MvxMultiWindowDemo` to build the single-window Playground.
