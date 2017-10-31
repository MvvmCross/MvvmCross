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

## [5.2.1](https://github.com/MvvmCross/MvvmCross/tree/5.2.1) (2017-09-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.2...5.2.1)

**Fixed bugs:**

- Wrong ViewModel data in cached Fragment [\#1986](https://github.com/MvvmCross/MvvmCross/issues/1986)
- Binding in an MvxFragment is incorrectly resetting modified values [\#1264](https://github.com/MvvmCross/MvvmCross/issues/1264)

**Closed issues:**

- Exception in MvxAppCompatDialogFragment\<T\> [\#2220](https://github.com/MvvmCross/MvvmCross/issues/2220)
- Initialize is not called [\#2212](https://github.com/MvvmCross/MvvmCross/issues/2212)
- MvxNSSwitchOnTargetBinding appears in MvvmCross.Mac and MvvmCross.Binding.Mac [\#2205](https://github.com/MvvmCross/MvvmCross/issues/2205)
- WindowController from Storyboard \(Mac\) gets disposed [\#2198](https://github.com/MvvmCross/MvvmCross/issues/2198)
- Programmatically switching tabbed viewmodels from RootViewModel - Mac [\#2191](https://github.com/MvvmCross/MvvmCross/issues/2191)
- Where is that IFactory\<T\>? [\#2186](https://github.com/MvvmCross/MvvmCross/issues/2186)
- Hang when awaiting code in Initialize in 5.2 [\#2182](https://github.com/MvvmCross/MvvmCross/issues/2182)
- Support Toolbar and Unified Toolbar bindings by view for Mac [\#2180](https://github.com/MvvmCross/MvvmCross/issues/2180)
- Missing StarterPack for MvvmCross.Forms [\#2073](https://github.com/MvvmCross/MvvmCross/issues/2073)
- Wrong behavior on Move in MvxCollectionViewSourceAnimated [\#2061](https://github.com/MvvmCross/MvvmCross/issues/2061)
- Fragment viewmodel life-cycle events are not called if viewmodel has saved state and calling ShowViewModel\<FragmentViewModel\>\(data\) with data [\#1373](https://github.com/MvvmCross/MvvmCross/issues/1373)

**Merged pull requests:**

- Documentation improvements for ViewModel lifecycle and Android Presenter [\#2229](https://github.com/MvvmCross/MvvmCross/pull/2229) ([nmilcoff](https://github.com/nmilcoff))
- Fix and workaround IMvxOverridePresentationAttribute Android [\#2226](https://github.com/MvvmCross/MvvmCross/pull/2226) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Add obsolete attribute [\#2224](https://github.com/MvvmCross/MvvmCross/pull/2224) ([jz5](https://github.com/jz5))
- Add ctor for generic fragment [\#2221](https://github.com/MvvmCross/MvvmCross/pull/2221) ([martijn00](https://github.com/martijn00))
- MvvmCross Forms StarterPack update [\#2219](https://github.com/MvvmCross/MvvmCross/pull/2219) ([mauricemarkvoort](https://github.com/mauricemarkvoort))
- MvxListview for Forms with "ItemClick command" [\#2218](https://github.com/MvvmCross/MvvmCross/pull/2218) ([MarcBruins](https://github.com/MarcBruins))
- Updated to new VS2017 extension [\#2217](https://github.com/MvvmCross/MvvmCross/pull/2217) ([jimbobbennett](https://github.com/jimbobbennett))
- nspopup goodies [\#2214](https://github.com/MvvmCross/MvvmCross/pull/2214) ([tofutim](https://github.com/tofutim))
- Make sure the binding context is created when loading from nib [\#2213](https://github.com/MvvmCross/MvvmCross/pull/2213) ([mvanbeusekom](https://github.com/mvanbeusekom))
- fix missing OneWay on toggle [\#2209](https://github.com/MvvmCross/MvvmCross/pull/2209) ([tofutim](https://github.com/tofutim))
- change FillBindingNames argument to 'registry' to be consistent [\#2208](https://github.com/MvvmCross/MvvmCross/pull/2208) ([tofutim](https://github.com/tofutim))
- add NSMenuItem command and state binding - Mac [\#2207](https://github.com/MvvmCross/MvvmCross/pull/2207) ([tofutim](https://github.com/tofutim))
- remove stray NSSwitchOnTargetBinding [\#2206](https://github.com/MvvmCross/MvvmCross/pull/2206) ([tofutim](https://github.com/tofutim))
- Fixed typo [\#2203](https://github.com/MvvmCross/MvvmCross/pull/2203) ([Digifais](https://github.com/Digifais))
- Add weak table to tie NSWindow and NSWindowController [\#2201](https://github.com/MvvmCross/MvvmCross/pull/2201) ([tofutim](https://github.com/tofutim))
- Android ViewPresenter: Simple caching strategy for fragments [\#2200](https://github.com/MvvmCross/MvvmCross/pull/2200) ([nmilcoff](https://github.com/nmilcoff))
- Add ability to set a default value for an attribute [\#2197](https://github.com/MvvmCross/MvvmCross/pull/2197) ([martijn00](https://github.com/martijn00))
- Cache parser variable to improve performance [\#2196](https://github.com/MvvmCross/MvvmCross/pull/2196) ([willsb](https://github.com/willsb))
- Re-add the generic navigate to the interface for unit-tests [\#2194](https://github.com/MvvmCross/MvvmCross/pull/2194) ([martijn00](https://github.com/martijn00))
- Feature/mactabviewcontroller [\#2193](https://github.com/MvvmCross/MvvmCross/pull/2193) ([tofutim](https://github.com/tofutim))
- Move creating of attributes to method [\#2190](https://github.com/MvvmCross/MvvmCross/pull/2190) ([martijn00](https://github.com/martijn00))
- Fix hang in Initialize when called from Navigation app start [\#2189](https://github.com/MvvmCross/MvvmCross/pull/2189) ([jimbobbennett](https://github.com/jimbobbennett))
- update MvxWindowPresentation to support creation from Storyboard for Mac [\#2185](https://github.com/MvvmCross/MvvmCross/pull/2185) ([tofutim](https://github.com/tofutim))
- Add a constructor with a binding context [\#2184](https://github.com/MvvmCross/MvvmCross/pull/2184) ([wh1t3cAt1k](https://github.com/wh1t3cAt1k))
- Update messenger.md [\#2183](https://github.com/MvvmCross/MvvmCross/pull/2183) ([mauricemarkvoort](https://github.com/mauricemarkvoort))
