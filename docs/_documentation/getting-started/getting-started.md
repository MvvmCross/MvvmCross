---
layout: documentation
title: Getting Started with MvvmCross
category: Getting-started
order: 1
---
NuGet | Continuous Integration
-------|-----------
[![NuGet](https://img.shields.io/nuget/v/MvvmCross.svg)](https://www.nuget.org/packages/MvvmCross/) | [![MyGet](https://img.shields.io/myget/mvvmcross/v/MvvmCross.svg)](https://www.myget.org/F/mvvmcross/api/v3/index.json)

# Introduction

MvvmCross is a cross-platform MVVM framework that enables developers to create powerful cross platform apps. It supports Xamarin.iOS, Xamarin.Android, Xamarin.Mac, Xamarin.Forms, Universal Windows Platform (UWP) and Windows Presentation Framework (WPF).

The high level features that MvvmCross provides you with are:
* MVVM architecture pattern
* Navigation system
* Data Binding
* Platform specifics support
* Inversion of Control container and Dependency Injection engine
* Lots of plugins for common functionalities
* Unit test helpers
* Complete flexibility - your app is King!

## Xamarin traditional vs Xamarin Forms

It doesn't matter if your next app is will be made using Xamarin traditional or Xamarin.Forms, because MvvmCross supports both approaches! 

# Creating a Project from Scratch

The easiest way to start a new MvvmCross based project is to use Plac3hold3r's [MvxScaffolding](https://github.com/Plac3hold3r/MvxScaffolding) templates. This guide will use the commandline version as it can be utilized by both Windows and macOS users.

*Window's users have the ability to use a Visual Studio Extension (complete with a GUI!) to create a new project.*

## Why use the Templates?

All of the boilerplate to have a running MvvmCross application is done for you.

The template creates a solution with the following projects and nugets:

* Core (.net standard 2.0)
    * MvvmCross 7.1.2
* Droid (Android Project)
    * MvvmCross 7.1.2
* iOS (iOS Project)
    * MvvmCross 7.1.2
    * [Cirrious.FluentLayout](https://github.com/FluentLayout/Cirrious.FluentLayout)
    
*The solution structure is not that different from the standard Xamarin template provided by Microsoft. The key difference here is the core project is a .net standard project instead of a shared project. Also mono.android.export is automatically added as a reference to the Droid project (a requirement of MvvmCross).*

### Core

* A project with a single ViewModel is created.
* MvxApplication class is created that will navigate to the ViewModel.

### iOS

* The template encourages the use of Cirrious.FluentLayout to create iOS views completely in code that utilize auto layout. 
* Base classes are provided for UIViewController that add methods to create, layout, and bind the view.
* The AppDelegate is subclassed from MvxApplicationDelegate (this is how MvvmCross connects to the shared MvxApplication class)
* An empty Setup class is provided. (This is where you initialize platform specific components.)

### Android

* The template sets up an android project that uses a base activity with the main view behind hosted as a fragment inside the base activity.
* Base classes are provided for Activity and Fragment with a property for the android layouts.
* Application and SplashScreen classes are created.
* An empty Setup class is provided. (This is where you initialize platform specific components.)

### Customization

If there's something you don't like about the default template there are plenty of options available. See the documentation [here](https://github.com/Plac3hold3r/MvxScaffolding/blob/develop/docs/template_dotnet_cli.md).

## Installing the Templates

### System Requirements

In order to make use of these templates you will need to have the following installed for Windows or macOS

**Required**

- .NET Core SDK 2.1.4+ ([Download SDK](https://www.microsoft.com/net/download))

**Optional**

- Xamarin Android SDK _(Recommended version 11+)_
- Xamarin iOS SDK _(Recommended version 14+)_
- UWP SDK _(**Windows Only**, recommended version 10.0.19041+)_

### Installation

To install the template run the `-i|--install` command

```text
dotnet new --install MvxScaffolding.Templates
```
## Using the Templates

At this point you need to decide if you wish to create a Xamarin.Forms or Native project.
While there's no reason you cannot use Forms, Native is where MvvmCross really shines.

| Project Type     | Pros                                 | Cons |
| ---------------- | ------------------------------------ | --------------------------------------------- |
| Native           | Easier for native mobile developers<br>Finer control of view implementation | Must create views for each platform        |
| Forms            | Easier for dotnet developers with no mobile experience<br>UI is shared via xaml implementation | UI is a subset of common elements from each platform (with exceptions) |

### MvxNative - Xamarin Native Template

To scaffold a new MvvmCross Xamarin application you must use the `mvxnative` command. To specify a name for the projects you can use the `-n|--name` option and `-sln|--solution-name` for the solution name.

___Example command___ to create a projects prefixed with `MyXamarinApp` and a solution file named `MyXamarinApp`

```text
dotnet new mvxnative --name MyXamarinApp --solution-name MyXamarinApp
```

For details on the available customisation options for the template use the `-h|--help` option

```text
dotnet new mvxnative --help
```

### MvxForms - Xamarin Forms Template

To scaffold a new MvvmCross Xamarin Forms application you must use the `mvxforms` command. To specify a name for the projects you can use the `-n|--name` option and `-sln|--solution-name` for the solution name.

___Example command___ to create a projects prefixed with `MyXamarinFormsApp` and a solution file named `MyXamarinApp`

```text
dotnet new mvxforms --name MyXamarinFormsApp --solution-name MyXamarinFormsApp
```

For details on the available customisation options for the template use the `-h|--help` option

```text
dotnet new mvxforms --help
```

[Source](https://github.com/Plac3hold3r/MvxScaffolding#dotnet-cli)

# Dive in to MvvmCross!

Please check [this document](https://www.mvvmcross.com/documentation/getting-started/mvvmcross-overview) to get an overview of how MvvmCross works.

You can also follow the [TipCalc tutorial](https://www.mvvmcross.com/documentation/tutorials/tipcalc/the-tip-calc-tutorial) which provides step by step instructions that will guide you through creating a simple tip calculator.

# Additional MvvmCross Templates

Other templates are available for Visual Studio / Visual Studio for Mac:

Name | Author | Link
---- | --------- | -------
XabluCross for MvvmCross | XabluCross | [Visual Studio](https://marketplace.visualstudio.com/items?itemName=XabluCross.XabluCrossVSPackage)
MvvmCross for Visual Studio | Jim Bennett | [Visual Studio](https://marketplace.visualstudio.com/items?itemName=JimBobBennett.MvvmCrossforVisualStudio-19327) - [Visual Studio for Mac](http://addins.monodevelop.com/Project/Index/227)
MvvmCross Plugin Template for Visual Studio | EShy | [Visual Studio](https://marketplace.visualstudio.com/items?itemName=EShy.MvvmCrossPluginTemplateforVisualStudio)
Xamarin MvvmCross Dreams | Artmdev | [Visual Studio](https://marketplace.visualstudio.com/items?itemName=Artmdev.XamarinMvvmCrossDREAMS)
Xamarin Forms 3 with MvvmCross 6 Solution Template | Paul Datsiuk | [Visual Studio](https://marketplace.visualstudio.com/items?itemName=PaulDatsiuk.XamarinFormswithMvvmCross5SolutionTemplate)
Ninja Coder For MvvmCross and Xamarin Forms | Ninja Coder for MvvmCross | [Visual Studio](https://marketplace.visualstudio.com/items?itemName=NinjaCoderforMvvmCross.NinjaCoderForMvvmCrossandXamarinForms)
MVXTemplates | Luke Pothier | [Visual Studio](https://marketplace.visualstudio.com/items?itemName=LukePothier.MVXTemplates)
Mvx Toolkit | Jeremy Tillman | [Visual Studio](https://marketplace.visualstudio.com/items?itemName=jtillman.mvxtoolkit)
MvxScaffolding | Plac3hold3r | [Visual Studio](https://marketplace.visualstudio.com/items?itemName=Plac3Hold3r.MvxScaffolding) - [dotnet template CLI](https://www.nuget.org/packages/MvxScaffolding.Templates/)

You can choose to download and install an extension manually, or you can get it from the Extension Manager in Visual Studio / the Add-In Gallery in Xamarin Studio (Visual Studio for Mac).

