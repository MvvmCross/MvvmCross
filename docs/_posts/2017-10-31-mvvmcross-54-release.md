---
layout: post
title: MvvmCross 5.4
date:   2017-10-31 14:00:00 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.4!

A new MvvmCross version is available on [NuGet](https://www.nuget.org/packages/MvvmCross/5.4.0)! You can always find the latest [changelog in the root of the repository](https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md) to see what has changed.

## Forms
We have had a couple of fixes for our Xamarin.Forms support
- navigation between native and Forms views has been improved
- fixes crashes when navigating between Pages with `WrapInNavigationPage` set
- when `WrapInNavigationPage` was set, title was missing
- binding markup extensions have been fixed so that you can use `Target="mvx:MvxBind Source` and `Target="mvx:MvxLang LocalizationKey"`
- design time checker added for MvvmCross bindings
- on Android you can now provide your own Resource Assembly if the one we guess for you is wrong. See the [documentation for more information on how to override this behavior](https://www.mvvmcross.com/documentation/platform/forms/xamarin-forms-customization)

## iOS
- Fixes in Sidebar navigation initialization where hamburger icon sometimes disappeared

## Android
- Added missing constructor in generic version of `MvxDialogFragment`

# New Logging interface

The reason to bump minor semver this time was that we have added a new Logging interface deprecating the old `IMvxTrace`. Upgrading to MvvmCross 5.4.0 you will start getting warnings in your project that `Mvx.Trace` and the likes are deprecated.

We now provide `IMvxLog` and `IMvxLogProvider` interfaces. You can inject them in your classes where needed and use this for logging. 

```c#
public class MyViewModel : MvxViewModel
{
    private readonly IMvxLog _log;
    public MyViewModel(IMvxLogProvider logProvider)
    {
        _log = logProvider.GetLogFor<MyViewModel>();
    }
	
	private void SomeMethod()
	{
		_log.ErrorException("Some message", new Exception());
	}
}
```

We also provide a bunch of custom log providers for common logging libraries, such as:

- EntLib
- Log4Net
- Loupe
- NLog
- Serilog

You can provide your log provider by overriding `GetDefaultLogProviderType()` in `Setup.cs`.

You can read more about [how to use these new interfaces in our documentation](https://www.mvvmcross.com/documentation/fundamentals/logging).

# Change Log

## [Unreleased](https://github.com/MvvmCross/MvvmCross/tree/HEAD)

[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.4.0...HEAD)

**Merged pull requests:**

- Fix indentation [\#2327](https://github.com/MvvmCross/MvvmCross/pull/2327) ([Cheesebaron](https://github.com/Cheesebaron))

## [5.4.0](https://github.com/MvvmCross/MvvmCross/tree/5.4.0) (2017-10-31)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.3.2...5.4.0)

**Fixed bugs:**

- MvxSidebarPresenter not adding drawer bar button and showing drawer [\#2247](https://github.com/MvvmCross/MvvmCross/issues/2247)
- Lack one constructor inside MvxDialogFragment [\#2294](https://github.com/MvvmCross/MvvmCross/issues/2294)
- Navigation between native and Forms is not correct [\#2292](https://github.com/MvvmCross/MvvmCross/issues/2292)
- iOS - Crashing on navigation [\#2289](https://github.com/MvvmCross/MvvmCross/issues/2289)
- Bug with toolbar on android MvvmCross 5.2.1 and Forms 2.4.0.282 [\#2252](https://github.com/MvvmCross/MvvmCross/issues/2252)
- Sidebar menu doesn't get initialised for first root controller in 5.2 [\#2188](https://github.com/MvvmCross/MvvmCross/issues/2188)
- mvx:La.ng in Xamarin.Forms not working [\#2176](https://github.com/MvvmCross/MvvmCross/issues/2176)
- Null reference error in MvxFormsAppCompatActivity on GetAccentColor [\#2117](https://github.com/MvvmCross/MvvmCross/issues/2117)
- Mvxforms droid resources fix [\#2305](https://github.com/MvvmCross/MvvmCross/pull/2305) ([johnnywebb](https://github.com/johnnywebb))

**Closed issues:**

- Xamarin.Forms / MasterDetailPage and ModalDialog: Navigation will break, if a page inside a MasterDetail navigation opens a modal dialog [\#2311](https://github.com/MvvmCross/MvvmCross/issues/2311)
- Xamarin Sidebar doesn't opens at first launch [\#2268](https://github.com/MvvmCross/MvvmCross/issues/2268)
- Crash MvxTabBarViewController ViewWillDisappear [\#2267](https://github.com/MvvmCross/MvvmCross/issues/2267)
- Xamarin.Forms / MasterDetailPage: Android crashes as soon as the master menu page has an icon [\#2310](https://github.com/MvvmCross/MvvmCross/issues/2310)
- Xamarin.Forms / MasterDetailPage: The master menu stays open after navigation [\#2307](https://github.com/MvvmCross/MvvmCross/issues/2307)
- Xamarin.Forms / MasterDetailPage breaks my app because of the slide to open menu gesture [\#2306](https://github.com/MvvmCross/MvvmCross/issues/2306)
- Improve logging and IMvxTrace [\#1649](https://github.com/MvvmCross/MvvmCross/issues/1649)

**Merged pull requests:**

- Corrected documentation [\#2298](https://github.com/MvvmCross/MvvmCross/pull/2298) ([gi097](https://github.com/gi097))
- Fix: When wrapping master page into navigation page, title was missing [\#2293](https://github.com/MvvmCross/MvvmCross/pull/2293) ([Grrbrr404](https://github.com/Grrbrr404))
- Fix the namespace in the MvvmCross.Forms.StarterPack [\#2325](https://github.com/MvvmCross/MvvmCross/pull/2325) ([flyingxu](https://github.com/flyingxu))
- Fixes for Forms presenter [\#2321](https://github.com/MvvmCross/MvvmCross/pull/2321) ([martijn00](https://github.com/martijn00))
- Move samples to playground to cleanup forms projects [\#2319](https://github.com/MvvmCross/MvvmCross/pull/2319) ([martijn00](https://github.com/martijn00))
- Droid ViewPresenter document: Add note for animations [\#2315](https://github.com/MvvmCross/MvvmCross/pull/2315) ([nmilcoff](https://github.com/nmilcoff))
- Fix Xamarin-Sidebar initialization  [\#2314](https://github.com/MvvmCross/MvvmCross/pull/2314) ([nmilcoff](https://github.com/nmilcoff))
- Add some missing binding extension properties [\#2313](https://github.com/MvvmCross/MvvmCross/pull/2313) ([martijn00](https://github.com/martijn00))
- Add design time checker to Forms bindings [\#2312](https://github.com/MvvmCross/MvvmCross/pull/2312) ([martijn00](https://github.com/martijn00))
- Improve MvvmCross Logging [\#2300](https://github.com/MvvmCross/MvvmCross/pull/2300) ([willsb](https://github.com/willsb))
- Add Forms projects and test projects for UWP WPF and Mac [\#2296](https://github.com/MvvmCross/MvvmCross/pull/2296) ([martijn00](https://github.com/martijn00))
- Add replace root to add safety check to replace it [\#2291](https://github.com/MvvmCross/MvvmCross/pull/2291) ([martijn00](https://github.com/martijn00))

## [5.3.2](https://github.com/MvvmCross/MvvmCross/tree/5.3.2) (2017-10-23)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.3.1...5.3.2)

**Fixed bugs:**

- MvxIosViewPresenter CloseModalViewController broken in 5.3 [\#2276](https://github.com/MvvmCross/MvvmCross/issues/2276)

**Merged pull requests:**

- ToList modals, as can't change list you are modifying [\#2284](https://github.com/MvvmCross/MvvmCross/pull/2284) ([adamped](https://github.com/adamped))
- Add new extension to the list [\#2286](https://github.com/MvvmCross/MvvmCross/pull/2286) ([CherechesC](https://github.com/CherechesC))
- Add option to clear root, set icon, and animate [\#2285](https://github.com/MvvmCross/MvvmCross/pull/2285) ([martijn00](https://github.com/martijn00))
- Make it possible to start Forms app without splashscreen [\#2283](https://github.com/MvvmCross/MvvmCross/pull/2283) ([martijn00](https://github.com/martijn00))
- Align Forms presenters, fix Modal issue and add MvxPage [\#2282](https://github.com/MvvmCross/MvvmCross/pull/2282) ([martijn00](https://github.com/martijn00))
- Update LinkerPleaseInclude files on TestProjects [\#2281](https://github.com/MvvmCross/MvvmCross/pull/2281) ([nmilcoff](https://github.com/nmilcoff))
- Cleanup EnsureBindingContextSet [\#2280](https://github.com/MvvmCross/MvvmCross/pull/2280) ([nmilcoff](https://github.com/nmilcoff))
- Fix modals close in MvxIosViewPresenter [\#2279](https://github.com/MvvmCross/MvvmCross/pull/2279) ([nmilcoff](https://github.com/nmilcoff))

## [5.3.1](https://github.com/MvvmCross/MvvmCross/tree/5.3.1) (2017-10-18)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.3.0...5.3.1)

**Fixed bugs:**

- Attempting to install Xamarin.Android.Support.Exif into iOS project [\#2272](https://github.com/MvvmCross/MvvmCross/issues/2272)

**Merged pull requests:**

- Update changelog [\#2278](https://github.com/MvvmCross/MvvmCross/pull/2278) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix issue's with modals and navigation stack [\#2277](https://github.com/MvvmCross/MvvmCross/pull/2277) ([martijn00](https://github.com/martijn00))
- Fix behavor of getting Forms resource assembly [\#2275](https://github.com/MvvmCross/MvvmCross/pull/2275) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix for android only dependency in PictureChooser Plugin [\#2273](https://github.com/MvvmCross/MvvmCross/pull/2273) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Add animated option to MvxSidebarPresentationAttribute [\#2271](https://github.com/MvvmCross/MvvmCross/pull/2271) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Fix droid resource issue [\#2270](https://github.com/MvvmCross/MvvmCross/pull/2270) ([johnnywebb](https://github.com/johnnywebb))
