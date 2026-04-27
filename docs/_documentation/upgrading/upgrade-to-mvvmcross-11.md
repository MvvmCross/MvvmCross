---
layout: documentation
title: Upgrade to MvvmCross 11
category: Upgrading
Order: 5
---

MvvmCross 11 replaces the custom IoC container (`IMvxIoCProvider`) with **Microsoft.Extensions.DependencyInjection (MEDI)** and introduces a new `MvxHostBuilder` / `MvxHost` startup model that replaces the old `App.cs` + `Setup.cs` pattern. This is the largest breaking change in MvvmCross history, but the new model is significantly simpler and aligns with the standard .NET hosting patterns you already know.

> This guide covers the most common migration scenarios. If you run into something not documented here, please open an issue or discussion on [GitHub][gh-issues].

---

## Quick comparison

**Android — before:**

```csharp
// App.cs (Core)
public class App : MvxApplication
{
    public override void Initialize()
    {
        CreatableTypes()
            .EndingWith("Service")
            .AsInterfaces()
            .RegisterAsLazySingleton();

        RegisterAppStart<RootViewModel>();
    }
}

// Setup.cs (Android)
public class Setup : MvxAndroidSetup<App>
{
    protected override ILoggerFactory CreateLogFactory() => new SerilogLoggerFactory();
    protected override ILoggerProvider CreateLogProvider() => new SerilogLoggerProvider();
}

// MainApplication.cs
public class MainApplication : MvxAndroidApplication<Setup, App> { ... }
```

**Android — after:**

```csharp
// MainApplication.cs (no App.cs or Setup.cs needed)
public class MainApplication : MvxAndroidApplication
{
    public override void OnCreate()
    {
        base.OnCreate();
        MvxAndroidHostBuilder.CreateBuilder(this)
            .StartWith<RootViewModel>()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IMyService, MyService>();
                services.AddLogging(l => l.AddSerilog());
                services.AddMvxViewModels(typeof(RootViewModel).Assembly);
                services.AddMvxAndroidViews(typeof(MainApplication).Assembly);
            })
            .Build()
            .Start()
            .GetAwaiter()
            .GetResult();
    }
}
```

---

## App.cs / MvxApplication

The `MvxApplication` base class and `IMvxApplication` interface are removed. Everything that used to live in `App.Initialize()` now moves into `ConfigureServices()` on the platform host builder.

| Old (App.cs) | New |
|---|---|
| `CreatableTypes().EndingWith("Service")...` | `services.AddSingleton<IMyService, MyService>()` — see [Service registration](#service-registration) |
| `Mvx.IoCProvider.RegisterSingleton<T>(...)` | `services.AddSingleton<T>(...)` |
| `Mvx.IoCProvider.RegisterType<T>(...)` | `services.AddTransient<T>(...)` |
| `RegisterAppStart<RootViewModel>()` | `builder.StartWith<RootViewModel>()` |
| `RegisterAppStart<MyAppStart>()` | `builder.UseAppStart<MyAppStart>()` |

---

## Service registration

### Explicit registration

Replace each service registered in `App.Initialize` with an explicit MEDI call inside `ConfigureServices`:

```csharp
// Before
Mvx.IoCProvider.RegisterSingleton<IAnalyticsService>(new AnalyticsService());
Mvx.IoCProvider.RegisterType<IProductService, ProductService>();

// After
services.AddSingleton<IAnalyticsService, AnalyticsService>();
services.AddTransient<IProductService, ProductService>();
```

### Convention-based registration (CreatableTypes replacement)

The `CreatableTypes().EndingWith("Service").AsInterfaces().RegisterAsLazySingleton()` pattern no longer exists. Register your services explicitly, or use the community library [Scrutor][scrutor] which adds assembly-scanning conventions on top of MEDI:

```csharp
// Using Scrutor for convention-based scanning
services.Scan(scan => scan
    .FromAssemblyOf<App>()
    .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
    .AsImplementedInterfaces()
    .WithSingletonLifetime());
```

---

## App start

### Default start (StartWith)

The simplest case — navigate to a single ViewModel on launch:

```csharp
// Before (App.cs)
RegisterAppStart<RootViewModel>();

// After (builder)
builder.StartWith<RootViewModel>();
```

### Custom app start (UseAppStart)

If you had a custom `MvxAppStart<T>` subclass to perform authentication checks or splash logic:

```csharp
// Before (App.cs)
RegisterAppStart<MyAppStart>();

// After (builder) — call before or after StartWith, last wins
builder.UseAppStart<MyAppStart>();
```

Your `MyAppStart` class should implement `IMvxAppStart` and can receive dependencies via constructor injection:

```csharp
public class MyAppStart : MvxAppStart<RootViewModel>
{
    private readonly IAuthService _authService;

    public MyAppStart(IMvxNavigationService navigationService, IAuthService authService)
        : base(navigationService)
    {
        _authService = authService;
    }

    protected override async Task NavigateToFirstViewModel(object? hint)
    {
        if (await _authService.IsLoggedInAsync())
            await NavigationService.Navigate<RootViewModel>();
        else
            await NavigationService.Navigate<LoginViewModel>();
    }
}
```

---

## Logging

Logging now uses the standard `Microsoft.Extensions.Logging` abstractions. The `CreateLogProvider()` and `CreateLogFactory()` virtual methods in Setup.cs are removed.

MvvmCross 11 automatically calls `services.AddLogging()` inside `AddMvxCore`, so `ILoggerFactory` and `ILogger<T>` are **always resolvable** — even if you never configure any logging provider. By default this gives you a no-op (silent) logger, which prevents framework classes such as `MvxNavigationViewModel` from throwing at DI resolution.

To attach real log output, call `AddLogging` in your `ConfigureServices` callback. Because `AddLogging` is idempotent, adding providers there simply layers them onto the already-registered factory:

```csharp
// Before (Setup.cs)
protected override ILoggerFactory CreateLogFactory() => new SerilogLoggerFactory();
protected override ILoggerProvider CreateLogProvider() => new SerilogLoggerProvider();

// After (ConfigureServices) — providers are added on top of the default no-op factory
services.AddLogging(logging =>
{
    logging.AddSerilog(dispose: true);
});
```

---

## Custom presenter

Override the view presenter registered by the platform host builder using the typed `UsePresenter` method. Call it **before** `StartWith` / `ConfigureServices` to keep the platform-typed fluent chain.

```csharp
// Before (platform Setup.cs)
protected override IMvxAndroidViewPresenter CreateViewPresenter()
    => new MyAndroidPresenter(AndroidViewAssemblies);

// After (Android builder)
MvxAndroidHostBuilder.CreateBuilder(this)
    .UsePresenter<MyAndroidPresenter>()  // call before StartWith
    .StartWith<RootViewModel>()
    .Build();
```

Platform-specific `UsePresenter` overloads exist for all platforms:

| Platform | Type constraint |
|---|---|
| Android | `IMvxAndroidViewPresenter` |
| iOS | `IMvxIosViewPresenter` |
| tvOS | `IMvxTvosViewPresenter` |
| macOS | `IMvxMacViewPresenter` |
| WPF | `IMvxWpfViewPresenter` |
| WinUI | `IMvxWindowsViewPresenter` |

A factory overload is also available when your presenter needs constructor arguments not in DI:

```csharp
.UsePresenter(sp => new MyAndroidPresenter(
    sp.GetRequiredService<IMvxAndroidActivityLifetimeListener>(),
    extraAssemblies))
```

---

## Custom navigation service

```csharp
// Before (Setup.cs)
protected override IMvxNavigationService CreateNavigationService(IMvxIoCProvider iocProvider)
    => new MyNavigationService(...);

// After (builder — call before or after StartWith, last wins)
builder.UseNavigationService<MyNavigationService>();

// Or with a factory for complex construction:
builder.UseNavigationService(sp => new MyNavigationService(
    sp.GetRequiredService<IMvxViewModelLoader>(),
    myExtraArg));
```

---

## Custom view dispatcher

The view dispatcher routes ViewModel show/close requests onto the main thread. Override it via `UseViewDispatcher` on any platform builder. This replaces the old `CreateViewDispatcher()` override in Setup.cs and has the same scope — only `IMvxViewDispatcher` is replaced; platform main-thread scheduling (`IMvxMainThreadAsyncDispatcher`, `IMvxMainThreadDispatcher`) remains unchanged.

```csharp
// Before (Setup.cs)
protected override IMvxViewDispatcher CreateViewDispatcher()
    => new MyViewDispatcher(Presenter);

// After (any builder — call before or after StartWith, last wins)
builder.UseViewDispatcher<MyViewDispatcher>();

// Or with a factory:
builder.UseViewDispatcher(sp => new MyViewDispatcher(
    sp.GetRequiredService<IMvxAndroidViewPresenter>()));
```

---

## Custom views container

The views container maps ViewModel types to their corresponding View types. Platform-specific `UseViewsContainer` overloads are available on the Android and iOS builders and replace the full alias chain so all internal consumers see the custom implementation.

```csharp
// Before (Setup.cs)
protected override IMvxViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider)
    => new MyAndroidViewsContainer(ApplicationContext);

// After (Android builder)
MvxAndroidHostBuilder.CreateBuilder(this)
    .UseViewsContainer<MyAndroidViewsContainer>()
    .StartWith<RootViewModel>()
    .Build();

// After (iOS builder)
MvxIosHostBuilder.CreateBuilder(window)
    .UseViewsContainer<MyIosViewsContainer>()
    .StartWith<RootViewModel>()
    .Build();
```

Platform type constraints for `UseViewsContainer`:

| Platform | Type constraint |
|---|---|
| Android | `IMvxAndroidViewsContainer, IMvxViewsContainer`¹ |
| iOS | `IMvxIosViewsContainer` |

> ¹ Android's `IMvxAndroidViewsContainer` does not inherit from `IMvxViewsContainer` (separate interface hierarchies), so both are required as constraints.

A factory overload is available when the container needs constructor arguments not in DI:

```csharp
.UseViewsContainer(sp => new MyAndroidViewsContainer(
    sp.GetRequiredService<Android.Content.Context>()))
```

---

## Initialization hooks (InitializeFirstChance / InitializeLastChance)

### Pre-build initialization (replaces InitializeFirstChance)

Anything that previously ran in `InitializeFirstChance` can move into `ConfigureServices`. At this point the container is not yet built so you register services rather than resolve them.

```csharp
// Before (Setup.cs)
protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
{
    iocProvider.RegisterSingleton<IEncryptionService>(new EncryptionService());
}

// After (ConfigureServices)
services.AddSingleton<IEncryptionService, EncryptionService>();
```

### Post-build initialization (replaces InitializeLastChance)

`InitializeLastChance` ran after the container was fully built. The equivalent in the new model is to subclass `MvxHost` and override `Start()`:

```csharp
// Before (Setup.cs)
protected override void InitializeLastChance(IMvxIoCProvider iocProvider)
{
    var cache = iocProvider.Resolve<IImageCache>();
    cache.Preload();
}

// After: subclass MvxHost
public class MyHost : MvxHost
{
    public MyHost(IServiceProvider services) : base(services) { }

    public override async Task Start()
    {
        // Runs after the container is built, before navigation starts
        Services.GetRequiredService<IImageCache>().Preload();
        await base.Start();
    }
}

// Wire it up by subclassing the platform builder
public class MyAndroidHostBuilder : MvxAndroidHostBuilder
{
    protected override MvxHost CreateHost(IServiceProvider sp) => new MyHost(sp);
}

// And use it instead of MvxAndroidHostBuilder.CreateBuilder(...)
```

---

## Binding customization

Binding is now registered via `AddMvxBindings` inside `ConfigureServices`. Each platform has its own extension with a typed configuration object that maps directly to the old `Fill*` virtual methods.

```csharp
// Before (platform Setup.cs)
protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
{
    base.FillTargetFactories(registry);
    registry.RegisterCustomBindingFactory<MyView>("MyProp", v => new MyViewBinding(v));
}

protected override void FillValueConverters(IMvxValueConverterRegistry registry)
{
    base.FillValueConverters(registry);
    registry.AddOrOverwrite("MyConverter", new MyValueConverter());
}

// After (ConfigureServices — Android example)
services.AddMvxBindings(config => config
    .FillTargetFactories(registry =>
        registry.RegisterCustomBindingFactory<MyView>(
            "MyProp", v => new MyViewBinding(v)))
    .FillValueConverters(registry =>
        registry.AddOrOverwrite("MyConverter", new MyValueConverter())));
```

All `Fill*` callbacks are available:

| Callback | Old virtual method |
|---|---|
| `FillTargetFactories` | `FillTargetFactories(IMvxTargetBindingFactoryRegistry)` |
| `FillValueConverters` | `FillValueConverters(IMvxValueConverterRegistry)` |
| `FillValueCombiners` | `FillValueCombiners(IMvxValueCombinerRegistry)` |
| `FillBindingNames` | `FillBindingNames(IMvxBindingNameRegistry)` |
| `FillViewTypes` (Android) | `FillViewTypes(IMvxTypeCache)` |
| `FillAxmlViewTypeResolver` (Android) | `FillAxmlViewTypeResolver(IMvxAxmlNameViewTypeResolver)` |
| `FillNamespaceListViewTypeResolver` (Android) | `FillNamespaceListViewTypeResolver(IMvxNamespaceListViewTypeResolver)` |

---

## Plugin system

The old `Plugin.cs` auto-loading system is removed. Each plugin now ships as a plain `IServiceCollection` extension method. Register only the plugins your app needs:

```csharp
// Before (App.cs)
public override void Initialize()
{
    // plugins loaded automatically via IMvxPluginManager
}

// After (ConfigureServices)
services.AddMvxMessenger();
services.AddMvxJson();
services.AddMvxVisibility();         // cross-platform
services.AddMvxVisibility();         // or the platform variant:
services.AddMvxAndroidVisibility();  // Android
services.AddMvxIosVisibility();      // iOS
services.AddMvxColor();
// etc.
```

---

## Mvx.IoCProvider static service locator

`Mvx.IoCProvider` is removed. The recommended replacement is **constructor injection** for any class that can participate in DI. For contexts that genuinely cannot (e.g. XAML markup extensions, legacy helpers), use `MvxHost.Current`:

```csharp
// Before
var service = Mvx.IoCProvider.Resolve<IMyService>();

// After — preferred: constructor injection
public class MyViewModel(IMyService service) : MvxViewModel { ... }

// After — escape hatch for non-DI contexts
var service = MvxHost.Current?.Services.GetRequiredService<IMyService>();
```

> **Note:** `MvxHost.Current` is null before `Start()` completes. Do not call it during service registration or container construction.

---

## View and ViewModel assembly registration

Platform-specific view registration is now explicit rather than convention-based:

```csharp
// Register ViewModels for name-based lookup (typically from the Core assembly)
services.AddMvxViewModels(typeof(RootViewModel).Assembly);

// Register platform views (Activities/Fragments on Android, ViewControllers on iOS, etc.)
services.AddMvxAndroidViews(typeof(MainApplication).Assembly);  // Android
services.AddMvxIosViews(typeof(SceneDelegate).Assembly);        // iOS
services.AddMvxWpfViews(typeof(App).Assembly);                  // WPF
services.AddMvxWinUiViews(typeof(App).Assembly);                // WinUI
```

---

## Removed APIs

The following types and patterns no longer exist in MvvmCross 11:

| Removed | Replacement |
|---|---|
| `MvxApplication` / `IMvxApplication` | `ConfigureServices()` on the host builder |
| `MvxSetup` / `IMvxSetup` / `MvxAndroidSetup<App>` etc. | Platform `MvxHostBuilder` subclasses |
| `IMvxIoCProvider` / `MvxIoCProvider` | `IServiceCollection` / `IServiceProvider` (MEDI) |
| `Mvx.IoCProvider` static | `MvxHost.Current?.Services` or constructor injection |
| `CreatableTypes()` | Manual registration or [Scrutor][scrutor] |
| `IMvxPlugin` / `IMvxPluginManager` | `AddMvx*` `IServiceCollection` extension methods |
| `IMvxBootstrapAction` / `MvxBootstrapRunner` | Call initialisation code directly in `ConfigureServices` |
| `MvxSetupSingleton` | `MvxHost.Current` |
| `MvxAndroidApplication<TSetup, TApp>` | `MvxAndroidApplication` |

---

## Complete example

The [Playground sample projects][playground] in the MvvmCross repository show a fully migrated app for each platform and are the best reference for real-world usage.

[gh-issues]: https://github.com/MvvmCross/MvvmCross/issues
[scrutor]: https://github.com/khellang/Scrutor
[playground]: https://github.com/MvvmCross/MvvmCross/tree/main/Projects/Playground
