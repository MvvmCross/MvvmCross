---
layout: post
title: MvvmCross 5.3
date:   2017-10-13 11:37:35 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.3!

A new MvvmCross version is available on [NuGet](https://www.nuget.org/packages/MvvmCross/5.3.0)! Most of our focus went out to creating new ViewPresenters for Xamarin.Forms and improving the code around that. With that said we are proud to announce these new ViewPresenters! 
If you have been using the default presenters we provide, you will find them very familiar. They work using the same view attributes system and are super flexible. 
But that is not everything. With MvvmCross 5.3, you will now be able to mix and match Native Xamarin views with Xamarin.Forms pages using ViewModel navigation!

## Lets look at a sample view:

```c#
[MvxModalPresentation(WrapInNavigationPage = true, Title = "My Modal Page")]
public partial class MyModalViewPage : MvxContentPage<MyModalViewModel>
{
	public MyModalViewPage()
	{
		InitializeComponent();
	}

    // your code
}
```

## Supported attributes

- MvxContentPagePresentation
- MvxMasterDetailPagePresentation
- MvxModalPresentation
- MvxNavigationPagePresentation
- MvxTabbedPagePresentation
- MvxCarouselPagePresentation

There are also new MvvmCross base classes for all Xamarin.Forms Page types supporting DataBinding, ViewModels and more from code and layout files!

To take advantage of all those new features make sure to use the Xamarin.Forms base classes for Setup, Appdelegate and others.
# Extended MvvmCross bindings support

## Already possible

```c#
<Label mvx:La.ng="Text ThisIsLocalized" />
```

## New bindings

```c#
<Label Text="{mvx:MvxLang ThisIsLocalizedToo}" />
<Label Text="{mvx:MvxBind TextSource, Mode=OneTime, Converter=Language, ConverterParameter=ThisIsLocalizedThroughMvxBind}" />
```

# Presenters improvements

To make the mixing for Xamarin.Forms and Native Xamarin views possible we had to align all the presenters. We've now done that and they use the same structure with View Attributes.
At the same time we improved those presenters and fixed issues with Dialogs and the MvxNavigationService.

## Breaking changes for presenters using view attributes

Now iOS, Android, macOS and WPF ViewPresenters implement an interface called `IMvxAttributeViewPresenter`, which is a common contract for all presenters using the view attributes system.
As part of this refactoring, the dictionary that hosts the attribute types / show delegates has become a dictionary with two delegates: one for show, one for close. 
None of the behavior has changed, but since a few signatures have changed, if you are subclassing a presenter you will need to adjust your code. Please go straight to the presenter code if something doesn't make sense, or ask us for help :).

## Hooks for Fragments in Android

To get more control over your Fragment lifecycle - or Activity - and transitions, you can now override the folling methods (you can also modify your fragment transitions):

```c#
void OnFragmentChanged(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute);

void OnFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)

void OnFragmentPopped(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
```

## Android Activity transitions

In case you want to override the transition options between 2 activities you can override the folling method:

```c#
ActivityOptionsCompat CreateActivityTransitionOptions(Android.Content.Intent intent,MvxActivityPresentationAttribute attribute);
```

## Open-Generic registration on IoC

There are situations where you have an interface with a generic type parameter `IFoo<T>` and you have to register it in the IoC Container with different T types. One way to do this is to register it as many times as T types you have:

    Mvx.RegisterType<IFoo<Bar1>, Foo<Bar1>>();
    Mvx.RegisterType<IFoo<Bar2>, Foo<Bar2>>();
    Mvx.RegisterType<IFoo<Bar3>, Foo<Bar3>>();
    Mvx.RegisterType<IFoo<Bar4>, Foo<Bar4>>();

But this creates boilerplate code and in case you need another instance with a different T type, you have to register it as well, which is error prone. To solve this situation, you can now register this interface as *open-generic*, i.e. you don't specify the generic type parameter in neither the interface nor the implementation:
    
    Mvx.RegisterType<IFoo<>, Foo<>>();
    
Then at the moment of resolving the interface the implementation takes the same generic type parameter that the interface, e.g. if you resolve `var foo = Mvx.Resolve<IFoo<Bar1>>();` then `foo` will be of type `Foo<Bar1>`.
As you can see this give us more flexibility and scalability because we can effortlessly change the generic type parameters at the moment of resolving the interface and we don't need to add anything to register the interface with a new generic type parameter.

# Change Log

## [5.3.0](https://github.com/MvvmCross/MvvmCross/tree/5.3.0) (2017-10-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.2.1...5.3.0)

**Fixed bugs:**

- Pass MvxViewModelRequest.PresentationValues when navigating to Fragment on to it's parent's Activity when navigating [\#2237](https://github.com/MvvmCross/MvvmCross/issues/2237)
- Wrong host activity gets shown [\#2222](https://github.com/MvvmCross/MvvmCross/issues/2222)
- Close\(this\) is not working for Theme.Dialog on configuration change [\#1411](https://github.com/MvvmCross/MvvmCross/issues/1411)
- MvxWindowsViewPresenter swallowing exceptions thrown in Init [\#1309](https://github.com/MvvmCross/MvvmCross/issues/1309)

**Closed issues:**

- Make ViewAppeared and ViewAppearing return Task, for async purposes [\#2265](https://github.com/MvvmCross/MvvmCross/issues/2265)
- Unable to update nuget packages from version 5.1.1 to version 5.2.1 \(ANDROID\) [\#2253](https://github.com/MvvmCross/MvvmCross/issues/2253)
- \[Android\] Dialog doesn't survive screen orientation change [\#2246](https://github.com/MvvmCross/MvvmCross/issues/2246)
- \[Android\] Dismissing dialog via back button doesn't cancel Task [\#2245](https://github.com/MvvmCross/MvvmCross/issues/2245)
- Can't install MvvmCross 5.2.1 in Xamarin Forms PCL Project [\#2240](https://github.com/MvvmCross/MvvmCross/issues/2240)
- OnStop needs a null check  [\#2238](https://github.com/MvvmCross/MvvmCross/issues/2238)
- IMvxOverridePresentationAttribute not working of any Android view [\#2225](https://github.com/MvvmCross/MvvmCross/issues/2225)
- 2 Factor Login required for the MvvmCross organization from 22 September [\#2195](https://github.com/MvvmCross/MvvmCross/issues/2195)
- New nav service "deep link" conflict with viewmodel parameters when param is string [\#2080](https://github.com/MvvmCross/MvvmCross/issues/2080)
- MvxStringToTypeParser doesn't support decimal. [\#1635](https://github.com/MvvmCross/MvvmCross/issues/1635)
- 4.5MB of MvxFragmentAttribute generated when starting up https://github.com/Noires/MvxRecyclerViewLeakTest [\#1368](https://github.com/MvvmCross/MvvmCross/issues/1368)
- Parameterless Prepare\(\) not called [\#2233](https://github.com/MvvmCross/MvvmCross/issues/2233)
- Feature Suggestion - Side by side Xamarin iOS/Android and Xamarin Forms [\#1889](https://github.com/MvvmCross/MvvmCross/issues/1889)

**Merged pull requests:**

- Forms presenters [\#2269](https://github.com/MvvmCross/MvvmCross/pull/2269) ([Grrbrr404](https://github.com/Grrbrr404))
- Uwp handling BackRequestedEventArgs.Handled [\#2266](https://github.com/MvvmCross/MvvmCross/pull/2266) ([duglah](https://github.com/duglah))
- Xamarin-Sidebar for iOS updated [\#2264](https://github.com/MvvmCross/MvvmCross/pull/2264) ([DarthRamone](https://github.com/DarthRamone))
- Added mixed navigation scenario [\#2263](https://github.com/MvvmCross/MvvmCross/pull/2263) ([Grrbrr404](https://github.com/Grrbrr404))
- Fix Playground.iOS storyboard and add document for iOS UI approaches [\#2262](https://github.com/MvvmCross/MvvmCross/pull/2262) ([nmilcoff](https://github.com/nmilcoff))
- Update the docs for Android presenters for Fragment lifecycle [\#2260](https://github.com/MvvmCross/MvvmCross/pull/2260) ([KoenDeleij](https://github.com/KoenDeleij))
- Added documentation to IoC Open-Generic registration [\#2259](https://github.com/MvvmCross/MvvmCross/pull/2259) ([fedemkr](https://github.com/fedemkr))
- Update File plugin docs [\#2258](https://github.com/MvvmCross/MvvmCross/pull/2258) ([mauricemarkvoort](https://github.com/mauricemarkvoort))
- Fix close for DialogFragments when using MvxNavigationService [\#2256](https://github.com/MvvmCross/MvvmCross/pull/2256) ([nmilcoff](https://github.com/nmilcoff))
- Android ViewPresenter: Copy PresentationValues when Showing a new Activity Host [\#2255](https://github.com/MvvmCross/MvvmCross/pull/2255) ([nmilcoff](https://github.com/nmilcoff))
- 2236 - updated nuspec with Xamarin.Android.Support.Exif dependency [\#2254](https://github.com/MvvmCross/MvvmCross/pull/2254) ([msioen](https://github.com/msioen))
- Updated 3rd party plugins list [\#2251](https://github.com/MvvmCross/MvvmCross/pull/2251) ([lothrop](https://github.com/lothrop))
- Separate the transition logic [\#2250](https://github.com/MvvmCross/MvvmCross/pull/2250) ([KoenDeleij](https://github.com/KoenDeleij))
- Allow multiple attributes / hosts for Viewpager/TabLayout fragments [\#2249](https://github.com/MvvmCross/MvvmCross/pull/2249) ([nmilcoff](https://github.com/nmilcoff))
- Update docs: getting started/packages [\#2244](https://github.com/MvvmCross/MvvmCross/pull/2244) ([mauricemarkvoort](https://github.com/mauricemarkvoort))
- Added OpenGenerics feature to the IoC [\#2242](https://github.com/MvvmCross/MvvmCross/pull/2242) ([fedemkr](https://github.com/fedemkr))
- Added & fixed null checks [\#2241](https://github.com/MvvmCross/MvvmCross/pull/2241) ([F1nZeR](https://github.com/F1nZeR))
- MvxAppCompatActivity: Add null check for OnStop [\#2239](https://github.com/MvvmCross/MvvmCross/pull/2239) ([nmilcoff](https://github.com/nmilcoff))
- Update Forms packages [\#2235](https://github.com/MvvmCross/MvvmCross/pull/2235) ([martijn00](https://github.com/martijn00))
- \[WIP\] New Xamarin.Forms presenters [\#2187](https://github.com/MvvmCross/MvvmCross/pull/2187) ([martijn00](https://github.com/martijn00))
- Direct Forms bindings with MvxBind and MvxLang [\#1763](https://github.com/MvvmCross/MvvmCross/pull/1763) ([LRP-sgravel](https://github.com/LRP-sgravel))