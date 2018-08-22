---
layout: post
title: MvvmCross 5.1
date:   2017-07-20 11:37:35 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.1!

With the release of version 5.1 we focused on fixing the regressions and bugs introduced with the release of 5.0.
Noteworthy are:

* The fixes for Xamarin.Forms which are explained in: [medium.com/@martijn00](https://medium.com/@martijn00/using-mvvmcross-with-xamarin-forms-part-1-eaee5815bb8c)
* Prefix the ViewModel LifeCycle events with View* [#2020](https://github.com/MvvmCross/MvvmCross/pull/2020)
* Nested modals on iOS [#2037](https://github.com/MvvmCross/MvvmCross/pull/2037)
* New MacOS presenter [#1913](https://github.com/MvvmCross/MvvmCross/pull/1913)
* Multiple fixes for duplicate ViewModels
* Cleanup of the codebase using CodeFactor

# .NET Summer Hackfest 2017

With MvvmCross we are joining the .NET Summer Hackfest 2017!

The goals for this 2 week session from 21 August to 2 September will be:
* Convert MvvmCross to .NET Standard
* Update samples to latest version
* More Async await
* Websites fixes
* Documentation improvements
* Up-for-grabs issues

We'll use the social media hashtag #dotnetsummer and #mvvmcross

More information on this is available at: [https://dotnetfoundation.org/blog/announcing-net-summer-hackfest-2017](https://dotnetfoundation.org/blog/announcing-net-summer-hackfest-2017)

# Change Log


## [5.1.0](https://github.com/MvvmCross/MvvmCross/tree/5.1.0) (2017-07-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.6...5.1.0)

**Fixed bugs:**

- Stop using unsupported applicationDidFinishLaunching on iOS. [\#2033](https://github.com/MvvmCross/MvvmCross/pull/2033) ([DanielStolt](https://github.com/DanielStolt))
- Don't mask exceptions when completing android initialization [\#1930](https://github.com/MvvmCross/MvvmCross/pull/1930) ([edwinvanderham](https://github.com/edwinvanderham))

**Closed issues:**

- ViewModel's constructor is called twice when using NavigationService [\#2038](https://github.com/MvvmCross/MvvmCross/issues/2038)
- MvxNavigationService never calls IMvxViewModelLocator.Load [\#2036](https://github.com/MvvmCross/MvvmCross/issues/2036)
- mvx:Warning:  0.25 No sidemenu found. To use a sidemenu decorate the viewcontroller class with the 'MvxPanelPresentationAttribute' class and set the panel to 'Left' or 'Right'. [\#2034](https://github.com/MvvmCross/MvvmCross/issues/2034)
- Exception being thrown on fresh app start, OnCreate inaccessible? [\#2032](https://github.com/MvvmCross/MvvmCross/issues/2032)
- MvxViewModel\<TParameter\> occurs exception. [\#2028](https://github.com/MvvmCross/MvvmCross/issues/2028)
- MvxNavigationService and Linker All does not work [\#2025](https://github.com/MvvmCross/MvvmCross/issues/2025)
- Feature request: Lifecycle event for OnCreate and ViewDidLoad  [\#2018](https://github.com/MvvmCross/MvvmCross/issues/2018)
- Multiple instances of viewmodels being created when navigating when using MvvmCross with Forms and Master-Detail [\#1979](https://github.com/MvvmCross/MvvmCross/issues/1979)
- Fused Location Provider throws IllegalStateException [\#1955](https://github.com/MvvmCross/MvvmCross/issues/1955)
- Consider more binding extensions... possibly Automated Form Validation? [\#133](https://github.com/MvvmCross/MvvmCross/issues/133)

**Merged pull requests:**

- Adjust dispatchers after merging mask exceptions pr [\#2045](https://github.com/MvvmCross/MvvmCross/pull/2045) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix samples and duplicate ViewModels on Xamarin.Forms [\#2044](https://github.com/MvvmCross/MvvmCross/pull/2044) ([martijn00](https://github.com/martijn00))
- Add MvxNavigationService to LinkerPleaseInclude [\#2043](https://github.com/MvvmCross/MvvmCross/pull/2043) ([willsb](https://github.com/willsb))
- Add appstart using navigationservice [\#2042](https://github.com/MvvmCross/MvvmCross/pull/2042) ([martijn00](https://github.com/martijn00))
- Don't try to use deprecated init when using navigationservice [\#2041](https://github.com/MvvmCross/MvvmCross/pull/2041) ([martijn00](https://github.com/martijn00))
- MvxSimpleWpfViewPresenter supports MvxClosePresentationHint [\#2040](https://github.com/MvvmCross/MvvmCross/pull/2040) ([jz5](https://github.com/jz5))
- Use ViewModelLocator to load view models [\#2039](https://github.com/MvvmCross/MvvmCross/pull/2039) ([martijn00](https://github.com/martijn00))
- iOS ViewPresenter: Support for nested modals [\#2037](https://github.com/MvvmCross/MvvmCross/pull/2037) ([nmilcoff](https://github.com/nmilcoff))
- Fix Forms implementation for MvvmCross [\#2035](https://github.com/MvvmCross/MvvmCross/pull/2035) ([martijn00](https://github.com/martijn00))
- Revert "target profile 78 like other plugins" [\#2030](https://github.com/MvvmCross/MvvmCross/pull/2030) ([Cheesebaron](https://github.com/Cheesebaron))
- target profile 78 like other plugins [\#2029](https://github.com/MvvmCross/MvvmCross/pull/2029) ([khoussem](https://github.com/khoussem))
- Introduced new lifecycle event "Created" in MvxViewModel [\#2020](https://github.com/MvvmCross/MvvmCross/pull/2020) ([KennethKr](https://github.com/KennethKr))

## [5.0.6](https://github.com/MvvmCross/MvvmCross/tree/5.0.6) (2017-07-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.5...5.0.6)

**Fixed bugs:**

- replace create MvxNavigationController method call in MvxIosViewPrese… [\#2010](https://github.com/MvvmCross/MvvmCross/pull/2010) ([KanLei](https://github.com/KanLei))

**Closed issues:**

- MvvmCross plugin PictureChooser 5.0.5 - profile 259 [\#2017](https://github.com/MvvmCross/MvvmCross/issues/2017)
- when i try to install MvvmCross.Droid.Support.Fragment getting error [\#2016](https://github.com/MvvmCross/MvvmCross/issues/2016)
- Support for Resharper PropertyChangedHandler [\#1994](https://github.com/MvvmCross/MvvmCross/issues/1994)
- Suggestion for a Mac presenter for MvvmCross [\#1278](https://github.com/MvvmCross/MvvmCross/issues/1278)
- Inline converter creation in WPF [\#2000](https://github.com/MvvmCross/MvvmCross/issues/2000)
- SetTitleAndTabBarItem not called on 5.0.5 [\#1995](https://github.com/MvvmCross/MvvmCross/issues/1995)

**Merged pull requests:**

- Add Android Application class [\#2027](https://github.com/MvvmCross/MvvmCross/pull/2027) ([martijn00](https://github.com/martijn00))
- Add change presentation to navigationservice [\#2026](https://github.com/MvvmCross/MvvmCross/pull/2026) ([martijn00](https://github.com/martijn00))
- Change size of headers in PR template [\#2024](https://github.com/MvvmCross/MvvmCross/pull/2024) ([willsb](https://github.com/willsb))
- Add MvxTableViewHeaderFooterView [\#2021](https://github.com/MvvmCross/MvvmCross/pull/2021) ([willsb](https://github.com/willsb))
- Remove virtual calls from constructor [\#2019](https://github.com/MvvmCross/MvvmCross/pull/2019) ([martijn00](https://github.com/martijn00))
- added ability to initialize converter inline in xaml [\#2015](https://github.com/MvvmCross/MvvmCross/pull/2015) ([mgochmuradov](https://github.com/mgochmuradov))
- Support ReSharper convert auto-properties into properties that support INotifyPropertyChanged [\#2014](https://github.com/MvvmCross/MvvmCross/pull/2014) ([mvanbeusekom](https://github.com/mvanbeusekom))
- The `AppBarLayout` should be direct child of the `CoordinatedLayout` [\#2012](https://github.com/MvvmCross/MvvmCross/pull/2012) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Fix `MissingMethodException` when using `MvxImageView` pre-Android API 21 [\#2011](https://github.com/MvvmCross/MvvmCross/pull/2011) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Test project.uwp [\#2007](https://github.com/MvvmCross/MvvmCross/pull/2007) ([flyingxu](https://github.com/flyingxu))
- View Presenter Schema image updated [\#2006](https://github.com/MvvmCross/MvvmCross/pull/2006) ([Daniel-Krzyczkowski](https://github.com/Daniel-Krzyczkowski))
- Pages abstraction and ViewModel binding section [\#2005](https://github.com/MvvmCross/MvvmCross/pull/2005) ([Daniel-Krzyczkowski](https://github.com/Daniel-Krzyczkowski))
- Create MvxMacViewPresenter doc [\#2003](https://github.com/MvvmCross/MvvmCross/pull/2003) ([nmilcoff](https://github.com/nmilcoff))
- Move the sidebar attribute to the root to avoid annoyance with usings [\#2002](https://github.com/MvvmCross/MvvmCross/pull/2002) ([martijn00](https://github.com/martijn00))
- Fix for issue 1995 without breaking method signatures [\#1998](https://github.com/MvvmCross/MvvmCross/pull/1998) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Updated the XamarinSidebar sample to use the navigationservice [\#1997](https://github.com/MvvmCross/MvvmCross/pull/1997) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Fix crash when composing email without attachments [\#1996](https://github.com/MvvmCross/MvvmCross/pull/1996) ([ilber](https://github.com/ilber))

## [5.0.5](https://github.com/MvvmCross/MvvmCross/tree/5.0.5) (2017-06-25)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.4...5.0.5)

**Closed issues:**

- iOS - MvxIosViewPresenter throwing KeyNotFoundException via Show\(\) [\#1991](https://github.com/MvvmCross/MvvmCross/issues/1991)

**Merged pull requests:**

- Prevent KeyNotFoundException from always being throw MvxIosViewPresenter  [\#1992](https://github.com/MvvmCross/MvvmCross/pull/1992) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Style fixes [\#1990](https://github.com/MvvmCross/MvvmCross/pull/1990) ([martijn00](https://github.com/martijn00))

## [5.0.4](https://github.com/MvvmCross/MvvmCross/tree/5.0.4) (2017-06-23)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.3...5.0.4)

**Fixed bugs:**

- MvxTabBarViewController cannot be shown as child [\#1967](https://github.com/MvvmCross/MvvmCross/issues/1967)
- App crash with missing constructor on MvxImageView [\#1915](https://github.com/MvvmCross/MvvmCross/issues/1915)
- MvxImageView ctor missing [\#1966](https://github.com/MvvmCross/MvvmCross/pull/1966) ([Cheesebaron](https://github.com/Cheesebaron))

**Closed issues:**

- Enable tests on CI builds [\#1751](https://github.com/MvvmCross/MvvmCross/issues/1751)
- MvxNavigationService.Close is not setting the ViewModel on the NavigateEventArgs [\#1983](https://github.com/MvvmCross/MvvmCross/issues/1983)
- Order of Initialize call in Android after Navigation service call [\#1968](https://github.com/MvvmCross/MvvmCross/issues/1968)

**Merged pull requests:**

- Codefactor cleanup [\#1939](https://github.com/MvvmCross/MvvmCross/pull/1939) ([Cheesebaron](https://github.com/Cheesebaron))
- StyleCop run on some issue's [\#1988](https://github.com/MvvmCross/MvvmCross/pull/1988) ([martijn00](https://github.com/martijn00))
- Fix a couple of style cop issues [\#1985](https://github.com/MvvmCross/MvvmCross/pull/1985) ([martijn00](https://github.com/martijn00))
- added features to be able to provided the selected image on tab item [\#1984](https://github.com/MvvmCross/MvvmCross/pull/1984) ([biozal](https://github.com/biozal))
- Update nugets [\#1980](https://github.com/MvvmCross/MvvmCross/pull/1980) ([Cheesebaron](https://github.com/Cheesebaron))
- MvxIosViewPresenter: TabBarViewController as child  [\#1977](https://github.com/MvvmCross/MvvmCross/pull/1977) ([nmilcoff](https://github.com/nmilcoff))
- Added FishAngler showcase [\#1976](https://github.com/MvvmCross/MvvmCross/pull/1976) ([jstawski](https://github.com/jstawski))
- Fix header [\#1973](https://github.com/MvvmCross/MvvmCross/pull/1973) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix Initialize order [\#1971](https://github.com/MvvmCross/MvvmCross/pull/1971) ([martijn00](https://github.com/martijn00))

## [5.0.3](https://github.com/MvvmCross/MvvmCross/tree/5.0.3) (2017-06-19)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.2...5.0.3)

**Fixed bugs:**

- New navigation service creates two instances of VM and initialize the wrong one [\#1943](https://github.com/MvvmCross/MvvmCross/issues/1943)
- WPF Navigation Service doesn't work with parameter [\#1921](https://github.com/MvvmCross/MvvmCross/issues/1921)
- JsonLocalization plugin can't load resources with dash \( - \) in path [\#1645](https://github.com/MvvmCross/MvvmCross/issues/1645)

**Closed issues:**

- Cannot resolve Assembly or Windows Metadata file 'MvvmCross.WindowsUWP.dll' [\#1952](https://github.com/MvvmCross/MvvmCross/issues/1952)
- mvvmcross 5.0 Fatal signal 11 \(SIGSEGV\) [\#1881](https://github.com/MvvmCross/MvvmCross/issues/1881)
- CanExecute does not fire when RaiseCanExecuteChanged\(\) is called. [\#1877](https://github.com/MvvmCross/MvvmCross/issues/1877)
- Website issues/features [\#1727](https://github.com/MvvmCross/MvvmCross/issues/1727)
- Add generic interface for IMvxCommand [\#1946](https://github.com/MvvmCross/MvvmCross/issues/1946)
- Navigation Issues UWP after Upgrade [\#1940](https://github.com/MvvmCross/MvvmCross/issues/1940)
- UWP MvxRegion split-view navigation broken in 5.0.2  [\#1920](https://github.com/MvvmCross/MvvmCross/issues/1920)
- Feature suggestion - PictureChooser WPF - Add gif and png files in DialogBox [\#1891](https://github.com/MvvmCross/MvvmCross/issues/1891)

**Merged pull requests:**

- Fix Testproject compiling & runtime errors [\#1965](https://github.com/MvvmCross/MvvmCross/pull/1965) ([flyingxu](https://github.com/flyingxu))
- \#1921 Changed wpf implementation to use view model parameter sent wit… [\#1963](https://github.com/MvvmCross/MvvmCross/pull/1963) ([Bowman74](https://github.com/Bowman74))
- Add more checks to avoid illegal states [\#1956](https://github.com/MvvmCross/MvvmCross/pull/1956) ([Cheesebaron](https://github.com/Cheesebaron))
- Updated namespace for UWP project. [\#1953](https://github.com/MvvmCross/MvvmCross/pull/1953) ([Daniel-Krzyczkowski](https://github.com/Daniel-Krzyczkowski))
- Add Caledos Runner as showcase for the website [\#1951](https://github.com/MvvmCross/MvvmCross/pull/1951) ([domedellolio](https://github.com/domedellolio))
- Add cancelation and presentation bundle to extensions as well [\#1949](https://github.com/MvvmCross/MvvmCross/pull/1949) ([martijn00](https://github.com/martijn00))
- Add generic interfaces for IMvxCommand [\#1948](https://github.com/MvvmCross/MvvmCross/pull/1948) ([willsb](https://github.com/willsb))
- \#1940 Made changes to UWP navigation so: [\#1945](https://github.com/MvvmCross/MvvmCross/pull/1945) ([Bowman74](https://github.com/Bowman74))
- Don't depend on NUnit, no code uses it in this package [\#1944](https://github.com/MvvmCross/MvvmCross/pull/1944) ([Cheesebaron](https://github.com/Cheesebaron))
- \#1920 Made changes to multi region presenter for UWP so new navigatio… [\#1942](https://github.com/MvvmCross/MvvmCross/pull/1942) ([Bowman74](https://github.com/Bowman74))
- Enable Unit Tests on CI [\#1938](https://github.com/MvvmCross/MvvmCross/pull/1938) ([Cheesebaron](https://github.com/Cheesebaron))
- PreferredContentSize [\#1937](https://github.com/MvvmCross/MvvmCross/pull/1937) ([g0rdan](https://github.com/g0rdan))
- Document: Fix header layout [\#1936](https://github.com/MvvmCross/MvvmCross/pull/1936) ([jz5](https://github.com/jz5))
- Fix codefactor comments warnings [\#1935](https://github.com/MvvmCross/MvvmCross/pull/1935) ([Cheesebaron](https://github.com/Cheesebaron))
- Reduce complexity of Binding Parsers [\#1933](https://github.com/MvvmCross/MvvmCross/pull/1933) ([Cheesebaron](https://github.com/Cheesebaron))
- Update the picture chooser for WPF [\#1932](https://github.com/MvvmCross/MvvmCross/pull/1932) ([mathieumack](https://github.com/mathieumack))
- Fix markdown syntax [\#1931](https://github.com/MvvmCross/MvvmCross/pull/1931) ([jz5](https://github.com/jz5))
- Implemented correct behavior as for resource name generation [\#1929](https://github.com/MvvmCross/MvvmCross/pull/1929) ([LRP-sgravel](https://github.com/LRP-sgravel))
- Remove unused references [\#1927](https://github.com/MvvmCross/MvvmCross/pull/1927) ([martijn00](https://github.com/martijn00))
- Fix codefactor warnings "Arithmetic expressions must declare precedence" [\#1926](https://github.com/MvvmCross/MvvmCross/pull/1926) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Cleanup codebase with Resharper [\#1925](https://github.com/MvvmCross/MvvmCross/pull/1925) ([martijn00](https://github.com/martijn00))
- Add support for canceling awaiting a result on a viewmodel [\#1923](https://github.com/MvvmCross/MvvmCross/pull/1923) ([martijn00](https://github.com/martijn00))
- Create IMvxInteraction docs [\#1919](https://github.com/MvvmCross/MvvmCross/pull/1919) ([Cheesebaron](https://github.com/Cheesebaron))
- \[WIP\] New MacOS ViewPresenter \(missing documentation update\) [\#1913](https://github.com/MvvmCross/MvvmCross/pull/1913) ([nmilcoff](https://github.com/nmilcoff))

## [5.0.2](https://github.com/MvvmCross/MvvmCross/tree/5.0.2) (2017-06-06)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.1...5.0.2)

**Fixed bugs:**

- Problem with Visibility plugin [\#1876](https://github.com/MvvmCross/MvvmCross/issues/1876)
- Uwp Navigation Service doesn't work with Parameter [\#1905](https://github.com/MvvmCross/MvvmCross/issues/1905)
- android.support.v7.widget.SearchView Query binding not working [\#1882](https://github.com/MvvmCross/MvvmCross/issues/1882)
- Error MvvmCross.Uwp.rd.xml does not exist when compiling 5.0.1 [\#1879](https://github.com/MvvmCross/MvvmCross/issues/1879)
- Kevinf/1880 memory leak [\#1907](https://github.com/MvvmCross/MvvmCross/pull/1907) ([Bowman74](https://github.com/Bowman74))
- Consolidate library output and embed rd.xml [\#1901](https://github.com/MvvmCross/MvvmCross/pull/1901) ([Cheesebaron](https://github.com/Cheesebaron))

**Closed issues:**

- FloatingActionButton Click event binding not working [\#1896](https://github.com/MvvmCross/MvvmCross/issues/1896)
- Documentation: ViewModel lifecycle doesn't explain how to deal with tombstoning [\#1892](https://github.com/MvvmCross/MvvmCross/issues/1892)
- Documents: Plugins README are old [\#1886](https://github.com/MvvmCross/MvvmCross/issues/1886)
- Back button navigation with navigation drawer in android not directly go back [\#1875](https://github.com/MvvmCross/MvvmCross/issues/1875)
- Rework 'tagged' logic in build script [\#1874](https://github.com/MvvmCross/MvvmCross/issues/1874)
- PictureChooser.iOS scales incorrectly on high DPI devices [\#1759](https://github.com/MvvmCross/MvvmCross/issues/1759)
- Synchronous view model initialization [\#1902](https://github.com/MvvmCross/MvvmCross/issues/1902)
- Appearing event called multiple times on Android platform [\#1894](https://github.com/MvvmCross/MvvmCross/issues/1894)
- Add a way to change presentation attribute of ViewController at runtime [\#1887](https://github.com/MvvmCross/MvvmCross/issues/1887)
- Navigation Bug introduced in 5.0.1: View.OnCreate reinstantiates existing target ViewModel  [\#1880](https://github.com/MvvmCross/MvvmCross/issues/1880)

**Merged pull requests:**

- Fix header markdown in docs [\#1916](https://github.com/MvvmCross/MvvmCross/pull/1916) ([clottman](https://github.com/clottman))
- Document: Fix dead link, Add C\# highlight [\#1914](https://github.com/MvvmCross/MvvmCross/pull/1914) ([jz5](https://github.com/jz5))
- Documentation: fixed typos [\#1911](https://github.com/MvvmCross/MvvmCross/pull/1911) ([F1nZeR](https://github.com/F1nZeR))
- Document: Fix links [\#1909](https://github.com/MvvmCross/MvvmCross/pull/1909) ([jz5](https://github.com/jz5))
- Ported the "Tables and Cells in iOS" article [\#1908](https://github.com/MvvmCross/MvvmCross/pull/1908) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Removed invalid code in App.cs example [\#1906](https://github.com/MvvmCross/MvvmCross/pull/1906) ([fyndor](https://github.com/fyndor))
- Correcting the spelling of my name :\) [\#1904](https://github.com/MvvmCross/MvvmCross/pull/1904) ([jimbobbennett](https://github.com/jimbobbennett))
- Fix IsTagged and IsRepository logic in build script [\#1903](https://github.com/MvvmCross/MvvmCross/pull/1903) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixes issues 1876 and 1880 [\#1900](https://github.com/MvvmCross/MvvmCross/pull/1900) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Adds support for INotifyPropertyChanged to MvxWithEventPropertyInfoTargetBinding [\#1899](https://github.com/MvvmCross/MvvmCross/pull/1899) ([DanielStolt](https://github.com/DanielStolt))
- Adds target binding for UIPageControl on iOS [\#1898](https://github.com/MvvmCross/MvvmCross/pull/1898) ([DanielStolt](https://github.com/DanielStolt))
- \#1894 Fixed problem where C\# version of event handlers do not unsubsc… [\#1897](https://github.com/MvvmCross/MvvmCross/pull/1897) ([Bowman74](https://github.com/Bowman74))
- ViewModel-Lifecycle doc: Bring back info from Wiki [\#1895](https://github.com/MvvmCross/MvvmCross/pull/1895) ([nmilcoff](https://github.com/nmilcoff))
- Delete plugins readme files \(\#1886\) [\#1890](https://github.com/MvvmCross/MvvmCross/pull/1890) ([jz5](https://github.com/jz5))
- Implement option to override iOS presentation attribute [\#1888](https://github.com/MvvmCross/MvvmCross/pull/1888) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Add setvalueimpl for SearchView Query and change to TwoWay. [\#1883](https://github.com/MvvmCross/MvvmCross/pull/1883) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix opencollective height [\#1878](https://github.com/MvvmCross/MvvmCross/pull/1878) ([Garfield550](https://github.com/Garfield550))
- Replaced `Init` and `Start` with `Initialize` method [\#1873](https://github.com/MvvmCross/MvvmCross/pull/1873) ([mvanbeusekom](https://github.com/mvanbeusekom))

## [5.0.1](https://github.com/MvvmCross/MvvmCross/tree/5.0.1) (2017-05-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0...5.0.1)

**Fixed bugs:**

- UWP build error after upgrade to MvvmCross 5.0 and MvvmCross.Forms 5.0  [\#1861](https://github.com/MvvmCross/MvvmCross/issues/1861)
- Repeated navigation between two viewmodels only returns a result and invokes Initialize\(\) the first time [\#1855](https://github.com/MvvmCross/MvvmCross/issues/1855)
- MvxNavigationService does not handle recursive awaitable navigation correctly [\#1817](https://github.com/MvvmCross/MvvmCross/issues/1817)

**Closed issues:**

- Documentation: Search result links are incorrect [\#1866](https://github.com/MvvmCross/MvvmCross/issues/1866)
- MvxAppCompatActivity does not call view model's lifecycle methods [\#1857](https://github.com/MvvmCross/MvvmCross/issues/1857)
- iOS Missing method CreateNavigationController inMvxIosViewPresenter [\#1856](https://github.com/MvvmCross/MvvmCross/issues/1856)
- Create Pull Request template [\#1848](https://github.com/MvvmCross/MvvmCross/issues/1848)
- Incorrect width with wrap\_content [\#1546](https://github.com/MvvmCross/MvvmCross/issues/1546)
- File Plugin: GetSize\(\), GetListWriteTimeUtc\(\) [\#1155](https://github.com/MvvmCross/MvvmCross/issues/1155)
- Add abstraction for path in File plug-in [\#393](https://github.com/MvvmCross/MvvmCross/issues/393)
- Add LazyInitialize to Mvx static class [\#321](https://github.com/MvvmCross/MvvmCross/issues/321)
- `Start\(\)` not called in ViewModel in iOS [\#1862](https://github.com/MvvmCross/MvvmCross/issues/1862)
- Presentation bundle parameter in new navigation service [\#1860](https://github.com/MvvmCross/MvvmCross/issues/1860)
- Make Code of Conduct more visible [\#1849](https://github.com/MvvmCross/MvvmCross/issues/1849)
- PhoneCall Plugin targeting Android Marshmallow [\#1847](https://github.com/MvvmCross/MvvmCross/issues/1847)

**Merged pull requests:**

- Add Pull Request template [\#1872](https://github.com/MvvmCross/MvvmCross/pull/1872) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix for view models not correctly restored in fragments [\#1871](https://github.com/MvvmCross/MvvmCross/pull/1871) ([martijn00](https://github.com/martijn00))
- Fixes \#1861 duplicate Library Output added in NuGet [\#1870](https://github.com/MvvmCross/MvvmCross/pull/1870) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix missing presentation bundle in url navigation [\#1869](https://github.com/MvvmCross/MvvmCross/pull/1869) ([martijn00](https://github.com/martijn00))
- Fix 404 page permalink [\#1868](https://github.com/MvvmCross/MvvmCross/pull/1868) ([Garfield550](https://github.com/Garfield550))
- MvxIosViewPresenter: Bring back CreateNavigationController method [\#1867](https://github.com/MvvmCross/MvvmCross/pull/1867) ([nmilcoff](https://github.com/nmilcoff))
- Fix opencollective width on 2k and 4k resolution [\#1865](https://github.com/MvvmCross/MvvmCross/pull/1865) ([Garfield550](https://github.com/Garfield550))
- Allow for setting of tab title and icon through interface [\#1864](https://github.com/MvvmCross/MvvmCross/pull/1864) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Fix the unit test and add presentation values to navigation [\#1863](https://github.com/MvvmCross/MvvmCross/pull/1863) ([martijn00](https://github.com/martijn00))
- Add 404 page and more [\#1859](https://github.com/MvvmCross/MvvmCross/pull/1859) ([Garfield550](https://github.com/Garfield550))
- Make sure lifecycle methods are called on ViewModel [\#1858](https://github.com/MvvmCross/MvvmCross/pull/1858) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Fix not being able to use NavigationService from the appstart [\#1853](https://github.com/MvvmCross/MvvmCross/pull/1853) ([martijn00](https://github.com/martijn00))
- New site theme: Mvxtheme 1.0 [\#1852](https://github.com/MvvmCross/MvvmCross/pull/1852) ([Garfield550](https://github.com/Garfield550))
- Update API check for MvxPhoneCallTask.cs [\#1850](https://github.com/MvvmCross/MvvmCross/pull/1850) ([bazookajoey](https://github.com/bazookajoey))
- Fix for \#1759 \(PictureChooser iOS incorrect scaling\) [\#1824](https://github.com/MvvmCross/MvvmCross/pull/1824) ([ben-dean](https://github.com/ben-dean))
