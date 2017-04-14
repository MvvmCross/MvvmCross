---
layout: post
title: MvvmCross 4.0 stable announcement!
date:   2016-02-05 11:37:35 +0100
categories: mvvmcross
---
## New website and logo

We are proud to present the new MvvmCross logo:
![alt text](http://i.imgur.com/BvdAtgT.png "New MvvmCross Logo")

MvvmCross have switched over to [Readme.io](http://readme.io) for their website, blog, and documentation. All of this is part of a bigger change in the general style and branding of MvvmCross.

Since the project is growing and more people are joining in, to help us make MvvmCross more awesome than ever before, we are keen to keep you updated on changes, new features, samples, and other useful things. [Readme.io](http://readme.io) helps us keeping track of changes to the documentation, and even **you** could propose an edit to any documentation page!

In addition to this we have restructured the GitHub repos. They are currently split up into:
- [MvvmCross](https://github.com/MvvmCross/MvvmCross)
- [Android-Support](https://github.com/MvvmCross/MvvmCross-AndroidSupport)
- [Forms](https://github.com/MvvmCross/MvvmCross-Forms)
- [Plugins](https://github.com/MvvmCross/MvvmCross-Plugins)
- [Samples](https://github.com/MvvmCross/MvvmCross-Samples)
- [iOS-Support](https://github.com/MvvmCross/MvvmCross-iOSSupport)

# New namespaces and package names

We've cleaned up file names, class names, method names, namespaces, and package names. Your existing code will break if you update to MvvmCross 4.0 (sorry). We've compiled [instructions](https://mvvmcross.readme.io/docs/upgrade-to-mvvmcross-40) to help your transition to MvvmCross 4.0 run as smoothly as possible.

# Android-support
We've developed a new system to show fragments with an Android MvvmCross App. Using Attributes on your MvxFragment classes you can show any fragment on any Activity!

```
[MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
[Register("example.droid.fragments.HomeFragment")]
public class HomeFragment : BaseFragment<HomeViewModel>
{

}
```

A complete sample of this is available in the [MvvmCross-AndroidSupport](https://github.com/MvvmCross/MvvmCross-AndroidSupport/tree/master/Samples) repo.

# Xamarin.Forms

MvvmCross-Forms is a derivative of MvvmCross that targets Xamarin.Forms cross-platform development. The objective is to make it easy for developers to fast track and get started with the combination of Xamarin.Forms and the MvvmCross framework and plugins. For MvvmCross to work with Xamarin.Forms we developed a set of components called Forms Presenters. Those presenters will open the Xamarin Forms page which belong to the ViewModel you open in the MvvmCross code. Xamarin.Forms is supported for Android, iOS, Windows Phone, Windows Store, and Windows UWP.

# UWP support

UWP support was added in one of the alpha versions of 4.0.0 and provides a base for your Windows UWP projects. For plugins both WindowsUWP and WindowsCommon plugins should be supported. 

# Xamarin.Mac

Mac support has been around for a while but the 4.0 release is the first to include support for Mac in the Nugets. If you're new to MvvmCross on Mac, simply create a PCL and a Xamarin.Mac (Unified) project in your solution and add the MvvmCross.StarterPack Nuget to those projects to have sample code created to help you get started.

# C#6 support / refactoring

After Microsoft released Visual Studio 2015, which comes with built-in C# 6 support, and Xamarin added it, too, in their latest stable release we decided to start using C# 6. We've gone through the entire code base to find and optimize the code so we can take advantage of the new functionality.

The philosophy behind this version is straightforward: Improve simple everyday coding scenarios, without adding much conceptual baggage. The features should make code lighter without making the language heavier.

The result of this is a lot less null reference exceptions, speed optimizations and better readability. Among the new code we use these C# 6 syntax now:

- Initializers for auto-properties
- Expression-bodied function members
- Null-conditional operators
- String interpolation
- `nameof` expressions
- Index initializers
- `await` in `catch` and `finally` blocks

# SQLitePCL

All plugins have been updated to the new namespace, but SQLitePCL has now replaced the deprecated SQLite plugin! In this PCL you can enjoy the power of portable plugins with the latest SQLite packages.

# Nuget 3.0 / Visual Studio 2015

With the new release we've updated the nuspecs to use the new features introduced in Nuget 3.x! This means you need at least nuget 2.8.7 to be able to install MvvmCross. This should not be a problem since Xamarin Studio and Visual Studio 2015 both have this included. If you have Visual Studio 2013 you can update Nuget easily by going to the tools menu and select "Updates and extensions", and update Nuget in there.

We have versioned DLL's of all the projects and put them to 4.0.0 to force updates to all external plugins. This is necessary because all the namespaces are changed and breaking changes have be done in the code.

# Roslyn analyzers

With the release of C# version 6 Microsoft has rewritten their C# and VB compilers from the ground up. They are known as the Microsoft .NET Compiler Platform (aka Roslyn). These compilers provide library builders to provide the compiler with a set of so-called Analyzers and CodeFixes. These can be shipped through a nuget package, and will light up in an IDE with support for these Roslyn based compilers.

**What does it do?**

In short it helps developers use the MvvmCross library the way it was intended to be used.
This first analyzer will identify the following code:

```
[Activity(...)]
class FirstView : MvxActivity
{
    public new FirstViewModel ViewModel => base.ViewModel as FirstViewModel;
}
```

And provide you with a IDE tooltip to transform the code into:

```
[Activity(...)]
class FirstView : MvxActivity<FirstViewModel> { ... }
```


# Updated samples and cross-platform menu sample

Most [samples](https://github.com/MvvmCross/MvvmCross-Samples) have been updated to use the new MvvmCross 4.0 nugets. In the folder ["OldSamples"](https://github.com/MvvmCross/MvvmCross-Samples/tree/master/OldSamples) you can still find some of the older samples, and we could use your help to migrate them to 4.0.

[XPlatformMenus](https://github.com/MvvmCross/MvvmCross-Samples/tree/master/XPlatformMenus) is a cross-platform navigation implementation showing how to implement hamburger menus on multiple platforms (currently Android and iOS). The demo applications also show a basic application structure including an initial login screen and custom application initialization approaches.

**Features**
- Hamburger menu navigation on all platforms
- Platform-agnostic services implemented in the Core
- Platform-specific services implemented in the platform projects
- Registration of platform-specific services during application start-up
- Custom application start-up (Core.AppStart)
- FluentLayout on iOS

# Support

Use [StackOverflow](http://stackoverflow.com/questions/tagged/mvvmcross) to find already asked questions, and post new questions with details about your problem. Do add tags to at least MvvmCross and Xamarin to be able to find it.

Use [Slack](https://xamarinchat.herokuapp.com/) to get in touch with us, and ask quick questions. After joining the Xamarin Chat, add the MvvmCross channel to find all other MvvmCross users.

Use the [Xamarin forums](http://forums.xamarin.com/) for specific questions where neither StackOverflow or Slack works for you.

# Contributions & Acknowledgements

We would like to thank everyone who has contributed to MvvmCross to bring closer towards the 4.0.0 version. We have had around 50 different names contributing in a smaller and bigger scale to the project. I have gathered the names I could find for the 4.0.0 alphas and betas and here is the list of names for you to appreciate:

Alex Petrenko
Alexander Marek
Alexandre Zollinger Chohfi
Alison Fernandes
Anatoliy Kolesnick
Andrey Kozhyn
Andy Joiner
Andy Lowe
Benedikt Neuhold
Bjarne Fisker Jensen
Boris Spinner
Clinton Rocksmith
Dave Leaver
Derek Beattie
Erik Baum
Erik van Seters
Geir Sagberg
Ggirard Distech
Greg Shackles
Guillaume Moisssaing
James Green
Jamie Clarke
Jeremy Kolb
JM Alfonsi
Johannes Sjøkvist
Jonathan Stoneman
Jose Pereira
Kerry W. Lothrop
Krishna Nadiminti
Marco Bellino
Martijn van Dijk
Matthew Whetton
Michael K. Daw
Michael Probst
Michal Mierzynski
Mikkel Jensen
Paul Leman
Pelle Ravn
Peter Burke
Przemyslaw Raciborski
Robert Baker
Sergei Ermakov
Shawn Castrianni
Stefan de Vogelaere
Stephan van Stekelenburg
Stuart Lodge
T Charriere
Tim Uy
Tomasz Cielecki

A big thanks to all that are on this list, but also to all those creating issues and hanging around on Slack and participating in various communities around spreading the word about MvvmCross. With that said. We are always looking for more contributors for MvvmCross. There are lots of areas we could use some help in. To mention some is production of samples, documentation, plugins and fixing bugs. Ideas for future releases are also very welcome in the Issues section of GitHub.

# Overview of all changes

You can find an overview of all changes, including small bug fixes in the tags for the separate repositories.

- [MvvmCross](https://github.com/MvvmCross/MvvmCross/issues?q=milestone%3A4.0.0+is%3Aclosed)
- [Android-Support](https://github.com/MvvmCross/MvvmCross-AndroidSupport/issues?q=milestone%3A4.0.0+is%3Aclosed)
- [Forms](https://github.com/MvvmCross/MvvmCross-Forms/issues?q=milestone%3A4.0.0+is%3Aclosed)
- [Plugins](https://github.com/MvvmCross/MvvmCross-Plugins/issues?q=milestone%3A4.0.0+is%3Aclosed)
- [Samples](https://github.com/MvvmCross/MvvmCross-Samples/issues?q=milestone%3A4.0.0+is%3Aclosed)
- [iOS-Support](https://github.com/MvvmCross/MvvmCross-iOSSupport/issues?q=milestone%3A4.0.0+is%3Aclosed)