---
layout: documentation
title: Testing
category: Developer-guide
---
In order to test objects that make use of MvvmCross infrastructure, like ViewModels and the IoC container, there are some setup steps that are required. 

## Creating a Test Assembly

For general testing, a good cross-platform framework to choose is NUnit - http://nunit.org/

While not currently available in pure PCL form, NUnit can easily be used to build .Net 4.5 test projects which can be quickly run from your build environment, and it opens the door to other test mechanisms such as testing with [NUnitLite in Xamarin.iOS](http://docs.xamarin.com/guides/ios/deployment,_testing,_and_metrics/touch.unit).

For Mocking, frameworks such as `Moq` - http://code.google.com/p/moq/ - can be used in the 'traditional' .Net runtime environments. However, they cannot be used in Ahead-Of-Time compilation targets such as Xamarin.iOS. Instead, if you want to test within Xamarin.iOS on a real device, then manual object mocking must be used.

For a basic .Net 4.5 test setup for, for example, a ViewModel, you can:

- create a .Net 4.5 library project
- use Nuget to add references to NUnit and to Moq
- use Nuget or a local binary folder to add references to all of:
  * `MvvmCross.Core`
  * `MvvmCross`
  * `MvvmCross.Tests`

**Note:** The last assembly (`MvvmCross.Tests`) is key, as it is defines a base class `MvxIoCSupportingTest` which can help with initialising IoC setup. 

## Test class declaration and setup

Your test classes should inherit from [`MvxIoCSupportingTest`](https://github.com/slodge/MvvmCross/blob/v3/Cirrious/Test/Cirrious.MvvmCross.Test.Core/MvxIoCSupportingTest.cs)

Each test method should then call the `Setup` method:

```csharp
using MvvmCross.Test.Core;
using Moq;
using NUnit.Framework;

[TestFixture]
public class MyTest : MvxIoCSupportingTest
{
    [Test]
	public void TestViewModel()
	{
		base.Setup(); // from MvxIoCSupportingTest
        
        // your test code
	}
}
```

## Registering objects and additional setup

Now that you have the bare bones for your test to work, you can use the `Ioc` property to register any singleton or regular types within MvvmCross. 

Also, there's a special method named `AdditionalSetup()` which can be overridden to automatically do custom initialisation: 

```csharp
protected override void AdditionalSetup() 
{
    // an automatically Mocked service:
    var firstService = new Mock<IFirstService>();
    Ioc.RegisterSingleton<IFirstService>(firstService.Object);

    // a manually Mocked service:
    var secondService = new MockSecondService();
    Ioc.RegisterSingleton<ISecondService>(secondService);
}
```

When creating `ViewModel` or `Service` test objects, one common requirement is to provide a mock object which implements both `IMvxViewDispatcher` and `IMvxMainThreadDispatcher`. These interfaces are required for MvvmCross UI thread marshalling and for MvvmCross ViewModel navigation. This object can be implemented using a class like [`MockDispatcher`](https://github.com/slodge/NPlus1DaysOfMvvmCross/blob/master/N-29-TipCalcTest/TipCalcTest.Tests/MockDispatcher.cs):

    public class MockDispatcher
        : MvxMainThreadDispatcher
          , IMvxViewDispatcher
    {
        public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest>();
        public readonly List<MvxPresentationHint> Hints = new List<MvxPresentationHint>();

        public bool RequestMainThreadAction(Action action)
        {
            action();
            return true;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            Requests.Add(request);
            return true;
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            Hints.Add(hint);
            return true;
        }
    }

which can be registered as:

```csharp

protected MockDispatcher MockDispatcher { get; private set; }

protected override void AdditionalSetup() 
{
    MockDispatcher = new MockDispatcher();
    Ioc.RegisterSingleton<IMvxViewDispatcher>(MockDispatcher);
    Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(MockDispatcher);
}
```

If you are also using object based navigation - e.g. `ShowViewModel<MyViewModel>(new { id = 12 })` - then you may also need to register an `IMvxStringToTypeParser` parser to facilitate this:

```csharp

protected MockDispatcher MockDispatcher { get; private set; }

protected override void AdditionalSetup() 
{
       MockDispatcher = new MockDispatcher();
       Ioc.RegisterSingleton<IMvxViewDispatcher>(MockDispatcher);
       Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(MockDispatcher);

       // for navigation parsing
       Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());
}
```

## Links and other references


* The [Twitter Search](https://github.com/slodge/MvvmCross-Tutorials/tree/master/Sample%20-%20TwitterSearch) example has a [test project](https://github.com/slodge/MvvmCross-Tutorials/tree/master/Sample%20-%20TwitterSearch/TwitterSearch.Test) which can be used as reference as well

* There is a [N=29 video tutorial](http://slodge.blogspot.co.uk/2013/06/n29-testing-n1-days-of-mvvmcross.html) on testing

* [MvvmCross: Enable Unit-testing](http://blog.fire-development.com/2013/06/29/mvvmcross-enable-unit-testing/). A blog post presenting a slightly different approach, using a custom `MvvmCrossTestSetup` class.

* [MvvmCross: Unit-testing with AutoFixture](http://blog.fire-development.com/2013/06/29/mvvmcross-unit-testing-with-autofixture/). A blog post introducing a way to combine MvvmCross with [AutoFixture](https://github.com/AutoFixture/AutoFixture)