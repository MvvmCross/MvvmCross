---
layout: post
title: MvvmCross 5.2
date:   2017-09-12 11:37:35 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.2!

With the release of 5.2 we focused on creating new presenters who are able to handle almost all navigation patterns for the platform. Since we already had new ones for iOS and MacOS we are now introducing new presenters for Android and WPF!
For the next release (5.3) we plan to continue working on this for UWP and Xamarin.Forms. Also a lot of work was done during the .NET Summer Hackfest, where you can read all about in [this page](https://www.xablu.com/2017/09/05/mvvmcross-xamarin-hackday-results/).

#### Updating packages in your solution

As the nuget package `MvvmCross.Droid.Shared` was no longer exists, please __force remove__ it first, and then update the rest of the packages.

# 5.2 major changes and improvements 

Please read carefully about the changes because some of them are breaking ones, but easy to fix!

## A new default Android presenter [#1934](https://github.com/MvvmCross/MvvmCross/issues/1934)

The current Android presenter is very limited. Out of the box there is no support for Fragments, Dialogs, or other navigation patterns. To fix this we developed a new presenter which will replace the current one as default. The new presenter supports:

- Fragments (Nested)
- Activity's
- Dialogs
- Tabs / ViewPager

Navigation patterns that should be easy to implement with this are:

- NavigationDrawer
- BottomNavigationBar
- BottomSheetDialog
- Master/Detail Flows
- Nested navigation

Additionally it adds support for:

- CustomAnimations per Attribute
- No dependency on MvxCachingFragmentActivity. All Activities can show Fragments now
- SharedElement transitions
- Support for Fragments in default non AppCompat presenter
- Closing of Dialog, Fragments, and Fragments when Activity closes
- Override behaviour on runtime with IMvxOverridePresentationAttribute

The new presenter is very easy to customize and extend. If you have an existing custom presenter we would advice to check compatibility and possible replace it with the new default. 

Please note that currently there is no caching mechanism for fragments in this ViewPresenter. Its design and scope is still under discussion. Feel free to help us getting it done!

Read more about the new Android ViewPresenter in the [documentation](https://www.mvvmcross.com/documentation/presenters/android-view-presenter)

## A new default WPF presenter [#2124](https://github.com/MvvmCross/MvvmCross/pull/2124)

The new WPF presenter enables to show modal/modal less window. It also changes the signature and some methods.

Read more in the [documentation](https://www.mvvmcross.com/documentation/presenters/wpf-view-presenter)

## NavigationService improvements 

Same as the previous 5.x releases, we continued improving and fixing our new NavigationService!

### Prepare method and close bug fixes [#2072](https://github.com/MvvmCross/MvvmCross/pull/2072)
There are breaking changes in the signature to prevent problems with async code. Any navigation done with the NavigationService and a parameter or result will now be triggered in the `Prepare` method. 

`Prepare` is therefore now part of the ViewModel lifecycle: It runs before the navigation is performed, But please note that all of your starter async code should still be called in `Initialize`.

```c#
public class MyViewModel : MvxViewModel
{
    private readonly IMvxNavigationService _navigationService;
    public MyViewModel(IMvxNavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
    }

    public override void Prepare()
    {
        // this method is run before the navigation is performed!
    }
    
    public override async Task Initialize()
    {
        //Do heavy work and data loading here
    }

    public async Task SomeMethod()
    {
        var result = await _navigationService.Navigate<NextViewModel, MyObject, MyReturnObject>(new MyObject());
        //Do something with the result MyReturnObject that you get back
    }
}

public class NextViewModel : MvxViewModel<MyObject, MyReturnObject>
{
    private readonly IMvxNavigationService _navigationService;
    public MyViewModel(IMvxNavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
    }
    
    public override void Prepare(MyObject parameter)
    {
        //Do anything before navigating to the view
        //Save the parameter to a property if you want to use it later
    }
    
    public override async Task Initialize()
    {
        //Do heavy work and data loading here
    }
    
    public async Task SomeMethodToClose()
    {
        await _navigationService.Close(this, new MyReturnObject());
    }
}
```

This prevents blocking the navigation before showing the view.

Read more in the [documentation](https://www.mvvmcross.com/documentation/fundamentals/navigation).

### Added support for navigating to ViewModel types [#2148](https://github.com/MvvmCross/MvvmCross/pull/2148)

`IMvxNavigationService` now allows you to navigate to ViewModels by passing the type as a parameter:

`_navigationService.Navigate(typeof(MyViewModel));`

### Support for all primitive types in navigation with parameters [#2171](https://github.com/MvvmCross/MvvmCross/pull/2171)

Many more primitive types can be used now as simple parameters when navigating! Read more in the [documentation](https://www.mvvmcross.com/documentation/fundamentals/navigation#navigation-with-parameters---using-an-anonymous-parameter-object).

## MvvmCross.Forms StarterPack [#2166](https://github.com/MvvmCross/MvvmCross/pull/2166)

You can now get started on a new Xamarin.Forms app with a set of files to get up to speed.
Just install the MvvmCross.Forms.StarterPack nuget package and you are good to go!

## Combiners with Fluent Bindings [#2143](https://github.com/MvvmCross/MvvmCross/pull/2143)

You can now use Expressions, pass each property individually and even pass an instance of your combiner instead of relying on a string that will retrieve a registered combiner.

Read more in the [documentation](https://www.mvvmcross.com/documentation/fundamentals/value-combiners).

## iOS ViewPresenter changes [#2093](https://github.com/MvvmCross/MvvmCross/pull/2093)

`IMvxTabBarViewController.ShowTabView` has been unified. Previously existed two overloads for it:

```c#
void ShowTabView(UIViewController viewController, string tabTitle, string tabIconName, string tabSelectedIconName = null, string tabAccessibilityIdentifier = null);

void ShowTabView(UIViewController viewController, string tabTitle, string tabIconName, string tabAccessibilityIdentifier = null);
```

Both have been replaced by a single method that is much easier to extend:

```c#
void ShowTabView(UIViewController viewController, MvxTabPresentationAttribute attribute);
```

## IMvxTextProvider improvements [#2150](https://github.com/MvvmCross/MvvmCross/pull/2150)

A method named `TryGetText` was added to `IMvxTextProvider`. This allows you to safely get translated values when using JsonLocalization or ResxLocalization plugins.

## Ongoing work to clean the samples

During the Hackfest we had a lot of contributions to the samples. This is an ongoing process where anyone including you can help in!


# Change Log

## [5.2](https://github.com/MvvmCross/MvvmCross/tree/5.2) (2017-09-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.1.1...5.2)

**Fixed bugs:**

- Add initWithCoder: constructor for MvxView [\#2156](https://github.com/MvvmCross/MvvmCross/issues/2156)
- New navigation service doesn't seem to be truly async as opposite to old ShowViewModel\(\) navigation [\#2071](https://github.com/MvvmCross/MvvmCross/issues/2071)
- App crash will null reference exception after cancelling sending email [\#1978](https://github.com/MvvmCross/MvvmCross/issues/1978)
- Call Close immediately after receiving the return value by NavigationService, view\(activity\) cannot close itself. [\#1851](https://github.com/MvvmCross/MvvmCross/issues/1851)
- Remove unnecessary additions to AndroidViewAssemblies [\#2170](https://github.com/MvvmCross/MvvmCross/pull/2170) ([Cheesebaron](https://github.com/Cheesebaron))

**Closed issues:**

- Auto-creation of window in MvvmCross Mac now default? [\#2178](https://github.com/MvvmCross/MvvmCross/issues/2178)
- MvxAppCompatSetup.cs AndroidViewAssemblies SlidingPaneLayout is not needed [\#2169](https://github.com/MvvmCross/MvvmCross/issues/2169)
- Cleanup MvvmCross simple implementation for Droid [\#2153](https://github.com/MvvmCross/MvvmCross/issues/2153)
- Display MvxAppCompatDialogFragment as a dialog [\#2152](https://github.com/MvvmCross/MvvmCross/issues/2152)
- IMvxPictureChooserTask iOS 10.3 app crash on selecting image from Gallery [\#2149](https://github.com/MvvmCross/MvvmCross/issues/2149)
- Support NavigationService by type [\#2147](https://github.com/MvvmCross/MvvmCross/issues/2147)
- Mvx 5.x navigation and TabBarViewController [\#2137](https://github.com/MvvmCross/MvvmCross/issues/2137)
- MvxAppCompatActivity doesn't work with IMvxNavigationService [\#2128](https://github.com/MvvmCross/MvvmCross/issues/2128)
- Renaming all classes [\#2123](https://github.com/MvvmCross/MvvmCross/issues/2123)
- Default bindings for Xamarin Forms [\#2119](https://github.com/MvvmCross/MvvmCross/issues/2119)
- Incorrect ViewModel Init\(\) params serialization/deserialization [\#2116](https://github.com/MvvmCross/MvvmCross/issues/2116)
- Crash when using a MvxTabBarViewController + Custom Presenter not derived from MvxIosViewPresenter [\#2112](https://github.com/MvvmCross/MvvmCross/issues/2112)
- Feedback: \(Semantic\) Versioning [\#2107](https://github.com/MvvmCross/MvvmCross/issues/2107)
- PictureChooser Android Incorrect Rotation [\#2096](https://github.com/MvvmCross/MvvmCross/issues/2096)
- ModalViewController dismissed on click native popup [\#2094](https://github.com/MvvmCross/MvvmCross/issues/2094)
- MvxTabBarViewController not working anymore without WrapInNavigationController parameter [\#2084](https://github.com/MvvmCross/MvvmCross/issues/2084)
- ViewAppearedFirstTime: ViewAppeared\(\) can be called multiple times on iOS [\#2075](https://github.com/MvvmCross/MvvmCross/issues/2075)
- Not being able to bind ItemClick in axml or programatically in Droid [\#2066](https://github.com/MvvmCross/MvvmCross/issues/2066)
- Navigation to tab bar models seems to be broken for MvvmCross/iOS 5.0.4 onwards [\#2046](https://github.com/MvvmCross/MvvmCross/issues/2046)
- iOS - public override async Task Initialize\(\) throws exception on launch [\#2009](https://github.com/MvvmCross/MvvmCross/issues/2009)
- ViewModel Life cycle events not called properly when bound to an MvxActivity [\#2001](https://github.com/MvvmCross/MvvmCross/issues/2001)
- New presenter for Android [\#1934](https://github.com/MvvmCross/MvvmCross/issues/1934)
- NavigationService: Close all Fragments of parent Activity from within fragment ViewModel [\#1917](https://github.com/MvvmCross/MvvmCross/issues/1917)
- UWP back button visibility suggestion [\#1183](https://github.com/MvvmCross/MvvmCross/issues/1183)

**Merged pull requests:**

- Update 2017-09-12-mvvmcross-52-release.md [\#2179](https://github.com/MvvmCross/MvvmCross/pull/2179) ([nighthawks](https://github.com/nighthawks))
- Update viewmodel-lifecylce documentation [\#2177](https://github.com/MvvmCross/MvvmCross/pull/2177) ([mauricemarkvoort](https://github.com/mauricemarkvoort))
- Clean up the Forms StarterPack [\#2173](https://github.com/MvvmCross/MvvmCross/pull/2173) ([Cheesebaron](https://github.com/Cheesebaron))
- Droid: Remove "Simple" folder and its files [\#2172](https://github.com/MvvmCross/MvvmCross/pull/2172) ([nmilcoff](https://github.com/nmilcoff))
- Support all primitive types \(except IntPtr/UIntPtr\) and decimal type in navigation with parameters [\#2171](https://github.com/MvvmCross/MvvmCross/pull/2171) ([jz5](https://github.com/jz5))
- Added ctor accepting an instance of 'NSCoder' [\#2168](https://github.com/MvvmCross/MvvmCross/pull/2168) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Added missing StarterPack for MvvmCross.Forms \#2073  [\#2166](https://github.com/MvvmCross/MvvmCross/pull/2166) ([brucewilkins](https://github.com/brucewilkins))
- Uwp back button visibility and handling [\#2165](https://github.com/MvvmCross/MvvmCross/pull/2165) ([duglah](https://github.com/duglah))
- Added MvxExpandableTableViewSource docs [\#2164](https://github.com/MvvmCross/MvvmCross/pull/2164) ([b099l3](https://github.com/b099l3))
- Extract parse method to inside MvxColor [\#2163](https://github.com/MvvmCross/MvvmCross/pull/2163) ([willsb](https://github.com/willsb))
- Remove mac solution [\#2162](https://github.com/MvvmCross/MvvmCross/pull/2162) ([tomcurran](https://github.com/tomcurran))
- Update viewmodel-lifecycle.md [\#2161](https://github.com/MvvmCross/MvvmCross/pull/2161) ([c-lamont](https://github.com/c-lamont))
- Fix CollectionViewSourceAnimated Move Action [\#2159](https://github.com/MvvmCross/MvvmCross/pull/2159) ([nmilcoff](https://github.com/nmilcoff))
- Add .NET Standard documentation [\#2158](https://github.com/MvvmCross/MvvmCross/pull/2158) ([Cheesebaron](https://github.com/Cheesebaron))
- Added custom bindings documentation [\#2155](https://github.com/MvvmCross/MvvmCross/pull/2155) ([Cheesebaron](https://github.com/Cheesebaron))
- Droid Presenter: Support for BottomSheetDialogFragment [\#2154](https://github.com/MvvmCross/MvvmCross/pull/2154) ([nmilcoff](https://github.com/nmilcoff))
- Data bindings docs [\#2151](https://github.com/MvvmCross/MvvmCross/pull/2151) ([Cheesebaron](https://github.com/Cheesebaron))
- Add a method which tries to retrieve a language translation [\#2150](https://github.com/MvvmCross/MvvmCross/pull/2150) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Add support for navigating to viewmodel types [\#2148](https://github.com/MvvmCross/MvvmCross/pull/2148) ([martijn00](https://github.com/martijn00))
- Add methods for using combiners with Fluent Bindings [\#2143](https://github.com/MvvmCross/MvvmCross/pull/2143) ([willsb](https://github.com/willsb))
- Update pp file for WPF StarterPack nuspec. Update doc. [\#2141](https://github.com/MvvmCross/MvvmCross/pull/2141) ([jz5](https://github.com/jz5))
- Add support for automatic IoC construct option for custom IMvxAppStart [\#2140](https://github.com/MvvmCross/MvvmCross/pull/2140) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Update WPF presenter [\#2136](https://github.com/MvvmCross/MvvmCross/pull/2136) ([jz5](https://github.com/jz5))
- Code factor style issues fix [\#2135](https://github.com/MvvmCross/MvvmCross/pull/2135) ([Prandtl](https://github.com/Prandtl))
- Fix \#2116 Incorrect ViewModel Init\(\) params serialization/deserialization [\#2133](https://github.com/MvvmCross/MvvmCross/pull/2133) ([jz5](https://github.com/jz5))
- Fix \#2096 PictureChooser Android Incorrect Rotation [\#2132](https://github.com/MvvmCross/MvvmCross/pull/2132) ([jz5](https://github.com/jz5))
- Android Presenter: Improve ViewModel loading method [\#2126](https://github.com/MvvmCross/MvvmCross/pull/2126) ([nmilcoff](https://github.com/nmilcoff))
- Operate with IMvxFragmentView in MvxCachingFragmentStatePagerAdapter [\#2125](https://github.com/MvvmCross/MvvmCross/pull/2125) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- New WPF presenter [\#2124](https://github.com/MvvmCross/MvvmCross/pull/2124) ([jz5](https://github.com/jz5))
- bug fix for UWP compilation [\#2122](https://github.com/MvvmCross/MvvmCross/pull/2122) ([mellson](https://github.com/mellson))
- Now Forms MasterDetail can launch on iOS [\#2121](https://github.com/MvvmCross/MvvmCross/pull/2121) ([mellson](https://github.com/mellson))
- Added UnitTest results to AppVeyor [\#2120](https://github.com/MvvmCross/MvvmCross/pull/2120) ([Cheesebaron](https://github.com/Cheesebaron))
- Code Behind Binding for Forms [\#2118](https://github.com/MvvmCross/MvvmCross/pull/2118) ([mellson](https://github.com/mellson))
- "Close" button of ChildView is not working if it's open from tabs [\#2115](https://github.com/MvvmCross/MvvmCross/pull/2115) ([flyingxu](https://github.com/flyingxu))
- Calls CloseTabBarViewController\(\) only for actual MvxIosViewPresenter [\#2113](https://github.com/MvvmCross/MvvmCross/pull/2113) ([aspnetde](https://github.com/aspnetde))
- Update Hackfest 2017 page [\#2111](https://github.com/MvvmCross/MvvmCross/pull/2111) ([Garfield550](https://github.com/Garfield550))
- Simplify MvxAdapter implemetation [\#2110](https://github.com/MvvmCross/MvvmCross/pull/2110) ([Cheesebaron](https://github.com/Cheesebaron))
- Allow for non-reference types to be passed as parameter [\#2106](https://github.com/MvvmCross/MvvmCross/pull/2106) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Extract MvxUIRefreshControl command execution logic into a method [\#2104](https://github.com/MvvmCross/MvvmCross/pull/2104) ([nmilcoff](https://github.com/nmilcoff))
- Add error solving guidance [\#2103](https://github.com/MvvmCross/MvvmCross/pull/2103) ([drungrin](https://github.com/drungrin))
- Fix Setup iOS wrong ctor [\#2100](https://github.com/MvvmCross/MvvmCross/pull/2100) ([martijn00](https://github.com/martijn00))
- \[WIP\] New default Android presenter [\#2099](https://github.com/MvvmCross/MvvmCross/pull/2099) ([martijn00](https://github.com/martijn00))
- Fix the Babel sample link [\#2098](https://github.com/MvvmCross/MvvmCross/pull/2098) ([TomasLinhart](https://github.com/TomasLinhart))
- Update Hackfest 2017 page [\#2095](https://github.com/MvvmCross/MvvmCross/pull/2095) ([Garfield550](https://github.com/Garfield550))
- iOS ShowTabView: Pass MvxTabPresentationAttribute instead of atomic values [\#2093](https://github.com/MvvmCross/MvvmCross/pull/2093) ([nmilcoff](https://github.com/nmilcoff))
- Update hackfest.html [\#2092](https://github.com/MvvmCross/MvvmCross/pull/2092) ([kkrzyz](https://github.com/kkrzyz))
- Update hackfest.html [\#2090](https://github.com/MvvmCross/MvvmCross/pull/2090) ([kkrzyz](https://github.com/kkrzyz))
- Allowing MvxNavigationFacades to set parameters [\#2088](https://github.com/MvvmCross/MvvmCross/pull/2088) ([b099l3](https://github.com/b099l3))
- Webpage update [\#2086](https://github.com/MvvmCross/MvvmCross/pull/2086) ([kkrzyz](https://github.com/kkrzyz))
- MvxIosViewPresenter: Fix ShowRootViewController [\#2085](https://github.com/MvvmCross/MvvmCross/pull/2085) ([nmilcoff](https://github.com/nmilcoff))
- Add a member list table and make hackfest page mobile friendly [\#2081](https://github.com/MvvmCross/MvvmCross/pull/2081) ([Garfield550](https://github.com/Garfield550))
- Move result close to the navigation service and change init order [\#2072](https://github.com/MvvmCross/MvvmCross/pull/2072) ([martijn00](https://github.com/martijn00))
- Eventhooks for activities fix [\#2056](https://github.com/MvvmCross/MvvmCross/pull/2056) ([orzech85](https://github.com/orzech85))

## [5.1.1](https://github.com/MvvmCross/MvvmCross/tree/5.1.1) (2017-07-28)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.1.0...5.1.1)

**Closed issues:**

- Cannot install the MvvmCross.Forms NuGet package on a fresh/clean Xamarin.Forms project. [\#2070](https://github.com/MvvmCross/MvvmCross/issues/2070)
- RaiseCanExecuteChanged on MvxCommand status is not checked again [\#2064](https://github.com/MvvmCross/MvvmCross/issues/2064)
- Tabs within fragment breaks on navigation [\#2055](https://github.com/MvvmCross/MvvmCross/issues/2055)
- Compilation issues with MvvmCross.StarterPack 5.1.0 [\#2063](https://github.com/MvvmCross/MvvmCross/issues/2063)
- new LinkerPleaseInclude.cs.pp \(ios\) does not compile [\#2054](https://github.com/MvvmCross/MvvmCross/issues/2054)
- MvxNavigationService and Linker All does not work [\#2025](https://github.com/MvvmCross/MvvmCross/issues/2025)
- Upgrade Analyzers VSIX project to VS2017 [\#1654](https://github.com/MvvmCross/MvvmCross/issues/1654)

**Merged pull requests:**

- Hackfest 2017 page! [\#2079](https://github.com/MvvmCross/MvvmCross/pull/2079) ([Garfield550](https://github.com/Garfield550))
- WrapInNavigationController for TabBarViewController/SplitViewController [\#2076](https://github.com/MvvmCross/MvvmCross/pull/2076) ([nmilcoff](https://github.com/nmilcoff))
- Add Transitioning sample to ModalView in Playground iOS [\#2068](https://github.com/MvvmCross/MvvmCross/pull/2068) ([nmilcoff](https://github.com/nmilcoff))
- Use MvxNavigationService in Playground TestProject [\#2067](https://github.com/MvvmCross/MvvmCross/pull/2067) ([nmilcoff](https://github.com/nmilcoff))
- Update color.md [\#2062](https://github.com/MvvmCross/MvvmCross/pull/2062) ([wojtczakmat](https://github.com/wojtczakmat))
- Add support for one sidemenu at a time [\#2058](https://github.com/MvvmCross/MvvmCross/pull/2058) ([martijn00](https://github.com/martijn00))
- fix navigationservice include  [\#2053](https://github.com/MvvmCross/MvvmCross/pull/2053) ([Cheesebaron](https://github.com/Cheesebaron))
- deprecation notice for old default MvxAppStart [\#2052](https://github.com/MvvmCross/MvvmCross/pull/2052) ([orzech85](https://github.com/orzech85))
- Links updated [\#2049](https://github.com/MvvmCross/MvvmCross/pull/2049) ([angelopolotto](https://github.com/angelopolotto))
- Updated analyzers to VS2017. [\#2047](https://github.com/MvvmCross/MvvmCross/pull/2047) ([azchohfi](https://github.com/azchohfi))
