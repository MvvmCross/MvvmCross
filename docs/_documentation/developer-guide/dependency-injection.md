---
title: "Dependency injection"
excerpt: ""
---
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

### What if... I want to mix Dynamic and Singleton types

If you use constructor injection, then for each dependency you can only ever receive a single instance. In some cases this may not be what you want. 

Take the following code: 

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

In this case, `FooSingleton` is registered as a singleton within MvvmCross, and when it is created it will receive a instance of `Bar`, which it will always use.

If instead, you wanted the `FooSingleton` to request a new instance each time then you could remove the constructor injection and instead use dynamic resolution - for example:

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

As another alternative, you could continue to use constructor injection, but could use an `IBarFactory` dependency instead of an `IBar` - e.g.:

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

Understanding object lifecycles in this type of situation - where some objects are dynamic and some are singletons - can be difficult, especially in large applications. To work with these type of objects it may help to adopt and follow patterns and naming conventions within your application - these may allow developers to more easily identify which interfaces should and should not be used dynamically.

  [1]: http://www.martinfowler.com/articles/injection.html
  [2]: http://joelabrahamsson.com/inversion-of-control-an-introduction-with-examples-in-net/
  [3]: https://github.com/slodge/MvvmCross-Presentations/blob/master/MvxDay/InterfaceDrivenDevelopment.pptx