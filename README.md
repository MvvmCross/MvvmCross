<h3 align="center">
  <a href="https://www.mvvmcross.com/" target="_blank">
    <img src="docs/assets/img/logo/MvvmCross-logo.png" alt="MvvmCross Logo" />
  </a>
</h3>

MvvmCross
==========
[![Twitter: @MvvmCross](https://img.shields.io/badge/contact-@MvvmCross-blue.svg?style=flat)](https://twitter.com/MvvmCross)
![license](https://img.shields.io/github/license/mvvmcross/mvvmcross.svg)
[![Build Status](https://dev.azure.com/Cheesebaron/MvvmCross/_apis/build/status/MvvmCross-develop?branchName=develop)](https://dev.azure.com/Cheesebaron/MvvmCross/_build/latest?definitionId=10&branchName=develop)
[![NuGet](https://img.shields.io/nuget/v/MvvmCross.svg)](https://www.nuget.org/packages/MvvmCross/)
[![NuGet Pre Release](https://img.shields.io/nuget/vpre/MvvmCross.svg)](https://www.nuget.org/packages/MvvmCross/)
[![MyGet](https://img.shields.io/myget/mvvmcross/v/MvvmCross.svg)](https://www.myget.org/F/mvvmcross/api/v3/index.json)
[![OpenCollective](https://opencollective.com/mvvmcross/backers/badge.svg)](#backers) 
[![OpenCollective](https://opencollective.com/mvvmcross/sponsors/badge.svg)](#sponsors)
[![CodeFactor](https://www.codefactor.io/repository/github/mvvmcross/mvvmcross/badge)](https://www.codefactor.io/repository/github/mvvmcross/mvvmcross)

👀 Check out [mvvmcross.com](https://www.mvvmcross.com) to get started with MvvmCross 👀

MvvmCross is a opinionated cross-platform MVVM framework. It enables developers to create apps using the MVVM pattern in the .NET ecosystem. We support Android, iOS, MacCatalyst, TvOS, macOS, WinUI, WPF. Using MvvmCross allows for better code sharing by allowing you to share behavior and business logic between platforms.

Among the features MvvmCross provides are:

- ViewModel to View bindings using own customizable binding engine, which allows you to create own binding definitions for own custom views
- ViewModel to ViewModel navigation, helps you share behavior on how and when to navigate
- Inversion of Control through Dependency Injection and Property Injection
- Plugin framework, which lets you plug-in cool stuff like GPS Location, Localization, Sensors, Binding Extensions and a huge selection of 3rd party community plug-ins

MvvmCross is extendable by you. We strive to let as much code be configurable and overridable, to let the developer decide how they want to use the framework. However, the framework is very usable without doing anything.

<hr />
<h4 align="center">
  Check out the <a href="https://www.mvvmcross.com/documentation">MvvmCross docs</a>
</h4>
<hr />

## Installation

Grab the latest [MvvmCross NuGet](https://www.nuget.org/packages/MvvmCross/) package and install in your solution.

> Install-Package MvvmCross

Make sure that both the shared core project and your application projects include the NuGet. For more details please visit the [Getting Started documentation,](https://www.mvvmcross.com/documentation/getting-started/getting-started) which also provides easier ways, through Visual Studio and Xamarin Studio plugins to install and manage MvvmCross in your project.

## Filing issues

We want to keep the GitHub issues list for bugs, features and other important project management tasks only. If you have questions please see the [Questions & support section below](#questions--support).

When filing issues, please select the appropriate [issue template](https://github.com/MvvmCross/MvvmCross/issues/new/choose). The best way to get your bug fixed is to be as detailed as you can be about the problem.
Providing a minimal git repository with a project showing how to reproduce the problem is ideal. Here are a couple of questions you can answer before filing a bug.

1. Did you try find your answer in the [documentation](https://www.mvvmcross.com)
2. Did you include a snippet of the broken code in the issue?
3. Can you reproduce the problem in a brand new project?
4. What are the _*EXACT*_ steps to reproduce this problem?
5. What platform(s) are you experiencing the problem on?

Remember GitHub issues support [markdown](https://github.github.com/github-flavored-markdown/). When filing bugs please make sure you check the formatting of the issue before clicking submit.

## Contributing code

We are happy to receive Pull Requests and code changes. Please read [CONTRIBUTING.md](https://github.com/MvvmCross/MvvmCross/blob/develop/CONTRIBUTING.md) for more information.

## Questions & support

* [StackOverflow](https://stackoverflow.com/questions/tagged/mvvmcross)
* [Discord](https://aka.ms/dotnet-discord) #mvvmcross channel

### Backers

Support us with a monthly donation and help us continue our activities. [[Become a backer](https://opencollective.com/mvvmcross#backer)]

[![Sponsors](https://opencollective.com/mvvmcross/sponsors.svg)](https://opencollective.com/mvvmcross)

### Sponsors

Become a sponsor and get your logo on our README on Github with a link to your site. [[Become a sponsor](https://opencollective.com/mvvmcross#sponsor)]

[![Backers](https://opencollective.com/mvvmcross/backers.svg)](https://opencollective.com/mvvmcross)

### Licensing

MvvmCross is licensed under the [MS-PL License](https://opensource.org/licenses/ms-pl.html)

* [MonoCross](https://code.google.com/p/monocross/) was the original starting point for this project, and was used as a reference under MIT
* Tiny bits of [MvvmLight](https://mvvmlight.codeplex.com/) are redistributed and modified under MIT
* Messenger ideas from [JonathanPeppers/XPlatUtils](https://github.com/jonathanpeppers/XPlatUtils) under Apache License Version 2.0, and from GrumpyDev/TinyMessenger under simple license of "THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY"
* [Color codes](https://github.com/mono/sysdrawing-coregraphics) under MIT License
* Some bits of [Mvvm.Async](https://github.com/StephenCleary/Mvvm.Async) are redistributed and modified under MIT License

### Acknowledgements

* Thanks to [McCannLondon](https://blogs.mccannlondon.co.uk/) for sponsoring the initial part of this work
* Thanks to [JetBrains](https://jetbrains.com) for a community Resharper license to use on this project

[so]: https://stackoverflow.com/questions/tagged/mvvmcross "MvvmCross on StackOverflow"
[xfmvx]: https://forums.xamarin.com/search?Search=mvvmcross "MvvmCross on Xamarin Forums"
[xf]: https://forums.xamarin.com "Xamarin Forums"

### .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
