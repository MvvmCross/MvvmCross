---
layout: post
title: MvvmCross 5.0 release!
date:   2017-05-22 11:37:35 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.0!

We're happy to announce the immediate availability of MvvmCross 5.0!
For the last 6 months we have been working on this release and we're really excited about it.

- Make sure to check [Upgrade from 4 to MvvmCross 5](https://www.mvvmcross.com/documentation/upgrading/upgrade-to-mvvmcross-50?scroll=830) to see what you need to change in your code!
- If you like MvvmCross and our work, make sure to **Star** us on [Github](https://github.com/MvvmCross/MvvmCross)!

Let's have a look at the highlights:

## Merge of repos

For a long time we have been looking at merging the different repositories of the project.
Besides a better overview one of the main advantages this brings is the ability to setup a proper CI process.
This is hugely beneficial in the release process itself and makes (patch) releasing MvvmCross a breeze. 

## New website with improved documentation

Over time we have received a lot of feedback from developers that use MvvmCross on a day-to-day basis. Besides the usual hugs and kisses, the most common remark we often got was: "sir, you need to improve the documentation". And as you all know: writing documentation is hard. And not always the most fun thing you can think of. So our first focus was enabling you to help out with improving the documentation. With 5.0, the documentation has landed in the GIT repo, making it possible to submit documentation (changes) just as you do with code: create a pull request. We're already seeing the benefits of this: the amount of community-driven documentation changes has increased tenfold.

Over the coming months we'll introduce a 'documentation policy' to make sure the documentation keeps on improving over time and keeps its uniformity.

For more information and to start contributing to the website see the [Documentation style guide](https://www.mvvmcross.com/documentation/contribute/mvvmcross-docs-style-guide?scroll=1355)

## Open Collective

As you all know, MvvmCross is an Open Source project, so that means we're not making any money out of it. But sometimes we're facing actual costs, which are always difficult to manage. To improve on this situation we've created the MvvmCross Open Collective - a place where you can donate your money to the project but also have full insight into what we're actually doing with it. We really hope you'll join this Open Collective!

[OpenCollective website](https://opencollective.com/mvvmcross)

## Improved support for Xamarin.Forms

In MvvmCross 5.0 we added much improved support for Xamarin.Forms! We added MvvmCross-specific Pages, App classes, and Setup for Forms to enable Bindings, Ioc, DI and much more! Make sure to use the new base classes to enable Forms support.

Find all the details in the updated [documentation](https://www.mvvmcross.com/documentation/platform/xamarin-forms)!

## New iOS presenter

Starting with MvvmCross 5.0, there is a new default Presenter for Views, namely `MvxIosViewPresenter`.

### View Presenter Overview

The default presenter that comes with MvvmCross offers out of the box support for the following navigation patterns/strategies:

- Stack navigation
- Tabs
- SplitView
- Modal
- Modal navigation

If your app needs another kind of presentation mode, you can also easily extend it!

### Presentation Attributes

The presenter uses a set of `PresentationAttributes` to define how a view will be displayed. The existing attributes are:

* MvxRootPresentationAttribute
* MvxChildPresentationAttribute
* MvxModalPresentationAttribute
* MvxTabPresentationAttribute
* MvxMasterSplitViewPresentationAttribute
* MvxDetailSplitViewPresentationAttribute

### Views without attributes: Default values

- If the initial view class of your app has no attribute over it, the presenter will assume stack navigation and will wrap your initial view in a `MvxNavigationController`.
- If a view class has no attribute over it, the presenter will assume _animated_ child presentation.

### Sample please!
You can browse the code of the [Playground](https://github.com/MvvmCross/MvvmCross/tree/master/TestProjects/Playground) iOS project to see this presenter in action.

## Improved navigation

MvvmCross 5 introduces a new NavigationService! The new navigation enables you to inject it into your ViewModels, which makes it more testable, and gives you the ability to implement your own navigation!

The main features of the new navigation are:

* Return a result to the ViewModel where you navigated from
* Check if you are able to navigate to a certain ViewModel
* Type safe
* Fully async await
* URL navigation with deeplinking to ViewModels
* Events on navigate

The following Api is available to use:

```c#
public interface IMvxNavigationService
{
    Task Navigate<TViewModel>() where TViewModel : IMvxViewModel;
    Task Navigate<TViewModel, TParameter>(TParameter param) where TViewModel : IMvxViewModel<TParameter> where TParameter : class;
    Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param) where TViewModel : IMvxViewModel<TParameter, TResult> where TParameter : class where TResult : class;
    Task<TResult> Navigate<TViewModel, TResult>() where TViewModel : IMvxViewModelResult<TResult> where TResult : class;
    Task Navigate(string path);
    Task Navigate<TParameter>(string path, TParameter param) where TParameter : class;
    Task<TResult> Navigate<TResult>(string path) where TResult : class;
    Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param) where TParameter : class where TResult : class;
    Task<bool> CanNavigate(string path);
    Task<bool> CanNavigate<TViewModel>() where TViewModel : IMvxViewModel;
    Task<bool> Close(IMvxViewModel viewModel);
}
```

For more details see [#1634](https://github.com/MvvmCross/MvvmCross/issues/1634)

The Uri navigation will build the navigation stack if required. This will also enable deeplinking and building up the navigationstack for it. Every ViewModel added to the stack can split up into multiple paths of it's own backstack. This will enable all kinds of layout structures as Hamburger, Tab or Top navigation.

If you want to intercept ViewModel navigation changes you can hook into the events of the NavigationService.

```c#
Mvx.Resolve<IMvxNavigationService>().AfterClose += (object sender, NavigateEventArgs e) => {
    //Do something with e.ViewModel
};
```

The events available are:
* BeforeNavigate
* AfterNavigate
* BeforeClose
* AfterClose

The new navigation also allows to navigate directly to an instance of a viewmodel instead of to a type.

A full explanation can be found on the [documentation](https://www.mvvmcross.com/documentation/fundamentals/navigation)

## Lifecycle / Event hooks

Starting from MvvmCross 5.0 ViewModels will be coupled to the lifecycle of the view. This means that the ViewModel has the following methods available:

```c#
    void Appearing();
    void Appeared();
    void Disappearing();
    void Disappeared();
```

The MvxViewController, MvxFragment(s), MvxActivity and the UWP views will call those methods open the platform specific events that are fired. This will give us a more refined control of the ViewModel and the state of its lifecycle. There may be binding that you want to update or resources to clean up, these lifecycle events can help with that.

It should be noted however that it is not 100% reliable but it should work for most of the apps. We don't know what you do in the lifecycle of your app and what could interfere with the called order of the viewmodel lifecycle events.

For more information on the implementation of this functionality please see [Github](https://github.com/MvvmCross/MvvmCross/pull/1601)

Documentation for this is available on the [website](https://www.mvvmcross.com/documentation/fundamentals/viewmodel-lifecycle)

## Generic and typed bindings

This change will add a generic "WithConversion" method. This will allow developers to strongly type the use of value converters, making refactoring a lot easier and more save. For example:

```c#
set.Bind(textField).To(vm => vm.Counter).WithConversion<SomeValueConverter>();
```

Add something about the Generic implementation of IMvxTargetBinding [#1610](https://github.com/MvvmCross/MvvmCross/pull/1610)

MvvmCross 5 now supports an additional option besides literal strings for MvvmCross defined custom bindings, via the use of binding extension methods. Binding with extension methods allows for compile time checking whether the binding is possible against the specified control base type, i.e. TouchUpInside binding works against UIControl inheritance and not a UIView.

#### Developer Usage

```c#
var labelButton = new UILabel();

var bindingSet = this.CreateBindingSet<HomeViewController, HomeViewModel>();
bindingSet.Bind(labelButton).For(c => c.BindText()).To(vm => vm.NextLabel);
bindingSet.Bind(labelButton).For(c => c.BindTap()).To(vm => vm.NextCommand);
bindingSet.Apply();
```

More information can be found in the [documentation](https://www.mvvmcross.com/documentation/fundamentals/data-binding)

## Removal of WindowsPhone 8.x and Windows 8.x
As is usual with a major release it's time to say goodbye to old friends. Windows(Phone) 8 has been deprecated for a long time; removing formal support for this platform is the right thing to do.

## Removal of deprecated plugins
MvvmCross' powerful plugin framework has brought us many good things. However, over time certain plugins have become obsolete, not maintained any longer or considered not useful anymore. With 5.0 we've decided to remove the following plugins:

#### AutoView
No maintained for a long time.
#### CrossUI
No maintained for a long time.
#### Dialog
No maintained for a long time.
#### SQLite plugin
No actual functionality was in this plugin. To use this, simpely include the [SQLite-PCL](https://www.nuget.org/packages/sqlite-net-pcl/) nuget.
#### Bookmarks
Only in use on Windows (Phone) 8
#### SoundEffects
Only in use on Windows (Phone) 8
#### ThreadUtils
Only in use on Windows (Phone) 8
#### JASidePanels
We had two different implementations of a Sidemenu on iOS in MvvmCross. The default one is now MvvmCross - XamarinSidebar. See the [documentation](https://www.mvvmcross.com/documentation/platform/ios-support-library?scroll=446#mvxsidebarpresenter) for more details.

## Other improvements

#### Improved starterpack
The default files installed when you add the [MvvmCross StarterPack](https://www.nuget.org/packages/MvvmCross.StarterPack/) nuget are updated and improved to reflect all changes in 5.0 and offer a more real-life situation base to start with.
#### tvOS support
tvOS support has been added and improved to enable even better cross-platform development.
#### Test projects in main repo
We've added a couple of test projects in the main repo so we can test and reproduce issues very quickly.
#### Migrate Test.Core to PCL
The Test packages are now based on PCL instead of NET45 so you can target more platforms.
#### Sidebar fixes
The now-default XamarinSidebar menu for MvvmCross iOS has been improved with a couple of new features. Read more about this in the [documentation](https://www.mvvmcross.com/documentation/platform/ios-support-library?scroll=446#mvxsidebarpresenter).

# Roadmap towards MvvmCross 6.0

Before MvvmCross 6.0 we want to release a couple of important fixes and new features. Among those are:
- Even better Xamarin.Forms support
- A new default presenter for Android
- Better handling of Fragments

For MvvmCross 6.0 the plan is to, in the first place support .NET Standard 2.0. At the same time we want to use that to refactor the plugins structure, make more use of async await, and `C#7`!

For the full overview and discussion on the roadmap for 6.0 see [#1415](https://github.com/MvvmCross/MvvmCross/issues/1415)

# Changelog

More than 150 PR's made it in this release from over 40 developers. So a big hug to all these contributors!

## [5.0.0](https://github.com/MvvmCross/MvvmCross/tree/5.0.0) (2017-05-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.11...5.0.0)

**Fixed bugs:**

- MvxTableViewController does not call MvxViewModel.Appearing\(\) et. al [\#1813](https://github.com/MvvmCross/MvvmCross/issues/1813)

**Closed issues:**

- Close a view with no Presentation Attribute that was opened from a Tab [\#1834](https://github.com/MvvmCross/MvvmCross/issues/1834)
- MvxViewModel completes its TCS in the wrong sequence [\#1821](https://github.com/MvvmCross/MvvmCross/issues/1821)
- MvxTabBarViewController forces all navigation bars to be opaque [\#1819](https://github.com/MvvmCross/MvvmCross/issues/1819)
- MvvmCross.Plugins.File MvxFileStore read using full path [\#1673](https://github.com/MvvmCross/MvvmCross/issues/1673)
- MvxUIDatePickerDateTargetBinding should not set default date to DateTime.Now if MaximumDate \< Now [\#1618](https://github.com/MvvmCross/MvvmCross/issues/1618)

**Merged pull requests:**

- Statically allocate operator characters [\#1846](https://github.com/MvvmCross/MvvmCross/pull/1846) ([kjeremy](https://github.com/kjeremy))
- Pretty urls for blogposts and icon for deeplink [\#1845](https://github.com/MvvmCross/MvvmCross/pull/1845) ([MarcBruins](https://github.com/MarcBruins))
- Add plugins to third-party [\#1844](https://github.com/MvvmCross/MvvmCross/pull/1844) ([willsb](https://github.com/willsb))
- Makes sure binding respects the configured max date [\#1843](https://github.com/MvvmCross/MvvmCross/pull/1843) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Describe how to add a link to another page in the documentation [\#1842](https://github.com/MvvmCross/MvvmCross/pull/1842) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Added description on iOS View Presenter and tab bar control [\#1841](https://github.com/MvvmCross/MvvmCross/pull/1841) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Fix a couple of navigation issue's and add support for viewmodel instances [\#1840](https://github.com/MvvmCross/MvvmCross/pull/1840) ([martijn00](https://github.com/martijn00))
- Added Visibility.Hidden [\#1839](https://github.com/MvvmCross/MvvmCross/pull/1839) ([tritter](https://github.com/tritter))
- Update file.md \#1827 [\#1837](https://github.com/MvvmCross/MvvmCross/pull/1837) ([jz5](https://github.com/jz5))
- Fix bug with Sidebar removing all nav controller buttons [\#1836](https://github.com/MvvmCross/MvvmCross/pull/1836) ([jamespettigrew](https://github.com/jamespettigrew))
- iOS View Presenter: Default child presentation when using MvxTabBarController as Root [\#1835](https://github.com/MvvmCross/MvvmCross/pull/1835) ([nmilcoff](https://github.com/nmilcoff))
- Docs - Updates for data binding [\#1832](https://github.com/MvvmCross/MvvmCross/pull/1832) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Docs - Added margin to bottom of tables [\#1831](https://github.com/MvvmCross/MvvmCross/pull/1831) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Android Binding - Removed redundant hint custom binding [\#1830](https://github.com/MvvmCross/MvvmCross/pull/1830) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Add support for all UIControlEvents [\#1829](https://github.com/MvvmCross/MvvmCross/pull/1829) ([tritter](https://github.com/tritter))
- Added support for ! and ^ operators in Tibet parsing [\#1828](https://github.com/MvvmCross/MvvmCross/pull/1828) ([tritter](https://github.com/tritter))
- Add GetSize and GetLastWriteTimeUtc \#1155 [\#1827](https://github.com/MvvmCross/MvvmCross/pull/1827) ([jz5](https://github.com/jz5))
- Fix close result on Android and add initialise for non parameter views [\#1825](https://github.com/MvvmCross/MvvmCross/pull/1825) ([martijn00](https://github.com/martijn00))
- Add sidebar menu open/close callbacks [\#1823](https://github.com/MvvmCross/MvvmCross/pull/1823) ([jamespettigrew](https://github.com/jamespettigrew))
- Improved sequencing of MvxViewModel.Close\(\). Fixes \#1821. [\#1822](https://github.com/MvvmCross/MvvmCross/pull/1822) ([DanielStolt](https://github.com/DanielStolt))
- Removed forced navigation bar opacity. Fixes \#1819. [\#1820](https://github.com/MvvmCross/MvvmCross/pull/1820) ([DanielStolt](https://github.com/DanielStolt))
- Added missing calls to view model lifecycle methods. Fixes \#1813. [\#1816](https://github.com/MvvmCross/MvvmCross/pull/1816) ([DanielStolt](https://github.com/DanielStolt))
- Prettify README [\#1815](https://github.com/MvvmCross/MvvmCross/pull/1815) ([Cheesebaron](https://github.com/Cheesebaron))
- Allow user to provide full path in the file plugin [\#1778](https://github.com/MvvmCross/MvvmCross/pull/1778) ([willsb](https://github.com/willsb))

## [5.0.0-beta.11](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.11) (2017-05-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.10...5.0.0-beta.11)

**Fixed bugs:**

- MvxTabBarViewController.VisibleUIViewController throws for tabs without a navigation controller [\#1812](https://github.com/MvvmCross/MvvmCross/issues/1812)

**Merged pull requests:**

- Fix MvxTabBarViewController.VisibleUIViewController [\#1814](https://github.com/MvvmCross/MvvmCross/pull/1814) ([nmilcoff](https://github.com/nmilcoff))
- Update navigation.md [\#1811](https://github.com/MvvmCross/MvvmCross/pull/1811) ([willsam100](https://github.com/willsam100))
- Fix name of presenter package [\#1810](https://github.com/MvvmCross/MvvmCross/pull/1810) ([jimbobbennett](https://github.com/jimbobbennett))
- Forms improvements for base classes [\#1809](https://github.com/MvvmCross/MvvmCross/pull/1809) ([martijn00](https://github.com/martijn00))
- Fix Android Fragment caching for new navigation [\#1808](https://github.com/MvvmCross/MvvmCross/pull/1808) ([martijn00](https://github.com/martijn00))
- add search-box 5px bottom padding [\#1807](https://github.com/MvvmCross/MvvmCross/pull/1807) ([Garfield550](https://github.com/Garfield550))
- Search field can be fully opened by default on mobile now [\#1804](https://github.com/MvvmCross/MvvmCross/pull/1804) ([Garfield550](https://github.com/Garfield550))
- Update the release blog [\#1803](https://github.com/MvvmCross/MvvmCross/pull/1803) ([martijn00](https://github.com/martijn00))
- Lost dependency gem 'jekyll-avatar' [\#1802](https://github.com/MvvmCross/MvvmCross/pull/1802) ([Garfield550](https://github.com/Garfield550))
- Correct link to doc [\#1801](https://github.com/MvvmCross/MvvmCross/pull/1801) ([AnthonyNjuguna](https://github.com/AnthonyNjuguna))

## [5.0.0-beta.10](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.10) (2017-05-15)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.9...5.0.0-beta.10)

**Merged pull requests:**

- add "Get Started" button hover effect [\#1800](https://github.com/MvvmCross/MvvmCross/pull/1800) ([Garfield550](https://github.com/Garfield550))
- Update release notes around extension method based binding [\#1799](https://github.com/MvvmCross/MvvmCross/pull/1799) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Fix some issues found in extension method based binding [\#1798](https://github.com/MvvmCross/MvvmCross/pull/1798) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Update release blog [\#1796](https://github.com/MvvmCross/MvvmCross/pull/1796) ([martijn00](https://github.com/martijn00))

## [5.0.0-beta.9](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.9) (2017-05-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.8...5.0.0-beta.9)

**Closed issues:**

- about .NET Standard Class Library [\#1787](https://github.com/MvvmCross/MvvmCross/issues/1787)

**Merged pull requests:**

- Fix warnings and errors in StarterPack content [\#1797](https://github.com/MvvmCross/MvvmCross/pull/1797) ([willsb](https://github.com/willsb))
- Update 2017-05-01-ReleaseMvvmCross5.md [\#1795](https://github.com/MvvmCross/MvvmCross/pull/1795) ([spockfish](https://github.com/spockfish))
- Fix for MvxRecyclerAdapter indexes [\#1794](https://github.com/MvvmCross/MvvmCross/pull/1794) ([fela98](https://github.com/fela98))
- Update 2017-05-01-ReleaseMvvmCross5.md [\#1793](https://github.com/MvvmCross/MvvmCross/pull/1793) ([spockfish](https://github.com/spockfish))
- Install System.Windows.Interactivity from nuget instead of requiring … [\#1792](https://github.com/MvvmCross/MvvmCross/pull/1792) ([kjeremy](https://github.com/kjeremy))
- Null check viewmodels before visual events [\#1791](https://github.com/MvvmCross/MvvmCross/pull/1791) ([kjeremy](https://github.com/kjeremy))
- Update 2017-05-01-ReleaseMvvmCross5.md [\#1790](https://github.com/MvvmCross/MvvmCross/pull/1790) ([spockfish](https://github.com/spockfish))
- Fix Android crash when app restart [\#1789](https://github.com/MvvmCross/MvvmCross/pull/1789) ([thongdoan](https://github.com/thongdoan))

## [5.0.0-beta.8](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.8) (2017-05-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.7...5.0.0-beta.8)

**Closed issues:**

- Feature request: Property binding in iOS designer  [\#1781](https://github.com/MvvmCross/MvvmCross/issues/1781)
- Make some methods in MvxWindowsMultiRegionViewPresenter protected [\#1779](https://github.com/MvvmCross/MvvmCross/issues/1779)
- the mvvmcross.com's Doc isn't too good show in mobile phone [\#1776](https://github.com/MvvmCross/MvvmCross/issues/1776)
- Realm and MvvmCross activity lifecycle, collection change notifications on destroyed fragments [\#1770](https://github.com/MvvmCross/MvvmCross/issues/1770)
- How to "hook into" and do some post processing after a call to BindingInflate? [\#1769](https://github.com/MvvmCross/MvvmCross/issues/1769)
- Kill `MvxAllThreadDispatchingObject` [\#1750](https://github.com/MvvmCross/MvvmCross/issues/1750)
-  MvxHorizontalGridView is broken [\#1743](https://github.com/MvvmCross/MvvmCross/issues/1743)
- Custom MvxRecyclerViewAdapters don't work anymore [\#1730](https://github.com/MvvmCross/MvvmCross/issues/1730)
- Improve MvvmCross.StarterPack [\#1659](https://github.com/MvvmCross/MvvmCross/issues/1659)
- Binding to Realm IList issue [\#1545](https://github.com/MvvmCross/MvvmCross/issues/1545)
- Network plugin rest client could support aborting the request [\#569](https://github.com/MvvmCross/MvvmCross/issues/569)

**Merged pull requests:**

- Added overridable methods to MvxAdapter for index modification. [\#1786](https://github.com/MvvmCross/MvvmCross/pull/1786) ([fela98](https://github.com/fela98))
- Navigation improvements [\#1784](https://github.com/MvvmCross/MvvmCross/pull/1784) ([martijn00](https://github.com/martijn00))
- fixed mis-capitalized 'Mvvmcross' root namespace in MvxImageView and … [\#1783](https://github.com/MvvmCross/MvvmCross/pull/1783) ([jhorv](https://github.com/jhorv))
- Changed modifiers so can be used in overrides [\#1780](https://github.com/MvvmCross/MvvmCross/pull/1780) ([eL-Prova](https://github.com/eL-Prova))
- Kill MvxAllThreadDispatchingObject [\#1777](https://github.com/MvvmCross/MvvmCross/pull/1777) ([willsb](https://github.com/willsb))
- Improve MvvmCross.StarterPack [\#1775](https://github.com/MvvmCross/MvvmCross/pull/1775) ([willsb](https://github.com/willsb))
- Fixed tipcalc tutorial [\#1772](https://github.com/MvvmCross/MvvmCross/pull/1772) ([MarcBruins](https://github.com/MarcBruins))
- Add analytics [\#1771](https://github.com/MvvmCross/MvvmCross/pull/1771) ([MarcBruins](https://github.com/MarcBruins))
- Revert MvxRecyclerView Header/Footer and Grouping Features [\#1758](https://github.com/MvvmCross/MvvmCross/pull/1758) ([kjeremy](https://github.com/kjeremy))

## [5.0.0-beta.7](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.7) (2017-05-03)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.6...5.0.0-beta.7)

**Merged pull requests:**

- View Presenter doc: Simplify steps for hints and fix enumeration [\#1768](https://github.com/MvvmCross/MvvmCross/pull/1768) ([nmilcoff](https://github.com/nmilcoff))
- Fix typo [\#1767](https://github.com/MvvmCross/MvvmCross/pull/1767) ([martijn00](https://github.com/martijn00))
- Updates for documentation [\#1766](https://github.com/MvvmCross/MvvmCross/pull/1766) ([martijn00](https://github.com/martijn00))
- Add documentation about the new navigation and fix copy pasta error [\#1765](https://github.com/MvvmCross/MvvmCross/pull/1765) ([martijn00](https://github.com/martijn00))
- change parameter type to TInit [\#1764](https://github.com/MvvmCross/MvvmCross/pull/1764) ([Hobbes1987](https://github.com/Hobbes1987))
- Android Forms activity needs to forward events to ActivityLifetimeListener [\#1762](https://github.com/MvvmCross/MvvmCross/pull/1762) ([LeRondPoint](https://github.com/LeRondPoint))
- Forms MvxImageView [\#1761](https://github.com/MvvmCross/MvvmCross/pull/1761) ([LeRondPoint](https://github.com/LeRondPoint))
- Moving from Old wiki - Create Customizing using App and Setup [\#1760](https://github.com/MvvmCross/MvvmCross/pull/1760) ([AnthonyNjuguna](https://github.com/AnthonyNjuguna))
- Make Example.Droid deployable in debug [\#1757](https://github.com/MvvmCross/MvvmCross/pull/1757) ([kjeremy](https://github.com/kjeremy))
- Create docs for ViewPresenter in fundamentals [\#1756](https://github.com/MvvmCross/MvvmCross/pull/1756) ([nmilcoff](https://github.com/nmilcoff))
- MvxForms lang bindings [\#1755](https://github.com/MvvmCross/MvvmCross/pull/1755) ([LeRondPoint](https://github.com/LeRondPoint))
- Do not override a user's custom LayoutManager [\#1732](https://github.com/MvvmCross/MvvmCross/pull/1732) ([kjeremy](https://github.com/kjeremy))

## [5.0.0-beta.6](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.6) (2017-05-01)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.5...5.0.0-beta.6)

**Merged pull requests:**

- More clean up of Warnings [\#1753](https://github.com/MvvmCross/MvvmCross/pull/1753) ([Cheesebaron](https://github.com/Cheesebaron))
- Added null checking on ViewModel for life-cycle events [\#1752](https://github.com/MvvmCross/MvvmCross/pull/1752) ([Plac3hold3r](https://github.com/Plac3hold3r))

## [5.0.0-beta.5](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.5) (2017-04-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.4...5.0.0-beta.5)

**Fixed bugs:**

- \[Android\] Setup initialization issue when app killed and resumed [\#1192](https://github.com/MvvmCross/MvvmCross/issues/1192)
- MvxAutoCompleteTextView/MvxFilteringAdapter locks when PartialText is the same value [\#1162](https://github.com/MvvmCross/MvvmCross/issues/1162)
- MvxPictureChooserTask does not \(always\) work when it get's called from a Android Fragment [\#1107](https://github.com/MvvmCross/MvvmCross/issues/1107)
- MvxNSDatePickerDateTargetBinding timezone issues [\#924](https://github.com/MvvmCross/MvvmCross/issues/924)
- picturechooser always show in portrait orientation [\#761](https://github.com/MvvmCross/MvvmCross/issues/761)
- Annoyance when using Converter [\#697](https://github.com/MvvmCross/MvvmCross/issues/697)
- Problem with Android.Dialog and text focus [\#337](https://github.com/MvvmCross/MvvmCross/issues/337)
- Sort out mess of IMvxTouchPlatformProperties [\#315](https://github.com/MvvmCross/MvvmCross/issues/315)

**Closed issues:**

- NullReferenceException in MvxPathSourceStep.ClearPathSourceBinding [\#1414](https://github.com/MvvmCross/MvvmCross/issues/1414)
- Bulk Registration by Convention work not correctly [\#1381](https://github.com/MvvmCross/MvvmCross/issues/1381)
- ViewModel is null in fragment if the fragment's MvxFragmentAttribute.IsCacheableFragment is set to false [\#1371](https://github.com/MvvmCross/MvvmCross/issues/1371)
- Axml binding performance issue [\#1342](https://github.com/MvvmCross/MvvmCross/issues/1342)
- SuspensionManager failed when migrating to UWP [\#1328](https://github.com/MvvmCross/MvvmCross/issues/1328)
- UWP ShowViewModel Too Early [\#1223](https://github.com/MvvmCross/MvvmCross/issues/1223)
- Initialize MvvmCross in background fetch on iOS [\#1219](https://github.com/MvvmCross/MvvmCross/issues/1219)
- ReloadFromBundle not called [\#1177](https://github.com/MvvmCross/MvvmCross/issues/1177)
- MvxTabBarViewController.ItemSelected override error [\#1167](https://github.com/MvvmCross/MvvmCross/issues/1167)
- \[Android\] RaiseCanExecuteChanged and UI Thread [\#1160](https://github.com/MvvmCross/MvvmCross/issues/1160)
- DefaultImagePath no longer working for MvxImageViewLoader? [\#1152](https://github.com/MvvmCross/MvvmCross/issues/1152)
- Update "ViewModel to ViewModel Navigation" article alternatives to pages [\#1140](https://github.com/MvvmCross/MvvmCross/issues/1140)
- MvxDatePicker value not updated on lollipop devices [\#1139](https://github.com/MvvmCross/MvvmCross/issues/1139)
- Calling Init\(\) on ViewModel creates exception without parameters [\#1132](https://github.com/MvvmCross/MvvmCross/issues/1132)
- IMvxLocationWatcher.Stop\(\) do not disable hardware GPS on iOS [\#1127](https://github.com/MvvmCross/MvvmCross/issues/1127)
- Could you add XML comments to classes and methods within Cirrious.MvvmCross? [\#1108](https://github.com/MvvmCross/MvvmCross/issues/1108)
- IMvxMainThreadDispatcher needs a method where you can wait for the UI task to finish [\#1033](https://github.com/MvvmCross/MvvmCross/issues/1033)
- Editing bound EditText with android:textAllCaps causes exeption [\#1002](https://github.com/MvvmCross/MvvmCross/issues/1002)
- Reason for multiple initialisation should not be called simultaneously? [\#955](https://github.com/MvvmCross/MvvmCross/issues/955)
- PictureChooser - Save image to gallery [\#936](https://github.com/MvvmCross/MvvmCross/issues/936)
- Unified API: binding target is garbage collected [\#902](https://github.com/MvvmCross/MvvmCross/issues/902)
- MvxTableLayout enhancement [\#850](https://github.com/MvvmCross/MvvmCross/issues/850)
- MvxStringToTypeParser does not parse using InvariantCulture [\#840](https://github.com/MvvmCross/MvvmCross/issues/840)
- CurrentCulture vs CurrentUICulture in bindings [\#839](https://github.com/MvvmCross/MvvmCross/issues/839)
- Be able to pass an Enum value as a CommandParameter for Android\(and possibly other platforms too\) [\#767](https://github.com/MvvmCross/MvvmCross/issues/767)
- Tutorial for MvxAutoCompleteTextView [\#702](https://github.com/MvvmCross/MvvmCross/issues/702)
- a better MvxPickerViewModel [\#674](https://github.com/MvvmCross/MvvmCross/issues/674)
- File plugin does not support embedded resource files for Android [\#635](https://github.com/MvvmCross/MvvmCross/issues/635)
- Allow custom JSON serialization / deserialization settings in JSON plugin [\#610](https://github.com/MvvmCross/MvvmCross/issues/610)
- AddHeader\AddFooter methods in MvxListView [\#602](https://github.com/MvvmCross/MvvmCross/issues/602)
- BindingText [\#585](https://github.com/MvvmCross/MvvmCross/issues/585)
- Could UIViewController/Activity code behind be used with Action\<IMvxListItemView\> to set list item bindings [\#573](https://github.com/MvvmCross/MvvmCross/issues/573)
- Rest Client could support XML as well as JSON [\#570](https://github.com/MvvmCross/MvvmCross/issues/570)
- Allow for setter only fluent property binding [\#567](https://github.com/MvvmCross/MvvmCross/issues/567)
- Could plugins be extended to some form of Core-UI project contract too [\#496](https://github.com/MvvmCross/MvvmCross/issues/496)
- "Nice to have" option to override plugin loading [\#492](https://github.com/MvvmCross/MvvmCross/issues/492)
- Reduce ObjectGraph by using HiddenReference for MvxActivity.BindingContext? [\#409](https://github.com/MvvmCross/MvvmCross/issues/409)
- Consider dropping DataBinding update priority [\#404](https://github.com/MvvmCross/MvvmCross/issues/404)
- Add new constructor to MvxGeneralEventSubscription with sourceEventName. [\#400](https://github.com/MvvmCross/MvvmCross/issues/400)
- Investigate better support for GetItemViewType [\#333](https://github.com/MvvmCross/MvvmCross/issues/333)
- Consider allowing nullable services in IoC constructors [\#239](https://github.com/MvvmCross/MvvmCross/issues/239)
- Add Facebook sharing for iOS [\#188](https://github.com/MvvmCross/MvvmCross/issues/188)
- Consider caching parse results [\#125](https://github.com/MvvmCross/MvvmCross/issues/125)

**Merged pull requests:**

- Fix a bunch of warnings [\#1749](https://github.com/MvvmCross/MvvmCross/pull/1749) ([Cheesebaron](https://github.com/Cheesebaron))

## [5.0.0-beta.4](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.4) (2017-04-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.3...5.0.0-beta.4)

**Fixed bugs:**

- Can an IMvxVisibleViewModel interface/pattern be added [\#74](https://github.com/MvvmCross/MvvmCross/issues/74)

**Closed issues:**

- Master Detail Example Project Doesn't Compile with Latest MvvmCross Nuget Packages [\#1699](https://github.com/MvvmCross/MvvmCross/issues/1699)
- Master detail - working xamarin forms example [\#1681](https://github.com/MvvmCross/MvvmCross/issues/1681)
- Sample Forms projects on older versions of MvvmCross and Xamarin.Forms [\#1679](https://github.com/MvvmCross/MvvmCross/issues/1679)
- Consolidate NuGet packages [\#1672](https://github.com/MvvmCross/MvvmCross/issues/1672)
- MvxFileStore FullPath Customization [\#1630](https://github.com/MvvmCross/MvvmCross/issues/1630)
- NullReferenceException in MvxTaskBasedBindingContext in Release mode [\#1508](https://github.com/MvvmCross/MvvmCross/issues/1508)
- New MvxListView bindings not working since last xamarin update [\#1395](https://github.com/MvvmCross/MvvmCross/issues/1395)
- Add build test of UWP project [\#1392](https://github.com/MvvmCross/MvvmCross/issues/1392)
- Can a general Close/Back event be added to MvxViewModel? [\#609](https://github.com/MvvmCross/MvvmCross/issues/609)
- MvxGridView: can not set gravity of child items because of proxied FrameLayout [\#539](https://github.com/MvvmCross/MvvmCross/issues/539)
- Unify MvxCollectionViewCell and MvxTableViewCell constructors [\#367](https://github.com/MvvmCross/MvvmCross/issues/367)
- In PhoneCall Plugin, would be nice if phone detected in API [\#95](https://github.com/MvvmCross/MvvmCross/issues/95)

**Merged pull requests:**

- Fix more samples, names and namespaces [\#1747](https://github.com/MvvmCross/MvvmCross/pull/1747) ([martijn00](https://github.com/martijn00))
- Fix remaining Forms issue's [\#1746](https://github.com/MvvmCross/MvvmCross/pull/1746) ([martijn00](https://github.com/martijn00))
- Cleanup of Forms project and packages [\#1745](https://github.com/MvvmCross/MvvmCross/pull/1745) ([martijn00](https://github.com/martijn00))
- Cleanup some code [\#1744](https://github.com/MvvmCross/MvvmCross/pull/1744) ([kjeremy](https://github.com/kjeremy))
- Check phone capabilities before calling [\#1742](https://github.com/MvvmCross/MvvmCross/pull/1742) ([Cheesebaron](https://github.com/Cheesebaron))
- Added MvxBindings to Forms integration [\#1741](https://github.com/MvvmCross/MvvmCross/pull/1741) ([LeRondPoint](https://github.com/LeRondPoint))
- Add editorconfig file to help with with formatting and conventions [\#1739](https://github.com/MvvmCross/MvvmCross/pull/1739) ([Plac3hold3r](https://github.com/Plac3hold3r))

## [5.0.0-beta.3](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.3) (2017-04-28)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.2...5.0.0-beta.3)

**Fixed bugs:**

- \[5.0.0-beta0001\] Some nuget packages are missing dll's [\#1725](https://github.com/MvvmCross/MvvmCross/issues/1725)
- FusedLocationHandler crashing with MvxException: SERVICE\_MISSING [\#1669](https://github.com/MvvmCross/MvvmCross/issues/1669)

**Closed issues:**

- Improve load time of plugins [\#1729](https://github.com/MvvmCross/MvvmCross/issues/1729)
- Generate PDBs for source linking with GitLink [\#1314](https://github.com/MvvmCross/MvvmCross/issues/1314)

**Merged pull requests:**

- Unify MvxTableViewCell, MvxCollectionViewCell and MvxCollectionReusableView constructors [\#1740](https://github.com/MvvmCross/MvvmCross/pull/1740) ([nmilcoff](https://github.com/nmilcoff))
- Don't throw exception in ctor for Fused [\#1738](https://github.com/MvvmCross/MvvmCross/pull/1738) ([Cheesebaron](https://github.com/Cheesebaron))
- Add some .gitignore [\#1737](https://github.com/MvvmCross/MvvmCross/pull/1737) ([kjeremy](https://github.com/kjeremy))
- Xamarin.Android.Support.\* 25.3.1 [\#1735](https://github.com/MvvmCross/MvvmCross/pull/1735) ([kjeremy](https://github.com/kjeremy))
- Fix Gitlink [\#1734](https://github.com/MvvmCross/MvvmCross/pull/1734) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix null check in Adapter setter [\#1733](https://github.com/MvvmCross/MvvmCross/pull/1733) ([kjeremy](https://github.com/kjeremy))

## [5.0.0-beta.2](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.2) (2017-04-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.1...5.0.0-beta.2)

**Closed issues:**

- Add open collective to the website [\#1723](https://github.com/MvvmCross/MvvmCross/issues/1723)
- Build broken after merge of \#1693 [\#1720](https://github.com/MvvmCross/MvvmCross/issues/1720)
- Website: Missing margins between logos [\#1712](https://github.com/MvvmCross/MvvmCross/issues/1712)

**Merged pull requests:**

- Fix nuspec derpage [\#1726](https://github.com/MvvmCross/MvvmCross/pull/1726) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixed link to documentation in Readme [\#1722](https://github.com/MvvmCross/MvvmCross/pull/1722) ([markuspalme](https://github.com/markuspalme))

## [5.0.0-beta.1](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.1) (2017-04-25)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.4.0...5.0.0-beta.1)

**Fixed bugs:**

- Android : Exceptions in MvxValueConverter\<TFrom, TTo\>.Convert are hidden [\#1655](https://github.com/MvvmCross/MvvmCross/issues/1655)
- MvxAsyncCommand inconsistencies on Android [\#1589](https://github.com/MvvmCross/MvvmCross/issues/1589)
- Error XA4204: Unable to resolve interface type 'Android.Gms.Common.Apis.GoogleApiClient/IConnectionCallbacks'. [\#1558](https://github.com/MvvmCross/MvvmCross/issues/1558)
- iOS Autocorrect UITextView not captured [\#1555](https://github.com/MvvmCross/MvvmCross/issues/1555)
- Cannot/how to set LayoutManager for children RecyclerViews bound with local:MvxItemTemplate - XML local:layoutManager does not work [\#1512](https://github.com/MvvmCross/MvvmCross/issues/1512)
- RadioButton's width doesn't use match\_parent [\#1451](https://github.com/MvvmCross/MvvmCross/issues/1451)
- Bindings in 4.2.x  System.AggregateException caught in HockeyApp [\#1398](https://github.com/MvvmCross/MvvmCross/issues/1398)
- AppCompatActivity and ActionBarActivity cause null reference exception onCreate\(Bundle ....\) [\#1112](https://github.com/MvvmCross/MvvmCross/issues/1112)
- Sometime CrossUI.Touch Elements don't repaint until rotated [\#977](https://github.com/MvvmCross/MvvmCross/issues/977)
- Sort out SQLite installer for winrt [\#307](https://github.com/MvvmCross/MvvmCross/issues/307)
- \[WIP\] Issue 1465 - Switch to ViewHolder pattern instead of wrapping in FrameLayout for MvxListView items [\#1535](https://github.com/MvvmCross/MvvmCross/pull/1535) ([orzech85](https://github.com/orzech85))

**Closed issues:**

- Remove JASidePanels nuspec [\#1708](https://github.com/MvvmCross/MvvmCross/issues/1708)
- Binding data with headers does not work  [\#1702](https://github.com/MvvmCross/MvvmCross/issues/1702)
- Remove Bookmarks plugin [\#1701](https://github.com/MvvmCross/MvvmCross/issues/1701)
- Remove SoundEffects [\#1700](https://github.com/MvvmCross/MvvmCross/issues/1700)
- Links in docs menu point at mvvmcross-docs repo [\#1688](https://github.com/MvvmCross/MvvmCross/issues/1688)
- Remove JASidePanels [\#1682](https://github.com/MvvmCross/MvvmCross/issues/1682)
- Registering services in InitializeFirstChance not working as expected [\#1676](https://github.com/MvvmCross/MvvmCross/issues/1676)
- Cleanup: Obsolete MvxFormFactorSpecificAttribute [\#1670](https://github.com/MvvmCross/MvvmCross/issues/1670)
- MvxAppCompatSpinner's Adapter's SimpleViewLayoutId should probably be Android.Resource.Layout.SimpleSpinnerItem [\#1666](https://github.com/MvvmCross/MvvmCross/issues/1666)
- RFE: remove plugin 'ThreadUtils' for 5.0 [\#1665](https://github.com/MvvmCross/MvvmCross/issues/1665)
- RFE: remove plugin 'ReflectionEx' for 5.0 [\#1664](https://github.com/MvvmCross/MvvmCross/issues/1664)
- Column with name "column" is not defined on the local table "table" [\#1662](https://github.com/MvvmCross/MvvmCross/issues/1662)
- TextView Click binding fails in Release [\#1651](https://github.com/MvvmCross/MvvmCross/issues/1651)
- MvxRadioGroup binding fails in Release after updating to 4.4.0 [\#1650](https://github.com/MvvmCross/MvvmCross/issues/1650)
- InfoWindowClick does not work with ShowViewModel [\#1647](https://github.com/MvvmCross/MvvmCross/issues/1647)
- New RecyclerView Grouping crash on Clear/Insert sequence [\#1640](https://github.com/MvvmCross/MvvmCross/issues/1640)
- RecyclerView Header/Footer crash on tap when using custom Header/Footer DataContext [\#1637](https://github.com/MvvmCross/MvvmCross/issues/1637)
- Remove Windows 8.1/WP8.1 examples [\#1620](https://github.com/MvvmCross/MvvmCross/issues/1620)
- Using viewmodel navigation with complex parameter fails silently [\#1615](https://github.com/MvvmCross/MvvmCross/issues/1615)
- Add id parameter to MvxTabPresentation [\#1614](https://github.com/MvvmCross/MvvmCross/issues/1614)
- Remove ReflectionEx Plugin [\#1606](https://github.com/MvvmCross/MvvmCross/issues/1606)
- Improving MvxTargetBindingFactoryRegistry [\#1594](https://github.com/MvvmCross/MvvmCross/issues/1594)
- Custom font not working with MvxAppCompatActivity [\#1592](https://github.com/MvvmCross/MvvmCross/issues/1592)
- Android Tabs Example [\#1586](https://github.com/MvvmCross/MvvmCross/issues/1586)
- How to hide all XamarinSidebar menu on page ? \(iOS\)  [\#1585](https://github.com/MvvmCross/MvvmCross/issues/1585)
- Toolbar Search Binding [\#1584](https://github.com/MvvmCross/MvvmCross/issues/1584)
- Virtualize MvxExpandableListAdapter methods [\#1582](https://github.com/MvvmCross/MvvmCross/issues/1582)
- TestProjects\Forms\Example001XAML\Example001XAML.sln does not compile [\#1580](https://github.com/MvvmCross/MvvmCross/issues/1580)
- Telerik 2017 R1 for Xamarin.Forms Android incompatible with MvvmCross [\#1579](https://github.com/MvvmCross/MvvmCross/issues/1579)
- Add Preserve attribute to all bindings  [\#1578](https://github.com/MvvmCross/MvvmCross/issues/1578)
- JASidePanels Slide Menu iOS Native Library Crash only on device [\#1577](https://github.com/MvvmCross/MvvmCross/issues/1577)
- Find alternative to readme.io [\#1575](https://github.com/MvvmCross/MvvmCross/issues/1575)
- Android projects have specific Compile version set [\#1573](https://github.com/MvvmCross/MvvmCross/issues/1573)
- Nuspec PCL profiles? [\#1572](https://github.com/MvvmCross/MvvmCross/issues/1572)
- \[AndroidSupport\] RecyclerView view created by adapter should be accessable in code after view is inflated [\#1566](https://github.com/MvvmCross/MvvmCross/issues/1566)
- \[AndroidSupport\] RecyclerView support for Footer/Header/Grouping \(sections\) [\#1565](https://github.com/MvvmCross/MvvmCross/issues/1565)
- \[AndroidSupport\] FAB AutoHide behavior - same support for new BottomBar [\#1563](https://github.com/MvvmCross/MvvmCross/issues/1563)
- Navigation Page shows foreign language "Zuruck".  How do I fix or override this? [\#1561](https://github.com/MvvmCross/MvvmCross/issues/1561)
- Error installing MvvmCross.Forms.Presenter [\#1560](https://github.com/MvvmCross/MvvmCross/issues/1560)
- \[Proposal\] Strongly typed MvvmCross code based binding properties [\#1557](https://github.com/MvvmCross/MvvmCross/issues/1557)
- Android send intent from external apps does not start main activity on some devices/OS [\#1551](https://github.com/MvvmCross/MvvmCross/issues/1551)
- MvxAndroidViewFactory CreateView question [\#1550](https://github.com/MvvmCross/MvvmCross/issues/1550)
- "Partial declarations of 'TestView' must not specify different base classes" error [\#1547](https://github.com/MvvmCross/MvvmCross/issues/1547)
- Mvx.MvxListView ItemAppearing  [\#1544](https://github.com/MvvmCross/MvvmCross/issues/1544)
- MvxListView ItemClick doesn't work [\#1543](https://github.com/MvvmCross/MvvmCross/issues/1543)
- iOS crash/ \<unknown\> \<0xffffffff\>/ Foundation.NSObject.ReleaseManagedRef /Foundation.NSObject/NSObject\_Disposer.Drain [\#1539](https://github.com/MvvmCross/MvvmCross/issues/1539)
- Create new event hooks for lifecycle events [\#1531](https://github.com/MvvmCross/MvvmCross/issues/1531)
- OnDataContextChanged should only be called when the data context actually changes [\#1522](https://github.com/MvvmCross/MvvmCross/issues/1522)
- cant get connection [\#1513](https://github.com/MvvmCross/MvvmCross/issues/1513)
- Fix references in sub projects [\#1506](https://github.com/MvvmCross/MvvmCross/issues/1506)
- Update Android OnAttach [\#1503](https://github.com/MvvmCross/MvvmCross/issues/1503)
- Move back sub repos into main [\#1500](https://github.com/MvvmCross/MvvmCross/issues/1500)
- app crashing after detaching from VS debugger [\#1498](https://github.com/MvvmCross/MvvmCross/issues/1498)
- Add ItemClick on LinearLayout as on ListView [\#1496](https://github.com/MvvmCross/MvvmCross/issues/1496)
- Databinding Not Working as Expected on MVVMCross [\#1487](https://github.com/MvvmCross/MvvmCross/issues/1487)
- MvxObservableCollection RemoveRange Exception [\#1485](https://github.com/MvvmCross/MvvmCross/issues/1485)
- Binding issue when doing full linking on Android for TextView on latest 4.4.0 [\#1482](https://github.com/MvvmCross/MvvmCross/issues/1482)
- StarterPack nuspec dependency doesn't get updated [\#1479](https://github.com/MvvmCross/MvvmCross/issues/1479)
- rendering issue with dynamic sized UILabels using Constraints [\#1468](https://github.com/MvvmCross/MvvmCross/issues/1468)
- Switch to ViewHolder pattern instead of wrapping in FrameLayout for MvxListView items [\#1465](https://github.com/MvvmCross/MvvmCross/issues/1465)
- MvxDialogViewController does not Dispose after assigning the Root element [\#1445](https://github.com/MvvmCross/MvvmCross/issues/1445)
- MvxImageView Issue with nested MvxRecyclerView [\#1444](https://github.com/MvvmCross/MvvmCross/issues/1444)
- Binding.Mac does not build on Windows [\#1437](https://github.com/MvvmCross/MvvmCross/issues/1437)
- Reloading the Main Activity : Navigation Stops working [\#1408](https://github.com/MvvmCross/MvvmCross/issues/1408)
- set\_TextFormatted leads to app crash [\#1405](https://github.com/MvvmCross/MvvmCross/issues/1405)
- A standardized way of doing vm -\> view communication [\#1386](https://github.com/MvvmCross/MvvmCross/issues/1386)
- MvvmCross Unity3d ugui [\#1380](https://github.com/MvvmCross/MvvmCross/issues/1380)
- NullReferenceException in MvxFullBinding [\#1378](https://github.com/MvvmCross/MvvmCross/issues/1378)
- MvvmCross.Tests nuget can't be added to a PCL project [\#1375](https://github.com/MvvmCross/MvvmCross/issues/1375)
- MyGet Nuget feed [\#1369](https://github.com/MvvmCross/MvvmCross/issues/1369)
- Support URL based navigation [\#1315](https://github.com/MvvmCross/MvvmCross/issues/1315)
- System.InvalidCastException on Android [\#1305](https://github.com/MvvmCross/MvvmCross/issues/1305)
- continuous integration and pre-release builds [\#1301](https://github.com/MvvmCross/MvvmCross/issues/1301)
- iOS EntryElement binding problem \(revised\) [\#1291](https://github.com/MvvmCross/MvvmCross/issues/1291)
- MvxModalNavSupportIosViewPresenter does not use CurrentTopViewController [\#1274](https://github.com/MvvmCross/MvvmCross/issues/1274)
- Sharing code across Apple platforms [\#1166](https://github.com/MvvmCross/MvvmCross/issues/1166)
- ValueConverter registration doesn't work anymore in v4.0 beta 3 [\#1154](https://github.com/MvvmCross/MvvmCross/issues/1154)
- Pushing symbols fails for many packages [\#1032](https://github.com/MvvmCross/MvvmCross/issues/1032)
- Add ability to request current device location with async API [\#1024](https://github.com/MvvmCross/MvvmCross/issues/1024)
- BindinEx wpa8.1 [\#967](https://github.com/MvvmCross/MvvmCross/issues/967)
- OnNavigatedTo Event Throwing Error in Windows 10 Preview [\#816](https://github.com/MvvmCross/MvvmCross/issues/816)
- MvxListItemView not measuring correctly? [\#774](https://github.com/MvvmCross/MvvmCross/issues/774)
- ICommand binding on iOS not working with CanExecute [\#720](https://github.com/MvvmCross/MvvmCross/issues/720)
- Add support for event to command for WP8 binding [\#640](https://github.com/MvvmCross/MvvmCross/issues/640)
- File Plugin for Windows Store needs using Statements around stream [\#637](https://github.com/MvvmCross/MvvmCross/issues/637)
- Can't pass decimal type in ViewModel Init function [\#564](https://github.com/MvvmCross/MvvmCross/issues/564)
- MvxIoCSupportingTest for WindowsPhone and others? [\#523](https://github.com/MvvmCross/MvvmCross/issues/523)
- Feature Requests: MultiLine TextEdit and Double button element for ios dialog [\#522](https://github.com/MvvmCross/MvvmCross/issues/522)
- Simple WebContentElement for iOS dialog [\#509](https://github.com/MvvmCross/MvvmCross/issues/509)
- CrossUI.Touch.Dialog.Elements.Section does not respect Visibility [\#407](https://github.com/MvvmCross/MvvmCross/issues/407)
- Problems with IElementSizing in Monotouch.Dialog included into MvvmCross [\#182](https://github.com/MvvmCross/MvvmCross/issues/182)

**Merged pull requests:**

- Fix missing overrides for presenter [\#1721](https://github.com/MvvmCross/MvvmCross/pull/1721) ([martijn00](https://github.com/martijn00))
- update order of the docs and add view model lifecycle documentation [\#1718](https://github.com/MvvmCross/MvvmCross/pull/1718) ([MarcBruins](https://github.com/MarcBruins))
- Fixed "Get started" link [\#1717](https://github.com/MvvmCross/MvvmCross/pull/1717) ([2moveit](https://github.com/2moveit))
- update current navigation and move tipcalc explanation to the tutorial section [\#1716](https://github.com/MvvmCross/MvvmCross/pull/1716) ([MarcBruins](https://github.com/MarcBruins))
- Fix solution release mode [\#1715](https://github.com/MvvmCross/MvvmCross/pull/1715) ([martijn00](https://github.com/martijn00))
- Remove windows solution and update mac [\#1714](https://github.com/MvvmCross/MvvmCross/pull/1714) ([martijn00](https://github.com/martijn00))
- Add backers and sponsors from Open Collective [\#1713](https://github.com/MvvmCross/MvvmCross/pull/1713) ([piamancini](https://github.com/piamancini))
- Fix broken link [\#1711](https://github.com/MvvmCross/MvvmCross/pull/1711) ([kiliman](https://github.com/kiliman))
- Remove bookmarks en sounds effect plugin [\#1710](https://github.com/MvvmCross/MvvmCross/pull/1710) ([martijn00](https://github.com/martijn00))
- Clean up JASidePanels stuff [\#1709](https://github.com/MvvmCross/MvvmCross/pull/1709) ([Cheesebaron](https://github.com/Cheesebaron))
- Speed up ioc type cache [\#1707](https://github.com/MvvmCross/MvvmCross/pull/1707) ([Cheesebaron](https://github.com/Cheesebaron))
- Create docs about new MvxIosViewPresenter [\#1706](https://github.com/MvvmCross/MvvmCross/pull/1706) ([nmilcoff](https://github.com/nmilcoff))
- Make SetProperty virtual [\#1705](https://github.com/MvvmCross/MvvmCross/pull/1705) ([kjeremy](https://github.com/kjeremy))
- Fixed headers in feature-overview.md [\#1703](https://github.com/MvvmCross/MvvmCross/pull/1703) ([kipters](https://github.com/kipters))
- Add missing MvxRecyclerViewHolder\(IntPtr, JniHandleOwnership\) constructor [\#1698](https://github.com/MvvmCross/MvvmCross/pull/1698) ([kjeremy](https://github.com/kjeremy))
- Make sure the correct default presentation attribute is returned [\#1697](https://github.com/MvvmCross/MvvmCross/pull/1697) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Update documentation to reflect changes in PR \#1694 [\#1696](https://github.com/MvvmCross/MvvmCross/pull/1696) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Xamarinsidebar Presenter improvements [\#1695](https://github.com/MvvmCross/MvvmCross/pull/1695) ([nmilcoff](https://github.com/nmilcoff))
- Refactor Sidemenu presenter [\#1694](https://github.com/MvvmCross/MvvmCross/pull/1694) ([martijn00](https://github.com/martijn00))
- Implement new navigation service [\#1693](https://github.com/MvvmCross/MvvmCross/pull/1693) ([martijn00](https://github.com/martijn00))
- Remember scroll position for documentation pages on website [\#1692](https://github.com/MvvmCross/MvvmCross/pull/1692) ([MarcBruins](https://github.com/MarcBruins))
- Cleanup iOS platform code // Fix XamarinSidebar Presenter [\#1691](https://github.com/MvvmCross/MvvmCross/pull/1691) ([nmilcoff](https://github.com/nmilcoff))
- Remove ThreadUtils [\#1690](https://github.com/MvvmCross/MvvmCross/pull/1690) ([martijn00](https://github.com/martijn00))
- Fixed Get Started documentation link [\#1689](https://github.com/MvvmCross/MvvmCross/pull/1689) ([tritter](https://github.com/tritter))
- Merge develop into master [\#1687](https://github.com/MvvmCross/MvvmCross/pull/1687) ([martijn00](https://github.com/martijn00))
- Merge docs into main repo [\#1686](https://github.com/MvvmCross/MvvmCross/pull/1686) ([martijn00](https://github.com/martijn00))
- Resolves problem when no menu is configured [\#1685](https://github.com/MvvmCross/MvvmCross/pull/1685) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Remove \(stale\) JASidePanels projects from iOS-Support and TestProjects [\#1684](https://github.com/MvvmCross/MvvmCross/pull/1684) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Add new MvvmCross logo [\#1683](https://github.com/MvvmCross/MvvmCross/pull/1683) ([martijn00](https://github.com/martijn00))
- Improvements on the Xamarin Sidebar component [\#1680](https://github.com/MvvmCross/MvvmCross/pull/1680) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Bump Xamarin.Build.Download to 0.4.3 [\#1677](https://github.com/MvvmCross/MvvmCross/pull/1677) ([kjeremy](https://github.com/kjeremy))
- Merge current Navigation changes [\#1674](https://github.com/MvvmCross/MvvmCross/pull/1674) ([martijn00](https://github.com/martijn00))
- New MvxIosViewPresenter for iOS [\#1671](https://github.com/MvvmCross/MvvmCross/pull/1671) ([nmilcoff](https://github.com/nmilcoff))
- update .mailmap [\#1668](https://github.com/MvvmCross/MvvmCross/pull/1668) ([spockfish](https://github.com/spockfish))
- Fix spinner styles [\#1667](https://github.com/MvvmCross/MvvmCross/pull/1667) ([kjeremy](https://github.com/kjeremy))
- update MvxTabsViewPresenter, and fix dispose modal view model after close [\#1663](https://github.com/MvvmCross/MvvmCross/pull/1663) ([KLytvynenko](https://github.com/KLytvynenko))
- Added ContainsImage to IMvxImageCache and added support for android.resource schema in MvxAndroidLocalFileImageLoader. [\#1660](https://github.com/MvvmCross/MvvmCross/pull/1660) ([Cybrosys](https://github.com/Cybrosys))
- Use new non RC VS2017 image for build [\#1657](https://github.com/MvvmCross/MvvmCross/pull/1657) ([Cheesebaron](https://github.com/Cheesebaron))
- Spit out error messages when conversions go wrong. Fixes \#1655 [\#1656](https://github.com/MvvmCross/MvvmCross/pull/1656) ([Cheesebaron](https://github.com/Cheesebaron))
- Email plugin for UWP with attachments [\#1653](https://github.com/MvvmCross/MvvmCross/pull/1653) ([khoussem](https://github.com/khoussem))
- Fixed permission error on Gmail application on Android \(Email Plugin\) [\#1648](https://github.com/MvvmCross/MvvmCross/pull/1648) ([ernestoyaquello](https://github.com/ernestoyaquello))
- fix for MvxRecyclerAdapter: FooterClickCommand was assigned for header [\#1644](https://github.com/MvvmCross/MvvmCross/pull/1644) ([voluntas88](https://github.com/voluntas88))
- Fixes RecyclerView crashes due to RecyclerViewHolder incosistency - w… [\#1641](https://github.com/MvvmCross/MvvmCross/pull/1641) ([thefex](https://github.com/thefex))
- Fixes crash when using custom DataContext of RecyclerView Header/Footer. [\#1638](https://github.com/MvvmCross/MvvmCross/pull/1638) ([thefex](https://github.com/thefex))
- Solve bug where auto replacement is not updated through the binding [\#1633](https://github.com/MvvmCross/MvvmCross/pull/1633) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Add missing generic WithConversion method in case TSource is missing [\#1632](https://github.com/MvvmCross/MvvmCross/pull/1632) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Add generic WithConversion method [\#1631](https://github.com/MvvmCross/MvvmCross/pull/1631) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Removed sneaky 's' sitting in MvvmCross.Tests.nuspec breaking CI [\#1629](https://github.com/MvvmCross/MvvmCross/pull/1629) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Added additional strongly type bindings for android support libraries [\#1628](https://github.com/MvvmCross/MvvmCross/pull/1628) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Fix for incorrect default value in MvxFragmentAttribute xml doc [\#1627](https://github.com/MvvmCross/MvvmCross/pull/1627) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Removed redundant register of android fragment presenter [\#1626](https://github.com/MvvmCross/MvvmCross/pull/1626) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Migrate Test.Core to PCL [\#1625](https://github.com/MvvmCross/MvvmCross/pull/1625) ([willsb](https://github.com/willsb))
- Update Xamarin Forms packages [\#1624](https://github.com/MvvmCross/MvvmCross/pull/1624) ([martijn00](https://github.com/martijn00))
- :fire: :gun: bye, bye.... fixes \#1620 [\#1621](https://github.com/MvvmCross/MvvmCross/pull/1621) ([Cheesebaron](https://github.com/Cheesebaron))
- Remove examples from release build [\#1619](https://github.com/MvvmCross/MvvmCross/pull/1619) ([Cheesebaron](https://github.com/Cheesebaron))
- Add AccessibilityIdentifier to Tabs [\#1617](https://github.com/MvvmCross/MvvmCross/pull/1617) ([nmilcoff](https://github.com/nmilcoff))
- Throw exception instead of failing silently when using complex viewmodel parameter navigation [\#1616](https://github.com/MvvmCross/MvvmCross/pull/1616) ([azzlack](https://github.com/azzlack))
- Fix forms test project example 1 xaml [\#1613](https://github.com/MvvmCross/MvvmCross/pull/1613) ([ehuna](https://github.com/ehuna))
- Update .mailmap [\#1612](https://github.com/MvvmCross/MvvmCross/pull/1612) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Update .mailmap [\#1611](https://github.com/MvvmCross/MvvmCross/pull/1611) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Generic implementation of IMvxTargetBinding [\#1610](https://github.com/MvvmCross/MvvmCross/pull/1610) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Strongly typed MvvmCross code based binding properties [\#1609](https://github.com/MvvmCross/MvvmCross/pull/1609) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Removes RefectionEx Plugin [\#1608](https://github.com/MvvmCross/MvvmCross/pull/1608) ([willsb](https://github.com/willsb))
- Remove usage of MvvmCross NuGets in plugins [\#1607](https://github.com/MvvmCross/MvvmCross/pull/1607) ([Cheesebaron](https://github.com/Cheesebaron))
- Added destroy lifecycle for viewmodel [\#1605](https://github.com/MvvmCross/MvvmCross/pull/1605) ([Nickolas-](https://github.com/Nickolas-))
- Add forms test projects to main solution [\#1604](https://github.com/MvvmCross/MvvmCross/pull/1604) ([ehuna](https://github.com/ehuna))
- Improve key generation [\#1603](https://github.com/MvvmCross/MvvmCross/pull/1603) ([Nickolas-](https://github.com/Nickolas-))
- Create view events in Viewmodels [\#1601](https://github.com/MvvmCross/MvvmCross/pull/1601) ([martijn00](https://github.com/martijn00))
- Use correct formatting [\#1600](https://github.com/MvvmCross/MvvmCross/pull/1600) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Update .mailmap [\#1599](https://github.com/MvvmCross/MvvmCross/pull/1599) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Add implementation for EventHooks in iOS [\#1598](https://github.com/MvvmCross/MvvmCross/pull/1598) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Add lifecycle support to the IMvxViewModel and MvxViewModel [\#1596](https://github.com/MvvmCross/MvvmCross/pull/1596) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Move routing to mvvmcross source [\#1595](https://github.com/MvvmCross/MvvmCross/pull/1595) ([martijn00](https://github.com/martijn00))
- stop potential NullReferenceException [\#1593](https://github.com/MvvmCross/MvvmCross/pull/1593) ([jvannoord](https://github.com/jvannoord))
- Url escaping for phonenumber in IOS [\#1591](https://github.com/MvvmCross/MvvmCross/pull/1591) ([svego](https://github.com/svego))
- Fix MvxFilteringAdapter unreliability. [\#1590](https://github.com/MvvmCross/MvvmCross/pull/1590) ([aaerox](https://github.com/aaerox))
- Fix some broken links in readme [\#1588](https://github.com/MvvmCross/MvvmCross/pull/1588) ([waf](https://github.com/waf))
- Update nuget packages [\#1587](https://github.com/MvvmCross/MvvmCross/pull/1587) ([martijn00](https://github.com/martijn00))
- Virtualize MvxExpandableListAdapter methods [\#1583](https://github.com/MvvmCross/MvvmCross/pull/1583) ([viniciusmaia](https://github.com/viniciusmaia))
- Publish nugets on master and develop [\#1574](https://github.com/MvvmCross/MvvmCross/pull/1574) ([Cheesebaron](https://github.com/Cheesebaron))
- \[WIP\] Fix nuspecs for build script [\#1571](https://github.com/MvvmCross/MvvmCross/pull/1571) ([Cheesebaron](https://github.com/Cheesebaron))
- RecyclerView Item Holder View is accessable in code after OnBind... [\#1570](https://github.com/MvvmCross/MvvmCross/pull/1570) ([thefex](https://github.com/thefex))
- Feature/android support/recycler view header footer grouping support [\#1568](https://github.com/MvvmCross/MvvmCross/pull/1568) ([thefex](https://github.com/thefex))
- \[WIP\] Preparing build script for CI [\#1567](https://github.com/MvvmCross/MvvmCross/pull/1567) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix/android support/fab hide behavior bottom bar support  [\#1564](https://github.com/MvvmCross/MvvmCross/pull/1564) ([thefex](https://github.com/thefex))
- Update nuspec for Xamarin.Forms 2.3.3.180 [\#1562](https://github.com/MvvmCross/MvvmCross/pull/1562) ([kjeremy](https://github.com/kjeremy))
- Xamarin.Forms.2.3.3.180 [\#1556](https://github.com/MvvmCross/MvvmCross/pull/1556) ([kjeremy](https://github.com/kjeremy))
- Remove sqllite plugin [\#1554](https://github.com/MvvmCross/MvvmCross/pull/1554) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix output paths [\#1553](https://github.com/MvvmCross/MvvmCross/pull/1553) ([Cheesebaron](https://github.com/Cheesebaron))
- Updates to Add Argument ShowViewModel Analyzer, IMvxViewModelInitializer [\#1552](https://github.com/MvvmCross/MvvmCross/pull/1552) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Fix iOS-Support Tabs TestProject setup [\#1549](https://github.com/MvvmCross/MvvmCross/pull/1549) ([nmilcoff](https://github.com/nmilcoff))
- Import the correct Xamarin.Mac targets [\#1548](https://github.com/MvvmCross/MvvmCross/pull/1548) ([kjeremy](https://github.com/kjeremy))
- Split some common code out of MvxFragmentExtensions [\#1540](https://github.com/MvvmCross/MvvmCross/pull/1540) ([kjeremy](https://github.com/kjeremy))
- Reduce memory pressure in MvxNotifyPropertyChanged [\#1537](https://github.com/MvvmCross/MvvmCross/pull/1537) ([kjeremy](https://github.com/kjeremy))
- Explicitly remove views from the RecyclerView when it is detached [\#1536](https://github.com/MvvmCross/MvvmCross/pull/1536) ([kjeremy](https://github.com/kjeremy))
- Allow user to specify the LayoutManager in axml [\#1534](https://github.com/MvvmCross/MvvmCross/pull/1534) ([kjeremy](https://github.com/kjeremy))
- \[WIP\] Add uwp projects and cleanup old windows projects [\#1532](https://github.com/MvvmCross/MvvmCross/pull/1532) ([martijn00](https://github.com/martijn00))
- Cleanup windows projects [\#1530](https://github.com/MvvmCross/MvvmCross/pull/1530) ([martijn00](https://github.com/martijn00))
- Remove obsolete CommonInflate [\#1529](https://github.com/MvvmCross/MvvmCross/pull/1529) ([kjeremy](https://github.com/kjeremy))
- Remove unused windows phone references [\#1528](https://github.com/MvvmCross/MvvmCross/pull/1528) ([martijn00](https://github.com/martijn00))
- Fix the Android and Forms test projects [\#1527](https://github.com/MvvmCross/MvvmCross/pull/1527) ([martijn00](https://github.com/martijn00))
- Remove portable support and nuget folder [\#1526](https://github.com/MvvmCross/MvvmCross/pull/1526) ([martijn00](https://github.com/martijn00))
- Only run through the data context change if it's actually changed [\#1524](https://github.com/MvvmCross/MvvmCross/pull/1524) ([kjeremy](https://github.com/kjeremy))
- Remove repositories.config [\#1521](https://github.com/MvvmCross/MvvmCross/pull/1521) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Remove CrossUI, AutoView and Dialog from Mac solution [\#1520](https://github.com/MvvmCross/MvvmCross/pull/1520) ([MarcBruins](https://github.com/MarcBruins))
- Remove CrossUI Directory [\#1519](https://github.com/MvvmCross/MvvmCross/pull/1519) ([kjeremy](https://github.com/kjeremy))
- Ignore lock files [\#1517](https://github.com/MvvmCross/MvvmCross/pull/1517) ([martijn00](https://github.com/martijn00))
- Delete obsolete .sln file \(iOSSupport is now part of main repo\) [\#1516](https://github.com/MvvmCross/MvvmCross/pull/1516) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Add the test projects for iOSSupport library to main repo [\#1515](https://github.com/MvvmCross/MvvmCross/pull/1515) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Change misleading trace message [\#1514](https://github.com/MvvmCross/MvvmCross/pull/1514) ([gartesk](https://github.com/gartesk))
- Remove AutoView, CrossUI and Dialog [\#1511](https://github.com/MvvmCross/MvvmCross/pull/1511) ([kjeremy](https://github.com/kjeremy))
- Fix build [\#1510](https://github.com/MvvmCross/MvvmCross/pull/1510) ([Cheesebaron](https://github.com/Cheesebaron))
- Update deprecated OnAttach\(Activity\) to OnAttach\(Context\)  \(\#1503\) [\#1509](https://github.com/MvvmCross/MvvmCross/pull/1509) ([orzech85](https://github.com/orzech85))
- Fix references and nuget packages [\#1507](https://github.com/MvvmCross/MvvmCross/pull/1507) ([martijn00](https://github.com/martijn00))
- Merge Sub repos into main repo [\#1505](https://github.com/MvvmCross/MvvmCross/pull/1505) ([martijn00](https://github.com/martijn00))
- Add tvOS as part of the mac solution [\#1504](https://github.com/MvvmCross/MvvmCross/pull/1504) ([martijn00](https://github.com/martijn00))
- \#1398 [\#1501](https://github.com/MvvmCross/MvvmCross/pull/1501) ([richardblewett](https://github.com/richardblewett))
- WeakReference.IsAlive may not be accurate if true [\#1497](https://github.com/MvvmCross/MvvmCross/pull/1497) ([kjeremy](https://github.com/kjeremy))
- Automate register of IMvxAndroidViewPresenter in MvxAndroidSetup [\#1495](https://github.com/MvvmCross/MvvmCross/pull/1495) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Optimize MvxObservableCollection a bit [\#1494](https://github.com/MvvmCross/MvvmCross/pull/1494) ([kjeremy](https://github.com/kjeremy))
- Allow table view cells which don't inherit MvxTableViewCell in MvxBaseTableViewSource [\#1491](https://github.com/MvvmCross/MvvmCross/pull/1491) ([jamie94bc](https://github.com/jamie94bc))
- Fixes  \#1485 - MvxObservableCollection RemoveRange raises Reset action [\#1486](https://github.com/MvvmCross/MvvmCross/pull/1486) ([Prin53](https://github.com/Prin53))
- Small fix to avoid an analyzer to throw NRE causing warning at compil… [\#1481](https://github.com/MvvmCross/MvvmCross/pull/1481) ([azchohfi](https://github.com/azchohfi))
