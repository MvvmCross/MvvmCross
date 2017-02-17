---
layout: documentation
title: The Core Project
category: Tutorials
---
MvvmCross application's are normally structured with:

* one shared 'core' Portable Class Library (PCL) project 
  * containing as much code as possible: models, view models, services, converters, etc
* one UI project per platform 
  * each containing the bootstrap and view-specific code for that platform

Normally, you start development from the core project - and that's exactly what we'll do here.

To create the core, you can use the Visual Studio project template wizards, but here we'll instead build up a new project 'from empty'.

## Create the new Portable Class Library

Using Visual Studio, create your new `Class Library (Portable)` project using the File|New Project wizard.

Call it something like TipCalc.Core.csproj and name the solution TipCalc.

When asked to choose platforms, select .NET Framework 4.5, Windows 8, Windows Phone Silverlight 8, Windows Phone 8.1, Xamarin.Android and Xamarin.iOS - this will ensure that the PCL is in **Profile259**.  If Visual Studio stops you selecting these targets with the error 'The selection does not match any portable APIs' then use the workaround described here: http://danrigby.com/2014/04/10/windowsphone81-pcl-xamarin-fix/ 

Profile259 defines a small subset of .Net including:

* Microsoft.CSharp
* mscorelib
* System.Collections
* System.ComponentModel
* System.Core
* System.Diagnostics
* System
* System.Globalization
* System.IO
* System.Linq 
* System.Net
* System.ObjectModel
* System.Reflection
* System.Resources.ResourceManager
* System.Runtime
* System.Security.Principal
* System.ServiceModel.Web
* System.Text.Encoding
* System.Text.RegularExpressions
* System.Threading
* System.Windows
* System.Xml

To see the full list of assemblies, look in `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETPortable\v4.5\Profile\Profile259`

Importantly for us this Profile259 includes everything we need to build our Mvvm applications.

## Delete Class1.cs

No-one really needs a `Class1` :)

## Install MvvmCross

In the Package Manager Console, enter...

    Install-Package MvvmCross.Core

## Add the Tip Calculation service

Create a folder called 'Services'

Within this folder create a new Interface which will be used for calculating tips:
```c# 

namespace TipCalc.Core.Services\n{\n    public interface ICalculation\n    {\n        double TipAmount(double subTotal, int generosity);\n    }\n}",
```
Within this folder create an implementation of this interface:
```c# 

namespace TipCalc.Core.Services\n{\n    public class Calculation : ICalculation\n    {\n        public double TipAmount(double subTotal, int generosity)\n        {\n            return subTotal * ((double)generosity)/100.0;\n        }\n    }\n}",
```
This provides us with some simple business logic for our app

## Add the ViewModel

At a sketch level, we want a user interface that:

* uses:
  * our calculation service to calculate the tip
* has inputs of:
  * the current bill (the subTotal)
  * a feeling for how much tip we'd like to leave (the generosity)
* has output displays of:
  * the calculated tip to leave

To represent this user interface we need to build a 'model' for the user interface - which is, of course, a 'ViewModel'

Within MvvmCross, all ViewModels should inherit from `MvxViewModel`.

So now create a ViewModels folder in our project, and in this folder add a new `TipViewModel` class like:
```c# 

using MvvmCross.Core.ViewModels;\nusing TipCalc.Core.Services;\n\nnamespace TipCalc.Core.ViewModels\n{\n    public class TipViewModel : MvxViewModel\n    {\n        readonly ICalculation _calculation;\n\n        public TipViewModel(ICalculation calculation)\n        {\n            _calculation = calculation;\n        }\n\n        public override void Start()\n        {\n            _subTotal = 100;\n            _generosity = 10;\n            Recalculate();\n            base.Start();\n        }\n\n        double _subTotal;\n\n        public double SubTotal\n        {\n            get { return _subTotal; }\n            set\n            {\n                _subTotal = value;\n                RaisePropertyChanged(() => SubTotal);\n                Recalculate();\n            }\n        }\n\n        int _generosity;\n\n        public int Generosity\n        {\n            get { return _generosity; }\n            set\n            {\n                _generosity = value;\n                RaisePropertyChanged(() => Generosity);\n                Recalculate();\n            }\n        }\n\n        double _tip;\n\n        public double Tip\n        {\n            get { return _tip; }\n            set\n            {\n                _tip = value;\n                RaisePropertyChanged(() => Tip);\n            }\n        }\n\n        void Recalculate()\n        {\n            Tip = _calculation.TipAmount(SubTotal, Generosity);\n        }\n    }\n}",
```
For many of you, this `TipViewModel` will already make sense to you. If it does then **skip ahead** to 'Add the App(lication)'. If not, then here are some simple explanations:

* the `TipViewModel` is constructed with an `ICalculation` service
```c# 

readonly ICalculation _calculation;\n\npublic TipViewModel(ICalculation calculation)\n{\n    _calculation = calculation;\n}",
```
* after construction, the `TipViewModel` will be started - during this it sets some initial values.
```c# 

public override void Start()\n{\n    // set some start values\n    SubTotal = 100.0;\n    Generosity = 10;\n    Recalculate();\n    base.Start();\n}",
```
* the view data held within the `TipViewModel` is exposed through properties. 
  * Each of these properties is backed by a private member variable
  * Each of these properties has a get and a set 
  * The set accessor for `Tip` is marked private
  * All of the set accessors call `RaisePropertyChanged` to tell the base `MvxViewModel` that the data has changed
  * The `SubTotal` and `Generosity` set accessors also call `Recalculate()`
```c# 

double _subTotal;\n\npublic double SubTotal\n{\n    get { return _subTotal; }\n    set\n    {\n        _subTotal = value;\n        RaisePropertyChanged(() => SubTotal);\n        Recalculate();\n    }\n}\n\nint _generosity;\n\npublic int Generosity\n{\n    get { return _generosity; }\n    set\n    {\n        _generosity = value;\n        RaisePropertyChanged(() => Generosity);\n        Recalculate();\n    }\n}\n\ndouble _tip;\n\npublic double Tip\n{\n    get { return _tip; }\n    set\n    {\n        _tip = value;\n        RaisePropertyChanged(() => Tip);\n    }\n}",
```
* The `Recalculate` method uses the `_calculation` service to update `Tip` from the current values in `SubTotal` and `Generosity`
```c# 

void Recalculate()\n{\n    Tip = _calculation.TipAmount(SubTotal, Generosity);\n}",
```
## Add the App(lication)

With our `Calculation` service and `TipViewModel` defined, we now just need to add the main `App` code.

This code;

* will sit in a single class within the root folder of our PCL core project. 
* this class will inherits from the `MvxApplication` class
* this class is normally just called `App`
* this class is responsible for providing:
  * registration of which interfaces and implementations the app uses
  * registration of which `ViewModel` the `App` will show when it starts
  * control of how `ViewModel`s are located - although most applications normally just use the default implementation of this supplied by the base `MvxApplication` class.

'Registration' here means creating an 'Inversion of Control' - IoC - record for an interface. This IoC record tells the MvvmCross framework what to do when anything asks for an instance of that interface.

For our Tip Calculation app:

* we register the `Calculation` class to implement the `ICalculation` service
```c# 

Mvx.RegisterType<ICalculation, Calculation>();",
```
this line tells the MvvmCross framework that whenever any code requests an `ICalculation` reference, then the framework should create a new instance of `Calculation`. Note the single static class `Mvx` which acts as a single place for both registering and resolving interfaces and their implementations.

* we want the app to start with the `TipViewModel`
```c# 

var appStart = new MvxAppStart<TipViewModel>();\nMvx.RegisterSingleton<IMvxAppStart>(appStart);",
```
 this line tells the MvvmCross framework that whenever any code requests an `IMvxAppStart` reference, then the framework should return that same `appStart` instance.

So here's what App.cs looks like:
```c# 

using MvvmCross.Core.ViewModels;\nusing MvvmCross.Platform;\nusing TipCalc.Core.Services;\nusing TipCalc.Core.ViewModels;\n\nnamespace TipCalc.Core\n{\n    public class App : MvxApplication\n    {\n        public App()\n        {\n            Mvx.RegisterType<ICalculation, Calculation>();\n           Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<TipViewModel>());\n        }\n    }\n}",
      "language": "csharp",
      "name": "App.cs"
    }
  ]
}
```
## Note: What is 'Inversion of Control'?

We won't go into depth here about what IoC - Inversion of Control - is.

Instead, we will just say that:

* Within each MvvmCross application, there is a single special object - a `singleton`
* This `singleton` lives within the `Mvx` static class.
* The application startup code can use the `Mvx.Register` methods in order to specify what will implement `interface`s during the lifetime of the app.
* After this has been done, then later in the life when any code needs an `interface` implementation, then it can request one using the `Mvx.Resolve` methods.

One common pattern that is seen is 'constructor injection':

* Our `TipViewModel` uses this pattern. 
* It presents a constructor like: `public TipViewModel(ICalculation calculation)`. 
* When the app is running a part of the MvvmCross framework called the `ViewModelLocator` is used to find and create `ViewModel`s
* when a `TipViewModel` is needed, the `ViewModelLocator` uses a call to `Mvx.IocConstruct` to create one.
* This `Mvx.IocConstruct` call creates the `TipViewModel` using the `ICalculation` implementation that it finds using `Mvx.Resolve`

This is obviously only a very brief introduction.

If you would like to know more, please see look up some of the excellent tutorials out there on the Internet - like http://joelabrahamsson.com/inversion-of-control-an-introduction-with-examples-in-net/

## The Core project is complete :)

Just to recap the steps we've followed:

1. We created a new PCL project using Profile259
2. We added the MvvmCross libraries
3. We added a `ICalculation` interface and implementation pair
4. We added a `TipViewModel` which:
  * inherited from `MvxViewModel`
  * used `ICalculation` 
  * presented a number of public properties each of which called `RaisePropertyChanged`
5. We added an `App` which:
  * inherited from `MvxApplication`
  * registered the `ICalculation`/`Calculation` pair
  * registered a special start object for `IMvxAppStart`

These are the same steps that you need to go through for every new MvvmCross application.

## Moving on

Next we'll start looking at how to add a first UI to this MvvmCross application.