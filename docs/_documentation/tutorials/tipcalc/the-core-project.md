---
layout: documentation
title: The Core Project
category: Tutorials
order: 2
---
MvvmCross applications normally consist on:

- A 'Core' project in the form of a .NET Standard library, which will contain all the shared code (so you want to maximize the amount of code placed in this project). The _Core_ will contain Models, ViewModels, Services, Converters, ...
- One 'Platform' project per targeted platform. These projects will contain some framework initialization code, Views and SDK dependant code.

Normally, you start development from the _Core_ project - and that's exactly what we'll do here.

Although it is recommended that you install any of the community made solution templates, we'll use a blank solution on this tutorial.

## Create the new .NET Standard library

Using Visual Studio, create your new `.NET Standard 2 Library` project using the File|New Project wizard.

Call it something like `TipCalc.Core` and name the solution `TipCalc`.

## Delete any auto-generated class

No-one really needs something like `Class1.cs` :)

## Install MvvmCross. Yey!

Open the Nuget Package Manager and search for the package `MvvmCross`.

If you don't really enjoy the NuGet UI experience, then you can alternatively open the Package Manager Console, and type:

    Install-Package MvvmCross

## Add the Tip Calculation Service

Create a folder called `Services`.

Within this folder create a new interface, which will be used for calculating tips:

```c#
namespace TipCalc.Core.Services
{
    public interface ICalculationService
    {
        double TipAmount(double subTotal, int generosity);
    }
}
```

Within the `Services` folder now create an implementation for the interface:

```c#
namespace TipCalc.Core.Services
{
    public class CalculationService : ICalculationService
    {
        public double TipAmount(double subTotal, int generosity)
        {
            return subTotal * ((double)generosity)/100.0;
        }
    }
}
```

This provides us with some simple business logic for our app. 

## Add the ViewModel

At a sketch level, we want a user interface that:

- Uses our calculation service to calculate the tip
- Has inputs of:
    - The current bill (the SubTotal)
    - A feeling for how much tip we'd like to leave (the generosity)
- Has an output for the calculated tip to leave

To represent this user interface we need to build a 'model' for it. In other words, we need a `ViewModel`.

Within MvvmCross, all ViewModels _should_ inherit from `MvxViewModel`.

So now let's create a folder called `ViewModels`, and inside of it a new class named `TipViewModel`. This is what it should look like:

```c#
using MvvmCross.ViewModels;
using TipCalc.Core.Services;
using System.Threading.Tasks;

namespace TipCalc.Core.ViewModels
{
    public class TipViewModel : MvxViewModel
    {
        readonly ICalculationService _calculationService;

        public TipViewModel(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            _subTotal = 100;
            _generosity = 10;

            Recalculate();
        }

        private double _subTotal;
        public double SubTotal
        {
            get => _subTotal;
            set
            {
                _subTotal = value;
                RaisePropertyChanged(() => SubTotal);

                Recalculate();
            }
        }

        private int _generosity;
        public int Generosity
        {
            get => _generosity;
            set
            {
                _generosity = value;
                RaisePropertyChanged(() => Generosity);

                Recalculate();
            }
        }

        private double _tip;
        public double Tip
        {
            get => _tip;
            set
            {
                _tip = value;
                RaisePropertyChanged(() => Tip);
            }
        }

        private void Recalculate()
        {
            Tip = _calculationService.TipAmount(SubTotal, Generosity);
        }
    }
}
```

It is possible that this `TipViewModel` will already make sense to you. If it does, then **skip ahead** to 'Add the App(lication)'. If not, then here are some further explanations:

- the `TipViewModel` is constructed with an `ICalculationService` service, which is injected using the MvvmCross Dependency Injection engine.

```c#
readonly ICalculationService _calculationService;

public TipViewModel(ICalculationService calculationService)
{
    _calculationService = calculationService;
}
```

- After construction, the `TipViewModel` runs the `Initialize` method, which is part of the [ViewModel lifecycle](https://www.mvvmcross.com/documentation/fundamentals/viewmodel-lifecycle) - during this it sets some initial values.

```c#
public override async Task Initialize()
{
    await base.Initialize();

    _subTotal = 100;
    _generosity = 10;

    Recalculate();
}
```

- The view data held within the `TipViewModel` is exposed through properties, where: 
    - Each of these properties is backed by a private member variable
    - Each of these properties has a getter and a setter
    - All of the set accessors call `RaisePropertyChanged` to tell the base `MvxViewModel` that the data has changed
    - The `SubTotal` and `Generosity` set accessors also call `Recalculate()`

```c#
private double _subTotal;
public double SubTotal
{
    get => _subTotal;
    set
    {
        _subTotal = value;
        RaisePropertyChanged(() => SubTotal);

        Recalculate();
    }
}

private int _generosity;
public int Generosity
{
    get => _generosity;
    set
    {
        _generosity = value;
        RaisePropertyChanged(() => Generosity);

        Recalculate();
    }
}

private double _tip;
public double Tip
{
    get => _tip;
    set
    {
        _tip = value;
        RaisePropertyChanged(() => Tip);
    }
}
```

- The private `Recalculate` method uses the `_calculationService` to update `Tip` from the current values of `SubTotal` and `Generosity`.

```c#
private void Recalculate()
{
    Tip = _calculationService.TipAmount(SubTotal, Generosity);
}
```

## Add the App(lication) class

With our `CalculationService` and our `TipViewModel` defined, we now just need to add the main `App` code. This code:

- Will sit in a single class within the root folder of our .NET Standard project. 
- Will inherit from the `MvxApplication` class
- Is normally just called `App`
- Is responsible for providing:
    - Registration of which interfaces and implementations the app uses
    - Registration of which `ViewModel` the `App` will show when it starts

'Registration' here means creating an entry on the 'Inversion of Control' Container - IoC -. This record tells the IoC Container what to do when anything asks for an instance of the registered interface.

Our "Tip Calculation" App class will register the `ICalculationService` as a dynamic service:

```c#
Mvx.RegisterType<ICalculationService, CalculationService>();
```

The previous line tells the IoC Container that whenever any code requests an `ICalculationService` reference, an object of type `CalculationService` should be created and returned.

Also note that the single static class `Mvx` acts as a single place for both registering and resolving interfaces and their implementations.

Within the App class we also decide that we want the app to start with the `TipViewModel`:

```c#
RegisterAppStart<TipViewModel>();
```

The previous line tells the MvvmCross framework that `TipViewModel` should be the first ViewModel / View pair that should appear on foreground when the app starts.

In summary, this is what App.cs should look like:

```c#
using MvvmCross;
using MvvmCross.ViewModels;
using TipCalc.Core.Services;
using TipCalc.Core.ViewModels;

namespace TipCalc.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterType<ICalculationService, CalculationService>();

            RegisterAppStart<TipViewModel>();
        }
    }
}
```

## Our Core project is complete :)

Just to recap the steps we've followed:

1. We created a new .NET Standard project
2. We added the MvvmCross libraries via NuGet
3. We added a `ICalculationService` interface and implementation pair
4. We added a `TipViewModel` which:
  - Inherits from `MvxViewModel`
  - Declares a dependency on `ICalculationService` on its constructor 
  - Presents a number of public properties each of which called `RaisePropertyChanged` internally
5. We added an `App` which:
  - Inherits from `MvxApplication`
  - Registers the `ICalculationService`/`CalculationService` pair
  - Registers a ViewModel to use for when the app starts

These are the same steps that you need to go through for every new MvvmCross application.

The next step is about building a first UI for our MvvmCross application.

[Next!](https://www.mvvmcross.com/documentation/tutorials/tipcalc/the-tip-calc-navigation)


## Side note: What is 'Inversion of Control'?

We won't go into depth here about what IoC - Inversion of Control - is.

Instead, we will just say that:

- Within each MvvmCross application, there is a very special object, which is a `singleton`.
- This `singleton` lives within the `Mvx` static class.
- The application startup code can use the `Mvx.Register...` methods to let `Mvx` know how to resolve certain requests during the lifetime of the app.
- After the registration has been done, then when any code asks for an `interface` implementation, it can do that by using the `Mvx.Resolve` methods.

One common pattern the app is also using is 'constructor injection':

- Our `TipViewModel` uses this pattern. 
- It presents a constructor like: `public TipViewModel(ICalculationService calculationService)`. 
- When the app is running a part of the MvvmCross framework called the `ViewModelLocator` is used to find and create ViewModels.
- When a `TipViewModel` is needed, the `ViewModelLocator` uses a call to `Mvx.IocConstruct` to create one.
- This `Mvx.IocConstruct` call creates the `TipViewModel` using the `ICalculationService` implementation that it finds using `Mvx.Resolve`

This is obviously only a very brief introduction.

