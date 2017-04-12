---
layout: documentation
title: Inversion of Control
category: Fundamentals
order: 6
---
Two key ideas that are used in MvvmCross are:

- the Service Locator pattern
- Inversion of Control

There are lots of articles and introductions available on this - some good starting places are [Martin Fowler's introduction][1] and [Joel Abrahamsson's IoC introduction][2]. I've also made some [animated slides][3] as a simple demonstration.


-----

Specifically within MvvmCross, we provide a single static class `Mvx` which acts as a single place for both registering and resolving interfaces and their implementations.

## Service Location - Registration and Resolution

The core idea of MvvmCross Service Location is that you can write classes and interfaces like:

```c#
public interface IFoo
{
    string Request();
}

public class Foo : IFoo
{
    public string Request()
    {
        return "Hello World";
    }
}
```

### Singleton Registration

With this pair written you could then register a `Foo` instance as a singleton which implements `IFoo` using:

        // every time someone needs an IFoo they will get the same one
        Mvx.RegisterSingleton<IFoo>(new Foo());

If you did this, then any code can call:

        var foo = Mvx.Resolve<IFoo>();

and every single call would return **the same instance** of Foo

An alternative syntax for singleton registration - especially useful when the registered type requires constructor dependency injection - is: 

        // every time someone needs an IFoo they will get the same one
        Mvx.ConstructAndRegisterSingleton<IFoo, Foo>(); 

### Lazy Singleton Registration

As a variation on this, you could register a lazy singleton. This is written

        // every time someone needs an IFoo they will get the same one
        // but we don't create it until someone asks for it
        Mvx.RegisterSingleton<IFoo>(() => new Foo());

In this case:

- no `Foo` is created initially
- the first time any code calls `Mvx.Resolve<IFoo>()` then a new `Foo` will be created and returned
- all subsequent calls will get the same instance that was created the first time

An alternative syntax for lazy singleton registration - especially useful when the registered type requires constructor dependency injection - is: 

        // every time someone needs an IFoo they will get the same one
        Mvx.LazyConstructAndRegisterSingleton<IFoo, Foo>(); 


### 'Dynamic' Registration

One final option, is that you can register the `IFoo` and `Foo` pair as:

        // every time someone needs an IFoo they will get a new one
        Mvx.RegisterType<IFoo, Foo>();

In this case, every call to `Mvx.Resolve<IFoo>()` will create a new `Foo` - every call will return a different `Foo`.

### Last-registered wins

If you create several implementations of an interface and register them all:

        Mvx.RegisterType<IFoo, Foo1>();
        Mvx.RegisterSingleton<IFoo>(new Foo2());
        Mvx.RegisterType<IFoo, Foo3>();

Then each call **replaces** the previous registration - so when a client calls `Mvx.Resolve<IFoo>()` then the most recent registration will be returned.

This can be useful for:

- overwriting default implementations
- replacing implementations depending on application state - e.g. after a user has been authenticated then you could replace an empty `IUserInfo` implementation with a real one.

### Bulk Registration by Convention

The default NuGet templates for MvvmCross contain a block of code in the core `App.cs` like:

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

This code uses Reflection to:

- find all classes in the Core assembly
    - which are `creatable` - i.e.:
        - have a public constructor
        - are not `abstract`
    - with names ending in Service
- find their interfaces
- register them as lazy singletons according to the interfaces they support

**Technical Note>** the lazy singleton implementation here is quite technical - it ensures that if a class implements `IOne` and `ITwo` then the same instance will be returned when resolving both `IOne` and `ITwo`.

The choice of name ending here - `Service` - and the choice to use Lazy singletons are only personal conventions. If you prefer to use other names or other lifetimes for your objects you can replace this code with a different call or with multiple calls like:

            CreatableTypes()
                .EndingWith("SingleFeed")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            CreatableTypes()
                .EndingWith("Generator")
                .AsInterfaces()
                .RegisterAsDynamic();
            CreatableTypes()
                .EndingWith("QuickSand")
                .AsInterfaces()
                .RegisterAsSingleton();

There you can also use additional `Linq` helper methods to help further define your registrations if you want to - e.g. `Inherits`, `Except`. `WithAttribute`, `Containing`, `InNamespace` ... e.g.

            CreatableTypes()
                .StartingWith("JDI")
                .InNamespace("MyApp.Core.HyperSpace")
                .WithAttribute(typeof(MySpecialAttribute))
                .AsInterfaces()
                .RegisterAsSingleton();

And you can also, of course, use the same type of registration logic on assemblies other than Core - e.g.:

            typeof(Reusable.Helpers.MyHelper).Assembly.CreatableTypes()
                .EndingWith("Helper")
                .AsInterfaces()
                .RegisterAsDynamic();

Alternatively, if you prefer not to use this Reflection based registration, then you can instead just manually register your implementations:

            Mvx.RegisterSingleton<IMixer>(new MyMixer());
            Mvx.RegisterSingleton<ICheese>(new MyCheese());
            Mvx.RegisterType<IBeer, Beer>();
            Mvx.RegisterType<IWine, Wine>();

The choice is **your's**

## Constructor Injection

As well as `Mvx.Resolve<T>`, the `Mvx` static class provides a reflection based mechanism to automatically resolve parameters during object construction.

For example, if we add a class like:

        public class Bar
        {
            public Bar(IFoo foo)
            {
                // do stuff
            }
        }

Then you can create this object using: 

        Mvx.IocConstruct<Bar>();

What happens during this call is: 

- MvvmCross:
  - uses Reflection to find the constructor of `Bar`
  - looks at the parameters for that constructor and sees it needs an `IFoo`
  - uses `Mvx.Resolve<IFoo>()` to get hold of the registered implementation for `IFoo`
  - uses Reflection to call the constructor with the `IFoo` parameter

### Constructor Injection and ViewModels

This "Constructor Injection" mechanism is used internally within MvvmCross when creating ViewModels. 

If you declare a ViewModel like:

         public class MyViewModel : MvxViewModel
         {
             public MyViewModel(IMvxJsonConverter jsonConverter, IMvxGeoLocationWatcher locationWatcher)
             {
                // ....
             }
         }

then MvvmCross will use the `Mvx` static class to resolve objects for `jsonConverter` and `locationWatcher` when a `MyViewModel` is created.

**This is important** because:

1. It allows you to easily provide different `locationWatcher` classes on different platforms (on iPhone you can use a watcher that talk to `CoreLocation`, on WindowsPhone you can use a watcher that talks to `System.Device.Location`
2. It allows you to easily provide mock implementations in your unit tests
3. It allows you to override default implementations - if you don't like the `Json.Net` implementation for Json, you can use a `ServiceStack.Text` implementation instead.

### Constructor Injection and Chaining

Internally, the `Mvx.Resolve<T>` mechanism uses constructor injection when new objects are needed.

This enables you to register implementations which depend on other interfaces like:

         public interface ITaxCalculator
         {
             double TaxDueFor(int customerId)
         }

         public class TaxCalculator
         {
             public TaxCalculator(ICustomerRepository customerRepository, IForeignExchange foreignExchange, ITaxRuleList taxRuleList)
             {
                 // code...
             }

             // code...
         }

If you then register this calculator as:

         Mvx.RegisterType<ITaxCalculator, TaxCalculator>();

Then when a client calls `Mvx.Resolve<ITaxCalculator>()` then what will happen is that MvvmCross will create a new `TaxCalculator` instance, resolving all of `ICustomerRepository` `IForeignExchange` and `ITaxRuleList` during the operation.

Further, this process is **recursive** - so if any of these returned objects requires another object  - e.g. if your `IForeignExchange` implementation requires a `IChargeCommission` object - then MvvmCross will use `Resolve` to provide an `IChargeCommission` instance for you.

## How do I use IoC when I need different implementations on different platforms?

Sometimes you need to use some platform specific functionality in your ViewModels. e.g. for example, you might want to get the current screen dimensions in your ViewModel - but there's no existing portable .Net call to do this.

When you want to include functionality like this, then there are two main choices:

1. Declare an interface in your core library, but then provide and register an implementation in each of your UI projects.
2. Use or create a *plugin*

### 1. PCL-Interface with Platform-Specific Implementation

In your core project, you can declare an interface and you can use that interface in your classes there - e.g.:

        public interface IScreenSize
        {
            double Height { get; }
            double Width { get; }
        }

        public class MyViewModel : MvxViewModel
        {
            private readonly IScreenSize _screenSize;

            public MyViewModel(IScreenSize screenSize)
            {
                 _screenSize = screenSize;
            }

            public double Ratio
            {
                get { return (_screenSize.Width / _screenSize.Height); }
            }
        }

In each UI project, you can then declare the platform-specific implementation for `IScreenSize` - e.g. a trivial example is:

        public class WindowsPhoneScreenSize : IScreenSize
        {
            public double Height { get { return 800.0; } }
            public double Width { get { return 480.0; } }
        }

You can then register these implementations in each of the platform-specific Setup files - e.g. you could override `MvxSetup.InitializeFirstChance` with

        protected override void InitializeFirstChance()
        {
            Mvx.RegisterSingleton<IScreenSize>(new WindowsPhoneScreenSize());
            base.InitializeFirstChance();
        }

With this done, then `MyViewModel` will get provided with the correct platform specific implementation of `IScreenSize` on each platform.

### 2. Use or create a *plugin*

A *Plugin* is an MvvmCross pattern for combining a PCL assembly, plus optionally some platform specific assemblies in order to package up some functionality.

This plugin layer is simply a pattern - some simple conventions - for naming related Assemblies, for including small `PluginLoader` and `Plugin` helper classes, and for using IoC. Through this pattern it allows functionality to be easily included, reused and tested across platforms and across applications.

For example, existing plugins include:

- a File plugin which provides access to `System.IO` type methods for manipulating files
- a Location plugin which provides access to GeoLocation information
- a Messenger plugin which provides access to a Messenger/Event Aggregator
- a PictureChooser plugin which provides access to the camera and to the media library
- a ResourceLoader plugin which provides a way to access resource files packaged within the .apk, .app or .ipa for the application
- a SQLite plugin which provides access to `SQLite-net` on all platforms.

####Plugin Use

If you want to see how these plugins can be used in your applications, then:

- the N+1 videos provide a good starting point - see http://mvvmcross.wordpress.com/ - especially :
  - N=8 - Location http://slodge.blogspot.co.uk/2013/05/n8-location-location-location-n1-days.html
  - N=9 - Messenger http://slodge.blogspot.co.uk/2013/05/n9-getting-message-n1-days-of-mvvmcross.html
  - N=10 - SQLite http://slodge.blogspot.co.uk/2013/05/n10-sqlite-persistent-data-storage-n1.html
  - N=12 -> N=17 - the Collect-A-Bull app http://slodge.blogspot.co.uk/2013/05/n12-collect-bull-full-app-part-1-n1.html
- see the [Plugins](https://github.com/slodge/MvvmCross/wiki/MvvmCross-plugins) article 

####Plugin Authoring

Writing plugins is easy to do, but can feel a bit daunting at first.

The key steps are:

1. Create the main PCL Assembly for the plugin - this should include:
   - the interfaces your plugin will register
   - any shared portable code (which may include implementations of one or more of the interfaces)
   - a special `PluginLoader` class which MvvmCross will use to start the plugin

2. Optionally create platform specific assemblies which:
   - is named the same as the main assembly but with a platform specific extension (.Droid, .WindowsPhone, etc(
   - contains 
       - any platform specific interface implementations
       - a special `Plugin` class which MvvmCross will use to start this platform-specific extension

3. Optionally provide extras like documentation and nuget packaging which will make the plugin easier to reuse.

I'm not going to go into any more detail on writing plugins here. 

If you'd like to see more about writing your own plugin, then:

- see the [Plugins](https://github.com/slodge/MvvmCross/wiki/MvvmCross-plugins) article 
- there's a presentation on this at https://speakerdeck.com/cirrious/plugins-in-mvvmcross
- there's a sample which creates a `Vibrate` plugin at https://github.com/slodge/MvvmCross-Tutorials/tree/master/GoodVibrations 


## What if...

### What if... I don't want to use Service Location or IoC

If you don't want to use this in your code, then don't.

Simply remove the `CreatableTypes()...` code from App.cs and then use 'normal code' in your ViewModels - e.g.:

         public class MyViewModel : MvxViewModel
         {
             private readonly ITaxService _taxService;

             public MyViewModel()
             {
                 _taxService = new TaxService();
             }
         }

### What if... I want to use a different Service Location or IoC mechanism

There are lots of **excellent** libraries out there including AutoFac, Funq, MEF, OpenNetCF, TinyIoC and many, many more!

If you want to replace the MvvmCross implementation, then you'll need to:

- write some kind of `Adapter` layer to provide their service location code as an `IMvxIoCProvider`
- override `CreateIocProvider` in your `Setup` class to provide this alternative `IMvxIoCProvider` implementation.

Alternatively, you may be able to organise a hybrid situation - where two IoC/ServiceLocation systems exist side-by-side.

### What if... I want to use Property Injection as an IoC mechanism?

From v3.1 of MvvmCross, Property Injection is supported in the default IoC container.

To enable this injection, you need to change your app `Setup` so that it overrides `CreateIocOptions()`

There are currently two ways you inject into properties:

- inject only into marked interface properties - `MvxInjectInterfaceProperties`
- inject into all interface properties - `AllInterfaceProperties`

In both cases, MvvmCross will perform the property injection as soon as construction is completed.

One further option is that the MvvmCross `IMvxPropertyInjector` can be used independently - you can choose to use this on your options if you want to.

#### `MvxInjectInterfaceProperties`

If you override options as:

      protected override IMvxIoCOptions CreateIocOptions()
      {
          return new MvxIocOptions() 
          {
                    PropertyInjectorOptions = MvxPropertyInjectorOptions.MvxInject
          }; 
      }

then this will enable injection into `public` writeable properties which are declared as interfaces and which have an `MvxInject` attribute - e.g. `Foo` below:

       public class MyViewModel : MvxViewModel
       {
           [MvxInject]
           public IFooService Foo { get; set; }
       }

#### `AllInterfaceProperties`

If you override options as:

      protected override IMvxIoCOptions CreateIocOptions()
      {
          return new MvxIocOptions() 
          {
                    PropertyInjectorOptions = MvxPropertyInjectorOptions.All
          }; 
      }

then this will enable injection into `public` writeable properties which are declared as interfaces - e.g. `Bar` below:

       public class MyViewModel : MvxViewModel
       {
           public IBarService Bar { get; set; }
       }

#### Using `IMvxPropertyInjector` directly

You can inject into your own objects independently of the MvvmCross IoC Container.

To do this, you can use:

     var injector = new MvxPropertyInjector()

or 

     var injector = Mvx.Resolve<IMvxPropertyInjector>();

and then:

     var foo = new Foo();
     injector.Inject(foo, MvxPropertyInjectorOptions.MvxInject);

or:

     var bar = new Bar();
     injector.Inject(bar, MvxPropertyInjectorOptions.All);

### What happens if... A needs a B which needs an A which ... ?

Circular references are a tricky problem in object construction - regardless of whether or not you use dependency injection. 

For example:

        public interface IA { }
        public interface IB { }
        public class A : IA
        { 
           public A(IB b) { } 
        }
        public class B : IB
        {
           public B(IA a) { } 
        }

At runtime, by default MvvmCross's Ioc will throw an `MvxIoCResolveException` from `Resolve` or return `false` from `TryResolve` if it detects recursion has occurred.

Generally in this situation you need to refactor your code to remove the circular dependency - for example see one suggestion in [http://stackoverflow.com/questions/1453128/is-there-a-good-proper-way-of-solving-the-dependency-injection-loop-problem-in-t/1453242#1453242](Stack Overflow) - other stackoverflow Q&As may also help.

However, if you feel the MvvmCross detection is wrong - if your app has some behaviour which means it can survive the recursive dependency - then you can turn this detection off if you want to using the options - e.g:

            var options = new MvxIocOptions()
            {
                TryToDetectDynamicCircularReferences = false
                TryToDetectSingletonCircularReferences = false
            };
            var instance = MvxSimpleIoCContainer.Initialize(options);

**Note:** in the event of recursion causing a stack overflow, some mobile runtimes will **not** throw a `StackOverlowException` - but will instead simply exit without warning - this situation can be hard to debug.

### What if... I want advanced IoC features like child containers

The IoC container in MvvmCross is designed to be quite lightweight and is targeted at a level of functionality required in the mobile applications I have built.

If you need more advanced/complex functionality, then you may need to use a different provider or a different approach - some suggestions for this are discussed in: http://stackoverflow.com/questions/16514691/child-containers-in-mvvmcross-ioc


### What if... I want to mix Dynamic and Singleton types

If you use constructor injection, then for each dependency you can only ever receive a single instance. In some cases this may not be what you want. 

Take the following code: 

```c#
// Registered with Mvx.RegisterType<IBar, Bar>();
public class Bar : IBar
{
    public void DoStuff()
    {
        // implementation
    }
}

// Registered with Mvx.ConstructAndRegisterSingleton<IFooSingleton, FooSingleton>();
public class FooSingleton : IFooSingleton
{
    private readonly IBar _bar;

    public FooSingleton(IBar bar)
    {
        // This "bar" instance will be held forever,
        // no other instance will be created for the
        // lifetime of this singleton
        _bar = bar;
    }

    public void DoFoo()
    {
        _bar.DoStuff();
    }
}
```

In this case, `FooSingleton` is registered as a singleton within MvvmCross, and when it is created it will receive a instance of `Bar`, which it will always use.

If instead, you wanted the `FooSingleton` to request a new instance each time then you could remove the constructor injection and instead use dynamic resolution - for example:

```c#
public class FooSingleton : IFooSingleton
{
    public FooSingleton()
    {
        // No "IBar" dependency in the constructor
    }

    public void DoFoo()
    {
        var bar = Mvx.Resolve<IBar>();
        bar.DoStuff();
    }
}
```

As another alternative, you could continue to use constructor injection, but could use an `IBarFactory` dependency instead of an `IBar` - e.g.:

```c#
public class FooSingleton : IFooSingleton
{
    private readonly IFactory<IBar> _barFactory;

    public FooSingleton(IFactory<IBar> barFactory)
    {
        _barFactory = barFactory;
    }

    public void DoFoo()
    {
        var bar = _barFactory.Create();
        bar.DoStuff();
    }
}
```

Understanding object lifecycles in this type of situation - where some objects are dynamic and some are singletons - can be difficult, especially in large applications. To work with these type of objects it may help to adopt and follow patterns and naming conventions within your application - these may allow developers to more easily identify which interfaces should and should not be used dynamically.

  [1]: http://www.martinfowler.com/articles/injection.html
  [2]: http://joelabrahamsson.com/inversion-of-control-an-introduction-with-examples-in-net/
  [3]: https://github.com/slodge/MvvmCross-Presentations/blob/master/MvxDay/InterfaceDrivenDevelopment.pptx

