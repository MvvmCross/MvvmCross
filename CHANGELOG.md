# Change Log

## [6.0.0-beta3](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta3) 
(2018-02-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta2...6.0.0-beta3)

**Fixed bugs:**

- PluginLoaders not found for platform specific plugins [\#2611](https://github.com/MvvmCross/MvvmCross/issues/2611)
- Child View Presentation does not work when using More Tabs \(more than five tabs\) \[iOS\]  [\#2609](https://github.com/MvvmCross/MvvmCross/issues/2609)
- MvxWindowsPage cannot navigate to MvxContentPage [\#2466](https://github.com/MvvmCross/MvvmCross/issues/2466)
- Allow more than 5 children in MoreNavigationController [\#2610](https://github.com/MvvmCross/MvvmCross/pull/2610) ([thefex](https://github.com/thefex))

**Closed issues:**

- MvxObservableCollection reports wrong index when doing AddRange [\#2515](https://github.com/MvvmCross/MvvmCross/issues/2515)

**Merged pull requests:**

- Improve the IMvxPluginManager interface [\#2616](https://github.com/MvvmCross/MvvmCross/pull/2616) ([willsb](https://github.com/willsb))
- Fixed issue \#2515 where MvxObservableCollection.AddRange\(\) reports wrong index [\#2614](https://github.com/MvvmCross/MvvmCross/pull/2614) ([Strifex](https://github.com/Strifex))
- Fix a bunch of Warnings [\#2613](https://github.com/MvvmCross/MvvmCross/pull/2613) ([Cheesebaron](https://github.com/Cheesebaron))
- Update Forms namespaces to match traditional Xamarin  [\#2612](https://github.com/MvvmCross/MvvmCross/pull/2612) ([nmilcoff](https://github.com/nmilcoff))
- New plugin architecture [\#2603](https://github.com/MvvmCross/MvvmCross/pull/2603) ([willsb](https://github.com/willsb))
- More unittests [\#2596](https://github.com/MvvmCross/MvvmCross/pull/2596) ([Cheesebaron](https://github.com/Cheesebaron))

## [6.0.0-beta2](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta2) (2018-02-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta1...6.0.0-beta2)

## [6.0.0-beta1](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta1) (2018-02-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.3...6.0.0-beta1)

**Fixed bugs:**

- Fragment close does not work if fragment presentation attribute has backstack set to false [\#2600](https://github.com/MvvmCross/MvvmCross/issues/2600)
- Platform.Mac startup exception Foundation.You\_Should\_Not\_Call\_base\_In\_This\_Method [\#2591](https://github.com/MvvmCross/MvvmCross/issues/2591)
- Custom Presentation Hint Handler is ignored [\#2589](https://github.com/MvvmCross/MvvmCross/issues/2589)
- UITextview binding - missing source event info in MvxWeakEventSubscription Parameter name: sourceEventInfo [\#2543](https://github.com/MvvmCross/MvvmCross/issues/2543)
- MvxBottomSheetDialogFragment OnDestroy does not consider finsished parameter [\#2525](https://github.com/MvvmCross/MvvmCross/issues/2525)
- RegisterNavigationServiceAppStart vs RegisterAppStart [\#2447](https://github.com/MvvmCross/MvvmCross/issues/2447)
- 'System.TypeInitializationException' In 'MvvmCross.Core.Platform.LogProviders.ConsoleLogProvider' On UWP Projects [\#2333](https://github.com/MvvmCross/MvvmCross/issues/2333)
- Inconsistent PCL profile for PictureChooser [\#2295](https://github.com/MvvmCross/MvvmCross/issues/2295)
- Align nuspec profiles with plugin PCL profiles [\#2031](https://github.com/MvvmCross/MvvmCross/issues/2031)
-  \#2600 no backstack fragment close does not work hotfix [\#2601](https://github.com/MvvmCross/MvvmCross/pull/2601) ([thefex](https://github.com/thefex))
- Fix inheritance for MvxBaseSplitViewController class with constraint [\#2564](https://github.com/MvvmCross/MvvmCross/pull/2564) ([nmilcoff](https://github.com/nmilcoff))
- MvxBottomSheetDialogFragment: Fix OnDestroy [\#2526](https://github.com/MvvmCross/MvvmCross/pull/2526) ([nmilcoff](https://github.com/nmilcoff))
- Improve implementation of IMvxAndroidCurrentTopActivity [\#2513](https://github.com/MvvmCross/MvvmCross/pull/2513) ([nmilcoff](https://github.com/nmilcoff))
- Fix MvxTabBarViewController.CloseTab: Pick correct ViewController [\#2506](https://github.com/MvvmCross/MvvmCross/pull/2506) ([nmilcoff](https://github.com/nmilcoff))
- Switch parameters in MvxException so that first exception is InnerException [\#2504](https://github.com/MvvmCross/MvvmCross/pull/2504) ([mubold](https://github.com/mubold))
- MvxNavigationServiceAppStart: Don't swallow exceptions [\#2471](https://github.com/MvvmCross/MvvmCross/pull/2471) ([nmilcoff](https://github.com/nmilcoff))

**Closed issues:**

- Correct way to register navigation service [\#2571](https://github.com/MvvmCross/MvvmCross/issues/2571)
- \[Question\] - thoughts on routing for navigation [\#2563](https://github.com/MvvmCross/MvvmCross/issues/2563)
- Remove usage of MvxTrace from code [\#2541](https://github.com/MvvmCross/MvvmCross/issues/2541)
- System.Reflection GetCustomAttributes [\#2535](https://github.com/MvvmCross/MvvmCross/issues/2535)
- Inconsistency between MvxCommand\<T\> and MvxAsyncCommand\<T\> implementing IMvxCommand [\#2520](https://github.com/MvvmCross/MvvmCross/issues/2520)
- Remove IMvxIosModalHost [\#2475](https://github.com/MvvmCross/MvvmCross/issues/2475)
- WithConversion\<...\> does not appear do the same as WithConversion\(new ...\) [\#2449](https://github.com/MvvmCross/MvvmCross/issues/2449)
- The new logger infrastructure should not send null messages to IMvxLog [\#2437](https://github.com/MvvmCross/MvvmCross/issues/2437)
- MvxExpandableListView GroupClick binding replaces group expanding functionality [\#2408](https://github.com/MvvmCross/MvvmCross/issues/2408)
- \[Android\] MvxSplashScreenActivity should not have to be the first Activity [\#2261](https://github.com/MvvmCross/MvvmCross/issues/2261)
- MvxViewModel\<TParameter\>.Init should be removed [\#2257](https://github.com/MvvmCross/MvvmCross/issues/2257)
- MvxTabLayoutPresentationAttribute title localization [\#2211](https://github.com/MvvmCross/MvvmCross/issues/2211)
- Android RecyclerView.ViewHolder DataContext property change view updates lost depending on RecyclerView item position on screen [\#2142](https://github.com/MvvmCross/MvvmCross/issues/2142)
- MvxObservableCollection cause Adapter to show wrong cells data. [\#2130](https://github.com/MvvmCross/MvvmCross/issues/2130)
- .NET Standard support [\#2059](https://github.com/MvvmCross/MvvmCross/issues/2059)

**Merged pull requests:**

- Fixing references and UWP/WPF playground samples [\#2585](https://github.com/MvvmCross/MvvmCross/pull/2585) ([nickrandolph](https://github.com/nickrandolph))
- Add back the other playground projects [\#2584](https://github.com/MvvmCross/MvvmCross/pull/2584) ([martijn00](https://github.com/martijn00))
- Move actual unit tests and test project to correct folders [\#2583](https://github.com/MvvmCross/MvvmCross/pull/2583) ([martijn00](https://github.com/martijn00))
- Correctly set nuget dependencies [\#2582](https://github.com/MvvmCross/MvvmCross/pull/2582) ([Cheesebaron](https://github.com/Cheesebaron))
- Open up localization for custom implementations [\#2579](https://github.com/MvvmCross/MvvmCross/pull/2579) ([martijn00](https://github.com/martijn00))
- Fix type in interface [\#2576](https://github.com/MvvmCross/MvvmCross/pull/2576) ([Cheesebaron](https://github.com/Cheesebaron))
- Move files into correct folders [\#2570](https://github.com/MvvmCross/MvvmCross/pull/2570) ([martijn00](https://github.com/martijn00))
- Add support for Tizen [\#2569](https://github.com/MvvmCross/MvvmCross/pull/2569) ([martijn00](https://github.com/martijn00))
- Add .NET Foundation file headers to all files [\#2568](https://github.com/MvvmCross/MvvmCross/pull/2568) ([martijn00](https://github.com/martijn00))
- Fix a couple of warnings [\#2567](https://github.com/MvvmCross/MvvmCross/pull/2567) ([martijn00](https://github.com/martijn00))
- Convert UnitTests to XUnit [\#2566](https://github.com/MvvmCross/MvvmCross/pull/2566) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixing startup issue where ioc isn't initialized [\#2565](https://github.com/MvvmCross/MvvmCross/pull/2565) ([nickrandolph](https://github.com/nickrandolph))
- Add back Android playground projects [\#2562](https://github.com/MvvmCross/MvvmCross/pull/2562) ([martijn00](https://github.com/martijn00))
- Remove memory inefficient droid layouts [\#2561](https://github.com/MvvmCross/MvvmCross/pull/2561) ([willsb](https://github.com/willsb))
- Add back WPF [\#2560](https://github.com/MvvmCross/MvvmCross/pull/2560) ([martijn00](https://github.com/martijn00))
- Remove ShowViewModel in favor for MvxNavigationService [\#2559](https://github.com/MvvmCross/MvvmCross/pull/2559) ([martijn00](https://github.com/martijn00))
- Align view presenters, registration and remove modal host [\#2558](https://github.com/MvvmCross/MvvmCross/pull/2558) ([martijn00](https://github.com/martijn00))
- Rename Android NativeColor method for consistency [\#2557](https://github.com/MvvmCross/MvvmCross/pull/2557) ([willsb](https://github.com/willsb))
- Netstandard fixes for the logging mechanism [\#2556](https://github.com/MvvmCross/MvvmCross/pull/2556) ([willsb](https://github.com/willsb))
- Implement IMvxCommand in MvxAsyncCommand\<T\> [\#2552](https://github.com/MvvmCross/MvvmCross/pull/2552) ([kipters](https://github.com/kipters))
- Add UWP StarterPack content [\#2551](https://github.com/MvvmCross/MvvmCross/pull/2551) ([kipters](https://github.com/kipters))
- Small fix downloadcache.md [\#2546](https://github.com/MvvmCross/MvvmCross/pull/2546) ([wcoder](https://github.com/wcoder))
- Updates for the docs [\#2542](https://github.com/MvvmCross/MvvmCross/pull/2542) ([martijn00](https://github.com/martijn00))
- Fixing build for MvvmCross.Forms [\#2531](https://github.com/MvvmCross/MvvmCross/pull/2531) ([nickrandolph](https://github.com/nickrandolph))
- Update to .NET Standard [\#2530](https://github.com/MvvmCross/MvvmCross/pull/2530) ([martijn00](https://github.com/martijn00))
- Fixing Android attributes [\#2529](https://github.com/MvvmCross/MvvmCross/pull/2529) ([nickrandolph](https://github.com/nickrandolph))
- Fixing layout of files for plugins for multi-targetting [\#2528](https://github.com/MvvmCross/MvvmCross/pull/2528) ([nickrandolph](https://github.com/nickrandolph))
- Update navigation.md [\#2524](https://github.com/MvvmCross/MvvmCross/pull/2524) ([asterixorobelix](https://github.com/asterixorobelix))
- Adding NativePage to Playground sample for UWP [\#2523](https://github.com/MvvmCross/MvvmCross/pull/2523) ([nickrandolph](https://github.com/nickrandolph))
- Fix native pages not loading without attribute [\#2522](https://github.com/MvvmCross/MvvmCross/pull/2522) ([martijn00](https://github.com/martijn00))
- Update mvvmcross-overview.md [\#2521](https://github.com/MvvmCross/MvvmCross/pull/2521) ([asterixorobelix](https://github.com/asterixorobelix))
- Fix minor typos [\#2519](https://github.com/MvvmCross/MvvmCross/pull/2519) ([programmation](https://github.com/programmation))
- MvxAndroidViewsContainer: Remove NewTask flag as default [\#2516](https://github.com/MvvmCross/MvvmCross/pull/2516) ([nmilcoff](https://github.com/nmilcoff))
- Add LogLevel availability check to IMvxLog [\#2514](https://github.com/MvvmCross/MvvmCross/pull/2514) ([willsb](https://github.com/willsb))
-  Make Mvx...Cell inherit from IMvxCell instead of IMvxElement [\#2511](https://github.com/MvvmCross/MvvmCross/pull/2511) ([mubold](https://github.com/mubold))
- Update message displayed for OnViewNewIntent [\#2510](https://github.com/MvvmCross/MvvmCross/pull/2510) ([nmilcoff](https://github.com/nmilcoff))
- Added reference to a newer blog post on resx localization by Daniel Krzyczkowski [\#2509](https://github.com/MvvmCross/MvvmCross/pull/2509) ([mellson](https://github.com/mellson))
- Checks fragment type when showing a DialogFragment and throws MvxException instead of InvalidCastException [\#2507](https://github.com/MvvmCross/MvvmCross/pull/2507) ([MKuckert](https://github.com/MKuckert))
- Merge \#2438 into develop [\#2505](https://github.com/MvvmCross/MvvmCross/pull/2505) ([martijn00](https://github.com/martijn00))
- Avoid the Foundation.You\_Should\_Not\_Call\_base\_In\_This\_Method exception [\#2499](https://github.com/MvvmCross/MvvmCross/pull/2499) ([flyingxu](https://github.com/flyingxu))
- Making it easier to override default IMvxViewModelTypeFinder implementation [\#2498](https://github.com/MvvmCross/MvvmCross/pull/2498) ([nickrandolph](https://github.com/nickrandolph))
- Upgrading iOS csproj to use Visible element instead of InProject [\#2493](https://github.com/MvvmCross/MvvmCross/pull/2493) ([nickrandolph](https://github.com/nickrandolph))
- New events binding [\#2490](https://github.com/MvvmCross/MvvmCross/pull/2490) ([Saratsin](https://github.com/Saratsin))
- Fix for Expand/Colapse problem when GroupClick is bound. [\#2489](https://github.com/MvvmCross/MvvmCross/pull/2489) ([AlexStefan](https://github.com/AlexStefan))
- Improve IMvxOverridePresentationAttribute by providing the MvxViewModelRequest as parameter [\#2483](https://github.com/MvvmCross/MvvmCross/pull/2483) ([nmilcoff](https://github.com/nmilcoff))
- Added support for nested fragments \(ChildFragmentManager\) [\#2482](https://github.com/MvvmCross/MvvmCross/pull/2482) ([Qwin](https://github.com/Qwin))
- Update WithDictionaryConversion binding to have optional fallback [\#2480](https://github.com/MvvmCross/MvvmCross/pull/2480) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Webbrowser plugin for mac [\#2464](https://github.com/MvvmCross/MvvmCross/pull/2464) ([tofutim](https://github.com/tofutim))
- mvxforms/droid-resources [\#2461](https://github.com/MvvmCross/MvvmCross/pull/2461) ([johnnywebb](https://github.com/johnnywebb))

## [5.6.3](https://github.com/MvvmCross/MvvmCross/tree/5.6.3) (2017-12-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.2...5.6.3)

**Fixed bugs:**

- UITextView target binding fails when subscribing to changes [\#2484](https://github.com/MvvmCross/MvvmCross/issues/2484)
- UISwitch binding doesn't work [\#2462](https://github.com/MvvmCross/MvvmCross/issues/2462)
- UIBarButtonItem binding doesn't work [\#2456](https://github.com/MvvmCross/MvvmCross/issues/2456)
- MvxUIDatePickerDateTargetBinding crash on subscription [\#2487](https://github.com/MvvmCross/MvvmCross/issues/2487)
- Fix MvxBaseUIDatePickerTargetBinding [\#2488](https://github.com/MvvmCross/MvvmCross/pull/2488) ([nmilcoff](https://github.com/nmilcoff))
- Fix MvxUITextViewTextTargetBinding crash on subscription [\#2460](https://github.com/MvvmCross/MvvmCross/pull/2460) ([nmilcoff](https://github.com/nmilcoff))

**Closed issues:**

- MvxRecyclerAdapter NotifyDataSetChanged not called on mainthread [\#2144](https://github.com/MvvmCross/MvvmCross/issues/2144)

**Merged pull requests:**

- Adds support for child IoC container [\#2438](https://github.com/MvvmCross/MvvmCross/pull/2438) ([MKuckert](https://github.com/MKuckert))
- Update v5.6 release notes [\#2486](https://github.com/MvvmCross/MvvmCross/pull/2486) ([kalpesh-infiswift](https://github.com/kalpesh-infiswift))
- Fixing navigation to master detail pages for UWP [\#2485](https://github.com/MvvmCross/MvvmCross/pull/2485) ([nickrandolph](https://github.com/nickrandolph))
- iOS LinkerPleaseInclude: Added DidProcessEditing line [\#2481](https://github.com/MvvmCross/MvvmCross/pull/2481) ([nmilcoff](https://github.com/nmilcoff))
- MvxRecyclerViewAdapter: Support worker threads [\#2477](https://github.com/MvvmCross/MvvmCross/pull/2477) ([nmilcoff](https://github.com/nmilcoff))
- Remove the 2 outdated forms samples [\#2476](https://github.com/MvvmCross/MvvmCross/pull/2476) ([martijn00](https://github.com/martijn00))
- Add mac target for Forms [\#2474](https://github.com/MvvmCross/MvvmCross/pull/2474) ([martijn00](https://github.com/martijn00))
- Fix post date since MvvmCross 5.6 was released in Dec not Nov [\#2473](https://github.com/MvvmCross/MvvmCross/pull/2473) ([ejlofgren](https://github.com/ejlofgren))
- Improve handling of navigation hints in XF when a modal is displayed [\#2472](https://github.com/MvvmCross/MvvmCross/pull/2472) ([nickrandolph](https://github.com/nickrandolph))
- Use non mvx specific pages to avoid loading viewmodels [\#2468](https://github.com/MvvmCross/MvvmCross/pull/2468) ([martijn00](https://github.com/martijn00))
- Fix UISwitch Target binding registration [\#2467](https://github.com/MvvmCross/MvvmCross/pull/2467) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix MvxUIBarButtonItemTargetBinding [\#2459](https://github.com/MvvmCross/MvvmCross/pull/2459) ([nmilcoff](https://github.com/nmilcoff))
- Fix weird adding behaviour for Android. The first elements in lists w… [\#2455](https://github.com/MvvmCross/MvvmCross/pull/2455) ([KoenDeleij](https://github.com/KoenDeleij))

## [5.6.2](https://github.com/MvvmCross/MvvmCross/tree/5.6.2) (2017-12-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.1...5.6.2)

## [5.6.1](https://github.com/MvvmCross/MvvmCross/tree/5.6.1) (2017-12-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.0...5.6.1)

**Merged pull requests:**

- Fix infinite loop in modal when having an open modal [\#2457](https://github.com/MvvmCross/MvvmCross/pull/2457) ([martijn00](https://github.com/martijn00))
- Allow RootViewController animations with attributes [\#2453](https://github.com/MvvmCross/MvvmCross/pull/2453) ([willsb](https://github.com/willsb))
- Dictionary conversion [\#2444](https://github.com/MvvmCross/MvvmCross/pull/2444) ([Tyron18](https://github.com/Tyron18))

## [5.6.0](https://github.com/MvvmCross/MvvmCross/tree/5.6.0) (2017-12-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.5.2...5.6.0)

**Fixed bugs:**

- MvxBottomSheetDialogFragment doesn't forward View events to ViewModel [\#2431](https://github.com/MvvmCross/MvvmCross/issues/2431)
- override Close is not called when Mac Window is closed using x button [\#2199](https://github.com/MvvmCross/MvvmCross/issues/2199)
- UWP MvxSuspensionManager does not call ReloadState nor ReloadFromBundle after migration to mvvmcross 5.4 [\#2388](https://github.com/MvvmCross/MvvmCross/issues/2388)
- macOS: Add lifecycle events and presenter improvement [\#2432](https://github.com/MvvmCross/MvvmCross/pull/2432) ([nmilcoff](https://github.com/nmilcoff))
- Fix MvxViewModelRequest for fragments and improve ViewDestroy callback [\#2420](https://github.com/MvvmCross/MvvmCross/pull/2420) ([nmilcoff](https://github.com/nmilcoff))

**Closed issues:**

- Preserve.cs [\#2446](https://github.com/MvvmCross/MvvmCross/issues/2446)
- ShowNestedFragment throws exception when host fragment is not visible [\#2442](https://github.com/MvvmCross/MvvmCross/issues/2442)
- Fragments inside ViewPager are not restored  [\#2403](https://github.com/MvvmCross/MvvmCross/issues/2403)
- MvxTableViewSource CollectionChangedOnCollectionChanged can be executed on a worker thread [\#2360](https://github.com/MvvmCross/MvvmCross/issues/2360)
- Change Event subscriptions in Target Bindings on iOS to Weak [\#2145](https://github.com/MvvmCross/MvvmCross/issues/2145)
- MasterDetail Icon disappears on navigation \(iPhone only\) [\#2427](https://github.com/MvvmCross/MvvmCross/issues/2427)
- Update tvOS presenters to the new iOS presenter [\#2108](https://github.com/MvvmCross/MvvmCross/issues/2108)

**Merged pull requests:**

- Improve iOS bindings [\#2452](https://github.com/MvvmCross/MvvmCross/pull/2452) ([willsb](https://github.com/willsb))
- Add hint to set the current page in a parent page [\#2451](https://github.com/MvvmCross/MvvmCross/pull/2451) ([martijn00](https://github.com/martijn00))
- added changes for tvOS [\#2450](https://github.com/MvvmCross/MvvmCross/pull/2450) ([biozal](https://github.com/biozal))
- Add event hooks to MvxNavigationService.ChangePresentation [\#2448](https://github.com/MvvmCross/MvvmCross/pull/2448) ([nmilcoff](https://github.com/nmilcoff))
- tvOS Split View Presentation Support [\#2443](https://github.com/MvvmCross/MvvmCross/pull/2443) ([biozal](https://github.com/biozal))
- MvxTableViewSource.CollectionChangedOnCollectionChanged: Allow execution from a worker thread [\#2441](https://github.com/MvvmCross/MvvmCross/pull/2441) ([nmilcoff](https://github.com/nmilcoff))
- Fix Playground.Droid tabs [\#2439](https://github.com/MvvmCross/MvvmCross/pull/2439) ([nmilcoff](https://github.com/nmilcoff))
- Fixes \#2431 MvxBottomSheetDialogFragment not forwarding events [\#2435](https://github.com/MvvmCross/MvvmCross/pull/2435) ([tbalcom](https://github.com/tbalcom))
- Adding reloading to existing view models [\#2434](https://github.com/MvvmCross/MvvmCross/pull/2434) ([nickrandolph](https://github.com/nickrandolph))
- Fixing issue where icon disappears when navigating within master deta… [\#2429](https://github.com/MvvmCross/MvvmCross/pull/2429) ([nickrandolph](https://github.com/nickrandolph))
- tvOS presentation update to match features in other platforms [\#2414](https://github.com/MvvmCross/MvvmCross/pull/2414) ([biozal](https://github.com/biozal))

## [5.5.2](https://github.com/MvvmCross/MvvmCross/tree/5.5.2) (2017-11-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.5.1...5.5.2)

**Fixed bugs:**

- MvxObservableCollection:  "Add" and "AddRange" methods generates an event arguments with different structures. [\#2338](https://github.com/MvvmCross/MvvmCross/issues/2338)
- Fixes Bugs in UWP PresentationAttribute handling [\#2424](https://github.com/MvvmCross/MvvmCross/pull/2424) ([strebbin](https://github.com/strebbin))
- Fixed broken MvxUIDatePickerCountdownDurationTargetBinding. [\#2419](https://github.com/MvvmCross/MvvmCross/pull/2419) ([DaRosenberg](https://github.com/DaRosenberg))
- Fixing missing icon on ios [\#2416](https://github.com/MvvmCross/MvvmCross/pull/2416) ([nickrandolph](https://github.com/nickrandolph))
- Fixing issue with default page presentationattribute where viewmodelt… [\#2409](https://github.com/MvvmCross/MvvmCross/pull/2409) ([nickrandolph](https://github.com/nickrandolph))
- Fixes MvxObservableCollection.AddRange firing wrong changed event [\#2407](https://github.com/MvvmCross/MvvmCross/pull/2407) ([MKuckert](https://github.com/MKuckert))
- Fixing double-navigation when navigation is hosted within master-detail [\#2406](https://github.com/MvvmCross/MvvmCross/pull/2406) ([nickrandolph](https://github.com/nickrandolph))

**Merged pull requests:**

- Add UIBarButtonItem target binding [\#2413](https://github.com/MvvmCross/MvvmCross/pull/2413) ([Cheesebaron](https://github.com/Cheesebaron))
- Add change presentation hints to Forms to allow pop [\#2412](https://github.com/MvvmCross/MvvmCross/pull/2412) ([martijn00](https://github.com/martijn00))
- Fixing issue where content page with attributes set to NoHistory and … [\#2410](https://github.com/MvvmCross/MvvmCross/pull/2410) ([nickrandolph](https://github.com/nickrandolph))
- Adds CountDownDuration target binding for UIDatePicker [\#2353](https://github.com/MvvmCross/MvvmCross/pull/2353) ([sfmskywalker](https://github.com/sfmskywalker))

## [5.5.1](https://github.com/MvvmCross/MvvmCross/tree/5.5.1) (2017-11-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.5.0...5.5.1)

**Fixed bugs:**

- UWP Presentation Attributes not working correctly [\#2423](https://github.com/MvvmCross/MvvmCross/issues/2423)
- empty view\(xaml page not loading\) [\#2404](https://github.com/MvvmCross/MvvmCross/issues/2404)

**Closed issues:**

- BarBackgroundColor does not work in the UWP [\#2405](https://github.com/MvvmCross/MvvmCross/issues/2405)

## [5.5.0](https://github.com/MvvmCross/MvvmCross/tree/5.5.0) (2017-11-23)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.4.2...5.5.0)

**Fixed bugs:**

- Forms: TargetInvocationException when using ShowViewModel with parameter [\#2363](https://github.com/MvvmCross/MvvmCross/issues/2363)
- ViewModel Initialize method not called using MvxViewPagerFragmentInfo [\#2297](https://github.com/MvvmCross/MvvmCross/issues/2297)
- ViewModel's SaveState/ReloadState and NavigationService [\#2167](https://github.com/MvvmCross/MvvmCross/issues/2167)
- Feedback: The new Navigation Service and the life cycle it introduces [\#2105](https://github.com/MvvmCross/MvvmCross/issues/2105)
- Navigation with Results and Configuration Change cause premature delivery of null result [\#2384](https://github.com/MvvmCross/MvvmCross/issues/2384)
- Playground.Forms.Droid can't resume after being hidden by back key [\#2332](https://github.com/MvvmCross/MvvmCross/issues/2332)
- MasterDetailExample.UWP crashes with 'System.NullReferenceException: Object reference not set to an instance of an object' [\#2304](https://github.com/MvvmCross/MvvmCross/issues/2304)
- Improvements to UIDatePicker target bindings [\#2375](https://github.com/MvvmCross/MvvmCross/pull/2375) ([DaRosenberg](https://github.com/DaRosenberg))

**Closed issues:**

- AndroidPresenter: Close all Fragments of Activity when doing CloseActivity\(\) [\#2398](https://github.com/MvvmCross/MvvmCross/issues/2398)
- Could not install package 'MvvmCross.Plugin.PictureChooser 5.4.2' into a "Profile78" project [\#2369](https://github.com/MvvmCross/MvvmCross/issues/2369)
- Initialize not called when manually instantiating an MvxViewModel [\#1972](https://github.com/MvvmCross/MvvmCross/issues/1972)
- UWP crash at launch "Failed to resolve type MvvmCross.Core.Views.IMvxViewPresenter" [\#2397](https://github.com/MvvmCross/MvvmCross/issues/2397)
- Resuming app with tabs as root page causes duplicate page to load as new navigation \(MvvmCross 5.4.2 / MvxTabbedPage root\) [\#2373](https://github.com/MvvmCross/MvvmCross/issues/2373)
- Migration issue from 5.0.3 to 5.1.1 - Application is null / MvxFormsAppCompatActivity / base.OnCreate\(bundle\) / Resolved, but useful info [\#2129](https://github.com/MvvmCross/MvvmCross/issues/2129)

**Merged pull requests:**

- Fix logging in netstandard [\#2399](https://github.com/MvvmCross/MvvmCross/pull/2399) ([willsb](https://github.com/willsb))
- Fix Droid Activities when navigating for result [\#2400](https://github.com/MvvmCross/MvvmCross/pull/2400) ([nmilcoff](https://github.com/nmilcoff))
- Add possibility to get translation directly instead of binding to object [\#2396](https://github.com/MvvmCross/MvvmCross/pull/2396) ([martijn00](https://github.com/martijn00))
- Update documentation about Xamarin.Forms Android [\#2395](https://github.com/MvvmCross/MvvmCross/pull/2395) ([wcoder](https://github.com/wcoder))
- Fix typo in 2017-11-15-joining-net-foundation.md [\#2393](https://github.com/MvvmCross/MvvmCross/pull/2393) ([ejlofgren](https://github.com/ejlofgren))
- Update 2017-11-15-joining-net-foundation.md [\#2391](https://github.com/MvvmCross/MvvmCross/pull/2391) ([Cheesebaron](https://github.com/Cheesebaron))
- Add .net foundation announcement post [\#2390](https://github.com/MvvmCross/MvvmCross/pull/2390) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixing Master-Detail Forms implementation [\#2387](https://github.com/MvvmCross/MvvmCross/pull/2387) ([nickrandolph](https://github.com/nickrandolph))
- Fixing visual studio load issue with playground projects [\#2386](https://github.com/MvvmCross/MvvmCross/pull/2386) ([nickrandolph](https://github.com/nickrandolph))
- Update docs for registering unbound generics. [\#2383](https://github.com/MvvmCross/MvvmCross/pull/2383) ([Slowbad](https://github.com/Slowbad))
- Add ability to show a different host view for a Forms page [\#2382](https://github.com/MvvmCross/MvvmCross/pull/2382) ([martijn00](https://github.com/martijn00))
- Separation of concerns for Playground sample v2 [\#2381](https://github.com/MvvmCross/MvvmCross/pull/2381) ([nickrandolph](https://github.com/nickrandolph))
- Add support for multiple resources in resx files [\#2380](https://github.com/MvvmCross/MvvmCross/pull/2380) ([martijn00](https://github.com/martijn00))
- Improve parameter overview [\#2377](https://github.com/MvvmCross/MvvmCross/pull/2377) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Add information on how to run Jekyll locally [\#2376](https://github.com/MvvmCross/MvvmCross/pull/2376) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Update doc to net standard 2 support [\#2371](https://github.com/MvvmCross/MvvmCross/pull/2371) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Make builds faster by enabling parallel build [\#2367](https://github.com/MvvmCross/MvvmCross/pull/2367) ([Cheesebaron](https://github.com/Cheesebaron))
- Improve UWP view presenter to use presentation attributes v2 [\#2366](https://github.com/MvvmCross/MvvmCross/pull/2366) ([nickrandolph](https://github.com/nickrandolph))
- Load viewmodels via request when loading directly [\#2364](https://github.com/MvvmCross/MvvmCross/pull/2364) ([martijn00](https://github.com/martijn00))
- Run Prepare and Initialize from MvxViewModelLoader [\#2359](https://github.com/MvvmCross/MvvmCross/pull/2359) ([nmilcoff](https://github.com/nmilcoff))

## [5.4.2](https://github.com/MvvmCross/MvvmCross/tree/5.4.2) (2017-11-07)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.4.1...5.4.2)

**Fixed bugs:**

- Setting Detail in MvxMasterDetailView added to stack and not replacing Detail [\#2347](https://github.com/MvvmCross/MvvmCross/issues/2347)
- Unable to use MvxTabbedPage in MvvmCross 5.4.0 [\#2345](https://github.com/MvvmCross/MvvmCross/issues/2345)
- Default root page will not load, if the MasterDetailPage is the app startup page [\#2309](https://github.com/MvvmCross/MvvmCross/issues/2309)
- Strange behaviour of the navigation stack with MVX [\#2308](https://github.com/MvvmCross/MvvmCross/issues/2308)
- Toolbar color can not be changed [\#2301](https://github.com/MvvmCross/MvvmCross/issues/2301)
- Add support for nested root pages of any type [\#2361](https://github.com/MvvmCross/MvvmCross/pull/2361) ([martijn00](https://github.com/martijn00))

**Closed issues:**

- MvxTabLayoutPresentation not working in Fragment [\#2335](https://github.com/MvvmCross/MvvmCross/issues/2335)
- IMvxNavigationService Stack Issue in Xamarin Forms [\#2202](https://github.com/MvvmCross/MvvmCross/issues/2202)

## [5.4.1](https://github.com/MvvmCross/MvvmCross/tree/5.4.1) (2017-11-07)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.4.0...5.4.1)

**Fixed bugs:**

- Xamarin.Forms / Setting NoHistory member has no effect [\#2320](https://github.com/MvvmCross/MvvmCross/issues/2320)
- mvx:MvxBind binding structure inconsistant [\#2299](https://github.com/MvvmCross/MvvmCross/issues/2299)
- Rotation crashes device [\#2274](https://github.com/MvvmCross/MvvmCross/issues/2274)
- Null reference exception in type initializer for ConsoleLogProvider on iOS 11 device [\#2342](https://github.com/MvvmCross/MvvmCross/issues/2342)
- App fails to launch when initial page has WrapInNavigationPage = false [\#2329](https://github.com/MvvmCross/MvvmCross/issues/2329)

**Closed issues:**

- MvxLayoutInflater Disposed exception [\#1924](https://github.com/MvvmCross/MvvmCross/issues/1924)
- mvx:MvxBind not setting selected item in MvxListView [\#2355](https://github.com/MvvmCross/MvvmCross/issues/2355)
- UIDatePikcer CountdownDuration Binding [\#2352](https://github.com/MvvmCross/MvvmCross/issues/2352)

**Merged pull requests:**

- Adds CountDownDuration target binding for UIDatePicker [\#2353](https://github.com/MvvmCross/MvvmCross/pull/2353) ([sfmskywalker](https://github.com/sfmskywalker))
- Add event source for Forms views [\#2357](https://github.com/MvvmCross/MvvmCross/pull/2357) ([martijn00](https://github.com/martijn00))
- Fix bindings without forms base type [\#2356](https://github.com/MvvmCross/MvvmCross/pull/2356) ([martijn00](https://github.com/martijn00))
- Improve issue template [\#2354](https://github.com/MvvmCross/MvvmCross/pull/2354) ([willsb](https://github.com/willsb))
- Make no history work with nested stacks [\#2350](https://github.com/MvvmCross/MvvmCross/pull/2350) ([martijn00](https://github.com/martijn00))
- Remove duplicate heading [\#2349](https://github.com/MvvmCross/MvvmCross/pull/2349) ([Cheesebaron](https://github.com/Cheesebaron))
- Update ViewPresenter files and add links for docs [\#2346](https://github.com/MvvmCross/MvvmCross/pull/2346) ([nmilcoff](https://github.com/nmilcoff))
- Update view-presenters.md [\#2344](https://github.com/MvvmCross/MvvmCross/pull/2344) ([turibbio](https://github.com/turibbio))
- Add linker include to forms starterpack [\#2343](https://github.com/MvvmCross/MvvmCross/pull/2343) ([martijn00](https://github.com/martijn00))
- Cleanup of logging within Setup [\#2339](https://github.com/MvvmCross/MvvmCross/pull/2339) ([martijn00](https://github.com/martijn00))
- Fix replacing of mainpage root in forms [\#2337](https://github.com/MvvmCross/MvvmCross/pull/2337) ([martijn00](https://github.com/martijn00))
- Fix console log provider [\#2334](https://github.com/MvvmCross/MvvmCross/pull/2334) ([willsb](https://github.com/willsb))

## [5.4.0](https://github.com/MvvmCross/MvvmCross/tree/5.4.0) (2017-10-31)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.3.2...5.4.0)

**Fixed bugs:**

- Navigation will break, if a page inside a MasterDetail navigation opens a modal dialog [\#2311](https://github.com/MvvmCross/MvvmCross/issues/2311)
- Android crashes as soon as the master menu page has an icon [\#2310](https://github.com/MvvmCross/MvvmCross/issues/2310)
- The master menu stays open after navigation [\#2307](https://github.com/MvvmCross/MvvmCross/issues/2307)
- MasterDetailPage breaks my app because of the slide to open menu gesture [\#2306](https://github.com/MvvmCross/MvvmCross/issues/2306)
- Lack one constructor inside MvxDialogFragment [\#2294](https://github.com/MvvmCross/MvvmCross/issues/2294)
- Navigation between native and Forms is not correct [\#2292](https://github.com/MvvmCross/MvvmCross/issues/2292)
- iOS - Crashing on navigation [\#2289](https://github.com/MvvmCross/MvvmCross/issues/2289)
- MvxIosViewPresenter CloseModalViewController broken in 5.3 [\#2276](https://github.com/MvvmCross/MvvmCross/issues/2276)
- Bug with toolbar on android MvvmCross 5.2.1 and Forms 2.4.0.282 [\#2252](https://github.com/MvvmCross/MvvmCross/issues/2252)
- MvxSidebarPresenter not adding drawer bar button and showing drawer [\#2247](https://github.com/MvvmCross/MvvmCross/issues/2247)
- Sidebar menu doesn't get initialised for first root controller in 5.2 [\#2188](https://github.com/MvvmCross/MvvmCross/issues/2188)
- mvx:La.ng in Xamarin.Forms not working [\#2176](https://github.com/MvvmCross/MvvmCross/issues/2176)
- Null reference error in MvxFormsAppCompatActivity on GetAccentColor [\#2117](https://github.com/MvvmCross/MvvmCross/issues/2117)
- Mvxforms droid resources fix [\#2305](https://github.com/MvvmCross/MvvmCross/pull/2305) ([johnnywebb](https://github.com/johnnywebb))

**Closed issues:**

- Xamarin Sidebar doesn't opens at first launch [\#2268](https://github.com/MvvmCross/MvvmCross/issues/2268)
- Feature request: Modal dialogs / SplitView with Xamarin.Forms [\#2074](https://github.com/MvvmCross/MvvmCross/issues/2074)

**Merged pull requests:**

- Fix indentation [\#2327](https://github.com/MvvmCross/MvvmCross/pull/2327) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix the namespace in the MvvmCross.Forms.StarterPack [\#2325](https://github.com/MvvmCross/MvvmCross/pull/2325) ([flyingxu](https://github.com/flyingxu))
- Fixes for Forms presenter [\#2321](https://github.com/MvvmCross/MvvmCross/pull/2321) ([martijn00](https://github.com/martijn00))
- Move samples to playground to cleanup forms projects [\#2319](https://github.com/MvvmCross/MvvmCross/pull/2319) ([martijn00](https://github.com/martijn00))
- Droid ViewPresenter document: Add note for animations [\#2315](https://github.com/MvvmCross/MvvmCross/pull/2315) ([nmilcoff](https://github.com/nmilcoff))
- Fix Xamarin-Sidebar initialization  [\#2314](https://github.com/MvvmCross/MvvmCross/pull/2314) ([nmilcoff](https://github.com/nmilcoff))
- Add some missing binding extension properties [\#2313](https://github.com/MvvmCross/MvvmCross/pull/2313) ([martijn00](https://github.com/martijn00))
- Add design time checker to Forms bindings [\#2312](https://github.com/MvvmCross/MvvmCross/pull/2312) ([martijn00](https://github.com/martijn00))
- Improve MvvmCross Logging [\#2300](https://github.com/MvvmCross/MvvmCross/pull/2300) ([willsb](https://github.com/willsb))
- Corrected documentation [\#2298](https://github.com/MvvmCross/MvvmCross/pull/2298) ([gi097](https://github.com/gi097))
- Add Forms projects and test projects for UWP WPF and Mac [\#2296](https://github.com/MvvmCross/MvvmCross/pull/2296) ([martijn00](https://github.com/martijn00))
- Fix: When wrapping master page into navigation page, title was missing [\#2293](https://github.com/MvvmCross/MvvmCross/pull/2293) ([Grrbrr404](https://github.com/Grrbrr404))
- Add replace root to add safety check to replace it [\#2291](https://github.com/MvvmCross/MvvmCross/pull/2291) ([martijn00](https://github.com/martijn00))

## [5.3.2](https://github.com/MvvmCross/MvvmCross/tree/5.3.2) (2017-10-23)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.3.1...5.3.2)

**Fixed bugs:**

- Crash MvxTabBarViewController ViewWillDisappear [\#2267](https://github.com/MvvmCross/MvvmCross/issues/2267)
- \[Android\] Dismissing dialog via back button doesn't cancel Task [\#2245](https://github.com/MvvmCross/MvvmCross/issues/2245)

**Merged pull requests:**

- Add new extension to the list [\#2286](https://github.com/MvvmCross/MvvmCross/pull/2286) ([CherechesC](https://github.com/CherechesC))
- Add option to clear root, set icon, and animate [\#2285](https://github.com/MvvmCross/MvvmCross/pull/2285) ([martijn00](https://github.com/martijn00))
- ToList modals, as can't change list you are modifying [\#2284](https://github.com/MvvmCross/MvvmCross/pull/2284) ([adamped](https://github.com/adamped))
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

## [5.3.0](https://github.com/MvvmCross/MvvmCross/tree/5.3.0) (2017-10-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.2.1...5.3.0)

**Fixed bugs:**

- OnStop needs a null check  [\#2238](https://github.com/MvvmCross/MvvmCross/issues/2238)
- Pass MvxViewModelRequest.PresentationValues when navigating to Fragment on to it's parent's Activity when navigating [\#2237](https://github.com/MvvmCross/MvvmCross/issues/2237)

**Closed issues:**

- New nav service "deep link" conflict with viewmodel parameters when param is string [\#2080](https://github.com/MvvmCross/MvvmCross/issues/2080)
- Dialog doesn't survive screen orientation change [\#2246](https://github.com/MvvmCross/MvvmCross/issues/2246)
- 2 Factor Login required for the MvvmCross organization from 22 September [\#2195](https://github.com/MvvmCross/MvvmCross/issues/2195)
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

## [5.2.1](https://github.com/MvvmCross/MvvmCross/tree/5.2.1) (2017-09-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.2...5.2.1)

**Fixed bugs:**

- IMvxOverridePresentationAttribute not working of any Android view [\#2225](https://github.com/MvvmCross/MvvmCross/issues/2225)
- Wrong host activity gets shown [\#2222](https://github.com/MvvmCross/MvvmCross/issues/2222)
- Exception in MvxAppCompatDialogFragment\<T\> [\#2220](https://github.com/MvvmCross/MvvmCross/issues/2220)
- MvxNSSwitchOnTargetBinding appears in MvvmCross.Mac and MvvmCross.Binding.Mac [\#2205](https://github.com/MvvmCross/MvvmCross/issues/2205)
- Wrong ViewModel data in cached Fragment [\#1986](https://github.com/MvvmCross/MvvmCross/issues/1986)

**Closed issues:**

- Wrong behavior on Move in MvxCollectionViewSourceAnimated [\#2061](https://github.com/MvvmCross/MvvmCross/issues/2061)
- Can't install MvvmCross 5.2.1 in Xamarin Forms PCL Project [\#2240](https://github.com/MvvmCross/MvvmCross/issues/2240)
- Programmatically switching tabbed viewmodels from RootViewModel [\#2191](https://github.com/MvvmCross/MvvmCross/issues/2191)
- Hang when awaiting code in Initialize in 5.2 [\#2182](https://github.com/MvvmCross/MvvmCross/issues/2182)
- Support Toolbar and Unified Toolbar bindings by view for Mac [\#2180](https://github.com/MvvmCross/MvvmCross/issues/2180)
- Auto-creation of window in MvvmCross Mac now default? [\#2178](https://github.com/MvvmCross/MvvmCross/issues/2178)
- Missing StarterPack for MvvmCross.Forms [\#2073](https://github.com/MvvmCross/MvvmCross/issues/2073)

**Merged pull requests:**

- Fix ViewModel Lifecycle and Android ViewPresenter docs [\#2232](https://github.com/MvvmCross/MvvmCross/pull/2232) ([nmilcoff](https://github.com/nmilcoff))
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

## [5.2](https://github.com/MvvmCross/MvvmCross/tree/5.2) (2017-09-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.1.1...5.2)

**Fixed bugs:**

- Add initWithCoder: constructor for MvxView [\#2156](https://github.com/MvvmCross/MvvmCross/issues/2156)
- New navigation service doesn't seem to be truly async as opposite to old ShowViewModel\(\) navigation [\#2071](https://github.com/MvvmCross/MvvmCross/issues/2071)
- Remove unnecessary additions to AndroidViewAssemblies [\#2170](https://github.com/MvvmCross/MvvmCross/pull/2170) ([Cheesebaron](https://github.com/Cheesebaron))

**Closed issues:**

- MvxAppCompatSetup.cs AndroidViewAssemblies SlidingPaneLayout is not needed [\#2169](https://github.com/MvvmCross/MvvmCross/issues/2169)
- Cleanup MvvmCross simple implementation for Droid [\#2153](https://github.com/MvvmCross/MvvmCross/issues/2153)
- Display MvxAppCompatDialogFragment as a dialog [\#2152](https://github.com/MvvmCross/MvvmCross/issues/2152)
- IMvxPictureChooserTask iOS 10.3 app crash on selecting image from Gallery [\#2149](https://github.com/MvvmCross/MvvmCross/issues/2149)
- Support NavigationService by type [\#2147](https://github.com/MvvmCross/MvvmCross/issues/2147)
- Mvx 5.x navigation and TabBarViewController [\#2137](https://github.com/MvvmCross/MvvmCross/issues/2137)
- MvxAppCompatActivity doesn't work with IMvxNavigationService [\#2128](https://github.com/MvvmCross/MvvmCross/issues/2128)
- Renaming all classes [\#2123](https://github.com/MvvmCross/MvvmCross/issues/2123)
- Incorrect ViewModel Init\(\) params serialization/deserialization [\#2116](https://github.com/MvvmCross/MvvmCross/issues/2116)
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
- Tabs within fragment breaks on navigation [\#2055](https://github.com/MvvmCross/MvvmCross/issues/2055)
- Compilation issues with MvvmCross.StarterPack 5.1.0 [\#2063](https://github.com/MvvmCross/MvvmCross/issues/2063)
- new LinkerPleaseInclude.cs.pp \(ios\) does not compile [\#2054](https://github.com/MvvmCross/MvvmCross/issues/2054)
- MvxNavigationService and Linker All does not work [\#2025](https://github.com/MvvmCross/MvvmCross/issues/2025)

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

## [5.1.0](https://github.com/MvvmCross/MvvmCross/tree/5.1.0) (2017-07-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.6...5.1.0)

**Fixed bugs:**

- Stop using unsupported applicationDidFinishLaunching on iOS. [\#2033](https://github.com/MvvmCross/MvvmCross/pull/2033) ([DaRosenberg](https://github.com/DaRosenberg))
- Don't mask exceptions when completing android initialization [\#1930](https://github.com/MvvmCross/MvvmCross/pull/1930) ([edwinvanderham](https://github.com/edwinvanderham))

**Closed issues:**

- Default bindings for Xamarin Forms [\#2119](https://github.com/MvvmCross/MvvmCross/issues/2119)
- Crash when using a MvxTabBarViewController + Custom Presenter not derived from MvxIosViewPresenter [\#2112](https://github.com/MvvmCross/MvvmCross/issues/2112)
- Feedback: \(Semantic\) Versioning [\#2107](https://github.com/MvvmCross/MvvmCross/issues/2107)
- RaiseCanExecuteChanged on MvxCommand status is not checked again [\#2064](https://github.com/MvvmCross/MvvmCross/issues/2064)
- MvxNavigationService never calls IMvxViewModelLocator.Load [\#2036](https://github.com/MvvmCross/MvvmCross/issues/2036)
- mvx:Warning:  0.25 No sidemenu found. To use a sidemenu decorate the viewcontroller class with the 'MvxPanelPresentationAttribute' class and set the panel to 'Left' or 'Right'. [\#2034](https://github.com/MvvmCross/MvvmCross/issues/2034)
- Exception being thrown on fresh app start, OnCreate inaccessible? [\#2032](https://github.com/MvvmCross/MvvmCross/issues/2032)
- MvxViewModel\<TParameter\> occurs exception. [\#2028](https://github.com/MvvmCross/MvvmCross/issues/2028)
- Feature request: Lifecycle event for OnCreate and ViewDidLoad  [\#2018](https://github.com/MvvmCross/MvvmCross/issues/2018)
- Multiple instances of viewmodels being created when navigating when using MvvmCross with Forms and Master-Detail [\#1979](https://github.com/MvvmCross/MvvmCross/issues/1979)
- CanExecute does not fire when RaiseCanExecuteChanged\(\) is called. [\#1877](https://github.com/MvvmCross/MvvmCross/issues/1877)

**Merged pull requests:**

- Revert "target profile 78 like other plugins" [\#2030](https://github.com/MvvmCross/MvvmCross/pull/2030) ([Cheesebaron](https://github.com/Cheesebaron))
- target profile 78 like other plugins [\#2029](https://github.com/MvvmCross/MvvmCross/pull/2029) ([khoussem](https://github.com/khoussem))
- Calls CloseTabBarViewController\(\) only for actual MvxIosViewPresenter [\#2113](https://github.com/MvvmCross/MvvmCross/pull/2113) ([aspnetde](https://github.com/aspnetde))
- Adjust dispatchers after merging mask exceptions pr [\#2045](https://github.com/MvvmCross/MvvmCross/pull/2045) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix samples and duplicate ViewModels on Xamarin.Forms [\#2044](https://github.com/MvvmCross/MvvmCross/pull/2044) ([martijn00](https://github.com/martijn00))
- Add MvxNavigationService to LinkerPleaseInclude [\#2043](https://github.com/MvvmCross/MvvmCross/pull/2043) ([willsb](https://github.com/willsb))
- Add appstart using navigationservice [\#2042](https://github.com/MvvmCross/MvvmCross/pull/2042) ([martijn00](https://github.com/martijn00))
- Don't try to use deprecated init when using navigationservice [\#2041](https://github.com/MvvmCross/MvvmCross/pull/2041) ([martijn00](https://github.com/martijn00))
- MvxSimpleWpfViewPresenter supports MvxClosePresentationHint [\#2040](https://github.com/MvvmCross/MvvmCross/pull/2040) ([jz5](https://github.com/jz5))
- Use ViewModelLocator to load view models [\#2039](https://github.com/MvvmCross/MvvmCross/pull/2039) ([martijn00](https://github.com/martijn00))
- iOS ViewPresenter: Support for nested modals [\#2037](https://github.com/MvvmCross/MvvmCross/pull/2037) ([nmilcoff](https://github.com/nmilcoff))
- Fix Forms implementation for MvvmCross [\#2035](https://github.com/MvvmCross/MvvmCross/pull/2035) ([martijn00](https://github.com/martijn00))
- Introduced new lifecycle event "Created" in MvxViewModel [\#2020](https://github.com/MvvmCross/MvvmCross/pull/2020) ([KennethKr](https://github.com/KennethKr))

## [5.0.6](https://github.com/MvvmCross/MvvmCross/tree/5.0.6) (2017-07-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.5...5.0.6)

**Fixed bugs:**

- App crash will null reference exception after cancelling sending email [\#1978](https://github.com/MvvmCross/MvvmCross/issues/1978)
- replace create MvxNavigationController method call in MvxIosViewPrese… [\#2010](https://github.com/MvvmCross/MvvmCross/pull/2010) ([KanLei](https://github.com/KanLei))

**Closed issues:**

- MvvmCross plugin PictureChooser 5.0.5 - profile 259 [\#2017](https://github.com/MvvmCross/MvvmCross/issues/2017)
- when i try to install MvvmCross.Droid.Support.Fragment getting error [\#2016](https://github.com/MvvmCross/MvvmCross/issues/2016)
- Inline converter creation in WPF [\#2000](https://github.com/MvvmCross/MvvmCross/issues/2000)
- SetTitleAndTabBarItem not called on 5.0.5 [\#1995](https://github.com/MvvmCross/MvvmCross/issues/1995)
- Support for Resharper PropertyChangedHandler [\#1994](https://github.com/MvvmCross/MvvmCross/issues/1994)

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

- MvxNavigationService.Close is not setting the ViewModel on the NavigateEventArgs [\#1983](https://github.com/MvvmCross/MvvmCross/issues/1983)
- Order of Initialize call in Android after Navigation service call [\#1968](https://github.com/MvvmCross/MvvmCross/issues/1968)

**Merged pull requests:**

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

**Closed issues:**

- Fused Location Provider throws IllegalStateException [\#1955](https://github.com/MvvmCross/MvvmCross/issues/1955)
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
- Codefactor cleanup [\#1939](https://github.com/MvvmCross/MvvmCross/pull/1939) ([Cheesebaron](https://github.com/Cheesebaron))
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
- New MacOS ViewPresenter [\#1913](https://github.com/MvvmCross/MvvmCross/pull/1913) ([nmilcoff](https://github.com/nmilcoff))

## [5.0.2](https://github.com/MvvmCross/MvvmCross/tree/5.0.2) (2017-06-06)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.1...5.0.2)

**Fixed bugs:**

- Uwp Navigation Service doesn't work with Parameter [\#1905](https://github.com/MvvmCross/MvvmCross/issues/1905)
- android.support.v7.widget.SearchView Query binding not working [\#1882](https://github.com/MvvmCross/MvvmCross/issues/1882)
- Error MvvmCross.Uwp.rd.xml does not exist when compiling 5.0.1 [\#1879](https://github.com/MvvmCross/MvvmCross/issues/1879)
- Kevinf/1880 memory leak [\#1907](https://github.com/MvvmCross/MvvmCross/pull/1907) ([Bowman74](https://github.com/Bowman74))
- Consolidate library output and embed rd.xml [\#1901](https://github.com/MvvmCross/MvvmCross/pull/1901) ([Cheesebaron](https://github.com/Cheesebaron))

**Closed issues:**

- Synchronous view model initialization [\#1902](https://github.com/MvvmCross/MvvmCross/issues/1902)
- Appearing event called multiple times on Android platform [\#1894](https://github.com/MvvmCross/MvvmCross/issues/1894)
- Documentation: ViewModel lifecycle doesn't explain how to deal with tombstoning [\#1892](https://github.com/MvvmCross/MvvmCross/issues/1892)
- Add a way to change presentation attribute of ViewController at runtime [\#1887](https://github.com/MvvmCross/MvvmCross/issues/1887)
- Documents: Plugins README are old [\#1886](https://github.com/MvvmCross/MvvmCross/issues/1886)
- mvvmcross 5.0 Fatal signal 11 \(SIGSEGV\) [\#1881](https://github.com/MvvmCross/MvvmCross/issues/1881)
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
- Adds support for INotifyPropertyChanged to MvxWithEventPropertyInfoTargetBinding [\#1899](https://github.com/MvvmCross/MvvmCross/pull/1899) ([DaRosenberg](https://github.com/DaRosenberg))
- Adds target binding for UIPageControl on iOS [\#1898](https://github.com/MvvmCross/MvvmCross/pull/1898) ([DaRosenberg](https://github.com/DaRosenberg))
- \#1894 Fixed problem where C\# version of event handlers do not unsubsc… [\#1897](https://github.com/MvvmCross/MvvmCross/pull/1897) ([Bowman74](https://github.com/Bowman74))
- ViewModel-Lifecycle doc: Bring back info from Wiki [\#1895](https://github.com/MvvmCross/MvvmCross/pull/1895) ([nmilcoff](https://github.com/nmilcoff))
- Delete plugins readme files \(\#1886\) [\#1890](https://github.com/MvvmCross/MvvmCross/pull/1890) ([jz5](https://github.com/jz5))
- Implement option to override iOS presentation attribute [\#1888](https://github.com/MvvmCross/MvvmCross/pull/1888) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Add setvalueimpl for SearchView Query and change to TwoWay. [\#1883](https://github.com/MvvmCross/MvvmCross/pull/1883) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix opencollective height [\#1878](https://github.com/MvvmCross/MvvmCross/pull/1878) ([Garfield550](https://github.com/Garfield550))

## [5.0.1](https://github.com/MvvmCross/MvvmCross/tree/5.0.1) (2017-05-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0...5.0.1)

**Closed issues:**

- ViewModel's constructor is called twice when using NavigationService [\#2038](https://github.com/MvvmCross/MvvmCross/issues/2038)

## [5.0.0](https://github.com/MvvmCross/MvvmCross/tree/5.0.0) (2017-05-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.12...5.0.0)

**Closed issues:**

- Cannot resolve Assembly or Windows Metadata file 'MvvmCross.WindowsUWP.dll' [\#1952](https://github.com/MvvmCross/MvvmCross/issues/1952)

## [5.0.0-beta.12](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.12) (2017-05-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.11...5.0.0-beta.12)

## [5.0.0-beta.11](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.11) (2017-05-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.10...5.0.0-beta.11)

## [5.0.0-beta.10](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.10) (2017-05-15)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.9...5.0.0-beta.10)

## [5.0.0-beta.9](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.9) (2017-05-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.8...5.0.0-beta.9)

## [5.0.0-beta.8](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.8) (2017-05-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.7...5.0.0-beta.8)

## [5.0.0-beta.7](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.7) (2017-05-03)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.6...5.0.0-beta.7)

## [5.0.0-beta.6](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.6) (2017-05-01)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.5...5.0.0-beta.6)

## [5.0.0-beta.5](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.5) (2017-04-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.4...5.0.0-beta.5)

## [5.0.0-beta.4](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.4) (2017-04-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.3...5.0.0-beta.4)

## [5.0.0-beta.3](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.3) (2017-04-28)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.2...5.0.0-beta.3)

## [5.0.0-beta.2](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.2) (2017-04-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.1...5.0.0-beta.2)

## [5.0.0-beta.1](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.1) (2017-04-25)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.4.0...5.0.0-beta.1)

## [4.4.0](https://github.com/MvvmCross/MvvmCross/tree/4.4.0) (2016-11-01)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.3.0...4.4.0)

## [4.3.0](https://github.com/MvvmCross/MvvmCross/tree/4.3.0) (2016-09-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.2.3...4.3.0)

## [4.2.3](https://github.com/MvvmCross/MvvmCross/tree/4.2.3) (2016-08-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.2.2...4.2.3)

## [4.2.2](https://github.com/MvvmCross/MvvmCross/tree/4.2.2) (2016-07-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.2.1...4.2.2)

## [4.2.1](https://github.com/MvvmCross/MvvmCross/tree/4.2.1) (2016-07-05)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.2.0...4.2.1)

## [4.2.0](https://github.com/MvvmCross/MvvmCross/tree/4.2.0) (2016-06-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.1.6...4.2.0)

## [4.1.6](https://github.com/MvvmCross/MvvmCross/tree/4.1.6) (2016-05-24)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.1.5...4.1.6)

## [4.1.5](https://github.com/MvvmCross/MvvmCross/tree/4.1.5) (2016-05-19)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.1.4...4.1.5)

## [4.1.4](https://github.com/MvvmCross/MvvmCross/tree/4.1.4) (2016-04-20)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/Binding_4.1.1...4.1.4)

## [Binding_4.1.1](https://github.com/MvvmCross/MvvmCross/tree/Binding_4.1.1) (2016-04-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/DroidShared_4.1.2...Binding_4.1.1)

## [DroidShared_4.1.2](https://github.com/MvvmCross/MvvmCross/tree/DroidShared_4.1.2) (2016-04-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/FullFragging_4.1.1...DroidShared_4.1.2)

## [FullFragging_4.1.1](https://github.com/MvvmCross/MvvmCross/tree/FullFragging_4.1.1) (2016-04-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.1.0...FullFragging_4.1.1)

## [4.1.0](https://github.com/MvvmCross/MvvmCross/tree/4.1.0) (2016-03-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0...4.1.0)

## [4.0.0](https://github.com/MvvmCross/MvvmCross/tree/4.0.0) (2016-02-02)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta8...4.0.0)

## [4.0.0-beta8](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta8) (2016-01-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta7...4.0.0-beta8)

## [4.0.0-beta7](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta7) (2015-12-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta5...4.0.0-beta7)

## [4.0.0-beta5](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta5) (2015-11-06)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta4...4.0.0-beta5)

## [4.0.0-beta4](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta4) (2015-10-20)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta3...4.0.0-beta4)

## [4.0.0-beta3](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta3) (2015-09-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta2...4.0.0-beta3)

## [4.0.0-beta2](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta2) (2015-08-18)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta1...4.0.0-beta2)

## [4.0.0-beta1](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta1) (2015-07-31)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-alpha9...4.0.0-beta1)

## [4.0.0-alpha9](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-alpha9) (2015-07-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-alpha8...4.0.0-alpha9)

## [4.0.0-alpha8](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-alpha8) (2015-07-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-alpha4...4.0.0-alpha8)

## [4.0.0-alpha4](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-alpha4) (2015-07-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-alpha3...4.0.0-alpha4)

## [4.0.0-alpha3](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-alpha3) (2015-07-15)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-alpha2...4.0.0-alpha3)

## [4.0.0-alpha2](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-alpha2) (2015-07-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-alpha1...4.0.0-alpha2)

## [4.0.0-alpha1](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-alpha1) (2015-07-09)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.2-alpha2...4.0.0-alpha1)

## [3.5.2-alpha2](https://github.com/MvvmCross/MvvmCross/tree/3.5.2-alpha2) (2015-06-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.2-alpha1...3.5.2-alpha2)

## [3.5.2-alpha1](https://github.com/MvvmCross/MvvmCross/tree/3.5.2-alpha1) (2015-06-16)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.1...3.5.2-alpha1)

## [3.5.1](https://github.com/MvvmCross/MvvmCross/tree/3.5.1) (2015-05-02)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.51-beta1...3.5.1)

## [3.51-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.51-beta1) (2015-03-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.1-alpha1...3.51-beta1)

## [3.5.1-alpha1](https://github.com/MvvmCross/MvvmCross/tree/3.5.1-alpha1) (2015-02-08)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.0...3.5.1-alpha1)

## [3.5.0](https://github.com/MvvmCross/MvvmCross/tree/3.5.0) (2015-01-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.0-beta2...3.5.0)

## [3.5.0-beta2](https://github.com/MvvmCross/MvvmCross/tree/3.5.0-beta2) (2014-12-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.0-beta1...3.5.0-beta2)

## [3.5.0-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.5.0-beta1) (2014-12-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.0-alpha2...3.5.0-beta1)

## [3.5.0-alpha2](https://github.com/MvvmCross/MvvmCross/tree/3.5.0-alpha2) (2014-11-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.2.2...3.5.0-alpha2)

## [3.2.2](https://github.com/MvvmCross/MvvmCross/tree/3.2.2) (2014-11-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/v3_5_alpha0...3.2.2)

## [v3_5_alpha0](https://github.com/MvvmCross/MvvmCross/tree/v3_5_alpha0) (2014-11-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.2.2-beta1...v3_5_alpha0)

## [3.2.2-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.2.2-beta1) (2014-10-21)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.2.1-beta3...3.2.2-beta1)

## [3.2.1-beta3](https://github.com/MvvmCross/MvvmCross/tree/3.2.1-beta3) (2014-08-25)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.2.1-beta2...3.2.1-beta3)

## [3.2.1-beta2](https://github.com/MvvmCross/MvvmCross/tree/3.2.1-beta2) (2014-07-27)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.2.1-beta1...3.2.1-beta2)

## [3.2.1-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.2.1-beta1) (2014-06-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.177-beta5...3.2.1-beta1)

## [build-3.1.177-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.177-beta5) (2014-04-09)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.2-beta1...build-3.1.177-beta5)

## [3.1.2-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.1.2-beta1) (2014-03-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.176-beta5...3.1.2-beta1)

## [build-3.1.176-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.176-beta5) (2014-03-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.175-beta5...build-3.1.176-beta5)

## [build-3.1.175-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.175-beta5) (2014-03-05)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.174-beta5...build-3.1.175-beta5)

## [build-3.1.174-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.174-beta5) (2014-02-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.173-beta5...build-3.1.174-beta5)

## [build-3.1.173-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.173-beta5) (2014-02-20)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.171-beta5...build-3.1.173-beta5)

## [build-3.1.171-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.171-beta5) (2014-02-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.170-beta5...build-3.1.171-beta5)

## [build-3.1.170-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.170-beta5) (2014-02-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.169-beta5...build-3.1.170-beta5)

## [build-3.1.169-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.169-beta5) (2014-02-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.168-beta5...build-3.1.169-beta5)

## [build-3.1.168-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.168-beta5) (2014-02-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1...build-3.1.168-beta5)

## [3.1.1](https://github.com/MvvmCross/MvvmCross/tree/3.1.1) (2014-02-09)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.167-beta5...3.1.1)

## [build-3.1.167-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.167-beta5) (2014-02-06)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.166-beta5...build-3.1.167-beta5)

## [build-3.1.166-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.166-beta5) (2014-02-06)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.163-beta5...build-3.1.166-beta5)

## [build-3.1.163-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.163-beta5) (2014-02-06)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.160-beta5...build-3.1.163-beta5)

## [build-3.1.160-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.160-beta5) (2014-02-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.159-beta5...build-3.1.160-beta5)

## [build-3.1.159-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.159-beta5) (2014-02-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.158-beta5...build-3.1.159-beta5)

## [build-3.1.158-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.158-beta5) (2014-02-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.157-beta5...build-3.1.158-beta5)

## [build-3.1.157-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.157-beta5) (2014-02-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.156-beta5...build-3.1.157-beta5)

## [build-3.1.156-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.156-beta5) (2014-02-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.155-beta5...build-3.1.156-beta5)

## [build-3.1.155-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.155-beta5) (2014-02-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.154-beta5...build-3.1.155-beta5)

## [build-3.1.154-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.154-beta5) (2014-02-03)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.151-beta1...build-3.1.154-beta5)

## [build-3.1.151-beta1](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.151-beta1) (2014-02-03)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.150-beta1...build-3.1.151-beta1)

## [build-3.1.150-beta1](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.150-beta1) (2014-02-03)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.149-beta1...build-3.1.150-beta1)

## [build-3.1.149-beta1](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.149-beta1) (2014-02-03)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1-beta5-attempt2...build-3.1.149-beta1)

## [3.1.1-beta5-attempt2](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta5-attempt2) (2014-02-01)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1-beta5...3.1.1-beta5-attempt2)

## [3.1.1-beta5](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta5) (2014-02-01)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1-beta4...3.1.1-beta5)

## [3.1.1-beta4](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta4) (2014-01-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1-beta3...3.1.1-beta4)

## [3.1.1-beta3](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta3) (2014-01-16)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.148-beta1...3.1.1-beta3)

## [build-3.1.148-beta1](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.148-beta1) (2014-01-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.132-beta1...build-3.1.148-beta1)

## [build-3.1.132-beta1](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.132-beta1) (2014-01-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.133-beta1...build-3.1.132-beta1)

## [build-3.1.133-beta1](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.133-beta1) (2014-01-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1-beta2...build-3.1.133-beta1)

## [3.1.1-beta2](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta2) (2014-01-03)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1-beta1...3.1.1-beta2)

## [3.1.1-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta1) (2013-12-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.131-beta1...3.1.1-beta1)

## [build-3.1.131-beta1](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.131-beta1) (2013-12-07)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.14...build-3.1.131-beta1)

## [3.0.14](https://github.com/MvvmCross/MvvmCross/tree/3.0.14) (2013-11-16)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.14-beta3...3.0.14)

## [3.0.14-beta3](https://github.com/MvvmCross/MvvmCross/tree/3.0.14-beta3) (2013-11-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.14-beta2-real...3.0.14-beta3)

## [3.0.14-beta2-real](https://github.com/MvvmCross/MvvmCross/tree/3.0.14-beta2-real) (2013-11-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.14-beta2...3.0.14-beta2-real)

## [3.0.14-beta2](https://github.com/MvvmCross/MvvmCross/tree/3.0.14-beta2) (2013-11-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.14-beta1...3.0.14-beta2)

## [3.0.14-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.0.14-beta1) (2013-11-02)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.13...3.0.14-beta1)

## [3.0.13](https://github.com/MvvmCross/MvvmCross/tree/3.0.13) (2013-10-08)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.13-beta4...3.0.13)

## [3.0.13-beta4](https://github.com/MvvmCross/MvvmCross/tree/3.0.13-beta4) (2013-10-05)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.13-beta3...3.0.13-beta4)

## [3.0.13-beta3](https://github.com/MvvmCross/MvvmCross/tree/3.0.13-beta3) (2013-10-05)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.12...3.0.13-beta3)

## [3.0.12](https://github.com/MvvmCross/MvvmCross/tree/3.0.12) (2013-09-08)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.11-final...3.0.12)

## [3.0.11-final](https://github.com/MvvmCross/MvvmCross/tree/3.0.11-final) (2013-09-08)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.11-beta1...3.0.11-final)

## [3.0.11-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.0.11-beta1) (2013-09-01)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.11...3.0.11-beta1)

## [3.0.11](https://github.com/MvvmCross/MvvmCross/tree/3.0.11) (2013-08-27)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.10...3.0.11)

## [3.0.10](https://github.com/MvvmCross/MvvmCross/tree/3.0.10) (2013-07-23)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.9...3.0.10)

## [3.0.9](https://github.com/MvvmCross/MvvmCross/tree/3.0.9) (2013-07-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/Release-3.0.8.1...3.0.9)

## [Release-3.0.8.1](https://github.com/MvvmCross/MvvmCross/tree/Release-3.0.8.1) (2013-06-09)


\* *This Change Log was automatically generated by [github_changelog_generator](https://github.com/skywinder/Github-Changelog-Generator)*