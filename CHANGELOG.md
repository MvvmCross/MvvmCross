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

## [5.3.0](https://github.com/MvvmCross/MvvmCross/tree/5.3.0) (2017-10-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.2.1...5.3.0)

**Fixed bugs:**

- Pass MvxViewModelRequest.PresentationValues when navigating to Fragment on to it's parent's Activity when navigating [\#2237](https://github.com/MvvmCross/MvvmCross/issues/2237)
- Wrong host activity gets shown [\#2222](https://github.com/MvvmCross/MvvmCross/issues/2222)
- Close\(this\) is not working for Theme.Dialog on configuration change [\#1411](https://github.com/MvvmCross/MvvmCross/issues/1411)
- MvxWindowsViewPresenter swallowing exceptions thrown in Init [\#1309](https://github.com/MvvmCross/MvvmCross/issues/1309)

**Closed issues:**

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

- Initialize is not called [\#2212](https://github.com/MvvmCross/MvvmCross/issues/2212)
- MvxNSSwitchOnTargetBinding appears in MvvmCross.Mac and MvvmCross.Binding.Mac [\#2205](https://github.com/MvvmCross/MvvmCross/issues/2205)
- WindowController from Storyboard \(Mac\) gets disposed [\#2198](https://github.com/MvvmCross/MvvmCross/issues/2198)
- Programmatically switching tabbed viewmodels from RootViewModel - Mac [\#2191](https://github.com/MvvmCross/MvvmCross/issues/2191)
- Where is that IFactory\<T\>? [\#2186](https://github.com/MvvmCross/MvvmCross/issues/2186)
- Support Toolbar and Unified Toolbar bindings by view for Mac [\#2180](https://github.com/MvvmCross/MvvmCross/issues/2180)
- Wrong behavior on Move in MvxCollectionViewSourceAnimated [\#2061](https://github.com/MvvmCross/MvvmCross/issues/2061)
- Exception in MvxAppCompatDialogFragment\<T\> [\#2220](https://github.com/MvvmCross/MvvmCross/issues/2220)
- Hang when awaiting code in Initialize in 5.2 [\#2182](https://github.com/MvvmCross/MvvmCross/issues/2182)
- Missing StarterPack for MvvmCross.Forms [\#2073](https://github.com/MvvmCross/MvvmCross/issues/2073)
- Fragment viewmodel life-cycle events are not called if viewmodel has saved state and calling ShowViewModel\<FragmentViewModel\>\(data\) with data [\#1373](https://github.com/MvvmCross/MvvmCross/issues/1373)

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

## [5.1.0](https://github.com/MvvmCross/MvvmCross/tree/5.1.0) (2017-07-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.6...5.1.0)

**Fixed bugs:**

- Stop using unsupported applicationDidFinishLaunching on iOS. [\#2033](https://github.com/MvvmCross/MvvmCross/pull/2033) ([DaRosenberg](https://github.com/DaRosenberg))
- Don't mask exceptions when completing android initialization [\#1930](https://github.com/MvvmCross/MvvmCross/pull/1930) ([edwinvanderham](https://github.com/edwinvanderham))

**Closed issues:**

- mvx:Warning:  0.25 No sidemenu found. To use a sidemenu decorate the viewcontroller class with the 'MvxPanelPresentationAttribute' class and set the panel to 'Left' or 'Right'. [\#2034](https://github.com/MvvmCross/MvvmCross/issues/2034)
- Exception being thrown on fresh app start, OnCreate inaccessible? [\#2032](https://github.com/MvvmCross/MvvmCross/issues/2032)
- Consider more binding extensions... possibly Automated Form Validation? [\#133](https://github.com/MvvmCross/MvvmCross/issues/133)
- MvxNavigationService never calls IMvxViewModelLocator.Load [\#2036](https://github.com/MvvmCross/MvvmCross/issues/2036)
- MvxViewModel\<TParameter\> occurs exception. [\#2028](https://github.com/MvvmCross/MvvmCross/issues/2028)
- Feature request: Lifecycle event for OnCreate and ViewDidLoad  [\#2018](https://github.com/MvvmCross/MvvmCross/issues/2018)
- Multiple instances of viewmodels being created when navigating when using MvvmCross with Forms and Master-Detail [\#1979](https://github.com/MvvmCross/MvvmCross/issues/1979)
- Update MacOS presenter to the new iOS presenter [\#1724](https://github.com/MvvmCross/MvvmCross/issues/1724)
- Improve MvvmCross.StarterPack [\#1659](https://github.com/MvvmCross/MvvmCross/issues/1659)

**Merged pull requests:**

- Revert "target profile 78 like other plugins" [\#2030](https://github.com/MvvmCross/MvvmCross/pull/2030) ([Cheesebaron](https://github.com/Cheesebaron))
- target profile 78 like other plugins [\#2029](https://github.com/MvvmCross/MvvmCross/pull/2029) ([khoussem](https://github.com/khoussem))
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

- replace create MvxNavigationController method call in MvxIosViewPrese… [\#2010](https://github.com/MvvmCross/MvvmCross/pull/2010) ([KanLei](https://github.com/KanLei))

**Closed issues:**

- MvvmCross plugin PictureChooser 5.0.5 - profile 259 [\#2017](https://github.com/MvvmCross/MvvmCross/issues/2017)
- when i try to install MvvmCross.Droid.Support.Fragment getting error [\#2016](https://github.com/MvvmCross/MvvmCross/issues/2016)
- Suggestion for a Mac presenter for MvvmCross [\#1278](https://github.com/MvvmCross/MvvmCross/issues/1278)
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

- Enable tests on CI builds [\#1751](https://github.com/MvvmCross/MvvmCross/issues/1751)
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
- JsonLocalization plugin can't load resources with dash \( - \) in path [\#1645](https://github.com/MvvmCross/MvvmCross/issues/1645)

**Closed issues:**

- mvvmcross 5.0 Fatal signal 11 \(SIGSEGV\) [\#1881](https://github.com/MvvmCross/MvvmCross/issues/1881)
- CanExecute does not fire when RaiseCanExecuteChanged\(\) is called. [\#1877](https://github.com/MvvmCross/MvvmCross/issues/1877)
- Website issues/features [\#1727](https://github.com/MvvmCross/MvvmCross/issues/1727)
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
- Problem with Visibility plugin [\#1876](https://github.com/MvvmCross/MvvmCross/issues/1876)
- Kevinf/1880 memory leak [\#1907](https://github.com/MvvmCross/MvvmCross/pull/1907) ([Bowman74](https://github.com/Bowman74))
- Consolidate library output and embed rd.xml [\#1901](https://github.com/MvvmCross/MvvmCross/pull/1901) ([Cheesebaron](https://github.com/Cheesebaron))

**Closed issues:**

- Documents: Plugins README are old [\#1886](https://github.com/MvvmCross/MvvmCross/issues/1886)
- Back button navigation with navigation drawer in android not directly go back [\#1875](https://github.com/MvvmCross/MvvmCross/issues/1875)
- Rework 'tagged' logic in build script [\#1874](https://github.com/MvvmCross/MvvmCross/issues/1874)
- Synchronous view model initialization [\#1902](https://github.com/MvvmCross/MvvmCross/issues/1902)
- Appearing event called multiple times on Android platform [\#1894](https://github.com/MvvmCross/MvvmCross/issues/1894)
- Documentation: ViewModel lifecycle doesn't explain how to deal with tombstoning [\#1892](https://github.com/MvvmCross/MvvmCross/issues/1892)
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
- Adds support for INotifyPropertyChanged to MvxWithEventPropertyInfoTargetBinding [\#1899](https://github.com/MvvmCross/MvvmCross/pull/1899) ([DaRosenberg](https://github.com/DaRosenberg))
- Adds target binding for UIPageControl on iOS [\#1898](https://github.com/MvvmCross/MvvmCross/pull/1898) ([DaRosenberg](https://github.com/DaRosenberg))
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
- Create Pull Request template [\#1848](https://github.com/MvvmCross/MvvmCross/issues/1848)
- Incorrect width with wrap\_content [\#1546](https://github.com/MvvmCross/MvvmCross/issues/1546)
- File Plugin: GetSize\(\), GetListWriteTimeUtc\(\) [\#1155](https://github.com/MvvmCross/MvvmCross/issues/1155)
- Add abstraction for path in File plug-in [\#393](https://github.com/MvvmCross/MvvmCross/issues/393)
- Add LazyInitialize to Mvx static class [\#321](https://github.com/MvvmCross/MvvmCross/issues/321)
- ViewModel's constructor is called twice when using NavigationService [\#2038](https://github.com/MvvmCross/MvvmCross/issues/2038)
- `Start\(\)` not called in ViewModel in iOS [\#1862](https://github.com/MvvmCross/MvvmCross/issues/1862)
- Presentation bundle parameter in new navigation service [\#1860](https://github.com/MvvmCross/MvvmCross/issues/1860)
- MvxAppCompatActivity does not call view model's lifecycle methods [\#1857](https://github.com/MvvmCross/MvvmCross/issues/1857)
- iOS Missing method CreateNavigationController inMvxIosViewPresenter [\#1856](https://github.com/MvvmCross/MvvmCross/issues/1856)
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

## [5.0.0](https://github.com/MvvmCross/MvvmCross/tree/5.0.0) (2017-05-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.12...5.0.0)

**Fixed bugs:**

- MvxTableViewController does not call MvxViewModel.Appearing\(\) et. al [\#1813](https://github.com/MvvmCross/MvvmCross/issues/1813)
- MvxTabBarViewController.VisibleUIViewController throws for tabs without a navigation controller [\#1812](https://github.com/MvvmCross/MvvmCross/issues/1812)
- \[5.0.0-beta0001\] Some nuget packages are missing dll's [\#1725](https://github.com/MvvmCross/MvvmCross/issues/1725)
- FusedLocationHandler crashing with MvxException: SERVICE\_MISSING [\#1669](https://github.com/MvvmCross/MvvmCross/issues/1669)
- Android : Exceptions in MvxValueConverter\<TFrom, TTo\>.Convert are hidden [\#1655](https://github.com/MvvmCross/MvvmCross/issues/1655)
- Exception Thrown in view model async Init method not propagated [\#1569](https://github.com/MvvmCross/MvvmCross/issues/1569)
- Error XA4204: Unable to resolve interface type 'Android.Gms.Common.Apis.GoogleApiClient/IConnectionCallbacks'. [\#1558](https://github.com/MvvmCross/MvvmCross/issues/1558)
- iOS Autocorrect UITextView not captured [\#1555](https://github.com/MvvmCross/MvvmCross/issues/1555)
- Cannot/how to set LayoutManager for children RecyclerViews bound with local:MvxItemTemplate - XML local:layoutManager does not work [\#1512](https://github.com/MvvmCross/MvvmCross/issues/1512)
- RadioButton's width doesn't use match\_parent [\#1451](https://github.com/MvvmCross/MvvmCross/issues/1451)
- MvxAutoCompleteTextView/MvxFilteringAdapter locks when PartialText is the same value [\#1162](https://github.com/MvvmCross/MvvmCross/issues/1162)
- Sort out mess of IMvxTouchPlatformProperties [\#315](https://github.com/MvvmCross/MvvmCross/issues/315)
- Can an IMvxVisibleViewModel interface/pattern be added [\#74](https://github.com/MvvmCross/MvvmCross/issues/74)
- \[WIP\] Issue 1465 - Switch to ViewHolder pattern instead of wrapping in FrameLayout for MvxListView items [\#1535](https://github.com/MvvmCross/MvvmCross/pull/1535) ([orzech85](https://github.com/orzech85))

**Closed issues:**

- Cannot resolve Assembly or Windows Metadata file 'MvvmCross.WindowsUWP.dll' [\#1952](https://github.com/MvvmCross/MvvmCross/issues/1952)
- Close a view with no Presentation Attribute that was opened from a Tab [\#1834](https://github.com/MvvmCross/MvvmCross/issues/1834)
- MvxViewModel completes its TCS in the wrong sequence [\#1821](https://github.com/MvvmCross/MvvmCross/issues/1821)
- MvxTabBarViewController forces all navigation bars to be opaque [\#1819](https://github.com/MvvmCross/MvvmCross/issues/1819)
- View model lifecycle events \(Appearing/Appeared/Disappearing/Disappeared\) are not called in Xamarin Forms on Android [\#1806](https://github.com/MvvmCross/MvvmCross/issues/1806)
- Make some methods in MvxWindowsMultiRegionViewPresenter protected [\#1779](https://github.com/MvvmCross/MvvmCross/issues/1779)
- PictureChooser.iOS scales incorrectly on high DPI devices [\#1759](https://github.com/MvvmCross/MvvmCross/issues/1759)
- Kill `MvxAllThreadDispatchingObject` [\#1750](https://github.com/MvvmCross/MvvmCross/issues/1750)
-  MvxHorizontalGridView is broken [\#1743](https://github.com/MvvmCross/MvvmCross/issues/1743)
- Custom MvxRecyclerViewAdapters don't work anymore [\#1730](https://github.com/MvvmCross/MvvmCross/issues/1730)
- Build broken after merge of \#1693 [\#1720](https://github.com/MvvmCross/MvvmCross/issues/1720)
- Remove JASidePanels nuspec [\#1708](https://github.com/MvvmCross/MvvmCross/issues/1708)
- Remove Bookmarks plugin [\#1701](https://github.com/MvvmCross/MvvmCross/issues/1701)
- Remove SoundEffects [\#1700](https://github.com/MvvmCross/MvvmCross/issues/1700)
- Master Detail Example Project Doesn't Compile with Latest MvvmCross Nuget Packages [\#1699](https://github.com/MvvmCross/MvvmCross/issues/1699)
- Links in docs menu point at mvvmcross-docs repo [\#1688](https://github.com/MvvmCross/MvvmCross/issues/1688)
- Remove JASidePanels [\#1682](https://github.com/MvvmCross/MvvmCross/issues/1682)
- Master detail - working xamarin forms example [\#1681](https://github.com/MvvmCross/MvvmCross/issues/1681)
- Sample Forms projects on older versions of MvvmCross and Xamarin.Forms [\#1679](https://github.com/MvvmCross/MvvmCross/issues/1679)
- MvvmCross.Plugins.File MvxFileStore read using full path [\#1673](https://github.com/MvvmCross/MvvmCross/issues/1673)
- Consolidate NuGet packages [\#1672](https://github.com/MvvmCross/MvvmCross/issues/1672)
- Cleanup: Obsolete MvxFormFactorSpecificAttribute [\#1670](https://github.com/MvvmCross/MvvmCross/issues/1670)
- MvxAppCompatSpinner's Adapter's SimpleViewLayoutId should probably be Android.Resource.Layout.SimpleSpinnerItem [\#1666](https://github.com/MvvmCross/MvvmCross/issues/1666)
- RFE: remove plugin 'ThreadUtils' for 5.0 [\#1665](https://github.com/MvvmCross/MvvmCross/issues/1665)
- RFE: remove plugin 'ReflectionEx' for 5.0 [\#1664](https://github.com/MvvmCross/MvvmCross/issues/1664)
- TextView Click binding fails in Release [\#1651](https://github.com/MvvmCross/MvvmCross/issues/1651)
- MvxRadioGroup binding fails in Release after updating to 4.4.0 [\#1650](https://github.com/MvvmCross/MvvmCross/issues/1650)
- New RecyclerView Grouping crash on Clear/Insert sequence [\#1640](https://github.com/MvvmCross/MvvmCross/issues/1640)
- RecyclerView Header/Footer crash on tap when using custom Header/Footer DataContext [\#1637](https://github.com/MvvmCross/MvvmCross/issues/1637)
- Remove Windows 8.1/WP8.1 examples [\#1620](https://github.com/MvvmCross/MvvmCross/issues/1620)
- MvxUIDatePickerDateTargetBinding should not set default date to DateTime.Now if MaximumDate \< Now [\#1618](https://github.com/MvvmCross/MvvmCross/issues/1618)
- Using viewmodel navigation with complex parameter fails silently [\#1615](https://github.com/MvvmCross/MvvmCross/issues/1615)
- Add id parameter to MvxTabPresentation [\#1614](https://github.com/MvvmCross/MvvmCross/issues/1614)
- Remove ReflectionEx Plugin [\#1606](https://github.com/MvvmCross/MvvmCross/issues/1606)
- Improving MvxTargetBindingFactoryRegistry [\#1594](https://github.com/MvvmCross/MvvmCross/issues/1594)
- Virtualize MvxExpandableListAdapter methods [\#1582](https://github.com/MvvmCross/MvvmCross/issues/1582)
- TestProjects\Forms\Example001XAML\Example001XAML.sln does not compile [\#1580](https://github.com/MvvmCross/MvvmCross/issues/1580)
- JASidePanels Slide Menu iOS Native Library Crash only on device [\#1577](https://github.com/MvvmCross/MvvmCross/issues/1577)
- Find alternative to readme.io [\#1575](https://github.com/MvvmCross/MvvmCross/issues/1575)
- Android projects have specific Compile version set [\#1573](https://github.com/MvvmCross/MvvmCross/issues/1573)
- \[AndroidSupport\] FAB AutoHide behavior - same support for new BottomBar [\#1563](https://github.com/MvvmCross/MvvmCross/issues/1563)
- \[Proposal\] Strongly typed MvvmCross code based binding properties [\#1557](https://github.com/MvvmCross/MvvmCross/issues/1557)
- Create new event hooks for lifecycle events [\#1531](https://github.com/MvvmCross/MvvmCross/issues/1531)
- OnDataContextChanged should only be called when the data context actually changes [\#1522](https://github.com/MvvmCross/MvvmCross/issues/1522)
- NullReferenceException in MvxTaskBasedBindingContext in Release mode [\#1508](https://github.com/MvvmCross/MvvmCross/issues/1508)
- Fix references in sub projects [\#1506](https://github.com/MvvmCross/MvvmCross/issues/1506)
- Update Android OnAttach [\#1503](https://github.com/MvvmCross/MvvmCross/issues/1503)
- Move back sub repos into main [\#1500](https://github.com/MvvmCross/MvvmCross/issues/1500)
- MvxObservableCollection RemoveRange Exception [\#1485](https://github.com/MvvmCross/MvvmCross/issues/1485)
- Binding issue when doing full linking on Android for TextView on latest 4.4.0 [\#1482](https://github.com/MvvmCross/MvvmCross/issues/1482)
- StarterPack nuspec dependency doesn't get updated [\#1479](https://github.com/MvvmCross/MvvmCross/issues/1479)
- rendering issue with dynamic sized UILabels using Constraints [\#1468](https://github.com/MvvmCross/MvvmCross/issues/1468)
- Switch to ViewHolder pattern instead of wrapping in FrameLayout for MvxListView items [\#1465](https://github.com/MvvmCross/MvvmCross/issues/1465)
- Figure out what to do with MvvmCross.Binding.Combiners.VeryExperimental namespace [\#1443](https://github.com/MvvmCross/MvvmCross/issues/1443)
- Binding.Mac does not build on Windows [\#1437](https://github.com/MvvmCross/MvvmCross/issues/1437)
- MvvmCross roadmap 5.x [\#1415](https://github.com/MvvmCross/MvvmCross/issues/1415)
- Add build test of UWP project [\#1392](https://github.com/MvvmCross/MvvmCross/issues/1392)
- MvvmCross.Tests nuget can't be added to a PCL project [\#1375](https://github.com/MvvmCross/MvvmCross/issues/1375)
- MyGet Nuget feed [\#1369](https://github.com/MvvmCross/MvvmCross/issues/1369)
- Support URL based navigation [\#1315](https://github.com/MvvmCross/MvvmCross/issues/1315)
- Generate PDBs for source linking with GitLink [\#1314](https://github.com/MvvmCross/MvvmCross/issues/1314)
- continuous integration and pre-release builds [\#1301](https://github.com/MvvmCross/MvvmCross/issues/1301)
- MvxModalNavSupportIosViewPresenter does not use CurrentTopViewController [\#1274](https://github.com/MvvmCross/MvvmCross/issues/1274)
- Add support for the new Xamarin.tvOS target platform [\#1153](https://github.com/MvvmCross/MvvmCross/issues/1153)
- Enable ShowViewModel to take a view model instance [\#1141](https://github.com/MvvmCross/MvvmCross/issues/1141)
- Update "ViewModel to ViewModel Navigation" article alternatives to pages [\#1140](https://github.com/MvvmCross/MvvmCross/issues/1140)
- Throwing exception in Init\(\) method isn't reported [\#1006](https://github.com/MvvmCross/MvvmCross/issues/1006)
- Reason for multiple initialisation should not be called simultaneously? [\#955](https://github.com/MvvmCross/MvvmCross/issues/955)
- Can a general Close/Back event be added to MvxViewModel? [\#609](https://github.com/MvvmCross/MvvmCross/issues/609)
- Network plugin rest client could support aborting the request [\#569](https://github.com/MvvmCross/MvvmCross/issues/569)
- MvxGridView: can not set gravity of child items because of proxied FrameLayout [\#539](https://github.com/MvvmCross/MvvmCross/issues/539)
- Unify MvxCollectionViewCell and MvxTableViewCell constructors [\#367](https://github.com/MvvmCross/MvvmCross/issues/367)
- In PhoneCall Plugin, would be nice if phone detected in API [\#95](https://github.com/MvvmCross/MvvmCross/issues/95)

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
- Improved sequencing of MvxViewModel.Close\(\). Fixes \#1821. [\#1822](https://github.com/MvvmCross/MvvmCross/pull/1822) ([DaRosenberg](https://github.com/DaRosenberg))
- Removed forced navigation bar opacity. Fixes \#1819. [\#1820](https://github.com/MvvmCross/MvvmCross/pull/1820) ([DaRosenberg](https://github.com/DaRosenberg))
- Added missing calls to view model lifecycle methods. Fixes \#1813. [\#1816](https://github.com/MvvmCross/MvvmCross/pull/1816) ([DaRosenberg](https://github.com/DaRosenberg))
- Prettify README [\#1815](https://github.com/MvvmCross/MvvmCross/pull/1815) ([Cheesebaron](https://github.com/Cheesebaron))
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
- add "Get Started" button hover effect [\#1800](https://github.com/MvvmCross/MvvmCross/pull/1800) ([Garfield550](https://github.com/Garfield550))
- Update release notes around extension method based binding [\#1799](https://github.com/MvvmCross/MvvmCross/pull/1799) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Fix some issues found in extension method based binding [\#1798](https://github.com/MvvmCross/MvvmCross/pull/1798) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Fix warnings and errors in StarterPack content [\#1797](https://github.com/MvvmCross/MvvmCross/pull/1797) ([willsb](https://github.com/willsb))
- Update release blog [\#1796](https://github.com/MvvmCross/MvvmCross/pull/1796) ([martijn00](https://github.com/martijn00))
- Update 2017-05-01-ReleaseMvvmCross5.md [\#1795](https://github.com/MvvmCross/MvvmCross/pull/1795) ([spockfish](https://github.com/spockfish))
- Fix for MvxRecyclerAdapter indexes [\#1794](https://github.com/MvvmCross/MvvmCross/pull/1794) ([fela98](https://github.com/fela98))
- Update 2017-05-01-ReleaseMvvmCross5.md [\#1793](https://github.com/MvvmCross/MvvmCross/pull/1793) ([spockfish](https://github.com/spockfish))
- Install System.Windows.Interactivity from nuget instead of requiring … [\#1792](https://github.com/MvvmCross/MvvmCross/pull/1792) ([kjeremy](https://github.com/kjeremy))
- Null check viewmodels before visual events [\#1791](https://github.com/MvvmCross/MvvmCross/pull/1791) ([kjeremy](https://github.com/kjeremy))
- Update 2017-05-01-ReleaseMvvmCross5.md [\#1790](https://github.com/MvvmCross/MvvmCross/pull/1790) ([spockfish](https://github.com/spockfish))
- Fix Android crash when app restart [\#1789](https://github.com/MvvmCross/MvvmCross/pull/1789) ([thongdoan](https://github.com/thongdoan))
- Added overridable methods to MvxAdapter for index modification. [\#1786](https://github.com/MvvmCross/MvvmCross/pull/1786) ([fela98](https://github.com/fela98))
- Navigation improvements [\#1784](https://github.com/MvvmCross/MvvmCross/pull/1784) ([martijn00](https://github.com/martijn00))
- fixed mis-capitalized 'Mvvmcross' root namespace in MvxImageView and … [\#1783](https://github.com/MvvmCross/MvvmCross/pull/1783) ([jhorv](https://github.com/jhorv))
- Changed modifiers so can be used in overrides [\#1780](https://github.com/MvvmCross/MvvmCross/pull/1780) ([eL-Prova](https://github.com/eL-Prova))
- Allow user to provide full path in the file plugin [\#1778](https://github.com/MvvmCross/MvvmCross/pull/1778) ([willsb](https://github.com/willsb))
- Kill MvxAllThreadDispatchingObject [\#1777](https://github.com/MvvmCross/MvvmCross/pull/1777) ([willsb](https://github.com/willsb))
- Improve MvvmCross.StarterPack [\#1775](https://github.com/MvvmCross/MvvmCross/pull/1775) ([willsb](https://github.com/willsb))
- Fixed tipcalc tutorial [\#1772](https://github.com/MvvmCross/MvvmCross/pull/1772) ([MarcBruins](https://github.com/MarcBruins))
- Add analytics [\#1771](https://github.com/MvvmCross/MvvmCross/pull/1771) ([MarcBruins](https://github.com/MarcBruins))
- View Presenter doc: Simplify steps for hints and fix enumeration [\#1768](https://github.com/MvvmCross/MvvmCross/pull/1768) ([nmilcoff](https://github.com/nmilcoff))
- Fix typo [\#1767](https://github.com/MvvmCross/MvvmCross/pull/1767) ([martijn00](https://github.com/martijn00))
- Updates for documentation [\#1766](https://github.com/MvvmCross/MvvmCross/pull/1766) ([martijn00](https://github.com/martijn00))
- Add documentation about the new navigation and fix copy pasta error [\#1765](https://github.com/MvvmCross/MvvmCross/pull/1765) ([martijn00](https://github.com/martijn00))
- change parameter type to TInit [\#1764](https://github.com/MvvmCross/MvvmCross/pull/1764) ([Hobbes1987](https://github.com/Hobbes1987))
- Android Forms activity needs to forward events to ActivityLifetimeListener [\#1762](https://github.com/MvvmCross/MvvmCross/pull/1762) ([LRP-sgravel](https://github.com/LRP-sgravel))
- Forms MvxImageView [\#1761](https://github.com/MvvmCross/MvvmCross/pull/1761) ([LRP-sgravel](https://github.com/LRP-sgravel))
- Moving from Old wiki - Create Customizing using App and Setup [\#1760](https://github.com/MvvmCross/MvvmCross/pull/1760) ([AnthonyNjuguna](https://github.com/AnthonyNjuguna))
- Revert MvxRecyclerView Header/Footer and Grouping Features [\#1758](https://github.com/MvvmCross/MvvmCross/pull/1758) ([kjeremy](https://github.com/kjeremy))
- Make Example.Droid deployable in debug [\#1757](https://github.com/MvvmCross/MvvmCross/pull/1757) ([kjeremy](https://github.com/kjeremy))
- Create docs for ViewPresenter in fundamentals [\#1756](https://github.com/MvvmCross/MvvmCross/pull/1756) ([nmilcoff](https://github.com/nmilcoff))
- MvxForms lang bindings [\#1755](https://github.com/MvvmCross/MvvmCross/pull/1755) ([LRP-sgravel](https://github.com/LRP-sgravel))
- More clean up of Warnings [\#1753](https://github.com/MvvmCross/MvvmCross/pull/1753) ([Cheesebaron](https://github.com/Cheesebaron))
- Added null checking on ViewModel for life-cycle events [\#1752](https://github.com/MvvmCross/MvvmCross/pull/1752) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Fix a bunch of warnings [\#1749](https://github.com/MvvmCross/MvvmCross/pull/1749) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix more samples, names and namespaces [\#1747](https://github.com/MvvmCross/MvvmCross/pull/1747) ([martijn00](https://github.com/martijn00))
- Fix remaining Forms issue's [\#1746](https://github.com/MvvmCross/MvvmCross/pull/1746) ([martijn00](https://github.com/martijn00))
- Cleanup of Forms project and packages [\#1745](https://github.com/MvvmCross/MvvmCross/pull/1745) ([martijn00](https://github.com/martijn00))
- Cleanup some code [\#1744](https://github.com/MvvmCross/MvvmCross/pull/1744) ([kjeremy](https://github.com/kjeremy))
- Check phone capabilities before calling [\#1742](https://github.com/MvvmCross/MvvmCross/pull/1742) ([Cheesebaron](https://github.com/Cheesebaron))
- Added MvxBindings to Forms integration [\#1741](https://github.com/MvvmCross/MvvmCross/pull/1741) ([LRP-sgravel](https://github.com/LRP-sgravel))
- Unify MvxTableViewCell, MvxCollectionViewCell and MvxCollectionReusableView constructors [\#1740](https://github.com/MvvmCross/MvvmCross/pull/1740) ([nmilcoff](https://github.com/nmilcoff))
- Add editorconfig file to help with with formatting and conventions [\#1739](https://github.com/MvvmCross/MvvmCross/pull/1739) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Don't throw exception in ctor for Fused [\#1738](https://github.com/MvvmCross/MvvmCross/pull/1738) ([Cheesebaron](https://github.com/Cheesebaron))
- Add some .gitignore [\#1737](https://github.com/MvvmCross/MvvmCross/pull/1737) ([kjeremy](https://github.com/kjeremy))
- Xamarin.Android.Support.\* 25.3.1 [\#1735](https://github.com/MvvmCross/MvvmCross/pull/1735) ([kjeremy](https://github.com/kjeremy))
- Fix Gitlink [\#1734](https://github.com/MvvmCross/MvvmCross/pull/1734) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix null check in Adapter setter [\#1733](https://github.com/MvvmCross/MvvmCross/pull/1733) ([kjeremy](https://github.com/kjeremy))
- Do not override a user's custom LayoutManager [\#1732](https://github.com/MvvmCross/MvvmCross/pull/1732) ([kjeremy](https://github.com/kjeremy))
- Fix nuspec derpage [\#1726](https://github.com/MvvmCross/MvvmCross/pull/1726) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixed link to documentation in Readme [\#1722](https://github.com/MvvmCross/MvvmCross/pull/1722) ([markuspalme](https://github.com/markuspalme))
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
- add Tomasz [\#1458](https://github.com/MvvmCross/MvvmCross/pull/1458) ([spockfish](https://github.com/spockfish))

## [5.0.0-beta.12](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.12) (2017-05-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.11...5.0.0-beta.12)

## [5.0.0-beta.11](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.11) (2017-05-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.10...5.0.0-beta.11)

## [5.0.0-beta.10](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.10) (2017-05-15)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.9...5.0.0-beta.10)

## [5.0.0-beta.9](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.9) (2017-05-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.8...5.0.0-beta.9)

**Closed issues:**

- about .NET Standard Class Library [\#1787](https://github.com/MvvmCross/MvvmCross/issues/1787)

## [5.0.0-beta.8](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.8) (2017-05-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.7...5.0.0-beta.8)

**Closed issues:**

- Feature request: Property binding in iOS designer  [\#1781](https://github.com/MvvmCross/MvvmCross/issues/1781)
- the mvvmcross.com's Doc isn't too good show in mobile phone [\#1776](https://github.com/MvvmCross/MvvmCross/issues/1776)
- Realm and MvvmCross activity lifecycle, collection change notifications on destroyed fragments [\#1770](https://github.com/MvvmCross/MvvmCross/issues/1770)
- How to "hook into" and do some post processing after a call to BindingInflate? [\#1769](https://github.com/MvvmCross/MvvmCross/issues/1769)
- Binding to Realm IList issue [\#1545](https://github.com/MvvmCross/MvvmCross/issues/1545)

## [5.0.0-beta.7](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.7) (2017-05-03)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.6...5.0.0-beta.7)

## [5.0.0-beta.6](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.6) (2017-05-01)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.5...5.0.0-beta.6)

## [5.0.0-beta.5](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.5) (2017-04-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.4...5.0.0-beta.5)

**Fixed bugs:**

- \[Android\] Setup initialization issue when app killed and resumed [\#1192](https://github.com/MvvmCross/MvvmCross/issues/1192)
- MvxPictureChooserTask does not \(always\) work when it get's called from a Android Fragment [\#1107](https://github.com/MvvmCross/MvvmCross/issues/1107)
- MvxNSDatePickerDateTargetBinding timezone issues [\#924](https://github.com/MvvmCross/MvvmCross/issues/924)
- picturechooser always show in portrait orientation [\#761](https://github.com/MvvmCross/MvvmCross/issues/761)
- Annoyance when using Converter [\#697](https://github.com/MvvmCross/MvvmCross/issues/697)
- Problem with Android.Dialog and text focus [\#337](https://github.com/MvvmCross/MvvmCross/issues/337)

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
- MvxDatePicker value not updated on lollipop devices [\#1139](https://github.com/MvvmCross/MvvmCross/issues/1139)
- Calling Init\(\) on ViewModel creates exception without parameters [\#1132](https://github.com/MvvmCross/MvvmCross/issues/1132)
- IMvxLocationWatcher.Stop\(\) do not disable hardware GPS on iOS [\#1127](https://github.com/MvvmCross/MvvmCross/issues/1127)
- Could you add XML comments to classes and methods within Cirrious.MvvmCross? [\#1108](https://github.com/MvvmCross/MvvmCross/issues/1108)
- IMvxMainThreadDispatcher needs a method where you can wait for the UI task to finish [\#1033](https://github.com/MvvmCross/MvvmCross/issues/1033)
- Editing bound EditText with android:textAllCaps causes exeption [\#1002](https://github.com/MvvmCross/MvvmCross/issues/1002)
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

## [5.0.0-beta.4](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.4) (2017-04-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.3...5.0.0-beta.4)

**Closed issues:**

- MvxFileStore FullPath Customization [\#1630](https://github.com/MvvmCross/MvvmCross/issues/1630)
- New MvxListView bindings not working since last xamarin update [\#1395](https://github.com/MvvmCross/MvvmCross/issues/1395)

## [5.0.0-beta.3](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.3) (2017-04-28)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.2...5.0.0-beta.3)

**Closed issues:**

- Improve load time of plugins [\#1729](https://github.com/MvvmCross/MvvmCross/issues/1729)

## [5.0.0-beta.2](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.2) (2017-04-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.1...5.0.0-beta.2)

**Closed issues:**

- Add open collective to the website [\#1723](https://github.com/MvvmCross/MvvmCross/issues/1723)
- Website: Missing margins between logos [\#1712](https://github.com/MvvmCross/MvvmCross/issues/1712)

## [5.0.0-beta.1](https://github.com/MvvmCross/MvvmCross/tree/5.0.0-beta.1) (2017-04-25)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.4.0...5.0.0-beta.1)

**Fixed bugs:**

- MvxAsyncCommand inconsistencies on Android [\#1589](https://github.com/MvvmCross/MvvmCross/issues/1589)
- Bindings in 4.2.x  System.AggregateException caught in HockeyApp [\#1398](https://github.com/MvvmCross/MvvmCross/issues/1398)
- AppCompatActivity and ActionBarActivity cause null reference exception onCreate\(Bundle ....\) [\#1112](https://github.com/MvvmCross/MvvmCross/issues/1112)
- Sometime CrossUI.Touch Elements don't repaint until rotated [\#977](https://github.com/MvvmCross/MvvmCross/issues/977)
- Sort out SQLite installer for winrt [\#307](https://github.com/MvvmCross/MvvmCross/issues/307)

**Closed issues:**

- Binding data with headers does not work  [\#1702](https://github.com/MvvmCross/MvvmCross/issues/1702)
- Registering services in InitializeFirstChance not working as expected [\#1676](https://github.com/MvvmCross/MvvmCross/issues/1676)
- Column with name "column" is not defined on the local table "table" [\#1662](https://github.com/MvvmCross/MvvmCross/issues/1662)
- InfoWindowClick does not work with ShowViewModel [\#1647](https://github.com/MvvmCross/MvvmCross/issues/1647)
- Custom font not working with MvxAppCompatActivity [\#1592](https://github.com/MvvmCross/MvvmCross/issues/1592)
- Android Tabs Example [\#1586](https://github.com/MvvmCross/MvvmCross/issues/1586)
- How to hide all XamarinSidebar menu on page ? \(iOS\)  [\#1585](https://github.com/MvvmCross/MvvmCross/issues/1585)
- Toolbar Search Binding [\#1584](https://github.com/MvvmCross/MvvmCross/issues/1584)
- Telerik 2017 R1 for Xamarin.Forms Android incompatible with MvvmCross [\#1579](https://github.com/MvvmCross/MvvmCross/issues/1579)
- Add Preserve attribute to all bindings  [\#1578](https://github.com/MvvmCross/MvvmCross/issues/1578)
- Nuspec PCL profiles? [\#1572](https://github.com/MvvmCross/MvvmCross/issues/1572)
- \[AndroidSupport\] RecyclerView view created by adapter should be accessable in code after view is inflated [\#1566](https://github.com/MvvmCross/MvvmCross/issues/1566)
- \[AndroidSupport\] RecyclerView support for Footer/Header/Grouping \(sections\) [\#1565](https://github.com/MvvmCross/MvvmCross/issues/1565)
- Navigation Page shows foreign language "Zuruck".  How do I fix or override this? [\#1561](https://github.com/MvvmCross/MvvmCross/issues/1561)
- Error installing MvvmCross.Forms.Presenter [\#1560](https://github.com/MvvmCross/MvvmCross/issues/1560)
- Android send intent from external apps does not start main activity on some devices/OS [\#1551](https://github.com/MvvmCross/MvvmCross/issues/1551)
- MvxAndroidViewFactory CreateView question [\#1550](https://github.com/MvvmCross/MvvmCross/issues/1550)
- "Partial declarations of 'TestView' must not specify different base classes" error [\#1547](https://github.com/MvvmCross/MvvmCross/issues/1547)
- Mvx.MvxListView ItemAppearing  [\#1544](https://github.com/MvvmCross/MvvmCross/issues/1544)
- MvxListView ItemClick doesn't work [\#1543](https://github.com/MvvmCross/MvvmCross/issues/1543)
- iOS crash/ \<unknown\> \<0xffffffff\>/ Foundation.NSObject.ReleaseManagedRef /Foundation.NSObject/NSObject\_Disposer.Drain [\#1539](https://github.com/MvvmCross/MvvmCross/issues/1539)
- cant get connection [\#1513](https://github.com/MvvmCross/MvvmCross/issues/1513)
- app crashing after detaching from VS debugger [\#1498](https://github.com/MvvmCross/MvvmCross/issues/1498)
- Add ItemClick on LinearLayout as on ListView [\#1496](https://github.com/MvvmCross/MvvmCross/issues/1496)
- Databinding Not Working as Expected on MVVMCross [\#1487](https://github.com/MvvmCross/MvvmCross/issues/1487)
- MvxDialogViewController does not Dispose after assigning the Root element [\#1445](https://github.com/MvvmCross/MvvmCross/issues/1445)
- MvxImageView Issue with nested MvxRecyclerView [\#1444](https://github.com/MvvmCross/MvvmCross/issues/1444)
- Reloading the Main Activity : Navigation Stops working [\#1408](https://github.com/MvvmCross/MvvmCross/issues/1408)
- set\_TextFormatted leads to app crash [\#1405](https://github.com/MvvmCross/MvvmCross/issues/1405)
- A standardized way of doing vm -\> view communication [\#1386](https://github.com/MvvmCross/MvvmCross/issues/1386)
- MvvmCross Unity3d ugui [\#1380](https://github.com/MvvmCross/MvvmCross/issues/1380)
- NullReferenceException in MvxFullBinding [\#1378](https://github.com/MvvmCross/MvvmCross/issues/1378)
- System.InvalidCastException on Android [\#1305](https://github.com/MvvmCross/MvvmCross/issues/1305)
- iOS EntryElement binding problem \(revised\) [\#1291](https://github.com/MvvmCross/MvvmCross/issues/1291)
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

## [4.4.0](https://github.com/MvvmCross/MvvmCross/tree/4.4.0) (2016-11-01)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.3.0...4.4.0)

**Closed issues:**

- Apps based on MvvmCross 4.3 crash right after started in iOS 10 emulator [\#1452](https://github.com/MvvmCross/MvvmCross/issues/1452)
- ClassNotFoundException using Mvx.MvxListView [\#1450](https://github.com/MvvmCross/MvvmCross/issues/1450)
- Calling ShowViewModel on activity, does not show child fragment [\#1441](https://github.com/MvvmCross/MvvmCross/issues/1441)
- ActionBar title stay black with Theme Material.Light.DarkActionBar [\#1440](https://github.com/MvvmCross/MvvmCross/issues/1440)
- Crash when rehydrating an activity on Android 2.3.3 [\#1431](https://github.com/MvvmCross/MvvmCross/issues/1431)
- MvxTaskBasedBindingContext issue with dynamic cell's height in iOS [\#1423](https://github.com/MvvmCross/MvvmCross/issues/1423)
- Updating From 4.1 to 4.2.2 breaks UITableViewAutomaticDimension [\#1409](https://github.com/MvvmCross/MvvmCross/issues/1409)
- WeakSubcribe an EventHandler without EventArgs? [\#1402](https://github.com/MvvmCross/MvvmCross/issues/1402)
- MvvMCross and WindowsStore. [\#1055](https://github.com/MvvmCross/MvvmCross/issues/1055)
- Success or error not firing in IMvxLocationWatcher \(Location PlugIn\) [\#655](https://github.com/MvvmCross/MvvmCross/issues/655)
- set\_TextFormatted leads to app crash [\#1502](https://github.com/MvvmCross/MvvmCross/issues/1502)
- Add MVVMCross ObservableCollection [\#1456](https://github.com/MvvmCross/MvvmCross/issues/1456)
- Cancel method declaration missing in IMvxAsyncCommand [\#1454](https://github.com/MvvmCross/MvvmCross/issues/1454)
- Add PreserveAttribute on classes known to always be used [\#1432](https://github.com/MvvmCross/MvvmCross/issues/1432)
- Improvement to pass Complex Dto between ViewModels [\#1302](https://github.com/MvvmCross/MvvmCross/issues/1302)

**Merged pull requests:**

- Added analyzer for possible ShowViewModel without arguments, when usi… [\#1477](https://github.com/MvvmCross/MvvmCross/pull/1477) ([azchohfi](https://github.com/azchohfi))
- Fix missing files for BindingEx [\#1476](https://github.com/MvvmCross/MvvmCross/pull/1476) ([Cheesebaron](https://github.com/Cheesebaron))
- Allow fragments with different java names \(via RegisterAttribute\) [\#1473](https://github.com/MvvmCross/MvvmCross/pull/1473) ([kjeremy](https://github.com/kjeremy))
- Simplify Nuspecs [\#1471](https://github.com/MvvmCross/MvvmCross/pull/1471) ([martijn00](https://github.com/martijn00))
- Drawable target binding [\#1470](https://github.com/MvvmCross/MvvmCross/pull/1470) ([kjeremy](https://github.com/kjeremy))
- Enable usage of Multiple MvxAttributes specifying different layouts [\#1469](https://github.com/MvvmCross/MvvmCross/pull/1469) ([martijn00](https://github.com/martijn00))
- Cast in case the value is null [\#1464](https://github.com/MvvmCross/MvvmCross/pull/1464) ([kjeremy](https://github.com/kjeremy))
- Virtualize methods on MvxExpandableListAdapter [\#1463](https://github.com/MvvmCross/MvvmCross/pull/1463) ([kjeremy](https://github.com/kjeremy))
- Add a MvvmCross ObservableCollection [\#1462](https://github.com/MvvmCross/MvvmCross/pull/1462) ([martijn00](https://github.com/martijn00))
- Fix merge to wrong branch [\#1461](https://github.com/MvvmCross/MvvmCross/pull/1461) ([martijn00](https://github.com/martijn00))
- Removal of WindowsPhone Silverlight [\#1460](https://github.com/MvvmCross/MvvmCross/pull/1460) ([martijn00](https://github.com/martijn00))
- Fix missing dependency update for starter pack nuspec [\#1459](https://github.com/MvvmCross/MvvmCross/pull/1459) ([martijn00](https://github.com/martijn00))
- Add Cancel for async command to interface [\#1455](https://github.com/MvvmCross/MvvmCross/pull/1455) ([martijn00](https://github.com/martijn00))
- Add test projects to easily test changes that are made to the project [\#1453](https://github.com/MvvmCross/MvvmCross/pull/1453) ([MarcBruins](https://github.com/MarcBruins))
- Promote VeryExperimental ValueCombiners to stable [\#1449](https://github.com/MvvmCross/MvvmCross/pull/1449) ([kjeremy](https://github.com/kjeremy))
- Add initial support for tvOS [\#1448](https://github.com/MvvmCross/MvvmCross/pull/1448) ([martijn00](https://github.com/martijn00))
- Protect Android Bindings [\#1446](https://github.com/MvvmCross/MvvmCross/pull/1446) ([kjeremy](https://github.com/kjeremy))
- Synchronous resizable cells for iOS [\#1442](https://github.com/MvvmCross/MvvmCross/pull/1442) ([MarcBruins](https://github.com/MarcBruins))
- Use the the lower level GetString on older platforms [\#1439](https://github.com/MvvmCross/MvvmCross/pull/1439) ([martijn00](https://github.com/martijn00))
- Merge \#1151 [\#1438](https://github.com/MvvmCross/MvvmCross/pull/1438) ([martijn00](https://github.com/martijn00))
- Add ViewDidLayoutSubviewsCalled to IMvxEventSourceViewController on iOS [\#1435](https://github.com/MvvmCross/MvvmCross/pull/1435) ([Costo](https://github.com/Costo))
- Improved support for binding to interfaces [\#1434](https://github.com/MvvmCross/MvvmCross/pull/1434) ([drungrin](https://github.com/drungrin))
- Add RegisterAttribute to Android classes [\#1433](https://github.com/MvvmCross/MvvmCross/pull/1433) ([kjeremy](https://github.com/kjeremy))
- Added support for view model instantiation using segue on iOS [\#1404](https://github.com/MvvmCross/MvvmCross/pull/1404) ([chrilith](https://github.com/chrilith))

## [4.3.0](https://github.com/MvvmCross/MvvmCross/tree/4.3.0) (2016-09-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.2.3...4.3.0)

**Fixed bugs:**

- Multiple issues in MvxFileDownloadCache [\#717](https://github.com/MvvmCross/MvvmCross/issues/717)

**Closed issues:**

- Another backstack flag not respected bug [\#1429](https://github.com/MvvmCross/MvvmCross/issues/1429)
- ShowFragment bug - if cache is empty - AddToBackStack flag does not work  [\#1427](https://github.com/MvvmCross/MvvmCross/issues/1427)
- MvvmCross initialization support for IntentService, BroadcastReceiver [\#1425](https://github.com/MvvmCross/MvvmCross/issues/1425)
- Issue template should include note about only filing bugs against latest version [\#1419](https://github.com/MvvmCross/MvvmCross/issues/1419)
- Decimal zero not correctly interpreted when used as part of boolean expression [\#1418](https://github.com/MvvmCross/MvvmCross/issues/1418)
- Snackbar Inflation Invalid Cast Exception [\#1412](https://github.com/MvvmCross/MvvmCross/issues/1412)

**Merged pull requests:**

- AddToBackStack flag not respected fix. [\#1430](https://github.com/MvvmCross/MvvmCross/pull/1430) ([thefex](https://github.com/thefex))
- AddToBackStack flag FullFragging fix [\#1428](https://github.com/MvvmCross/MvvmCross/pull/1428) ([thefex](https://github.com/thefex))
- Mvx IntentService/BroadcastReceiver support [\#1426](https://github.com/MvvmCross/MvvmCross/pull/1426) ([thefex](https://github.com/thefex))
- Adds decimal handling to Combiners [\#1421](https://github.com/MvvmCross/MvvmCross/pull/1421) ([willsb](https://github.com/willsb))
- Fix splash screen initialization [\#1420](https://github.com/MvvmCross/MvvmCross/pull/1420) ([andyci](https://github.com/andyci))
- MvxImageViewResourceNameTargetBinding \(VectorDrawable resources\) [\#1417](https://github.com/MvvmCross/MvvmCross/pull/1417) ([fedemkr](https://github.com/fedemkr))

## [4.2.3](https://github.com/MvvmCross/MvvmCross/tree/4.2.3) (2016-08-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.2.2...4.2.3)

**Fixed bugs:**

- Problems with type based parameter serialization/deserialization [\#450](https://github.com/MvvmCross/MvvmCross/issues/450)

**Closed issues:**

- Bug MasterDetail duplicate toolbar [\#1406](https://github.com/MvvmCross/MvvmCross/issues/1406)
- Bug MasterDetail duplicate lines top [\#1401](https://github.com/MvvmCross/MvvmCross/issues/1401)
- After upgrade from 4.1.6 to 4.2.1 can't build UWP project [\#1391](https://github.com/MvvmCross/MvvmCross/issues/1391)
- Mac/XamMac/XamMac.Unified to Mac configuration [\#923](https://github.com/MvvmCross/MvvmCross/issues/923)
- XamMac vs MonoMac [\#516](https://github.com/MvvmCross/MvvmCross/issues/516)
- Support Nullable types in Navigation parameters [\#505](https://github.com/MvvmCross/MvvmCross/issues/505)
- Including mac build in nuget [\#502](https://github.com/MvvmCross/MvvmCross/issues/502)
- Tweaks for Macintosh automated build [\#356](https://github.com/MvvmCross/MvvmCross/issues/356)

## [4.2.2](https://github.com/MvvmCross/MvvmCross/tree/4.2.2) (2016-07-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.2.1...4.2.2)

**Closed issues:**

- Android LinkerPleaseInclude compiler error [\#1389](https://github.com/MvvmCross/MvvmCross/issues/1389)
- MVVMCross 4.2.1 System.Collections.Concurrent and PCL Issue [\#1388](https://github.com/MvvmCross/MvvmCross/issues/1388)
- Can't update MvvmCross.Binding to 4.2.1 on OS X [\#1387](https://github.com/MvvmCross/MvvmCross/issues/1387)
- SIGSEV in Bindings/SetValueImpl [\#1379](https://github.com/MvvmCross/MvvmCross/issues/1379)

## [4.2.1](https://github.com/MvvmCross/MvvmCross/tree/4.2.1) (2016-07-05)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.2.0...4.2.1)

**Fixed bugs:**

- Bindings project does not build on MAC [\#1385](https://github.com/MvvmCross/MvvmCross/issues/1385)
- MemoryIssue with MvxRecyclerview and ObservableCollection [\#1343](https://github.com/MvvmCross/MvvmCross/issues/1343)

**Closed issues:**

- Failed to resolve !!0\[\] System.Array::Empty\(\) [\#1374](https://github.com/MvvmCross/MvvmCross/issues/1374)
- 4.2.0 Starter package installs 4.1.4 [\#1367](https://github.com/MvvmCross/MvvmCross/issues/1367)
- Android: "Method 'Array.Empty' not found" after upgrading to MvvmCross 4.2.0  [\#1363](https://github.com/MvvmCross/MvvmCross/issues/1363)
- docs [\#1362](https://github.com/MvvmCross/MvvmCross/issues/1362)
- Using MvxFragmentsPresenter MvxFragment doesn't close at vm Close\(this\) [\#1341](https://github.com/MvvmCross/MvvmCross/issues/1341)

## [4.2.0](https://github.com/MvvmCross/MvvmCross/tree/4.2.0) (2016-06-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.1.6...4.2.0)

**Fixed bugs:**

- MvxValueConverter with NativeColor expression returns string in iOS [\#1149](https://github.com/MvvmCross/MvvmCross/issues/1149)
- iOS IMvxPictureChooserTaskpictureAvailable Action is just called once [\#952](https://github.com/MvvmCross/MvvmCross/issues/952)
- Problems with `set only` properties [\#907](https://github.com/MvvmCross/MvvmCross/issues/907)

**Closed issues:**

- Xamarin Android Tip Calculator [\#1347](https://github.com/MvvmCross/MvvmCross/issues/1347)
- Wpf multiwindow support [\#1340](https://github.com/MvvmCross/MvvmCross/issues/1340)
- Link error [\#1334](https://github.com/MvvmCross/MvvmCross/issues/1334)
- Android: "Method 'Array.Empty' not found" after upgrading to MvvmCross 4.1.5 [\#1330](https://github.com/MvvmCross/MvvmCross/issues/1330)
- Create Visual Studio and Xamarin Studio project templates [\#1329](https://github.com/MvvmCross/MvvmCross/issues/1329)
- Support bind iOS NSLayoutConstraint Constant [\#1326](https://github.com/MvvmCross/MvvmCross/issues/1326)
- \[UWP\] SplitView Navigation [\#1257](https://github.com/MvvmCross/MvvmCross/issues/1257)
- Possible bug with ImageView droid binding. [\#1227](https://github.com/MvvmCross/MvvmCross/issues/1227)
- Support for generic MvxAdapter\<T\> [\#1222](https://github.com/MvvmCross/MvvmCross/issues/1222)

**Merged pull requests:**

- Allow multiple EnsureInitialized calls [\#1253](https://github.com/MvvmCross/MvvmCross/pull/1253) ([andyci](https://github.com/andyci))
- Add support for own base class, ShowViewModel with parameter serialization [\#1410](https://github.com/MvvmCross/MvvmCross/pull/1410) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Fix line endings [\#1397](https://github.com/MvvmCross/MvvmCross/pull/1397) ([kjeremy](https://github.com/kjeremy))
- Fix linker include errors [\#1394](https://github.com/MvvmCross/MvvmCross/pull/1394) ([martijn00](https://github.com/martijn00))
- Remove dependency on System.Collections.Concurrent nuget [\#1393](https://github.com/MvvmCross/MvvmCross/pull/1393) ([Cheesebaron](https://github.com/Cheesebaron))
- Added code to prevent race condition for rapid UI updates [\#1390](https://github.com/MvvmCross/MvvmCross/pull/1390) ([obergshavefun](https://github.com/obergshavefun))
- When using MvxTaskBasedBindingContext \(default\) with full link enabled. [\#1384](https://github.com/MvvmCross/MvvmCross/pull/1384) ([softlion](https://github.com/softlion))
- new animation enabled collection source, revert legacy collection source code [\#1383](https://github.com/MvvmCross/MvvmCross/pull/1383) ([softlion](https://github.com/softlion))
- revert changes in MvxCollectionViewSource [\#1382](https://github.com/MvvmCross/MvvmCross/pull/1382) ([softlion](https://github.com/softlion))
- Adapter tweaks [\#1377](https://github.com/MvvmCross/MvvmCross/pull/1377) ([kjeremy](https://github.com/kjeremy))
- Fix MvxCollectionViewSource [\#1376](https://github.com/MvvmCross/MvvmCross/pull/1376) ([softlion](https://github.com/softlion))
- MvxAutoCompleteTextView\*TargetBinding require the target to actually … [\#1366](https://github.com/MvvmCross/MvvmCross/pull/1366) ([kjeremy](https://github.com/kjeremy))
- Fixed close viewmodel in caching fragment [\#1365](https://github.com/MvvmCross/MvvmCross/pull/1365) ([martijn00](https://github.com/martijn00))
- Add optional parameters to generic init [\#1364](https://github.com/MvvmCross/MvvmCross/pull/1364) ([martijn00](https://github.com/martijn00))
- Null-propagation for event handlers [\#1360](https://github.com/MvvmCross/MvvmCross/pull/1360) ([kjeremy](https://github.com/kjeremy))
- Fix spelling: amy =\> may [\#1359](https://github.com/MvvmCross/MvvmCross/pull/1359) ([kjeremy](https://github.com/kjeremy))
- Move away from MvxAndroidTargetBinding and use MvxConvertingTargetBinding [\#1357](https://github.com/MvvmCross/MvvmCross/pull/1357) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix for issue \#907 \(copied changes from commit "59e35d45d6e5ddf463070… [\#1356](https://github.com/MvvmCross/MvvmCross/pull/1356) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Dispose of the base after cleaning up the derived instance [\#1354](https://github.com/MvvmCross/MvvmCross/pull/1354) ([kjeremy](https://github.com/kjeremy))
- Add binding for TextView.Hint [\#1353](https://github.com/MvvmCross/MvvmCross/pull/1353) ([kjeremy](https://github.com/kjeremy))
- Misc cleanups [\#1352](https://github.com/MvvmCross/MvvmCross/pull/1352) ([kjeremy](https://github.com/kjeremy))
- Misc fixes [\#1351](https://github.com/MvvmCross/MvvmCross/pull/1351) ([kjeremy](https://github.com/kjeremy))
- Fix compilation error [\#1350](https://github.com/MvvmCross/MvvmCross/pull/1350) ([kjeremy](https://github.com/kjeremy))
- Use drawable instead of resource, which is not UI thread bound [\#1349](https://github.com/MvvmCross/MvvmCross/pull/1349) ([Cheesebaron](https://github.com/Cheesebaron))
- Don't resubscribe in Dispose [\#1348](https://github.com/MvvmCross/MvvmCross/pull/1348) ([kjeremy](https://github.com/kjeremy))
- Fix failing unit tests [\#1346](https://github.com/MvvmCross/MvvmCross/pull/1346) ([Cheesebaron](https://github.com/Cheesebaron))
- Unsubscribe events [\#1345](https://github.com/MvvmCross/MvvmCross/pull/1345) ([Cheesebaron](https://github.com/Cheesebaron))
- Remove LINQ from ReflectionExtensions [\#1344](https://github.com/MvvmCross/MvvmCross/pull/1344) ([Cheesebaron](https://github.com/Cheesebaron))
- MultiRegionViewPresenter - Support frame search failure. [\#1339](https://github.com/MvvmCross/MvvmCross/pull/1339) ([ErezG](https://github.com/ErezG))
- Add UI refresh control bindings [\#1338](https://github.com/MvvmCross/MvvmCross/pull/1338) ([martijn00](https://github.com/martijn00))
- Allows binding to EditTextPreference.Text and TwoStatePreference.Chec… [\#1337](https://github.com/MvvmCross/MvvmCross/pull/1337) ([kjeremy](https://github.com/kjeremy))
- Cache Java.Lang.Boolean.True since in Xamarin it results in a call [\#1336](https://github.com/MvvmCross/MvvmCross/pull/1336) ([kjeremy](https://github.com/kjeremy))
- Raise AttachCalled with the passed in activity, not this.Activity [\#1335](https://github.com/MvvmCross/MvvmCross/pull/1335) ([kjeremy](https://github.com/kjeremy))
- Animate UICollectionView changes [\#1333](https://github.com/MvvmCross/MvvmCross/pull/1333) ([softlion](https://github.com/softlion))
- Generic MvxAdapter implementation [\#1332](https://github.com/MvvmCross/MvvmCross/pull/1332) ([MarcBruins](https://github.com/MarcBruins))
- Ignore transparent proxies on Mono, \#1308. [\#1325](https://github.com/MvvmCross/MvvmCross/pull/1325) ([yallie](https://github.com/yallie))
- Added Code of Conduct based on contributor-covenant [\#1324](https://github.com/MvvmCross/MvvmCross/pull/1324) ([PerPage](https://github.com/PerPage))
- UWP - Fixed 'ViewModel.Close\(this\)'. [\#1323](https://github.com/MvvmCross/MvvmCross/pull/1323) ([pauloapsantos](https://github.com/pauloapsantos))
- TaskBasedBindingContext allows smoother scrolling by databinding on a worker threads [\#1322](https://github.com/MvvmCross/MvvmCross/pull/1322) ([softlion](https://github.com/softlion))
- New PR for \#1099 - Use Reflection instead of Expression.Compile\(\) [\#1321](https://github.com/MvvmCross/MvvmCross/pull/1321) ([Cheesebaron](https://github.com/Cheesebaron))

## [4.1.6](https://github.com/MvvmCross/MvvmCross/tree/4.1.6) (2016-05-24)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.1.5...4.1.6)

**Fixed bugs:**

- MvxAndroidTask defined twice [\#1327](https://github.com/MvvmCross/MvvmCross/issues/1327)

**Closed issues:**

- The obs collection ++ thread [\#257](https://github.com/MvvmCross/MvvmCross/issues/257)

## [4.1.5](https://github.com/MvvmCross/MvvmCross/tree/4.1.5) (2016-05-19)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.1.4...4.1.5)

**Closed issues:**

- MvxSpinner won't work inside fragment [\#1320](https://github.com/MvvmCross/MvvmCross/issues/1320)
- website down [\#1318](https://github.com/MvvmCross/MvvmCross/issues/1318)
- MvxCachingFragmentCompatActivity not finding registered fragments [\#1311](https://github.com/MvvmCross/MvvmCross/issues/1311)
- IoC doesn't support transparent proxies on Xamarin.Android [\#1308](https://github.com/MvvmCross/MvvmCross/issues/1308)
- MvxTapGestureRecognizerBehaviour with CancelsTouchesInView [\#1303](https://github.com/MvvmCross/MvvmCross/issues/1303)
- Mvx Resolve faile on Windows 10 \( UWP \) project. [\#1255](https://github.com/MvvmCross/MvvmCross/issues/1255)
- MvvmCross team meeting at Evolve 2016 [\#1197](https://github.com/MvvmCross/MvvmCross/issues/1197)
- java.lang.RuntimeException: Unable to start activity ComponentInfo...android.content.res.Resources$NotFoundException [\#1147](https://github.com/MvvmCross/MvvmCross/issues/1147)
- Object stored in PageStackEntry.Parameter has changed from MvxViewModelRequest to String [\#1111](https://github.com/MvvmCross/MvvmCross/issues/1111)
- Various solutions [\#917](https://github.com/MvvmCross/MvvmCross/issues/917)
- "project governance" - aka a "bigger, more fun party" [\#841](https://github.com/MvvmCross/MvvmCross/issues/841)

## [4.1.4](https://github.com/MvvmCross/MvvmCross/tree/4.1.4) (2016-04-20)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/Binding_4.1.1...4.1.4)

**Fixed bugs:**

- MvxDynamicImageHelper modifies UI in non UI thread [\#1184](https://github.com/MvvmCross/MvvmCross/issues/1184)

**Closed issues:**

- MvxLayoutInflater missing JNI constructors [\#1299](https://github.com/MvvmCross/MvvmCross/issues/1299)
- ReloadFromBundle/SaveStateToBundle [\#1296](https://github.com/MvvmCross/MvvmCross/issues/1296)
- Shouldn't we introduce an `IMvxAsyncCommand` interface? [\#1293](https://github.com/MvvmCross/MvvmCross/issues/1293)
- Failed to resolve System.Boolean [\#1289](https://github.com/MvvmCross/MvvmCross/issues/1289)
- Investigate whether we should remove all logic from ACW Constructors [\#1285](https://github.com/MvvmCross/MvvmCross/issues/1285)
- How to use prepopulated DB in sqlite plugin [\#1280](https://github.com/MvvmCross/MvvmCross/issues/1280)
- How to use prepopulated DB in sqlite plugin [\#1279](https://github.com/MvvmCross/MvvmCross/issues/1279)
- iOS EntryElement binding problem [\#1259](https://github.com/MvvmCross/MvvmCross/issues/1259)
- \[UWP\]Plugins PictureChooser PluginLoader can`t load [\#1258](https://github.com/MvvmCross/MvvmCross/issues/1258)
- Mvvmcross 4 SQLite plugin iOS build error [\#1181](https://github.com/MvvmCross/MvvmCross/issues/1181)

## [Binding_4.1.1](https://github.com/MvvmCross/MvvmCross/tree/Binding_4.1.1) (2016-04-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/DroidShared_4.1.2...Binding_4.1.1)

## [DroidShared_4.1.2](https://github.com/MvvmCross/MvvmCross/tree/DroidShared_4.1.2) (2016-04-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/FullFragging_4.1.1...DroidShared_4.1.2)

## [FullFragging_4.1.1](https://github.com/MvvmCross/MvvmCross/tree/FullFragging_4.1.1) (2016-04-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.1.0...FullFragging_4.1.1)

**Closed issues:**

- Bump latest version in mvvmcross.com [\#1276](https://github.com/MvvmCross/MvvmCross/issues/1276)
- Mvvmcross picture chooser ios issue [\#1275](https://github.com/MvvmCross/MvvmCross/issues/1275)
- MvxCachingFragmentActivity crash when minimizing with MVVMCross 4.1.0 [\#1273](https://github.com/MvvmCross/MvvmCross/issues/1273)
- Android use Custom ViewFactory  [\#1271](https://github.com/MvvmCross/MvvmCross/issues/1271)

## [4.1.0](https://github.com/MvvmCross/MvvmCross/tree/4.1.0) (2016-03-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0...4.1.0)

**Closed issues:**

- Missing stylable field MvxExpandableListView\_GroupItemTemplate [\#1265](https://github.com/MvvmCross/MvvmCross/issues/1265)
- MvxListView with Custom MvxAdapter doesn't work in MvxFragment [\#1263](https://github.com/MvvmCross/MvvmCross/issues/1263)
- Missing MvxLayoutInflaterCompat+FactoryWrapper2 JNI constructors [\#1260](https://github.com/MvvmCross/MvvmCross/issues/1260)
- MvvmCross Android :Two way binding is not working on Fragment ViewModel of a ViewPager. [\#1256](https://github.com/MvvmCross/MvvmCross/issues/1256)
- MvxBind breaks after upgrading to MvvMCross 4.0 [\#1254](https://github.com/MvvmCross/MvvmCross/issues/1254)
- Error inflating class Mvx.MvxListView [\#1250](https://github.com/MvvmCross/MvvmCross/issues/1250)
- Please make Close\(IMvxViewModel viewModel\) virtual in MvxFragmentCompatActivity class [\#1248](https://github.com/MvvmCross/MvvmCross/issues/1248)
- MvxSpinner's SelectedItem binding broken  [\#1242](https://github.com/MvvmCross/MvvmCross/issues/1242)
- Shouldn't MvxWrappingCommand implement IMvxCommand? [\#1240](https://github.com/MvvmCross/MvvmCross/issues/1240)
- Links in ReadMe do not work [\#1233](https://github.com/MvvmCross/MvvmCross/issues/1233)
- Proguard necessary naming conventions [\#1228](https://github.com/MvvmCross/MvvmCross/issues/1228)
- Bluetooth BLE Support [\#543](https://github.com/MvvmCross/MvvmCross/issues/543)
- MvxImageView - loading local images more efficiently using expected width and height [\#471](https://github.com/MvvmCross/MvvmCross/issues/471)

**Merged pull requests:**

- Add bitmap existance check in Droid's MvxImageView [\#1267](https://github.com/MvvmCross/MvvmCross/pull/1267) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- Added swipe-gesture-recognizer [\#1266](https://github.com/MvvmCross/MvvmCross/pull/1266) ([SimonSimCity](https://github.com/SimonSimCity))
- Fix Android crash when Bitmap updated after MvxImageView destroyed [\#1262](https://github.com/MvvmCross/MvvmCross/pull/1262) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- Added missing JNI interop constructors in MvxLayoutInflaterCompat [\#1261](https://github.com/MvvmCross/MvvmCross/pull/1261) ([thefex](https://github.com/thefex))
- \[Analyzer/CodeFix\] RaiseCanExecuteChanged never called when needed [\#1252](https://github.com/MvvmCross/MvvmCross/pull/1252) ([azchohfi](https://github.com/azchohfi))
- MvxWindowsMultiRegionViewPresenter - support searching for a frame which exists inside a frame [\#1249](https://github.com/MvvmCross/MvvmCross/pull/1249) ([ErezG](https://github.com/ErezG))
- Added helpers for localization with fluent binding [\#1247](https://github.com/MvvmCross/MvvmCross/pull/1247) ([lothrop](https://github.com/lothrop))
- Fixed CodeAnalysis.nuspec. [\#1246](https://github.com/MvvmCross/MvvmCross/pull/1246) ([azchohfi](https://github.com/azchohfi))
- Removes mandatory parameters from MvxWrappingCommand [\#1244](https://github.com/MvvmCross/MvvmCross/pull/1244) ([willsb](https://github.com/willsb))
- Fix for forced fade animation on replaced rows [\#1243](https://github.com/MvvmCross/MvvmCross/pull/1243) ([bspinner](https://github.com/bspinner))
- Added MvxCollectionReusableView [\#1241](https://github.com/MvvmCross/MvvmCross/pull/1241) ([PelleRavn](https://github.com/PelleRavn))
- Added Analysers test project. [\#1238](https://github.com/MvvmCross/MvvmCross/pull/1238) ([azchohfi](https://github.com/azchohfi))
- Renamed icon location [\#1237](https://github.com/MvvmCross/MvvmCross/pull/1237) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Use mipmap as icon for splash screen [\#1236](https://github.com/MvvmCross/MvvmCross/pull/1236) ([martijn00](https://github.com/martijn00))
- Help the coder for NotifyDataSetChanged exceptions [\#1231](https://github.com/MvvmCross/MvvmCross/pull/1231) ([roubachof](https://github.com/roubachof))
- Updates to fix MvxExpandableListAdapter when creating adapter in code [\#1317](https://github.com/MvvmCross/MvvmCross/pull/1317) ([tomcurran](https://github.com/tomcurran))
- Apply must be called analyzer and codefix. [\#1313](https://github.com/MvvmCross/MvvmCross/pull/1313) ([azchohfi](https://github.com/azchohfi))
- Replaced relative path with build variable for Program Files folder. [\#1312](https://github.com/MvvmCross/MvvmCross/pull/1312) ([azchohfi](https://github.com/azchohfi))
- Set image resource directly on the ImageView instead of decoding it first [\#1307](https://github.com/MvvmCross/MvvmCross/pull/1307) ([tiftof](https://github.com/tiftof))
- New analyzer/codefix for MvxMessengerSubscription return value. [\#1306](https://github.com/MvvmCross/MvvmCross/pull/1306) ([azchohfi](https://github.com/azchohfi))
- MvxTapGestureRecognizerBehaviour with CancelsTouchesInView [\#1304](https://github.com/MvvmCross/MvvmCross/pull/1304) ([promontis](https://github.com/promontis))
- Add missing JNI constructors [\#1300](https://github.com/MvvmCross/MvvmCross/pull/1300) ([kjeremy](https://github.com/kjeremy))
- Introduces the IMvxAsyncCommand interface [\#1298](https://github.com/MvvmCross/MvvmCross/pull/1298) ([willsb](https://github.com/willsb))
- add .mailmap file so 'git shortlog' shows sane output [\#1295](https://github.com/MvvmCross/MvvmCross/pull/1295) ([spockfish](https://github.com/spockfish))
- Add a way to interact on Content with MvxFrameControl [\#1294](https://github.com/MvvmCross/MvvmCross/pull/1294) ([sescandell](https://github.com/sescandell))
- Json streams [\#1292](https://github.com/MvvmCross/MvvmCross/pull/1292) ([martijn00](https://github.com/martijn00))
- Refactor plugin registry [\#1290](https://github.com/MvvmCross/MvvmCross/pull/1290) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- Fix droid shared output path and remove designer files [\#1288](https://github.com/MvvmCross/MvvmCross/pull/1288) ([martijn00](https://github.com/martijn00))
- Fragment presenter multi parent [\#1287](https://github.com/MvvmCross/MvvmCross/pull/1287) ([thefex](https://github.com/thefex))
- Don't put any special logic in the ACW constructors [\#1286](https://github.com/MvvmCross/MvvmCross/pull/1286) ([kjeremy](https://github.com/kjeremy))
- Add possibility to pass custom objects with Viewmodel navigation [\#1284](https://github.com/MvvmCross/MvvmCross/pull/1284) ([martijn00](https://github.com/martijn00))
- Fixes: System.InvalidOperationExceptionSequence [\#1283](https://github.com/MvvmCross/MvvmCross/pull/1283) ([robertbaker](https://github.com/robertbaker))
- Added missing constructor [\#1277](https://github.com/MvvmCross/MvvmCross/pull/1277) ([slown1](https://github.com/slown1))
- Fix caching and some bugs for full-fragging [\#1272](https://github.com/MvvmCross/MvvmCross/pull/1272) ([martijn00](https://github.com/martijn00))
- Support for deserializing json streams [\#1270](https://github.com/MvvmCross/MvvmCross/pull/1270) ([Cheesebaron](https://github.com/Cheesebaron))
- UIStepper Value target binding and UIControl ValueChanged command binding [\#1269](https://github.com/MvvmCross/MvvmCross/pull/1269) ([Cheesebaron](https://github.com/Cheesebaron))
- Move shared fragment logic to shared lib [\#1268](https://github.com/MvvmCross/MvvmCross/pull/1268) ([martijn00](https://github.com/martijn00))

## [4.0.0](https://github.com/MvvmCross/MvvmCross/tree/4.0.0) (2016-02-02)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta8...4.0.0)

**Fixed bugs:**

- View namespace abbreviations not working in 3.5.2 alpha [\#1057](https://github.com/MvvmCross/MvvmCross/issues/1057)

**Closed issues:**

- MvxCommand\(async \(\) =\> await ...\) blocking UI [\#1215](https://github.com/MvvmCross/MvvmCross/issues/1215)
- Data Access Layer abstraction for MvVMCross [\#738](https://github.com/MvvmCross/MvvmCross/issues/738)
- Rework ShowViewModel to support preconditions [\#736](https://github.com/MvvmCross/MvvmCross/issues/736)
- Custom ViewDispatcher that wraps default ViewDispatcher cannot be used for navigation [\#542](https://github.com/MvvmCross/MvvmCross/issues/542)
- MvvmCross Starter 4 Beta 8 - LinkerPleaseInclude use C\# 6 String Interpolation [\#1221](https://github.com/MvvmCross/MvvmCross/issues/1221)
- Cleanup [\#1190](https://github.com/MvvmCross/MvvmCross/issues/1190)
- Plugins on UWP app tries to load the \*.WindowsUWP \(but almost none exist\) [\#1142](https://github.com/MvvmCross/MvvmCross/issues/1142)
- MvxWindowsStoreFileStore.EnsureFolderExists\(\) fails for absolute path [\#1104](https://github.com/MvvmCross/MvvmCross/issues/1104)
- Update nuspecs to support new Nuget configuration [\#1082](https://github.com/MvvmCross/MvvmCross/issues/1082)
- App crash on orientation change.  [\#1081](https://github.com/MvvmCross/MvvmCross/issues/1081)
- Nuget 3.5.2-alpha2 DLL mismatch problem [\#1080](https://github.com/MvvmCross/MvvmCross/issues/1080)
- Fix SQLite nuspecs [\#1072](https://github.com/MvvmCross/MvvmCross/issues/1072)
- Bring back FullFragging [\#1070](https://github.com/MvvmCross/MvvmCross/issues/1070)
- MvxExpandableList throws NullReferenceException [\#1061](https://github.com/MvvmCross/MvvmCross/issues/1061)
- MvxImageLoader / DownloadCache doesn't make images appear correctly in cells \(iOS\) [\#1058](https://github.com/MvvmCross/MvvmCross/issues/1058)
- Images never load first time using DownloadCache plugin [\#1056](https://github.com/MvvmCross/MvvmCross/issues/1056)
- Remove NuSpec dependency MvvmCross.HotTuna.CrossCore -\> PortableSupport [\#1053](https://github.com/MvvmCross/MvvmCross/issues/1053)
- Release new MvvmCross version [\#1052](https://github.com/MvvmCross/MvvmCross/issues/1052)
- File plugin: Getting "Cannot access the file... because it is being used by another process" while trying to READ it [\#1050](https://github.com/MvvmCross/MvvmCross/issues/1050)
- Using multiple MvxRadioGroup bindings in a view only causes the first to be bound [\#1045](https://github.com/MvvmCross/MvvmCross/issues/1045)
- iOS: Replacing an image using MvxImageViewLoader fails ... [\#1041](https://github.com/MvvmCross/MvvmCross/issues/1041)
- Update to latest JSON.Net [\#1037](https://github.com/MvvmCross/MvvmCross/issues/1037)
- MvxFragment error [\#1030](https://github.com/MvvmCross/MvvmCross/issues/1030)
- Android GroupItemTemplate instead of MvxGroupItemTemplate attribute for MvxExpandableListView [\#1029](https://github.com/MvvmCross/MvvmCross/issues/1029)
- ArgumentNullException with Email plugin [\#1025](https://github.com/MvvmCross/MvvmCross/issues/1025)
- How to share a ViewModel? Add the Fragment parameter to OnFragmentChanging\(\) event? [\#1015](https://github.com/MvvmCross/MvvmCross/issues/1015)
- Windows 10 UWP \(Universal Windows Platform\) apps  [\#1009](https://github.com/MvvmCross/MvvmCross/issues/1009)
- Warning issue around MvxAdapter code [\#1004](https://github.com/MvvmCross/MvvmCross/issues/1004)
- Alternate view inflation [\#1000](https://github.com/MvvmCross/MvvmCross/issues/1000)
- MvxModalNavSupportTouchViewPresenter constructor does not match IUIApplicationDelegate signature [\#995](https://github.com/MvvmCross/MvvmCross/issues/995)
- Fragging cannot inflate MvxFragment [\#994](https://github.com/MvvmCross/MvvmCross/issues/994)
- Installing AppCompat 22.1.1 causes inflation of Mvx classes to fail [\#981](https://github.com/MvvmCross/MvvmCross/issues/981)
- MvxCachingFragmentActivity.ShowFragment loading the same cached fragment even with a different bundle [\#966](https://github.com/MvvmCross/MvvmCross/issues/966)
- About the MvxWpfFileStore.cs [\#912](https://github.com/MvvmCross/MvvmCross/issues/912)
- MvvmCross SQLite.Net-PCL wrapper [\#899](https://github.com/MvvmCross/MvvmCross/issues/899)
- Consider adding OnLoseFocus Text binding for Android [\#894](https://github.com/MvvmCross/MvvmCross/issues/894)
- Possible need for more binding auto-conversion for new unified numeric types [\#878](https://github.com/MvvmCross/MvvmCross/issues/878)
- Windows Phone - MvxWindowsPage layout issues [\#868](https://github.com/MvvmCross/MvvmCross/issues/868)
- Mac and XamMac nugets [\#856](https://github.com/MvvmCross/MvvmCross/issues/856)
- Xamarin.Forms? [\#826](https://github.com/MvvmCross/MvvmCross/issues/826)
- Handling async work with MvxCommand [\#821](https://github.com/MvvmCross/MvvmCross/issues/821)
- Please Version DLLs [\#740](https://github.com/MvvmCross/MvvmCross/issues/740)
- Async File Plugin for WP and WinRT [\#714](https://github.com/MvvmCross/MvvmCross/issues/714)
- Xamarin.Forms integration [\#703](https://github.com/MvvmCross/MvvmCross/issues/703)
- Setting ViewModel property on a Fragment is not correct [\#636](https://github.com/MvvmCross/MvvmCross/issues/636)

**Merged pull requests:**

- Removed HotTuna nuspecs \(not needed anymore\). [\#1239](https://github.com/MvvmCross/MvvmCross/pull/1239) ([lothrop](https://github.com/lothrop))
- Also pack CodeAnalysis nuget [\#1229](https://github.com/MvvmCross/MvvmCross/pull/1229) ([Stephanvs](https://github.com/Stephanvs))
- Missing changes for adapter long-life subscription sub-issue on \#1217 [\#1225](https://github.com/MvvmCross/MvvmCross/pull/1225) ([roubachof](https://github.com/roubachof))
- Adapter long-life subscription sub-issue on \#1217 [\#1220](https://github.com/MvvmCross/MvvmCross/pull/1220) ([slodge](https://github.com/slodge))
- Update titles for nuget to new names [\#1216](https://github.com/MvvmCross/MvvmCross/pull/1216) ([martijn00](https://github.com/martijn00))
- Copy support fragments to full fragging [\#1214](https://github.com/MvvmCross/MvvmCross/pull/1214) ([martijn00](https://github.com/martijn00))
- Made MvvmCross.StarterPack pull in MvvmCross, not the other way around. [\#1213](https://github.com/MvvmCross/MvvmCross/pull/1213) ([lothrop](https://github.com/lothrop))
- Added dummy packages to ease migration from HotTuna. [\#1211](https://github.com/MvvmCross/MvvmCross/pull/1211) ([lothrop](https://github.com/lothrop))
- Got nuspecs working for all platforms. [\#1210](https://github.com/MvvmCross/MvvmCross/pull/1210) ([lothrop](https://github.com/lothrop))
- Nugets for Mac [\#1209](https://github.com/MvvmCross/MvvmCross/pull/1209) ([lothrop](https://github.com/lothrop))
- Fixes to get pack.ps1 to run again. [\#1207](https://github.com/MvvmCross/MvvmCross/pull/1207) ([lothrop](https://github.com/lothrop))
- More cleanup of Touch and Cirrious. [\#1206](https://github.com/MvvmCross/MvvmCross/pull/1206) ([lothrop](https://github.com/lothrop))
- Fixed templates to match new naming structure. [\#1205](https://github.com/MvvmCross/MvvmCross/pull/1205) ([lothrop](https://github.com/lothrop))
- Rename all occurrences of "Touch" to "iOS" [\#1204](https://github.com/MvvmCross/MvvmCross/pull/1204) ([lothrop](https://github.com/lothrop))
- Adapt nuspecs to new structure [\#1203](https://github.com/MvvmCross/MvvmCross/pull/1203) ([lothrop](https://github.com/lothrop))
- Fixed usings in BindingEx. [\#1202](https://github.com/MvvmCross/MvvmCross/pull/1202) ([lothrop](https://github.com/lothrop))
- Remove unused/outdated scripts and updated packaging script. [\#1201](https://github.com/MvvmCross/MvvmCross/pull/1201) ([lothrop](https://github.com/lothrop))
- Restructure and cleanup of projects and files [\#1200](https://github.com/MvvmCross/MvvmCross/pull/1200) ([martijn00](https://github.com/martijn00))
- Windows Store and Windows Phone [\#1199](https://github.com/MvvmCross/MvvmCross/pull/1199) ([clintonrocksmith](https://github.com/clintonrocksmith))
- Rework of Mac implementation. [\#1198](https://github.com/MvvmCross/MvvmCross/pull/1198) ([lothrop](https://github.com/lothrop))
- Use XIB and auto layouts for iOS Starter Pack example [\#1196](https://github.com/MvvmCross/MvvmCross/pull/1196) ([lothrop](https://github.com/lothrop))
- Restructure [\#1195](https://github.com/MvvmCross/MvvmCross/pull/1195) ([clintonrocksmith](https://github.com/clintonrocksmith))
- Introduce non-support-dependent version of MvxPreferenceFragment [\#1189](https://github.com/MvvmCross/MvvmCross/pull/1189) ([mgj](https://github.com/mgj))
- Fix merge of PR 1186 [\#1188](https://github.com/MvvmCross/MvvmCross/pull/1188) ([dbeattie71](https://github.com/dbeattie71))
- Moved caching logic from support library. [\#1186](https://github.com/MvvmCross/MvvmCross/pull/1186) ([thefex](https://github.com/thefex))
- Cleanup of code using C\#6 [\#1182](https://github.com/MvvmCross/MvvmCross/pull/1182) ([martijn00](https://github.com/martijn00))
- Add better description, update version, add tags [\#1180](https://github.com/MvvmCross/MvvmCross/pull/1180) ([martijn00](https://github.com/martijn00))
- 4.0 android search view custom binding [\#1179](https://github.com/MvvmCross/MvvmCross/pull/1179) ([mgj](https://github.com/mgj))
- MultipleViewModel cache was not prepared for multiple instances of ViewModel [\#1178](https://github.com/MvvmCross/MvvmCross/pull/1178) ([thefex](https://github.com/thefex))
- Integrated new MvxUIActivityIndicatorViewHiddenTargetBinding from munkii. [\#1176](https://github.com/MvvmCross/MvvmCross/pull/1176) ([lothrop](https://github.com/lothrop))
- Fix some nuspecs and files [\#1175](https://github.com/MvvmCross/MvvmCross/pull/1175) ([martijn00](https://github.com/martijn00))
- Removed broken solution file [\#1170](https://github.com/MvvmCross/MvvmCross/pull/1170) ([Stephanvs](https://github.com/Stephanvs))
- Fix Mac presenter [\#1169](https://github.com/MvvmCross/MvvmCross/pull/1169) ([martijn00](https://github.com/martijn00))
- Bump Android target version to Android lvl 21 [\#1168](https://github.com/MvvmCross/MvvmCross/pull/1168) ([martijn00](https://github.com/martijn00))
- Improved ViewModelLoader and ViewModelLocator with viewmodel reloading [\#1165](https://github.com/MvvmCross/MvvmCross/pull/1165) ([vvolkgang](https://github.com/vvolkgang))
- Update logo and version numbers [\#1158](https://github.com/MvvmCross/MvvmCross/pull/1158) ([martijn00](https://github.com/martijn00))
- Add MvxWindowsMultiRegionPresenter [\#1157](https://github.com/MvvmCross/MvvmCross/pull/1157) ([Stephanvs](https://github.com/Stephanvs))
- Discover CanExecuteXyz properties in ancestors. [\#1150](https://github.com/MvvmCross/MvvmCross/pull/1150) ([dbeattie71](https://github.com/dbeattie71))
- Change so WindowsCommon are valid plugins for UWP [\#1146](https://github.com/MvvmCross/MvvmCross/pull/1146) ([azchohfi](https://github.com/azchohfi))
- Added Android specific MvxPropertyChangedListener [\#1145](https://github.com/MvvmCross/MvvmCross/pull/1145) ([jamie94bc](https://github.com/jamie94bc))
- Added GetFragmentInfoByTag to MvxCachingFragmentActivity [\#1143](https://github.com/MvvmCross/MvvmCross/pull/1143) ([mattwhetton](https://github.com/mattwhetton))
- Fixed unified value types conversion when binding to a nfloat, nint or uint from a managed equivalent value type. [\#1131](https://github.com/MvvmCross/MvvmCross/pull/1131) ([ggirard07](https://github.com/ggirard07))
- Moved plugins to plugins repo [\#1129](https://github.com/MvvmCross/MvvmCross/pull/1129) ([martijn00](https://github.com/martijn00))
- Convert full paths to relative in EnsureFolderExists\(...\) \(Issue 1104\) [\#1128](https://github.com/MvvmCross/MvvmCross/pull/1128) ([tekkies](https://github.com/tekkies))
- Fix SelectionChangedCommand CanExecute not being called [\#1126](https://github.com/MvvmCross/MvvmCross/pull/1126) ([charri](https://github.com/charri))
- Fix nunit path in plugins [\#1125](https://github.com/MvvmCross/MvvmCross/pull/1125) ([tekkies](https://github.com/tekkies))
- Cleanup of repo [\#1122](https://github.com/MvvmCross/MvvmCross/pull/1122) ([martijn00](https://github.com/martijn00))
- Add initial support for a MvxPageViewController that dynamically crea… [\#1121](https://github.com/MvvmCross/MvvmCross/pull/1121) ([scastria](https://github.com/scastria))
- It's recommended to have at least the `analyzers` tag in your nuget  [\#1119](https://github.com/MvvmCross/MvvmCross/pull/1119) ([Stephanvs](https://github.com/Stephanvs))
- Nuspec fixes for Code Analysis [\#1118](https://github.com/MvvmCross/MvvmCross/pull/1118) ([martijn00](https://github.com/martijn00))
- Add CodeAnalysis Project with Roslyn Code Analyzer [\#1117](https://github.com/MvvmCross/MvvmCross/pull/1117) ([Stephanvs](https://github.com/Stephanvs))
- Removed unnecessary permissions [\#1116](https://github.com/MvvmCross/MvvmCross/pull/1116) ([martijn00](https://github.com/martijn00))
- Fixed it \(properly\) for 'Release' mode. Should work if Release\Any CPU [\#1110](https://github.com/MvvmCross/MvvmCross/pull/1110) ([krishna-nadiminti](https://github.com/krishna-nadiminti))
- Generate lib layout to fix broken rd.xml reference in nupkg for UWP [\#1109](https://github.com/MvvmCross/MvvmCross/pull/1109) ([krishna-nadiminti](https://github.com/krishna-nadiminti))
- Adding Cirrious.MvvmCross.WindowsUWP.rd.xml in nuspec. [\#1103](https://github.com/MvvmCross/MvvmCross/pull/1103) ([michaeldaw](https://github.com/michaeldaw))
- Return full paths from MvxWindowsCommonBlockingFileStore.GetFilesIn\(...\) [\#1102](https://github.com/MvvmCross/MvvmCross/pull/1102) ([tekkies](https://github.com/tekkies))
- Added UWP project [\#1101](https://github.com/MvvmCross/MvvmCross/pull/1101) ([martijn00](https://github.com/martijn00))
- Added new presentation hint handling to console [\#1100](https://github.com/MvvmCross/MvvmCross/pull/1100) ([jamie94bc](https://github.com/jamie94bc))
- A nicer way to handle handling multiple presentation hints [\#1097](https://github.com/MvvmCross/MvvmCross/pull/1097) ([jamie94bc](https://github.com/jamie94bc))
- Version dlls and set copyright to 2015 [\#1095](https://github.com/MvvmCross/MvvmCross/pull/1095) ([martijn00](https://github.com/martijn00))
- Added Text focus code for Android and iOS [\#1094](https://github.com/MvvmCross/MvvmCross/pull/1094) ([martijn00](https://github.com/martijn00))
- Rename UIApplicationDelegate to IUIApplicationDelegate [\#1093](https://github.com/MvvmCross/MvvmCross/pull/1093) ([martijn00](https://github.com/martijn00))
- Improve trace message for MvxAdapter [\#1092](https://github.com/MvvmCross/MvvmCross/pull/1092) ([martijn00](https://github.com/martijn00))
- Remove abstract from MvxStorePage [\#1091](https://github.com/MvvmCross/MvvmCross/pull/1091) ([martijn00](https://github.com/martijn00))
- Cleanup and better gitignore [\#1090](https://github.com/MvvmCross/MvvmCross/pull/1090) ([martijn00](https://github.com/martijn00))
- Added dotnet files, updated version [\#1089](https://github.com/MvvmCross/MvvmCross/pull/1089) ([martijn00](https://github.com/martijn00))
- Fixed GetSizeInBytes\(\) to proper handle memory cache limits for the Images [\#1087](https://github.com/MvvmCross/MvvmCross/pull/1087) ([IlSocio](https://github.com/IlSocio))
- Force replace a fragment with the same viewmodel at the same contentid [\#1085](https://github.com/MvvmCross/MvvmCross/pull/1085) ([martijn00](https://github.com/martijn00))
- Update nuspecs to work with VS2015 [\#1084](https://github.com/MvvmCross/MvvmCross/pull/1084) ([martijn00](https://github.com/martijn00))
- Back stack fix [\#1079](https://github.com/MvvmCross/MvvmCross/pull/1079) ([martijn00](https://github.com/martijn00))
- Pass the java handle down to the base constructor to avoid a potentia… [\#1074](https://github.com/MvvmCross/MvvmCross/pull/1074) ([kjeremy](https://github.com/kjeremy))
- MvxAndroidViewFactory: Remove error message [\#1073](https://github.com/MvvmCross/MvvmCross/pull/1073) ([kjeremy](https://github.com/kjeremy))
- Added back FullFragging [\#1071](https://github.com/MvvmCross/MvvmCross/pull/1071) ([martijn00](https://github.com/martijn00))
- Fixed crash if same picture is added twice [\#1068](https://github.com/MvvmCross/MvvmCross/pull/1068) ([PelleRavn](https://github.com/PelleRavn))
- Network Plugin: fixed disposing response stream after finishing successAction [\#1065](https://github.com/MvvmCross/MvvmCross/pull/1065) ([SButterfly](https://github.com/SButterfly))
- Fix custom namespace inflation if a factory is not installed [\#1064](https://github.com/MvvmCross/MvvmCross/pull/1064) ([kjeremy](https://github.com/kjeremy))
- Android: make methods virtual in MvxBindingLayoutInflaterFactory [\#1063](https://github.com/MvvmCross/MvvmCross/pull/1063) ([gshackles](https://github.com/gshackles))
- Improved MvxExpandableListAdapter implementation [\#1062](https://github.com/MvvmCross/MvvmCross/pull/1062) ([jamie94bc](https://github.com/jamie94bc))
- Fix download cache [\#1060](https://github.com/MvvmCross/MvvmCross/pull/1060) ([Cheesebaron](https://github.com/Cheesebaron))
- Made signature of RaiseAndSetIfChanged method more friendly [\#1059](https://github.com/MvvmCross/MvvmCross/pull/1059) ([kolesnick](https://github.com/kolesnick))
- Nuke dependency HotTuna.CrossCore -\> PortableSupport [\#1054](https://github.com/MvvmCross/MvvmCross/pull/1054) ([PeterBurke](https://github.com/PeterBurke))
- Fix for https://github.com/MvvmCross/MvvmCross/issues/1050.  [\#1051](https://github.com/MvvmCross/MvvmCross/pull/1051) ([JanZeman](https://github.com/JanZeman))
- Removed Fragging and FullFragging [\#1049](https://github.com/MvvmCross/MvvmCross/pull/1049) ([martijn00](https://github.com/martijn00))
- Prevent view id collision when generating RadioButton ids [\#1048](https://github.com/MvvmCross/MvvmCross/pull/1048) ([kjeremy](https://github.com/kjeremy))
- Save the original IMvxLayoutInflaterHolderFactory when inflating views and restore at the end [\#1047](https://github.com/MvvmCross/MvvmCross/pull/1047) ([kjeremy](https://github.com/kjeremy))
- Add javaReference/transfer constructors to fragments [\#1043](https://github.com/MvvmCross/MvvmCross/pull/1043) ([andyci](https://github.com/andyci))
- Properly inflate android.widget classes [\#1035](https://github.com/MvvmCross/MvvmCross/pull/1035) ([kjeremy](https://github.com/kjeremy))
- Renaming "GroupItemTemplate" android attribute to "MvxGroupItemTemplate" [\#1031](https://github.com/MvvmCross/MvvmCross/pull/1031) ([ljeancler](https://github.com/ljeancler))
- Merge pull request \#996 into 3.5 [\#1028](https://github.com/MvvmCross/MvvmCross/pull/1028) ([martijn00](https://github.com/martijn00))
- Fixes Casting for SelectedElement on UISegmentedControl [\#1027](https://github.com/MvvmCross/MvvmCross/pull/1027) ([aspnetde](https://github.com/aspnetde))
- Inflation fixes final [\#1022](https://github.com/MvvmCross/MvvmCross/pull/1022) ([kjeremy](https://github.com/kjeremy))
- Added dialogTitle for Android Chooser. Short XML doc and attachment logic cleanup [\#1019](https://github.com/MvvmCross/MvvmCross/pull/1019) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixed queuing unindexed files [\#1017](https://github.com/MvvmCross/MvvmCross/pull/1017) ([mierzynskim](https://github.com/mierzynskim))
- Build in MvxBindingAttributes.xml + TextFormatted binding [\#1013](https://github.com/MvvmCross/MvvmCross/pull/1013) ([Cheesebaron](https://github.com/Cheesebaron))
- Adding Close Hint support for MvxConsole [\#1012](https://github.com/MvvmCross/MvvmCross/pull/1012) ([PaulLeman](https://github.com/PaulLeman))
- Generic MvxCachingFragmentActivity [\#1007](https://github.com/MvvmCross/MvvmCross/pull/1007) ([martijn00](https://github.com/martijn00))
- 3.5 downloadcache all async [\#1005](https://github.com/MvvmCross/MvvmCross/pull/1005) ([gentledepp](https://github.com/gentledepp))
- Load local images asynchronously and scale them down \(android\) [\#1003](https://github.com/MvvmCross/MvvmCross/pull/1003) ([gentledepp](https://github.com/gentledepp))
- Added a ctor to Setup that takes a presenter. [\#998](https://github.com/MvvmCross/MvvmCross/pull/998) ([jfoshee](https://github.com/jfoshee))
- Handling async work with MvxCommand [\#997](https://github.com/MvvmCross/MvvmCross/pull/997) ([guillaume-fr](https://github.com/guillaume-fr))
- Add override to determine if the requested fragment [\#996](https://github.com/MvvmCross/MvvmCross/pull/996) ([andyci](https://github.com/andyci))
- FileStore is async with CancellationToken support on all platforms [\#993](https://github.com/MvvmCross/MvvmCross/pull/993) ([jonstoneman](https://github.com/jonstoneman))
- Configurable root folder for WPF file store [\#978](https://github.com/MvvmCross/MvvmCross/pull/978) ([guillaume-fr](https://github.com/guillaume-fr))
- CanExecuteChanged thread safe an raisable on UI thread [\#976](https://github.com/MvvmCross/MvvmCross/pull/976) ([guillaume-fr](https://github.com/guillaume-fr))
- Implement async file for windows and optimize WriteFile [\#975](https://github.com/MvvmCross/MvvmCross/pull/975) ([guillaume-fr](https://github.com/guillaume-fr))
- DeselectRow support in base UITableViewSource [\#941](https://github.com/MvvmCross/MvvmCross/pull/941) ([alexsorokoletov](https://github.com/alexsorokoletov))
- Location plugin provides event to track permission now [\#898](https://github.com/MvvmCross/MvvmCross/pull/898) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- Fixed Bug where MvxCommand Parameter could not be parsed / casted, when ... [\#842](https://github.com/MvvmCross/MvvmCross/pull/842) ([crazyfx1](https://github.com/crazyfx1))

## [4.0.0-beta8](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta8) (2016-01-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta7...4.0.0-beta8)

**Closed issues:**

- MvxViewModel Close\(this\) crashes [\#1194](https://github.com/MvvmCross/MvvmCross/issues/1194)
- Question: Tear down application programatically for Component/Integrationtests? [\#1191](https://github.com/MvvmCross/MvvmCross/issues/1191)

## [4.0.0-beta7](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta7) (2015-12-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta5...4.0.0-beta7)

**Fixed bugs:**

- duplicate references to System.IO,System.Runtime and System.Threading [\#875](https://github.com/MvvmCross/MvvmCross/issues/875)

**Closed issues:**

- mvvcross4.0-beta5: Binary XML file line \#1: Error inflating class Mvx.MvxListView [\#1187](https://github.com/MvvmCross/MvvmCross/issues/1187)
- 4.0 beta 5 - Cirrious.MvvmCross.Binding.Droid is not referenced/does not exist [\#1185](https://github.com/MvvmCross/MvvmCross/issues/1185)
- Problem creating target binding for CompoundButton [\#1174](https://github.com/MvvmCross/MvvmCross/issues/1174)
- Android - ViewType not found when using fully qualified name in .axml file [\#1173](https://github.com/MvvmCross/MvvmCross/issues/1173)
- Picture Chooser returns wrong Image Size on iPhone [\#1172](https://github.com/MvvmCross/MvvmCross/issues/1172)
- cannot create an UWP app in VS2015 [\#1171](https://github.com/MvvmCross/MvvmCross/issues/1171)
- UWP TipCalc tutorial [\#1161](https://github.com/MvvmCross/MvvmCross/issues/1161)
- Make MvxOwnedViewModelFragment Opt-out instead of in [\#1138](https://github.com/MvvmCross/MvvmCross/issues/1138)
- Use Same Bundle for InitFromBundle and SaveStateToBundle [\#949](https://github.com/MvvmCross/MvvmCross/issues/949)

## [4.0.0-beta5](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta5) (2015-11-06)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta4...4.0.0-beta5)

**Closed issues:**

- Documentation Wrong step for android setup: adding an xml file defining the binding attributes. [\#1144](https://github.com/MvvmCross/MvvmCross/issues/1144)
- I would like to see these updates applied to the MT.Dialog fork [\#972](https://github.com/MvvmCross/MvvmCross/issues/972)
- How to go about creating a binding library for a new platform? [\#612](https://github.com/MvvmCross/MvvmCross/issues/612)
- Do Plugins need a new home? [\#462](https://github.com/MvvmCross/MvvmCross/issues/462)

## [4.0.0-beta4](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta4) (2015-10-20)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta3...4.0.0-beta4)

**Closed issues:**

- Problem to link assemblies of MvxFileDownloadCache with iOS 9 SDK [\#1135](https://github.com/MvvmCross/MvvmCross/issues/1135)
- new MvxFragmentStatePagerAdapter.FragmentInfo doesn't call Start\(\) on the ViewModel when instantiated [\#1133](https://github.com/MvvmCross/MvvmCross/issues/1133)
- Nuget Install Error VS2015, Windows10, PCL, Uwp, Droid and iOS Project [\#1130](https://github.com/MvvmCross/MvvmCross/issues/1130)
- Android EditText binding failure [\#1114](https://github.com/MvvmCross/MvvmCross/issues/1114)
- Windows Phone 8.1 back navigation issue [\#1018](https://github.com/MvvmCross/MvvmCross/issues/1018)

## [4.0.0-beta3](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta3) (2015-09-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta2...4.0.0-beta3)

**Fixed bugs:**

- MvxWindowsCommonBlockingFileStore.GetFilesIn\(path\) output does not include full path [\#1086](https://github.com/MvvmCross/MvvmCross/issues/1086)

**Closed issues:**

- MvxListView binding doesn't work [\#1120](https://github.com/MvvmCross/MvvmCross/issues/1120)
- Unnecessary permissions exist in Android FullFragging AssemblyInfo.cs [\#1115](https://github.com/MvvmCross/MvvmCross/issues/1115)
- VS2015 NuGet Installation Failure for Xamarin.Android project: MvvmCross.PortableSupport 3.5.1. [\#1113](https://github.com/MvvmCross/MvvmCross/issues/1113)
- Another nuget issue with portable class library %2B [\#1106](https://github.com/MvvmCross/MvvmCross/issues/1106)
- onTrimMemory is used only for API level 14 , I need to detect when app is backgrounded for api level \<14  [\#1105](https://github.com/MvvmCross/MvvmCross/issues/1105)
- Error installing MvvmCross.HotTuna.MvvmCrossLibraries v4.0.0-beta1 in Xamarin Studio [\#1088](https://github.com/MvvmCross/MvvmCross/issues/1088)
- MvxAndroidSetupSingleton crashes on splash screen with SIGSEGV [\#1069](https://github.com/MvvmCross/MvvmCross/issues/1069)
- MvvmCross as a Code-Aware Library with Roslyn Code Analyzers [\#1040](https://github.com/MvvmCross/MvvmCross/issues/1040)
- Android SIGSEGV SEGV\_MAPERR on Samsung Galaxy S5 with DownloadCache-v3.5.1 [\#1036](https://github.com/MvvmCross/MvvmCross/issues/1036)

## [4.0.0-beta2](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta2) (2015-08-18)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-beta1...4.0.0-beta2)

**Fixed bugs:**

- Droid Button click binding affects Enabled property [\#729](https://github.com/MvvmCross/MvvmCross/issues/729)
- CommandParameter binding expression [\#704](https://github.com/MvvmCross/MvvmCross/issues/704)

**Closed issues:**

- Manually mapping views with viewmodels [\#1083](https://github.com/MvvmCross/MvvmCross/issues/1083)
- Could not load signature of Cirrious.CrossCore.Droid.Views.MvxEventSourceActivity [\#1076](https://github.com/MvvmCross/MvvmCross/issues/1076)
- Fatal signal 11 \(SIGSEGV\) during startup [\#1067](https://github.com/MvvmCross/MvvmCross/issues/1067)
- MvvmCross support for Windows 10 apps\(UWP\) [\#1066](https://github.com/MvvmCross/MvvmCross/issues/1066)
- Can't add nuget package 3.5.1 into project targets 'MonoAndroid, Version=v5.0' [\#1001](https://github.com/MvvmCross/MvvmCross/issues/1001)
- Can't add Microsoft HTTP Client Libraries to a project with MvvmCross 3.5.0-beta2 [\#866](https://github.com/MvvmCross/MvvmCross/issues/866)
- Picture chooser does not work, 3.2.1 WinRT\(8.1\) Cirrious.MvvmCross.Plugins.PictureChooser.WindowsCommon [\#810](https://github.com/MvvmCross/MvvmCross/issues/810)
- New Relic [\#723](https://github.com/MvvmCross/MvvmCross/issues/723)
- adding getFoldersIn to File Plugin [\#721](https://github.com/MvvmCross/MvvmCross/issues/721)
- Adding Universal App support [\#656](https://github.com/MvvmCross/MvvmCross/issues/656)
- Nested MvxListView-MvxLinearCollection crashes with System.InvalidOperationException [\#645](https://github.com/MvvmCross/MvvmCross/issues/645)
- Location Plugin missing for WPF project. [\#630](https://github.com/MvvmCross/MvvmCross/issues/630)
- ".nuspec" folder instead of "nuspec"? [\#514](https://github.com/MvvmCross/MvvmCross/issues/514)

## [4.0.0-beta1](https://github.com/MvvmCross/MvvmCross/tree/4.0.0-beta1) (2015-07-31)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/4.0.0-alpha9...4.0.0-beta1)

**Closed issues:**

- Windows Phone 8.1 ViewModel Property is Null [\#1078](https://github.com/MvvmCross/MvvmCross/issues/1078)

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

**Closed issues:**

- Upgrading MvvmCross via NuGet sometimes wants to overwrite files [\#790](https://github.com/MvvmCross/MvvmCross/issues/790)
- MvvmCross on Silverlight 5 ? [\#681](https://github.com/MvvmCross/MvvmCross/issues/681)

## [3.5.2-alpha2](https://github.com/MvvmCross/MvvmCross/tree/3.5.2-alpha2) (2015-06-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.2-alpha1...3.5.2-alpha2)

**Fixed bugs:**

- ChoosePictureFromLibrary does not invoke OnPicture Action in WP8.1 [\#1039](https://github.com/MvvmCross/MvvmCross/issues/1039)

**Closed issues:**

- Use of First\(\) in Get\<T\> [\#1038](https://github.com/MvvmCross/MvvmCross/issues/1038)
- Improve Droid Email plugin Intent handling [\#574](https://github.com/MvvmCross/MvvmCross/issues/574)

## [3.5.2-alpha1](https://github.com/MvvmCross/MvvmCross/tree/3.5.2-alpha1) (2015-06-16)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.1...3.5.2-alpha1)

**Fixed bugs:**

- RaiseCanExecuteChanged is not marshalled to UI thread automatically like RaisePropertyChanged  [\#954](https://github.com/MvvmCross/MvvmCross/issues/954)

**Closed issues:**

- NuGet unable to install MvvmCross.HotTuna.MvvmCrossLibraries in VS2015 RC [\#1023](https://github.com/MvvmCross/MvvmCross/issues/1023)
- SaveState and RestoreState not being called for iOS applications   [\#1016](https://github.com/MvvmCross/MvvmCross/issues/1016)
- App size is too large [\#1014](https://github.com/MvvmCross/MvvmCross/issues/1014)
- SQLiteNet can't handle ConvertChecked [\#1008](https://github.com/MvvmCross/MvvmCross/issues/1008)
- Does SubscribeOnMainThread enables the subscriber to receive messages published on the main thread? [\#999](https://github.com/MvvmCross/MvvmCross/issues/999)
- MvxOwnerViewModelFragment missin [\#992](https://github.com/MvvmCross/MvvmCross/issues/992)
- \[Feature Request\] MvxRequestedBy has more useful property [\#968](https://github.com/MvvmCross/MvvmCross/issues/968)
- Move "ResourcesToCopy" attributes into the droid binding library [\#742](https://github.com/MvvmCross/MvvmCross/issues/742)
- Need Strong Names for assemblies [\#232](https://github.com/MvvmCross/MvvmCross/issues/232)
- Can resources now be embedded in class libraries? [\#112](https://github.com/MvvmCross/MvvmCross/issues/112)

**Merged pull requests:**

- Navigation cache logic for WP8.1 [\#760](https://github.com/MvvmCross/MvvmCross/pull/760) ([promontis](https://github.com/promontis))

## [3.5.1](https://github.com/MvvmCross/MvvmCross/tree/3.5.1) (2015-05-02)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.51-beta1...3.5.1)

**Fixed bugs:**

- Explicitly use `\[Register\]` flags on all classes referencable from AXML [\#958](https://github.com/MvvmCross/MvvmCross/issues/958)
- Not possible to use MvxCachingFragmentActivity with ActionBarActivity [\#887](https://github.com/MvvmCross/MvvmCross/issues/887)

**Closed issues:**

- Can't add MvvmCross Nuget package to portable class library [\#970](https://github.com/MvvmCross/MvvmCross/issues/970)
- Q: How old is the MT.D fork inside this codebase? [\#969](https://github.com/MvvmCross/MvvmCross/issues/969)
- Visible view converter doesn't work on \<include\> tag [\#961](https://github.com/MvvmCross/MvvmCross/issues/961)
- Factoring out MvxNotifyPropertyChanged [\#959](https://github.com/MvvmCross/MvvmCross/issues/959)
- Unable to install mvvmcross using nuget in vs2015 ctp6 [\#951](https://github.com/MvvmCross/MvvmCross/issues/951)
- Command execute many times [\#948](https://github.com/MvvmCross/MvvmCross/issues/948)
- After updating an existing app to the unified API in Xamarin, Failed to resolve "System.Boolean [\#946](https://github.com/MvvmCross/MvvmCross/issues/946)
- MvxAutoComplete quits working when you backspace until the TextView is empty. [\#945](https://github.com/MvvmCross/MvvmCross/issues/945)
- Bug with MvxSpinner when used in an MvxItemList [\#944](https://github.com/MvvmCross/MvvmCross/issues/944)
- about MvvmCross 3.5.0 in Visual Studio 2015 errorlist [\#943](https://github.com/MvvmCross/MvvmCross/issues/943)
- ShowViewModel Twice when i click fast [\#942](https://github.com/MvvmCross/MvvmCross/issues/942)
- FloatElement not properly styled [\#932](https://github.com/MvvmCross/MvvmCross/issues/932)
- MvxCommand constructor exception [\#930](https://github.com/MvvmCross/MvvmCross/issues/930)
- Presenter for Tabbar based on MvxModalNavSupportTouchViewPresenter not possible [\#927](https://github.com/MvvmCross/MvvmCross/issues/927)
- listview.PointToPosition not returning the correct values [\#926](https://github.com/MvvmCross/MvvmCross/issues/926)
- Xamarin.Android.Support.v4 version 22.1.1.1 seems to break Fragment data binding [\#990](https://github.com/MvvmCross/MvvmCross/issues/990)
- 3.5.1 Checklist [\#989](https://github.com/MvvmCross/MvvmCross/issues/989)
- Xamarin.Android 5.1 changes how classes are named in Java, breaking bindings in AXML [\#987](https://github.com/MvvmCross/MvvmCross/issues/987)
- MvxFragment fails to get instantiated with Xamarin.Android 5.1 [\#984](https://github.com/MvvmCross/MvvmCross/issues/984)
- Add License headers to Fragging stuff [\#950](https://github.com/MvvmCross/MvvmCross/issues/950)
- Missing constructors \(JniHandleOwnership, mono\) [\#928](https://github.com/MvvmCross/MvvmCross/issues/928)
- Fragment regression since 3.5 [\#910](https://github.com/MvvmCross/MvvmCross/issues/910)
- Change Seekbar subscriber to use an event \(not a listener\) [\#884](https://github.com/MvvmCross/MvvmCross/issues/884)
- Publish symbols packages [\#711](https://github.com/MvvmCross/MvvmCross/issues/711)
- Add ability to clear the Download cache [\#499](https://github.com/MvvmCross/MvvmCross/issues/499)

**Merged pull requests:**

- Reimplement LayoutInflaterCompat in MvvmCross. [\#988](https://github.com/MvvmCross/MvvmCross/pull/988) ([kjeremy](https://github.com/kjeremy))
- Migrate to Automatic Package Restore [\#985](https://github.com/MvvmCross/MvvmCross/pull/985) ([kjeremy](https://github.com/kjeremy))
- Avoid ArgumentOutOfRangeException when ItemSource is empty [\#982](https://github.com/MvvmCross/MvvmCross/pull/982) ([mohibsheth](https://github.com/mohibsheth))
- ability to clear the Download cache [\#980](https://github.com/MvvmCross/MvvmCross/pull/980) ([guillaume-fr](https://github.com/guillaume-fr))
- EnsureFolderExists support nested folders on Windows [\#974](https://github.com/MvvmCross/MvvmCross/pull/974) ([guillaume-fr](https://github.com/guillaume-fr))
- Avoid loading resource when resourceId == 0 in MvxImageViewDrawableTargetBinding. [\#971](https://github.com/MvvmCross/MvvmCross/pull/971) ([danielcweber](https://github.com/danielcweber))
- Android ACW naming [\#965](https://github.com/MvvmCross/MvvmCross/pull/965) ([martijn00](https://github.com/martijn00))
- Added licenses for fragment files [\#964](https://github.com/MvvmCross/MvvmCross/pull/964) ([martijn00](https://github.com/martijn00))
- Add missing OnSaveInstanceState override in MvxEventSource\[Dialog|List\]Fragment [\#963](https://github.com/MvvmCross/MvvmCross/pull/963) ([nextmunich](https://github.com/nextmunich))
- Lambda is replaced by action call [\#960](https://github.com/MvvmCross/MvvmCross/pull/960) ([evnik](https://github.com/evnik))
- Changed Cirrious.MvvmCross.Droid.FullFragging to use API 15 [\#956](https://github.com/MvvmCross/MvvmCross/pull/956) ([jihlee](https://github.com/jihlee))
- MvxAndroidLocalFileImageLoader: implemented mem-cache [\#947](https://github.com/MvvmCross/MvvmCross/pull/947) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- Fix for \#932 [\#940](https://github.com/MvvmCross/MvvmCross/pull/940) ([Cheesebaron](https://github.com/Cheesebaron))
- Update Droid MvxComposeEmailTask \(GMail-App 5, Multiple File Attachments\) [\#939](https://github.com/MvvmCross/MvvmCross/pull/939) ([Dragon160](https://github.com/Dragon160))
- Support open generic types in MvxBindingNameRegistry [\#938](https://github.com/MvvmCross/MvvmCross/pull/938) ([ckimes89](https://github.com/ckimes89))
- Fixed passing of some tests on systems with non-english culture [\#935](https://github.com/MvvmCross/MvvmCross/pull/935) ([evnik](https://github.com/evnik))
- MvxDynamicImageHelper not reset when recycling view/cell during scroll [\#934](https://github.com/MvvmCross/MvvmCross/pull/934) ([PaulLeman](https://github.com/PaulLeman))
- Missing mono-related jni constructors [\#929](https://github.com/MvvmCross/MvvmCross/pull/929) ([JanZeman](https://github.com/JanZeman))
- I made changes to MvxTouchSetup and MvxTouchDialogSetup to take a new in... [\#920](https://github.com/MvvmCross/MvvmCross/pull/920) ([Bowman74](https://github.com/Bowman74))
- Unified nuspec [\#918](https://github.com/MvvmCross/MvvmCross/pull/918) ([tofutim](https://github.com/tofutim))
- Added support for storyboard views [\#916](https://github.com/MvvmCross/MvvmCross/pull/916) ([geirsagberg](https://github.com/geirsagberg))
- Require opt-in for Fragments' ViewModel create/save/restore lifecycle being owned by MvvmCross. [\#915](https://github.com/MvvmCross/MvvmCross/pull/915) ([davidschwegler](https://github.com/davidschwegler))
- Adding nuspec for Console [\#914](https://github.com/MvvmCross/MvvmCross/pull/914) ([tofutim](https://github.com/tofutim))
- Unified enum-compatible segmented control with default binding [\#913](https://github.com/MvvmCross/MvvmCross/pull/913) ([TheAlmightyBob](https://github.com/TheAlmightyBob))
- Set GroupTemplateId on MvxExpandableListView [\#911](https://github.com/MvvmCross/MvvmCross/pull/911) ([kjeremy](https://github.com/kjeremy))
- Fix crash caused by the MvvmCross environment not being initialized yet ... [\#909](https://github.com/MvvmCross/MvvmCross/pull/909) ([davidschwegler](https://github.com/davidschwegler))
- SeekBar binding to use event subscription instead of on change listener [\#905](https://github.com/MvvmCross/MvvmCross/pull/905) ([tom-pratt](https://github.com/tom-pratt))
- MvxExpandableListView, adapter and selected item binding [\#903](https://github.com/MvvmCross/MvvmCross/pull/903) ([kjeremy](https://github.com/kjeremy))
- Added virtual IocOptions property [\#897](https://github.com/MvvmCross/MvvmCross/pull/897) ([geirsagberg](https://github.com/geirsagberg))
- Make presenter serializer protected so it can be used when inheriting MvxFragmentsPresenter [\#895](https://github.com/MvvmCross/MvvmCross/pull/895) ([martijn00](https://github.com/martijn00))
- Added protected constructor to MvxFrameControl... [\#893](https://github.com/MvvmCross/MvvmCross/pull/893) ([azchohfi](https://github.com/azchohfi))
- Add source files to nuget symbol packages [\#889](https://github.com/MvvmCross/MvvmCross/pull/889) ([kjeremy](https://github.com/kjeremy))
- Fixed typo in MvxSeekBarProgressTargetBinding [\#885](https://github.com/MvvmCross/MvvmCross/pull/885) ([martijn00](https://github.com/martijn00))
- Added missing generic MvxDialogViewController [\#882](https://github.com/MvvmCross/MvvmCross/pull/882) ([jamie94bc](https://github.com/jamie94bc))
- PictureChooser iOS: Avoid view controller leaking. [\#881](https://github.com/MvvmCross/MvvmCross/pull/881) ([vzsg](https://github.com/vzsg))
- Mvx550 windows suspension [\#867](https://github.com/MvvmCross/MvvmCross/pull/867) ([tal33](https://github.com/tal33))

## [3.51-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.51-beta1) (2015-03-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.1-alpha1...3.51-beta1)

**Closed issues:**

- \[Xamarin + Android\] Finishing the activity using a MvxPresentationHint - Cast Context to Activity fails [\#937](https://github.com/MvvmCross/MvvmCross/issues/937)
- Could not install package 'MvvmCross.PortableSupport 3.5.0' [\#925](https://github.com/MvvmCross/MvvmCross/issues/925)
- Mac events [\#922](https://github.com/MvvmCross/MvvmCross/issues/922)
- Missing stylable field MvxExpandableListView [\#921](https://github.com/MvvmCross/MvvmCross/issues/921)
- 3.5.0 Dispose MvxViewClickBinding carshing in release mode [\#919](https://github.com/MvvmCross/MvvmCross/issues/919)
- MvxFragments auto-creating their ViewModels causes crashes in 3.5 when restored from saved state [\#908](https://github.com/MvvmCross/MvvmCross/issues/908)

## [3.5.1-alpha1](https://github.com/MvvmCross/MvvmCross/tree/3.5.1-alpha1) (2015-02-08)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.0...3.5.1-alpha1)

**Fixed bugs:**

- PictureChooser plugin 3.2.2 memory leak on iOS [\#876](https://github.com/MvvmCross/MvvmCross/issues/876)
- Suspect RaiseCanExecuteChanged MvxCommand weak reference not sufficient on Android [\#668](https://github.com/MvvmCross/MvvmCross/issues/668)
- Have to work out how to make Json.Net import easier [\#304](https://github.com/MvvmCross/MvvmCross/issues/304)
- Exception seen during demo - PictureChooser [\#269](https://github.com/MvvmCross/MvvmCross/issues/269)

**Closed issues:**

- Possible bug in Touch File Plugin [\#901](https://github.com/MvvmCross/MvvmCross/issues/901)
- Configurations Issues in iPAD Classic Api Project [\#891](https://github.com/MvvmCross/MvvmCross/issues/891)
- MvxFrameControl does not expose the protected constructor with IntPtr... [\#890](https://github.com/MvvmCross/MvvmCross/issues/890)
- Can't add NuGet MvvmCross 3.5 [\#888](https://github.com/MvvmCross/MvvmCross/issues/888)
- BindableViewPager leaking [\#886](https://github.com/MvvmCross/MvvmCross/issues/886)
- Failed to resolve System.Void UIKit.UIGestureRecognizer::set\_Delegate reference [\#880](https://github.com/MvvmCross/MvvmCross/issues/880)
- ReflectionTypeLoadException on Program Start [\#870](https://github.com/MvvmCross/MvvmCross/issues/870)
- ViewModelLocator swallows exceptions in TryLoad [\#837](https://github.com/MvvmCross/MvvmCross/issues/837)
- Re-used MvxImageViews show old image before new one is loaded in ListViews and TableViews [\#725](https://github.com/MvvmCross/MvvmCross/issues/725)
- Basic Integration with System.Reactive \(Rx\) [\#695](https://github.com/MvvmCross/MvvmCross/issues/695)
- MvvmCross.Dialog.Droid Dialog Views not opening with ART Runtime [\#667](https://github.com/MvvmCross/MvvmCross/issues/667)
- Error Validation [\#652](https://github.com/MvvmCross/MvvmCross/issues/652)
- Re-add the Generics :\) [\#648](https://github.com/MvvmCross/MvvmCross/issues/648)
- Compile error [\#646](https://github.com/MvvmCross/MvvmCross/issues/646)
- Building MvvmCross.Dialog.Droid in VS2013 [\#586](https://github.com/MvvmCross/MvvmCross/issues/586)
- Update ToDo-MvvmCross text files in NuGet packages [\#562](https://github.com/MvvmCross/MvvmCross/issues/562)
- CallerMemberAttribute [\#536](https://github.com/MvvmCross/MvvmCross/issues/536)
- Installing Color Plugin in nuget doesn't update everything to 3.0.13 [\#472](https://github.com/MvvmCross/MvvmCross/issues/472)
- Separate out Interactivity dependency on WIndowsPhone assembly [\#425](https://github.com/MvvmCross/MvvmCross/issues/425)
- Nuget support for Cirrious.MvvmCross.Droid.Maps. [\#423](https://github.com/MvvmCross/MvvmCross/issues/423)
- Tibet binding operator precedence  [\#384](https://github.com/MvvmCross/MvvmCross/issues/384)
- More helpers needed in BindLanguage \(maybe Fluent Helpers\) [\#263](https://github.com/MvvmCross/MvvmCross/issues/263)
- Gyroscope Plugin requested [\#242](https://github.com/MvvmCross/MvvmCross/issues/242)
- Vee Three - provide guidance/workaround/code for app Tombstoning [\#143](https://github.com/MvvmCross/MvvmCross/issues/143)

## [3.5.0](https://github.com/MvvmCross/MvvmCross/tree/3.5.0) (2015-01-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.0-beta2...3.5.0)

**Closed issues:**

- The type or namespace name 'Touch' does not exist in the namespace 'Cirrious.MvvmCross' [\#874](https://github.com/MvvmCross/MvvmCross/issues/874)
- Error: The type 'MvxEventToCommand' from assembly 'Cirrious.MvvmCross.Wpf' is built with an older version of the Blend SDK [\#873](https://github.com/MvvmCross/MvvmCross/issues/873)
- could not load plugin assembly for type Cirrious.MvvmCross.Plugins.WebBrowser.PluginLoader [\#872](https://github.com/MvvmCross/MvvmCross/issues/872)
- \[HowTo\]\[IoC\] Using MvvmX from Background Tasks [\#871](https://github.com/MvvmCross/MvvmCross/issues/871)
- iOS Project Fails to Compile [\#869](https://github.com/MvvmCross/MvvmCross/issues/869)
- MvxTableViewCell / UITableViewCell without using XCode [\#865](https://github.com/MvvmCross/MvvmCross/issues/865)
- NSDate Link Error [\#864](https://github.com/MvvmCross/MvvmCross/issues/864)
- Could not find view for Viewmodel [\#863](https://github.com/MvvmCross/MvvmCross/issues/863)
- Can't resolve logger in MvxSetup.CreateDebugTrace [\#862](https://github.com/MvvmCross/MvvmCross/issues/862)
- AppCompat 21+ widget themes [\#861](https://github.com/MvvmCross/MvvmCross/issues/861)
- Touch issues with new Xamarin update? [\#858](https://github.com/MvvmCross/MvvmCross/issues/858)
- MvxDynamicImageHelper not reset when recycling view/cell during scroll [\#933](https://github.com/MvvmCross/MvvmCross/issues/933)
- RadioButton tinting using appcompat v21 doesn't work [\#904](https://github.com/MvvmCross/MvvmCross/issues/904)
- WinStore: suspension changes to allow persistent serialisation/deserialisation [\#550](https://github.com/MvvmCross/MvvmCross/issues/550)
- Make Android view inflation more customisable [\#491](https://github.com/MvvmCross/MvvmCross/issues/491)

**Merged pull requests:**

- Fixed ViewModel property in MvxStorePage\<T\>. [\#879](https://github.com/MvvmCross/MvvmCross/pull/879) ([lothrop](https://github.com/lothrop))
- Added port for Xamarin.Mac Unified API. [\#860](https://github.com/MvvmCross/MvvmCross/pull/860) ([lothrop](https://github.com/lothrop))
- Added missing constructors to generic ViewControllers [\#859](https://github.com/MvvmCross/MvvmCross/pull/859) ([tomgilder](https://github.com/tomgilder))
- Updates for UnifiedAPI [\#857](https://github.com/MvvmCross/MvvmCross/pull/857) ([maxgrig](https://github.com/maxgrig))
- 3.5tc [\#853](https://github.com/MvvmCross/MvvmCross/pull/853) ([tofutim](https://github.com/tofutim))
- Allow IMvxViewModelLocator to throw exceptions [\#838](https://github.com/MvvmCross/MvvmCross/pull/838) ([jesperll](https://github.com/jesperll))
- Fix import targets for Unified API [\#833](https://github.com/MvvmCross/MvvmCross/pull/833) ([lothrop](https://github.com/lothrop))
- Fully qualify the namespace of the Fragment class we are using [\#822](https://github.com/MvvmCross/MvvmCross/pull/822) ([kjeremy](https://github.com/kjeremy))

## [3.5.0-beta2](https://github.com/MvvmCross/MvvmCross/tree/3.5.0-beta2) (2014-12-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.0-beta1...3.5.0-beta2)

## [3.5.0-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.5.0-beta1) (2014-12-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.5.0-alpha2...3.5.0-beta1)

**Fixed bugs:**

- ListFragment Typo [\#854](https://github.com/MvvmCross/MvvmCross/issues/854)
- MvxDownloadCacheConfiguration.Default invalid folder on ios 8 [\#799](https://github.com/MvvmCross/MvvmCross/issues/799)
- Cherry pick dd3c20a9df8547ad447289bd5f6a3f605ff7941c into 3.2 [\#793](https://github.com/MvvmCross/MvvmCross/issues/793)

**Closed issues:**

- TeamCity .nuget package restore [\#852](https://github.com/MvvmCross/MvvmCross/issues/852)
- MvxBindingExtensions.ShouldSkipSetValueAsHaveNearlyIdenticalNumericText does not care about Ints [\#849](https://github.com/MvvmCross/MvvmCross/issues/849)
- File Plugin Access Denied error [\#843](https://github.com/MvvmCross/MvvmCross/issues/843)
- Tombstoning doesn't work when showing ViewModel with parameters. [\#834](https://github.com/MvvmCross/MvvmCross/issues/834)
- 64 bit support for Xamarin.iOS and Xamarin.Mac [\#801](https://github.com/MvvmCross/MvvmCross/issues/801)
- New XIB structure created with Xamarin Studio not working [\#797](https://github.com/MvvmCross/MvvmCross/issues/797)
- Compile Issue when using IEnumerable\<PropertyInfo\> [\#796](https://github.com/MvvmCross/MvvmCross/issues/796)
- Can't install MvvmCross.PortableSupport on WP7 [\#792](https://github.com/MvvmCross/MvvmCross/issues/792)
- MvxListViewItem cannot be used unless attached to window [\#775](https://github.com/MvvmCross/MvvmCross/issues/775)
- Cannot send file multipart to server using MvxMultiPartFormRestRequest [\#759](https://github.com/MvvmCross/MvvmCross/issues/759)
- ScrollView not found? [\#756](https://github.com/MvvmCross/MvvmCross/issues/756)
- Crashes in mvxlistview in 3.1.1 [\#751](https://github.com/MvvmCross/MvvmCross/issues/751)
- Standard Android project with MvvmCross 3.1.1 crashes at startup on Dell Venue 7 \(Intel Atom x86\) [\#748](https://github.com/MvvmCross/MvvmCross/issues/748)
- Extract IoC container from Mvx [\#744](https://github.com/MvvmCross/MvvmCross/issues/744)

**Merged pull requests:**

- Updated ShouldSkipSetValueAsHaveNearlyIdenticalNumericText so that the t... [\#855](https://github.com/MvvmCross/MvvmCross/pull/855) ([munkii](https://github.com/munkii))
- Fixed app freeze when animating state change of a UISwitch used as acces... [\#844](https://github.com/MvvmCross/MvvmCross/pull/844) ([ggirard07](https://github.com/ggirard07))

## [3.5.0-alpha2](https://github.com/MvvmCross/MvvmCross/tree/3.5.0-alpha2) (2014-11-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.2.2...3.5.0-alpha2)

## [3.2.2](https://github.com/MvvmCross/MvvmCross/tree/3.2.2) (2014-11-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/v3_5_alpha0...3.2.2)

## [v3_5_alpha0](https://github.com/MvvmCross/MvvmCross/tree/v3_5_alpha0) (2014-11-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.2.2-beta1...v3_5_alpha0)

**Closed issues:**

- can't install MVVMCross to Xamarin.ios project from nuget [\#831](https://github.com/MvvmCross/MvvmCross/issues/831)
- SaveStateToBundle, is it supposed to work in this way? [\#829](https://github.com/MvvmCross/MvvmCross/issues/829)
- PictureChooser Plugin throwing exception on TakePictureAsync \(Windows Phone 8.1 XAML\) [\#828](https://github.com/MvvmCross/MvvmCross/issues/828)
- When trying to add material design using AppCompat Fragging picks up the wrong Fragment class [\#823](https://github.com/MvvmCross/MvvmCross/issues/823)
- Xamarin does not support API 4-8 anymore in beta/alpha [\#820](https://github.com/MvvmCross/MvvmCross/issues/820)
- EditText with DateTime does not set the property on the ViewModel [\#815](https://github.com/MvvmCross/MvvmCross/issues/815)

**Merged pull requests:**

- Add UILabel.AttributedText and MvxViewController.Title to LinkerPleaseInclude.cs.pp [\#825](https://github.com/MvvmCross/MvvmCross/pull/825) ([mgjhl](https://github.com/mgjhl))
- Fully qualify the namespace of the Fragment class we are using [\#824](https://github.com/MvvmCross/MvvmCross/pull/824) ([kjeremy](https://github.com/kjeremy))
- Added Heading \(Course\) to location plugin for Windows Phone [\#819](https://github.com/MvvmCross/MvvmCross/pull/819) ([trik](https://github.com/trik))
- Initialize the media capture. [\#818](https://github.com/MvvmCross/MvvmCross/pull/818) ([luke-barnett](https://github.com/luke-barnett))
- Email Plugin: Only show real email clients on android [\#812](https://github.com/MvvmCross/MvvmCross/pull/812) ([mgjhl](https://github.com/mgjhl))
- Implementation of Generic views [\#808](https://github.com/MvvmCross/MvvmCross/pull/808) ([Stephanvs](https://github.com/Stephanvs))

## [3.2.2-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.2.2-beta1) (2014-10-21)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.2.1-beta3...3.2.2-beta1)

**Fixed bugs:**

- MvxUIButtonTitleTargetBinding not working  [\#763](https://github.com/MvvmCross/MvvmCross/issues/763)

**Closed issues:**

- Different resume function depending on first or second run of new app in Android [\#807](https://github.com/MvvmCross/MvvmCross/issues/807)
- APK size increase after installing 3.2.1 [\#806](https://github.com/MvvmCross/MvvmCross/issues/806)
- MvvmCross not working with Htmlagilitypack [\#805](https://github.com/MvvmCross/MvvmCross/issues/805)
- Binding android title gets stripped by linker [\#803](https://github.com/MvvmCross/MvvmCross/issues/803)
- Generic IView implementations for Strongly Typed ViewModel access [\#802](https://github.com/MvvmCross/MvvmCross/issues/802)
- MvxBindingSingletonCache.Instance.BindingDescriptionParser Parse and ParseSingle produce different output [\#800](https://github.com/MvvmCross/MvvmCross/issues/800)
- MvvmCross on VS2013 is not working with Windows Phone 8 \(Silverlight\) on 8.0 Emulator \(only 8.1\) [\#794](https://github.com/MvvmCross/MvvmCross/issues/794)
- Binding to UITextView.AttributedText crashes on ios 7.0.3 simulator [\#791](https://github.com/MvvmCross/MvvmCross/issues/791)
- Location.Touch does not work on iOS 8 [\#788](https://github.com/MvvmCross/MvvmCross/issues/788)
- \[Plugins.File\] fileStorage.Exists\(\) returns true, but TryReadTextFile\(\) throws "The system cannot find the file specified" [\#787](https://github.com/MvvmCross/MvvmCross/issues/787)
- MvxAutocompleteTextView binding [\#786](https://github.com/MvvmCross/MvvmCross/issues/786)
- Sqlite plugin doesn't appear to work in MVVMCross 3.2.1-beta3 for Unit Test core project [\#784](https://github.com/MvvmCross/MvvmCross/issues/784)
- PictureChooser does not work in Windows Phone 8.1 [\#782](https://github.com/MvvmCross/MvvmCross/issues/782)
- MvxPhoneViewPresenter is missing in beta3 ? [\#778](https://github.com/MvvmCross/MvvmCross/issues/778)
- GetPluginConfiguration\(Type plugin\) is not expected [\#776](https://github.com/MvvmCross/MvvmCross/issues/776)
- CheckedTextView text binding doesn't work in Release mode with Link SDK Only [\#769](https://github.com/MvvmCross/MvvmCross/issues/769)
- Duplicate folders when adding by NuGet on Android [\#768](https://github.com/MvvmCross/MvvmCross/issues/768)
- PictureChooser.Touch always returns image resized to maxPixelDimension [\#764](https://github.com/MvvmCross/MvvmCross/issues/764)
- NugetTempalates need changing for the winstore app [\#762](https://github.com/MvvmCross/MvvmCross/issues/762)

**Merged pull requests:**

- When deserializing dictionary -\> object, include base class properties [\#809](https://github.com/MvvmCross/MvvmCross/pull/809) ([gshackles](https://github.com/gshackles))
- Issue \#803 - Linker stripping android property [\#804](https://github.com/MvvmCross/MvvmCross/pull/804) ([mgjhl](https://github.com/mgjhl))
- Performance fix for checking if file exists [\#798](https://github.com/MvvmCross/MvvmCross/pull/798) ([vegardlarsen](https://github.com/vegardlarsen))
- Supported iOS 8 LocationService setup [\#789](https://github.com/MvvmCross/MvvmCross/pull/789) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- Added Universal Windows Apps to TipCalc [\#781](https://github.com/MvvmCross/MvvmCross/pull/781) ([jonstoneman](https://github.com/jonstoneman))
- Added Universal Windows Apps to TipCalc [\#780](https://github.com/MvvmCross/MvvmCross/pull/780) ([jonstoneman](https://github.com/jonstoneman))
- Weak ref change for \#668 and \#729 [\#779](https://github.com/MvvmCross/MvvmCross/pull/779) ([kjeremy](https://github.com/kjeremy))
- LinkerPleaseInclude: Add CheckedTextView Switch [\#773](https://github.com/MvvmCross/MvvmCross/pull/773) ([kjeremy](https://github.com/kjeremy))

## [3.2.1-beta3](https://github.com/MvvmCross/MvvmCross/tree/3.2.1-beta3) (2014-08-25)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.2.1-beta2...3.2.1-beta3)

**Closed issues:**

- MVVMCross package install error [\#758](https://github.com/MvvmCross/MvvmCross/issues/758)
- Can't compile main git source [\#755](https://github.com/MvvmCross/MvvmCross/issues/755)
- MvxAdapter::GetItem\(int position\) returns null [\#750](https://github.com/MvvmCross/MvvmCross/issues/750)
- ViewModel associated MvxFragment never calls ReloadState/SaveState [\#745](https://github.com/MvvmCross/MvvmCross/issues/745)

**Merged pull requests:**

- PictureChooser.Touch does not upscale image to maxPixelDimension anymore [\#772](https://github.com/MvvmCross/MvvmCross/pull/772) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- Infinite loop in DebugTrace.cs after a FormatException [\#770](https://github.com/MvvmCross/MvvmCross/pull/770) ([vzsg](https://github.com/vzsg))
- Default parameter for ValueConverter in iOS fluent binding [\#766](https://github.com/MvvmCross/MvvmCross/pull/766) ([lothrop](https://github.com/lothrop))
- Fix NullReferenceException in ReflectionExtensions.GetProperties\(\) [\#765](https://github.com/MvvmCross/MvvmCross/pull/765) ([PedroLamas](https://github.com/PedroLamas))
- Fix for not compiling due to ImeAction.Previous being only avaiable for SDK 11+. [\#757](https://github.com/MvvmCross/MvvmCross/pull/757) ([azchohfi](https://github.com/azchohfi))
- Add serialize to JSON Serializer loop handling to prevent crashing for self referencing objects [\#753](https://github.com/MvvmCross/MvvmCross/pull/753) ([pcmichaels](https://github.com/pcmichaels))

## [3.2.1-beta2](https://github.com/MvvmCross/MvvmCross/tree/3.2.1-beta2) (2014-07-27)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.2.1-beta1...3.2.1-beta2)

**Fixed bugs:**

- Universal App - Cannot resolve Assembly or Windows Metadata file 'System.Windows.Interactivity.dll' [\#743](https://github.com/MvvmCross/MvvmCross/issues/743)

**Closed issues:**

- Memory leaks in MvxLinearLayout, MvxListView [\#747](https://github.com/MvvmCross/MvvmCross/issues/747)
- use of System.Diagnostics.ConditionalAttribute [\#746](https://github.com/MvvmCross/MvvmCross/issues/746)
- GetRequestFromXamlUri - Invalid Character in Url [\#739](https://github.com/MvvmCross/MvvmCross/issues/739)
- Why is a View instatiated before its ViewModel [\#737](https://github.com/MvvmCross/MvvmCross/issues/737)
- MvvmCros issue when using BugSense for Xamarin Androi plugin. [\#694](https://github.com/MvvmCross/MvvmCross/issues/694)
- MvxSpinner does not hold value when in a ListView while scrolling in Android [\#686](https://github.com/MvvmCross/MvvmCross/issues/686)

**Merged pull requests:**

- Update MvxPhoneViewsContainer.cs [\#741](https://github.com/MvvmCross/MvvmCross/pull/741) ([kadamgreene](https://github.com/kadamgreene))
- Fix WriteFile flushing stream more than once [\#734](https://github.com/MvvmCross/MvvmCross/pull/734) ([toolboc](https://github.com/toolboc))
- Add XPlatformCloudKit to MVVMCross projects list [\#733](https://github.com/MvvmCross/MvvmCross/pull/733) ([toolboc](https://github.com/toolboc))
- Fixed Email.Touch plugin bug for CC emails [\#731](https://github.com/MvvmCross/MvvmCross/pull/731) ([Laumania](https://github.com/Laumania))
- iOS : Load image from bundle [\#726](https://github.com/MvvmCross/MvvmCross/pull/726) ([usami-k](https://github.com/usami-k))

## [3.2.1-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.2.1-beta1) (2014-06-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.177-beta5...3.2.1-beta1)

**Fixed bugs:**

- MvxWeakEventSubscription property picker has wrong signature [\#707](https://github.com/MvvmCross/MvvmCross/issues/707)
- NullPointerException on Spinner in MvvmCross causes app to crash on Android 4.4 [\#507](https://github.com/MvvmCross/MvvmCross/issues/507)

**Closed issues:**

- Messenger plugin: unusable to get results from a ViewModel without lots of code [\#722](https://github.com/MvvmCross/MvvmCross/issues/722)
- is there a debug mode and a release mode MvvmCross? in Windows Phone Application [\#716](https://github.com/MvvmCross/MvvmCross/issues/716)
- ViewModel Init method: a "bool?" parameter generates an error when bound to \(nothing\) [\#715](https://github.com/MvvmCross/MvvmCross/issues/715)
- Impossible to bind property named Mode [\#713](https://github.com/MvvmCross/MvvmCross/issues/713)
- RaiseCanExecuteChanged doesn't always make it to the UI [\#708](https://github.com/MvvmCross/MvvmCross/issues/708)
- mvvmcross 3.1.2 beta1 does not support Profile 111  [\#706](https://github.com/MvvmCross/MvvmCross/issues/706)
- MvvmCross PortableSupport 3.1.1Library Installations Fails in Visual Studio 2013 [\#701](https://github.com/MvvmCross/MvvmCross/issues/701)
- Visibility not working [\#700](https://github.com/MvvmCross/MvvmCross/issues/700)
- Empty editview in android not converting [\#699](https://github.com/MvvmCross/MvvmCross/issues/699)
- The system cannot find the file specified. templateName=MvvmCross.Core.zip [\#698](https://github.com/MvvmCross/MvvmCross/issues/698)
- Bind a list of view models to CheckboxElements [\#693](https://github.com/MvvmCross/MvvmCross/issues/693)
- Xamarin linker stripping MvxPropertyInjector and INotifyPropertyChanged [\#689](https://github.com/MvvmCross/MvvmCross/issues/689)
- WinRT NavigationState is not serializable [\#685](https://github.com/MvvmCross/MvvmCross/issues/685)
- Where is the MvvmCross 3.1.1 Branch/Tag? [\#683](https://github.com/MvvmCross/MvvmCross/issues/683)
- MvxRGBAValueConverter - what about ARGB? [\#682](https://github.com/MvvmCross/MvvmCross/issues/682)
- Default image is shown on HTTP error in MvxDynamicImageHelper [\#679](https://github.com/MvvmCross/MvvmCross/issues/679)
- How do I make json localization work in the Test project [\#676](https://github.com/MvvmCross/MvvmCross/issues/676)
- better RaisePropertyChanged [\#675](https://github.com/MvvmCross/MvvmCross/issues/675)
- Messenger plugin "bug" [\#669](https://github.com/MvvmCross/MvvmCross/issues/669)
- Android: Full fragment support [\#666](https://github.com/MvvmCross/MvvmCross/issues/666)
- How to MvxAutoCompleteTextView ? [\#665](https://github.com/MvvmCross/MvvmCross/issues/665)
- Viewmodel Init method [\#664](https://github.com/MvvmCross/MvvmCross/issues/664)
- Integrating Fody.PropertyChanged with MVx [\#663](https://github.com/MvvmCross/MvvmCross/issues/663)
- iOS UIDatePicker Date Off By One Day - Timezone Issue [\#661](https://github.com/MvvmCross/MvvmCross/issues/661)
- Expression binding does not support variables in indexing operators [\#657](https://github.com/MvvmCross/MvvmCross/issues/657)
- Add error trace to MvxDynamicImageHelper\<T\> [\#649](https://github.com/MvvmCross/MvvmCross/issues/649)
- \[Feature Request\] Support NoSQL - Couchbase-lite [\#428](https://github.com/MvvmCross/MvvmCross/issues/428)
- Add Symbol-Package to symbolsource.org [\#314](https://github.com/MvvmCross/MvvmCross/issues/314)

**Merged pull requests:**

- \#314 Source package [\#712](https://github.com/MvvmCross/MvvmCross/pull/712) ([kjeremy](https://github.com/kjeremy))
- Fixed binding bug in MvxUIDatePickerTimeTargetBinding [\#705](https://github.com/MvvmCross/MvvmCross/pull/705) ([Costo](https://github.com/Costo))
- Set AutocorrectionType on the UITextField if it has been created. [\#696](https://github.com/MvvmCross/MvvmCross/pull/696) ([ddunkin](https://github.com/ddunkin))
- WindowsStore MvxPhoneCallTask doesn't compile [\#692](https://github.com/MvvmCross/MvvmCross/pull/692) ([guillaume-fr](https://github.com/guillaume-fr))
- Performance improvements for DownloadCache plugin. [\#691](https://github.com/MvvmCross/MvvmCross/pull/691) ([ddunkin](https://github.com/ddunkin))
- SafeDeleteFile should return true if file doesn't exists [\#690](https://github.com/MvvmCross/MvvmCross/pull/690) ([guillaume-fr](https://github.com/guillaume-fr))
- Stil display older image in MvxImageView \(iOS\) when loading and DefaultImagePath not present [\#680](https://github.com/MvvmCross/MvvmCross/pull/680) ([iseebi](https://github.com/iseebi))
- Use the tel uri in windows store for phone calls [\#678](https://github.com/MvvmCross/MvvmCross/pull/678) ([guillaume-fr](https://github.com/guillaume-fr))
- Async File plugin [\#677](https://github.com/MvvmCross/MvvmCross/pull/677) ([molinch](https://github.com/molinch))
- DeleteFile should not throw FileNotFoundException [\#671](https://github.com/MvvmCross/MvvmCross/pull/671) ([guillaume-fr](https://github.com/guillaume-fr))
- fixes 661 iOS UIDatePicker Date Off By One Day - Timezone Issue [\#662](https://github.com/MvvmCross/MvvmCross/pull/662) ([benhysell](https://github.com/benhysell))
- Added support for variables in indexing operators [\#659](https://github.com/MvvmCross/MvvmCross/pull/659) ([ckimes89](https://github.com/ckimes89))

## [build-3.1.177-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.177-beta5) (2014-04-09)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.2-beta1...build-3.1.177-beta5)

**Closed issues:**

- Is there any interest in adding these two methods to MvxFragmentActivity? [\#654](https://github.com/MvvmCross/MvvmCross/issues/654)
- MvxValueConverter not called when value is null [\#651](https://github.com/MvvmCross/MvvmCross/issues/651)
- Instantiating MvxCommand crash [\#650](https://github.com/MvvmCross/MvvmCross/issues/650)

**Merged pull requests:**

- Fix lock that should be on the lock object rather than on 'this'. [\#653](https://github.com/MvvmCross/MvvmCross/pull/653) ([danielcweber](https://github.com/danielcweber))

## [3.1.2-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.1.2-beta1) (2014-03-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.176-beta5...3.1.2-beta1)

**Closed issues:**

- binding android Drawable using mvvmcross [\#644](https://github.com/MvvmCross/MvvmCross/issues/644)
- Navigation Design Question [\#643](https://github.com/MvvmCross/MvvmCross/issues/643)
- Mac Binding build is failing [\#638](https://github.com/MvvmCross/MvvmCross/issues/638)
- Allow to cancel HttpRequest in Network plugin [\#631](https://github.com/MvvmCross/MvvmCross/issues/631)
- Xamarin.Android - Compile error for "Release" build with Linking="Sdk Assemblies only" [\#629](https://github.com/MvvmCross/MvvmCross/issues/629)
- Placeholder text getting overridden on UITextField \(was UITextView\) [\#628](https://github.com/MvvmCross/MvvmCross/issues/628)
- Android PictureChooser images load unrotated according to Exif data [\#626](https://github.com/MvvmCross/MvvmCross/issues/626)
- ItemsSource binding to ObservableDictionary [\#625](https://github.com/MvvmCross/MvvmCross/issues/625)
- RaiseCanExecuteChanged MvxCommand weak reference not sufficient on WindowsPhone only [\#623](https://github.com/MvvmCross/MvvmCross/issues/623)
- Make navobject use of IMvxBundle rather than MvxBundle [\#618](https://github.com/MvvmCross/MvvmCross/issues/618)
- Collection was modified; enumeration operation may not execute [\#615](https://github.com/MvvmCross/MvvmCross/issues/615)
- MvxCommandCollection should support multiple commands tied to the same IsEnabled property [\#531](https://github.com/MvvmCross/MvvmCross/issues/531)

**Merged pull requests:**

- typo in warning [\#642](https://github.com/MvvmCross/MvvmCross/pull/642) ([pnewhook](https://github.com/pnewhook))
- Fix null exception after Windows Phone location watcher stop [\#641](https://github.com/MvvmCross/MvvmCross/pull/641) ([kohlerb](https://github.com/kohlerb))
- Fix Mac Binding build is failing [\#639](https://github.com/MvvmCross/MvvmCross/pull/639) ([ashokgelal](https://github.com/ashokgelal))
- Add AsyncRequestHandle class, to support request cancel in future. [\#634](https://github.com/MvvmCross/MvvmCross/pull/634) ([wedkarz](https://github.com/wedkarz))
- Fix incorrect unsubscribe on Dispose [\#633](https://github.com/MvvmCross/MvvmCross/pull/633) ([rb1234](https://github.com/rb1234))
- Add location watcher plugin for WPF. [\#632](https://github.com/MvvmCross/MvvmCross/pull/632) ([AlanYost](https://github.com/AlanYost))
- Added exif rotation to the Android PictureChooser plugin for issue \#626 [\#627](https://github.com/MvvmCross/MvvmCross/pull/627) ([jsmarcus](https://github.com/jsmarcus))

## [build-3.1.176-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.176-beta5) (2014-03-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.175-beta5...build-3.1.176-beta5)

**Closed issues:**

- problems using MvxDialogFragment [\#622](https://github.com/MvvmCross/MvvmCross/issues/622)
- How to map the ActionIconTapped event handler of PhoneTextBox using mvvmcross [\#621](https://github.com/MvvmCross/MvvmCross/issues/621)
- How to map the textbox lost focus event using mvvmcross for windows phone [\#620](https://github.com/MvvmCross/MvvmCross/issues/620)

**Merged pull requests:**

- Added missing extension method for issue \#507 [\#624](https://github.com/MvvmCross/MvvmCross/pull/624) ([jamie94bc](https://github.com/jamie94bc))

## [build-3.1.175-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.175-beta5) (2014-03-05)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.174-beta5...build-3.1.175-beta5)

**Closed issues:**

- mvxlist itemclick not firing when  MvxItemTemplate contains checkboxes  [\#617](https://github.com/MvvmCross/MvvmCross/issues/617)
- CrossUI Droid [\#614](https://github.com/MvvmCross/MvvmCross/issues/614)

**Merged pull requests:**

- Added option to MvxTableViewSource to refresh on every NPC event [\#619](https://github.com/MvvmCross/MvvmCross/pull/619) ([ckimes89](https://github.com/ckimes89))

## [build-3.1.174-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.174-beta5) (2014-02-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.173-beta5...build-3.1.174-beta5)

**Fixed bugs:**

- MvxBaseListItemView Java.Lang.NoSuchMethodError when minSdkVersion \<= 11 [\#607](https://github.com/MvvmCross/MvvmCross/issues/607)

**Closed issues:**

- Real MultiBinding support [\#613](https://github.com/MvvmCross/MvvmCross/issues/613)

**Merged pull requests:**

- Issue607: check that environment actually supports API Level 11 [\#611](https://github.com/MvvmCross/MvvmCross/pull/611) ([rb1234](https://github.com/rb1234))

## [build-3.1.173-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.173-beta5) (2014-02-20)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.171-beta5...build-3.1.173-beta5)

**Fixed bugs:**

- MvxColor.ToString\(\) not displaying colours in correct order [\#575](https://github.com/MvvmCross/MvvmCross/issues/575)

**Closed issues:**

- Mvx.MvxListView Missing Method Exception [\#606](https://github.com/MvvmCross/MvvmCross/issues/606)
- Droid MvxListView does not allow headers or footers [\#604](https://github.com/MvvmCross/MvvmCross/issues/604)
-  Error seen during navigation request [\#601](https://github.com/MvvmCross/MvvmCross/issues/601)
- An exception of type 'System.MissingMethodException' occurred in Cirrious.MvvmCross.DLL [\#600](https://github.com/MvvmCross/MvvmCross/issues/600)
- Huge MvxSpinner in 3.1.1 [\#599](https://github.com/MvvmCross/MvvmCross/issues/599)
- MvxAutoCompleteTextView is crashing with exception “System.ArgumentOutOfRangeException: Argument is out of range” [\#594](https://github.com/MvvmCross/MvvmCross/issues/594)

**Merged pull requests:**

- Update \_ Windows Store UI.txt for Win 8.1 [\#605](https://github.com/MvvmCross/MvvmCross/pull/605) ([RobGibbens](https://github.com/RobGibbens))
- Fixes for MvxImageView on Android not using ErrorImagePath. [\#598](https://github.com/MvvmCross/MvvmCross/pull/598) ([Cheesebaron](https://github.com/Cheesebaron))
- Extend file plugin to support methods that return Streams [\#593](https://github.com/MvvmCross/MvvmCross/pull/593) ([danielcweber](https://github.com/danielcweber))
- Add ioc register type overload [\#591](https://github.com/MvvmCross/MvvmCross/pull/591) ([danielcweber](https://github.com/danielcweber))

## [build-3.1.171-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.171-beta5) (2014-02-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.170-beta5...build-3.1.171-beta5)

**Closed issues:**

- UI Thread issue with view model in MVVMCross [\#597](https://github.com/MvvmCross/MvvmCross/issues/597)
- mvvm dialog ValueChanged fires for EntryElements but not StringElements [\#596](https://github.com/MvvmCross/MvvmCross/issues/596)
- Strange behavior with Textview.AfterTextChanged with Fluent but not Inline bindings [\#595](https://github.com/MvvmCross/MvvmCross/issues/595)
- How to test the sqlite related code [\#590](https://github.com/MvvmCross/MvvmCross/issues/590)

## [build-3.1.170-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.170-beta5) (2014-02-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.169-beta5...build-3.1.170-beta5)

## [build-3.1.169-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.169-beta5) (2014-02-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.168-beta5...build-3.1.169-beta5)

## [build-3.1.168-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.168-beta5) (2014-02-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1...build-3.1.168-beta5)

**Closed issues:**

- Wrong null evalution in WP BindingEx [\#589](https://github.com/MvvmCross/MvvmCross/issues/589)

## [3.1.1](https://github.com/MvvmCross/MvvmCross/tree/3.1.1) (2014-02-09)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.167-beta5...3.1.1)

**Fixed bugs:**

- Check TryCreateSpecificFactoryBinding for null handling [\#584](https://github.com/MvvmCross/MvvmCross/issues/584)
- Fixed FolderExists in FileStore for WindowsStore [\#580](https://github.com/MvvmCross/MvvmCross/pull/580) ([mgulati](https://github.com/mgulati))

**Closed issues:**

- ViewModel not being displayed on Windows Tablet RT 8.1 [\#587](https://github.com/MvvmCross/MvvmCross/issues/587)

**Merged pull requests:**

- Mac cherry [\#583](https://github.com/MvvmCross/MvvmCross/pull/583) ([tofutim](https://github.com/tofutim))

## [build-3.1.167-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.167-beta5) (2014-02-06)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.166-beta5...build-3.1.167-beta5)

## [build-3.1.166-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.166-beta5) (2014-02-06)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.163-beta5...build-3.1.166-beta5)

## [build-3.1.163-beta5](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.163-beta5) (2014-02-06)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.160-beta5...build-3.1.163-beta5)

**Fixed bugs:**

- Memory Leak using Tibet Binding on Android plateform [\#581](https://github.com/MvvmCross/MvvmCross/issues/581)

**Merged pull requests:**

- Allow creation of MvxViewModelLoader to be overridden. [\#579](https://github.com/MvvmCross/MvvmCross/pull/579) ([danielcweber](https://github.com/danielcweber))
- Update MvxColor.cs [\#576](https://github.com/MvvmCross/MvvmCross/pull/576) ([iankerr](https://github.com/iankerr))

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

**Fixed bugs:**

- Binding causes critical exception when using MvxListView and adaptor? [\#485](https://github.com/MvvmCross/MvvmCross/issues/485)
- Check and improve SuspensionManager - SaveAsync in WinRT [\#415](https://github.com/MvvmCross/MvvmCross/issues/415)
- WinRT - BackStack clearing [\#96](https://github.com/MvvmCross/MvvmCross/issues/96)

**Closed issues:**

- Default binding for UIProgressView [\#571](https://github.com/MvvmCross/MvvmCross/issues/571)
- Add ability to plug-in alternate implementations for creating actual View from type [\#561](https://github.com/MvvmCross/MvvmCross/issues/561)
- When debugging with MvvMCross Binaries XamStudio breaks execution all the time making it impossible to debug [\#487](https://github.com/MvvmCross/MvvmCross/issues/487)
- MvxListView activated indicator doesn't work on 3.0.13 [\#481](https://github.com/MvvmCross/MvvmCross/issues/481)
- Add MvxXmlRestClient. [\#362](https://github.com/MvvmCross/MvvmCross/issues/362)
- Simple MessageBox/Dialog/Prompt? [\#259](https://github.com/MvvmCross/MvvmCross/issues/259)
- Make Wiki publicly editable / Wiki Pull Request [\#252](https://github.com/MvvmCross/MvvmCross/issues/252)
- File plugin for winrt doesn't implement the folder exists/delete methods [\#105](https://github.com/MvvmCross/MvvmCross/issues/105)
- Implement CanExecute more fully across all platforms/bindings [\#64](https://github.com/MvvmCross/MvvmCross/issues/64)

**Merged pull requests:**

- Virtual Convert and ConvertBack methods in MvxNativeValueConverter [\#572](https://github.com/MvvmCross/MvvmCross/pull/572) ([damirarh](https://github.com/damirarh))

## [3.1.1-beta5-attempt2](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta5-attempt2) (2014-02-01)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1-beta5...3.1.1-beta5-attempt2)

**Closed issues:**

- Improve error message in windows binding for missing view property [\#563](https://github.com/MvvmCross/MvvmCross/issues/563)

## [3.1.1-beta5](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta5) (2014-02-01)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1-beta4...3.1.1-beta5)

**Closed issues:**

- Object serialization does not support nullable types [\#568](https://github.com/MvvmCross/MvvmCross/issues/568)
- Binding parser too agressive looking for null [\#566](https://github.com/MvvmCross/MvvmCross/issues/566)
- Bug in 3.0.14 picturechooser plugin android [\#565](https://github.com/MvvmCross/MvvmCross/issues/565)
- IoC crashes on resolving a circular reference [\#553](https://github.com/MvvmCross/MvvmCross/issues/553)
- Memory leak using "Tibet" binding with WinStore and WPF mvvmcoss apps [\#552](https://github.com/MvvmCross/MvvmCross/issues/552)
- suggestion for MvxTabsFragmentActivity [\#541](https://github.com/MvvmCross/MvvmCross/issues/541)

## [3.1.1-beta4](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta4) (2014-01-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1-beta3...3.1.1-beta4)

**Closed issues:**

- Cirrious.CrossCore and Cirrious.MvvmCross are missing company name property [\#558](https://github.com/MvvmCross/MvvmCross/issues/558)
- Missing BindingTargets for UIView.BackgroundColor and UIButton.BorderWidth [\#557](https://github.com/MvvmCross/MvvmCross/issues/557)
- Problems with SelectionChangedCommand binding and TableView in Bindind.Touch [\#555](https://github.com/MvvmCross/MvvmCross/issues/555)
- MvxtabsFragmentActivity IllegalStateException / CommitAllowingStateLoss [\#554](https://github.com/MvvmCross/MvvmCross/issues/554)
- MvxCommandCollectionBuilder does not allow command methods with return values [\#532](https://github.com/MvvmCross/MvvmCross/issues/532)
- Rio binding causing performance issue and failing intermittently [\#528](https://github.com/MvvmCross/MvvmCross/issues/528)
- Windows Store file access to slow for many files [\#521](https://github.com/MvvmCross/MvvmCross/issues/521)

## [3.1.1-beta3](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta3) (2014-01-16)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.148-beta1...3.1.1-beta3)

**Closed issues:**

- Feature Request for MvxCommandCollectionBuilder [\#556](https://github.com/MvvmCross/MvvmCross/issues/556)
- Unable to unpack Components in Fragging in Team City [\#548](https://github.com/MvvmCross/MvvmCross/issues/548)
- Setting up components in TeamCity [\#547](https://github.com/MvvmCross/MvvmCross/issues/547)
- Receiving message multiple times when using Messenger plugin [\#546](https://github.com/MvvmCross/MvvmCross/issues/546)
- suggestion: add an overload of StartActivityForResult in MvxActivity [\#544](https://github.com/MvvmCross/MvvmCross/issues/544)

**Merged pull requests:**

- Switch to using Newtonsoft.Json nuget package [\#551](https://github.com/MvvmCross/MvvmCross/pull/551) ([rossdargan](https://github.com/rossdargan))
- Access to IMvxValueConverter from native wrapper [\#549](https://github.com/MvvmCross/MvvmCross/pull/549) ([damirarh](https://github.com/damirarh))
- Email Attachment support for Android [\#545](https://github.com/MvvmCross/MvvmCross/pull/545) ([hlogmans](https://github.com/hlogmans))

## [build-3.1.148-beta1](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.148-beta1) (2014-01-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.132-beta1...build-3.1.148-beta1)

## [build-3.1.132-beta1](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.132-beta1) (2014-01-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.133-beta1...build-3.1.132-beta1)

## [build-3.1.133-beta1](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.133-beta1) (2014-01-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1-beta2...build-3.1.133-beta1)

**Closed issues:**

- Cannot create a file when that file already exists [\#500](https://github.com/MvvmCross/MvvmCross/issues/500)
- Where to submit plugin contributions  [\#493](https://github.com/MvvmCross/MvvmCross/issues/493)
- Cannot add MvvmCross NuGet package to Xamarin.iOS project [\#486](https://github.com/MvvmCross/MvvmCross/issues/486)

## [3.1.1-beta2](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta2) (2014-01-03)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.1.1-beta1...3.1.1-beta2)

**Closed issues:**

- Unable to add Fragment Support to Android Core Project [\#540](https://github.com/MvvmCross/MvvmCross/issues/540)
- Make IMvxListItemView.Content public [\#538](https://github.com/MvvmCross/MvvmCross/issues/538)
- Bug and fix for MvxAdapter \(android\) [\#537](https://github.com/MvvmCross/MvvmCross/issues/537)
- IMvxMessenger weak reference not working as expected [\#535](https://github.com/MvvmCross/MvvmCross/issues/535)
- \(deleted\) [\#533](https://github.com/MvvmCross/MvvmCross/issues/533)
- i use the .net framework 4.0 ,could i use mvvmcross ? [\#527](https://github.com/MvvmCross/MvvmCross/issues/527)
- Request for sample for Action View [\#526](https://github.com/MvvmCross/MvvmCross/issues/526)
- Samples not working [\#525](https://github.com/MvvmCross/MvvmCross/issues/525)
- Fix for ContentView null in MvxTableViewCell derived classes [\#524](https://github.com/MvvmCross/MvvmCross/issues/524)
- bug in picture chooser plugin for android [\#511](https://github.com/MvvmCross/MvvmCross/issues/511)

## [3.1.1-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.1.1-beta1) (2013-12-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/build-3.1.131-beta1...3.1.1-beta1)

**Fixed bugs:**

- Null exception in webcontentelement [\#508](https://github.com/MvvmCross/MvvmCross/issues/508)

**Closed issues:**

- ShouldAlwaysRaiseInpcOnUserInterfaceThread [\#519](https://github.com/MvvmCross/MvvmCross/issues/519)
- \_.\_ in nuspec [\#518](https://github.com/MvvmCross/MvvmCross/issues/518)
- MvvmCross\_All getting fat [\#517](https://github.com/MvvmCross/MvvmCross/issues/517)
- Async/await MVVM support [\#488](https://github.com/MvvmCross/MvvmCross/issues/488)
- Asycnrhonous Calls to Inactive ViewModels cause Application Crash [\#482](https://github.com/MvvmCross/MvvmCross/issues/482)
- Are you going to move to PCL v4.6 \(Profile44\) [\#474](https://github.com/MvvmCross/MvvmCross/issues/474)
- Any feedback in WriteFile [\#449](https://github.com/MvvmCross/MvvmCross/issues/449)
- Consider adding DependecyProperty.UnsetValue or Binding.DoNothing [\#424](https://github.com/MvvmCross/MvvmCross/issues/424)
- Use Mvx.Resolve\<IMvxAndroidBinding\>\(\) instead of new MvxAndroidBinding\(\) [\#411](https://github.com/MvvmCross/MvvmCross/issues/411)
- Fragments - without support library [\#406](https://github.com/MvvmCross/MvvmCross/issues/406)
- Email plugin should have CanSendEmail method [\#335](https://github.com/MvvmCross/MvvmCross/issues/335)

## [build-3.1.131-beta1](https://github.com/MvvmCross/MvvmCross/tree/build-3.1.131-beta1) (2013-12-07)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.14...build-3.1.131-beta1)

**Closed issues:**

- CRLF/LF settings [\#515](https://github.com/MvvmCross/MvvmCross/issues/515)
- experimenting with profile 158 [\#512](https://github.com/MvvmCross/MvvmCross/issues/512)
- IMvxFileStore IoC Issue  [\#510](https://github.com/MvvmCross/MvvmCross/issues/510)
- Adding support for JVFloatSharp [\#506](https://github.com/MvvmCross/MvvmCross/issues/506)
- layoutName parameter never used in most cases [\#504](https://github.com/MvvmCross/MvvmCross/issues/504)
- MVVMCross Monodroid.Dialog odd behavior with EntryElement when using ImageElement [\#501](https://github.com/MvvmCross/MvvmCross/issues/501)
- System.ArgumentNullException: missing source event info in MvxWeakEventSubscription [\#498](https://github.com/MvvmCross/MvvmCross/issues/498)

**Merged pull requests:**

- Fixed ARM/x64 configurations for TeamCity \(v3.1\), Mac components, nuspec [\#513](https://github.com/MvvmCross/MvvmCross/pull/513) ([tofutim](https://github.com/tofutim))

## [3.0.14](https://github.com/MvvmCross/MvvmCross/tree/3.0.14) (2013-11-16)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.14-beta3...3.0.14)

**Fixed bugs:**

- Check why `Visible Errors\['Name'\]` binding doesn't work [\#494](https://github.com/MvvmCross/MvvmCross/issues/494)

**Closed issues:**

- Support for Windows Phone 8 Xaml [\#495](https://github.com/MvvmCross/MvvmCross/issues/495)

## [3.0.14-beta3](https://github.com/MvvmCross/MvvmCross/tree/3.0.14-beta3) (2013-11-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.14-beta2-real...3.0.14-beta3)

**Closed issues:**

- MvxLinearLayout broken in 3.0.14 beta 1 [\#484](https://github.com/MvvmCross/MvvmCross/issues/484)
- Null ViewModel in directly navigated Activity. [\#483](https://github.com/MvvmCross/MvvmCross/issues/483)

## [3.0.14-beta2-real](https://github.com/MvvmCross/MvvmCross/tree/3.0.14-beta2-real) (2013-11-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.14-beta2...3.0.14-beta2-real)

## [3.0.14-beta2](https://github.com/MvvmCross/MvvmCross/tree/3.0.14-beta2) (2013-11-04)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.14-beta1...3.0.14-beta2)

**Closed issues:**

- Bindings disposal for long-living nested observable collections [\#468](https://github.com/MvvmCross/MvvmCross/issues/468)

## [3.0.14-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.0.14-beta1) (2013-11-02)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.13...3.0.14-beta1)

**Fixed bugs:**

- Underscores can cause issues in the source property parsing stage \(turning property path into tokens\) [\#461](https://github.com/MvvmCross/MvvmCross/issues/461)
- ParseSingle and Fluent Described is confusing - currently parses sourcepath only but typical use expects target + specification [\#441](https://github.com/MvvmCross/MvvmCross/issues/441)

**Closed issues:**

- MvxListView selected activated indicator doesn't work with 3.0.13 [\#480](https://github.com/MvvmCross/MvvmCross/issues/480)
- ImageButton android in LinkerPleaseInclude [\#479](https://github.com/MvvmCross/MvvmCross/issues/479)
- Feature Request: Bring SQLite-Net Extensions as MvvmCross Plugin [\#477](https://github.com/MvvmCross/MvvmCross/issues/477)
- Change stack overflow link [\#476](https://github.com/MvvmCross/MvvmCross/issues/476)
- MvxViewModel Start,InitFromBundle and ReloadFromBundle not being called when ViewModel is cached [\#475](https://github.com/MvvmCross/MvvmCross/issues/475)
- Problem seen creating View-ViewModel lookup table - you have more than one View registered for the ViewModels [\#473](https://github.com/MvvmCross/MvvmCross/issues/473)
- BindingEx \(and other libraries dependent on starter pack\) [\#466](https://github.com/MvvmCross/MvvmCross/issues/466)
- Is there an accepted method for "Single Instance" ViewModel use? [\#465](https://github.com/MvvmCross/MvvmCross/issues/465)
- Click Binding not Working in Release Build [\#464](https://github.com/MvvmCross/MvvmCross/issues/464)
- Add binding support to include XML layout [\#463](https://github.com/MvvmCross/MvvmCross/issues/463)
- CacheIndex sharing violation in Cirrious.MvvmCross.Plugins.File.MvxFileStore.WriteFileCommon [\#460](https://github.com/MvvmCross/MvvmCross/issues/460)
- Namespace collision with Sqlite-net component [\#432](https://github.com/MvvmCross/MvvmCross/issues/432)
- Dramatically improved perfromances of OneWay binding on TextView \(Android\) [\#421](https://github.com/MvvmCross/MvvmCross/issues/421)
- Standard Android ImageView binding for local resource images do not handle screen scaling. [\#290](https://github.com/MvvmCross/MvvmCross/issues/290)
- Consider bumping all AssemblyVersion's to 3 [\#248](https://github.com/MvvmCross/MvvmCross/issues/248)

**Merged pull requests:**

- Android RatingBar Target Binding [\#470](https://github.com/MvvmCross/MvvmCross/pull/470) ([ShawInnes](https://github.com/ShawInnes))
- Update SimplePickerElement.cs [\#469](https://github.com/MvvmCross/MvvmCross/pull/469) ([benpage](https://github.com/benpage))
- Provide IntPtr Constructors, Allow Optional Init Parameters and Small ios7 Fix [\#467](https://github.com/MvvmCross/MvvmCross/pull/467) ([csteeg](https://github.com/csteeg))
- Added ThreadUtils WPF project [\#459](https://github.com/MvvmCross/MvvmCross/pull/459) ([ShawInnes](https://github.com/ShawInnes))

## [3.0.13](https://github.com/MvvmCross/MvvmCross/tree/3.0.13) (2013-10-08)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.13-beta4...3.0.13)

**Fixed bugs:**

- Date and Time pickers not working in ios7 for CrossUI.Dialog :/ [\#458](https://github.com/MvvmCross/MvvmCross/issues/458)

**Closed issues:**

- Droid/Touch Dialog and Droid fragging rely on StarterPack [\#457](https://github.com/MvvmCross/MvvmCross/issues/457)

## [3.0.13-beta4](https://github.com/MvvmCross/MvvmCross/tree/3.0.13-beta4) (2013-10-05)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.13-beta3...3.0.13-beta4)

## [3.0.13-beta3](https://github.com/MvvmCross/MvvmCross/tree/3.0.13-beta3) (2013-10-05)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.12...3.0.13-beta3)

**Fixed bugs:**

- MvxColor overloads not working as expected [\#455](https://github.com/MvvmCross/MvvmCross/issues/455)
- Touch.Dialog StyledStringElement does not use Visible [\#403](https://github.com/MvvmCross/MvvmCross/issues/403)
- ItemLongClick [\#281](https://github.com/MvvmCross/MvvmCross/issues/281)

**Closed issues:**

- Views and Viewmodels in referenced libraries [\#452](https://github.com/MvvmCross/MvvmCross/issues/452)
- \_updatingX in TargetBinding [\#446](https://github.com/MvvmCross/MvvmCross/issues/446)
- How to handle properties that are deeper... [\#445](https://github.com/MvvmCross/MvvmCross/issues/445)
- Could not AOT [\#444](https://github.com/MvvmCross/MvvmCross/issues/444)
- Overwriting existing content [\#443](https://github.com/MvvmCross/MvvmCross/issues/443)
- GetUTCOffset in Newtonsoft JSON.net throws null reference error on android [\#442](https://github.com/MvvmCross/MvvmCross/issues/442)
- Drawable by name resource image binding would be nice in Droid [\#435](https://github.com/MvvmCross/MvvmCross/issues/435)
- Email Plugin "to" argument is ignored [\#433](https://github.com/MvvmCross/MvvmCross/issues/433)
- Building in TeamCity [\#429](https://github.com/MvvmCross/MvvmCross/issues/429)
- Integrate SQLite Async [\#422](https://github.com/MvvmCross/MvvmCross/issues/422)
- Recheck BindingContext lifecycle - especially for independent mvxFrameView and MvxListViewItem [\#417](https://github.com/MvvmCross/MvvmCross/issues/417)
-   Add .Net's System.Drawing predefined colors, to MvxColor  [\#412](https://github.com/MvvmCross/MvvmCross/issues/412)
- Add MvxViewModel.OnTrimMemory. [\#410](https://github.com/MvvmCross/MvvmCross/issues/410)
- New Radio List Element for Android.Dialog [\#392](https://github.com/MvvmCross/MvvmCross/issues/392)
- Nullable\<int\> binding to text boxes - should it update empty string as Null [\#373](https://github.com/MvvmCross/MvvmCross/issues/373)
- No need for both MvxSimpleWpfViewPresenter.cs and MvxWpfSimpleViewPresenter.cs [\#359](https://github.com/MvvmCross/MvvmCross/issues/359)
- MvxViewExtensionMethods.OnViewCreate throws NullReferenceException [\#320](https://github.com/MvvmCross/MvvmCross/issues/320)
- StoreDateTimeAsTicks in ISQLiteConnectionFactory [\#213](https://github.com/MvvmCross/MvvmCross/issues/213)
- Consider adding MovementThreshold code to the geolocation implementation [\#90](https://github.com/MvvmCross/MvvmCross/issues/90)

**Merged pull requests:**

- MvxRadioGroup [\#454](https://github.com/MvvmCross/MvvmCross/pull/454) ([BenGladman](https://github.com/BenGladman))
- Fixed an issue when INotifyCollectionChanged.CollectionChanged event stripped by Xamarin linker. [\#453](https://github.com/MvvmCross/MvvmCross/pull/453) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- Do change dependency to libraries instead of Starter Package [\#434](https://github.com/MvvmCross/MvvmCross/pull/434) ([sschoeb](https://github.com/sschoeb))

## [3.0.12](https://github.com/MvvmCross/MvvmCross/tree/3.0.12) (2013-09-08)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.11-final...3.0.12)

## [3.0.11-final](https://github.com/MvvmCross/MvvmCross/tree/3.0.11-final) (2013-09-08)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.11-beta1...3.0.11-final)

**Closed issues:**

- Samples are failing with NullReferenceException on Xamarin 4.0.12 and Mono 3.2.0 [\#418](https://github.com/MvvmCross/MvvmCross/issues/418)
- To add tableView argument for MvxBaseTableViewSource.GetItemAt\(\) method  [\#388](https://github.com/MvvmCross/MvvmCross/issues/388)

**Merged pull requests:**

- removed a couple of duplicate usings [\#420](https://github.com/MvvmCross/MvvmCross/pull/420) ([MihaMarkic](https://github.com/MihaMarkic))
- Mark ImageHelperOnImageChanged as virtual to make it override [\#419](https://github.com/MvvmCross/MvvmCross/pull/419) ([jmgomez](https://github.com/jmgomez))

## [3.0.11-beta1](https://github.com/MvvmCross/MvvmCross/tree/3.0.11-beta1) (2013-09-01)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.11...3.0.11-beta1)

## [3.0.11](https://github.com/MvvmCross/MvvmCross/tree/3.0.11) (2013-08-27)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.10...3.0.11)

**Fixed bugs:**

- Touch.Dialog StyledStringElement TableViewCellStyle should be Default [\#402](https://github.com/MvvmCross/MvvmCross/issues/402)
- MvxWrappingCommand doesn't use commandParameterOverride for CanExecute [\#378](https://github.com/MvvmCross/MvvmCross/issues/378)
- Resource Loader in WindowsStore doesn't like '/' and '\' changes [\#332](https://github.com/MvvmCross/MvvmCross/issues/332)

**Closed issues:**

- Add some way to allow plugins to register ViewModels [\#405](https://github.com/MvvmCross/MvvmCross/issues/405)
- Problem with Include\(UIImageView\) in MonoTouch LinkerPleaseInclude [\#399](https://github.com/MvvmCross/MvvmCross/issues/399)
- Remove System.ServiceModel from Cirrious.MvvmCross.Plugins.File [\#394](https://github.com/MvvmCross/MvvmCross/issues/394)
- Android LinkerPleaseInclude needs TextView.Hint [\#391](https://github.com/MvvmCross/MvvmCross/issues/391)
- Setting Adapter on MvxLinearLayout Crashes Application [\#390](https://github.com/MvvmCross/MvvmCross/issues/390)
- ImageButton Click binding doens't check CanExecute [\#386](https://github.com/MvvmCross/MvvmCross/issues/386)
- Mail plugin problem with android [\#385](https://github.com/MvvmCross/MvvmCross/issues/385)
- Cannot compile for the followings \(in Unity only\) [\#383](https://github.com/MvvmCross/MvvmCross/issues/383)
- Request for additional file to be added to Nuget package [\#382](https://github.com/MvvmCross/MvvmCross/issues/382)
- ToSimplePropertyDictionary doesn't use NavigationSerializer? [\#380](https://github.com/MvvmCross/MvvmCross/issues/380)
- iOS LinkerPleaseInclude.cs does not compile [\#379](https://github.com/MvvmCross/MvvmCross/issues/379)
- 'MvxListView' in Android is not updated when using a dynamic ItemSource [\#377](https://github.com/MvvmCross/MvvmCross/issues/377)
- ConverterParameter error message in lang bindings confusing [\#376](https://github.com/MvvmCross/MvvmCross/issues/376)
- ItemSource with Converter and ObservableCollection [\#375](https://github.com/MvvmCross/MvvmCross/issues/375)
- Add MvxAsyncCommand [\#374](https://github.com/MvvmCross/MvvmCross/issues/374)
- AssetImagePath on ImageView broken again in 3.0.10 [\#372](https://github.com/MvvmCross/MvvmCross/issues/372)
- nuget install of 'MvvmCross.HotTuna.Touch.Dialog 3.0.10' fails. [\#371](https://github.com/MvvmCross/MvvmCross/issues/371)
- Credentials [\#366](https://github.com/MvvmCross/MvvmCross/issues/366)
- Need to investigate/tidy the Next behaviour in Touch.Dialog [\#361](https://github.com/MvvmCross/MvvmCross/issues/361)
- Provide INC based member access for indexed properties [\#353](https://github.com/MvvmCross/MvvmCross/issues/353)
- Binding decimal to UITextField removes decimal point [\#350](https://github.com/MvvmCross/MvvmCross/issues/350)
- Add option to RequestLocalFilePath in MvxFileDownloadCache to only fetch local? [\#319](https://github.com/MvvmCross/MvvmCross/issues/319)
- Start a REST plugin? [\#240](https://github.com/MvvmCross/MvvmCross/issues/240)
- Samples needed for Pickers including DateTimePickers in iOS \(new support classes may also be needed?\) [\#229](https://github.com/MvvmCross/MvvmCross/issues/229)
- VeeThree - Bindable Pager [\#155](https://github.com/MvvmCross/MvvmCross/issues/155)

**Merged pull requests:**

- Remove unneeded System.ServiceModel references [\#397](https://github.com/MvvmCross/MvvmCross/pull/397) ([brianchance](https://github.com/brianchance))
- fixed the issue with multiple call to HandleImagePick [\#381](https://github.com/MvvmCross/MvvmCross/pull/381) ([aboo](https://github.com/aboo))

## [3.0.10](https://github.com/MvvmCross/MvvmCross/tree/3.0.10) (2013-07-23)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/3.0.9...3.0.10)

**Fixed bugs:**

- Wpf ResourceLoader  [\#355](https://github.com/MvvmCross/MvvmCross/issues/355)
- MvxPickerViewModel works with ObservableCollection changes, but not with property changes [\#354](https://github.com/MvvmCross/MvvmCross/issues/354)
- Crash reported on sample app rehydration  [\#316](https://github.com/MvvmCross/MvvmCross/issues/316)
- AssetImagePath on ImageView no longer works [\#311](https://github.com/MvvmCross/MvvmCross/issues/311)
- MvxEnumerableExtensions.GetPosition - The operator '==' is not having the expected result \(IMHO\) [\#309](https://github.com/MvvmCross/MvvmCross/issues/309)

**Closed issues:**

- Incorrect WindowsStore path in SQLite nuspec [\#369](https://github.com/MvvmCross/MvvmCross/issues/369)
- Unify virtual use of DataContext property [\#368](https://github.com/MvvmCross/MvvmCross/issues/368)
- Skipping serialization of property Chars passing a string as parameter in ShowViewModel [\#364](https://github.com/MvvmCross/MvvmCross/issues/364)
- Geolocation plugin does not start GPS on real device \(SIII\) any idea ? [\#360](https://github.com/MvvmCross/MvvmCross/issues/360)
- Consider a ClearBindings API [\#358](https://github.com/MvvmCross/MvvmCross/issues/358)
- MvxInvertedVisibilityValueConverter null ref in VS 2012 designer  [\#357](https://github.com/MvvmCross/MvvmCross/issues/357)
- Make MessageToken's disposable  [\#343](https://github.com/MvvmCross/MvvmCross/issues/343)
- Xaml based loading of ValueConverter for BindingEx [\#342](https://github.com/MvvmCross/MvvmCross/issues/342)
- Alignment does not work for StyledStringElements [\#328](https://github.com/MvvmCross/MvvmCross/issues/328)
- Auto ICommands [\#301](https://github.com/MvvmCross/MvvmCross/issues/301)
- Declarative binding dependencies [\#300](https://github.com/MvvmCross/MvvmCross/issues/300)
- Rio-based ViewModels [\#299](https://github.com/MvvmCross/MvvmCross/issues/299)
- Tibet Binding [\#298](https://github.com/MvvmCross/MvvmCross/issues/298)
- Sort out FallbackValue execution path [\#297](https://github.com/MvvmCross/MvvmCross/issues/297)
- MvxTouchBindingBuilder has duplicate registry entry for UISlider [\#295](https://github.com/MvvmCross/MvvmCross/issues/295)
- folder and namespace name is not the same [\#270](https://github.com/MvvmCross/MvvmCross/issues/270)

## [3.0.9](https://github.com/MvvmCross/MvvmCross/tree/3.0.9) (2013-07-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/Release-3.0.8.1...3.0.9)

**Fixed bugs:**

- Suspected issue in Indexed property binding [\#345](https://github.com/MvvmCross/MvvmCross/issues/345)
- Check OneTime binding implementation [\#310](https://github.com/MvvmCross/MvvmCross/issues/310)
- No WPF assemblies in Nuget plugins packages [\#267](https://github.com/MvvmCross/MvvmCross/issues/267)

**Closed issues:**

- Can I use MvxNotifyPropertyChanged as a base class for Design Time Data [\#347](https://github.com/MvvmCross/MvvmCross/issues/347)
- Possible memory leak binding to a command in iOS [\#340](https://github.com/MvvmCross/MvvmCross/issues/340)
- Problem with FloatElement in Droid Dialog [\#338](https://github.com/MvvmCross/MvvmCross/issues/338)
- Email plugin Android implementation MIME type is backwards [\#336](https://github.com/MvvmCross/MvvmCross/issues/336)
- MvxListView does not allow header/footer views. [\#331](https://github.com/MvvmCross/MvvmCross/issues/331)
- Can you use Reactive Extensions with MVVM Cross? [\#330](https://github.com/MvvmCross/MvvmCross/issues/330)
- Consider making central ValueConverter registration more efficient \( minor\) [\#327](https://github.com/MvvmCross/MvvmCross/issues/327)
- MvxListFragment not worked out? [\#326](https://github.com/MvvmCross/MvvmCross/issues/326)
- Design time support for Visibility and Color [\#323](https://github.com/MvvmCross/MvvmCross/issues/323)
- Make Cirrious.MvvmCross.Test.Core into a Nuget package [\#322](https://github.com/MvvmCross/MvvmCross/issues/322)
- check - can reflection subscribe with eventargs to TEventArgs handler [\#313](https://github.com/MvvmCross/MvvmCross/issues/313)
- Consider adding handler for AccessoryButtonTapped [\#312](https://github.com/MvvmCross/MvvmCross/issues/312)
- \[PATCH\] BugFix: correct dependeny for cirrous.crosscore.droid project. [\#305](https://github.com/MvvmCross/MvvmCross/issues/305)
- IMvxCombinedPictureChooserTask causes Windows Store Catastrophic Failure [\#303](https://github.com/MvvmCross/MvvmCross/issues/303)
- Consider adding a general ItemsSource interface for simpler binding [\#279](https://github.com/MvvmCross/MvvmCross/issues/279)
- A compile error from MvxPropertyChangedListener.cs [\#245](https://github.com/MvvmCross/MvvmCross/issues/245)
- Consider adding a new BindingParameter or BindingTag field [\#238](https://github.com/MvvmCross/MvvmCross/issues/238)
- Consider adding common LinkerPleaseInclude.cs [\#236](https://github.com/MvvmCross/MvvmCross/issues/236)
- unity3d mvvmcross porting thread [\#215](https://github.com/MvvmCross/MvvmCross/issues/215)
- Base bound views needed for iOS and Android [\#201](https://github.com/MvvmCross/MvvmCross/issues/201)
- VeeThree - Provide a helper generic class for Computed Observable Properties [\#158](https://github.com/MvvmCross/MvvmCross/issues/158)
- Razor HTML Application Plugin [\#139](https://github.com/MvvmCross/MvvmCross/issues/139)
- Possible problems with the HTTP Image caching code [\#69](https://github.com/MvvmCross/MvvmCross/issues/69)

**Merged pull requests:**

- Droid dialog improvements [\#344](https://github.com/MvvmCross/MvvmCross/pull/344) ([hhubschle](https://github.com/hhubschle))
- the ITableQuery abstraction in SQLite plugin prevents you from running anything but `select \*` [\#341](https://github.com/MvvmCross/MvvmCross/pull/341) ([robfe](https://github.com/robfe))
- Fallback value changes [\#318](https://github.com/MvvmCross/MvvmCross/pull/318) ([slodge](https://github.com/slodge))
- Merge Targetframeworkfixes [\#308](https://github.com/MvvmCross/MvvmCross/pull/308) ([slodge](https://github.com/slodge))

## [Release-3.0.8.1](https://github.com/MvvmCross/MvvmCross/tree/Release-3.0.8.1) (2013-06-09)
**Fixed bugs:**

- adding an object to an ObservableCollection bound to a MvxLinearLayout makes two UI items appear [\#296](https://github.com/MvvmCross/MvvmCross/issues/296)
- MvxSplashScreenActivity can be instantiated multiple times during instantiation [\#274](https://github.com/MvvmCross/MvvmCross/issues/274)
- Monodroid Dialog out of date [\#273](https://github.com/MvvmCross/MvvmCross/issues/273)
- Language Converter still not registered [\#261](https://github.com/MvvmCross/MvvmCross/issues/261)
- merge/commit issue with MvxPropertyChangedListener.cs [\#258](https://github.com/MvvmCross/MvvmCross/issues/258)
- iOS5 Location doesn't work [\#256](https://github.com/MvvmCross/MvvmCross/issues/256)
- LoadStateBundle in WinRT Page throws a cast exception because of Dictionary type mismatch [\#227](https://github.com/MvvmCross/MvvmCross/issues/227)
- MvxSpinner still working in v3?  [\#207](https://github.com/MvvmCross/MvvmCross/issues/207)
- SQLiteNet.cs link broken in Cirrious.MvvmCross.Plugins.Sqlite.Console [\#198](https://github.com/MvvmCross/MvvmCross/issues/198)
- Check  OnChildViewRemoved in LinearLayout - is it leaking [\#194](https://github.com/MvvmCross/MvvmCross/issues/194)
- SQLite won't load under new file naming conventions for WP7 [\#192](https://github.com/MvvmCross/MvvmCross/issues/192)
- Subscriptions Need patching in the messenger plugin - they are too weak [\#164](https://github.com/MvvmCross/MvvmCross/issues/164)
- VeeThree - Patch up the Conference sample for leaks [\#162](https://github.com/MvvmCross/MvvmCross/issues/162)
- AutoComplete sample no longer seems to work [\#145](https://github.com/MvvmCross/MvvmCross/issues/145)
- Consider re-removing ExecuteScalar\<T\> from ISQLiteConnection [\#137](https://github.com/MvvmCross/MvvmCross/issues/137)
- MvxShareTask constructor update [\#124](https://github.com/MvvmCross/MvvmCross/issues/124)
- VeeThree: Implement Touch without Generic UIViewControllers [\#118](https://github.com/MvvmCross/MvvmCross/issues/118)
- Unable to use Email/Photo plugins on touch [\#116](https://github.com/MvvmCross/MvvmCross/issues/116)
- Sort out dispose of \_locationManager [\#93](https://github.com/MvvmCross/MvvmCross/issues/93)
- Touch should support RequestRemoveBackStep [\#80](https://github.com/MvvmCross/MvvmCross/issues/80)
- Mismatch between Attributes [\#76](https://github.com/MvvmCross/MvvmCross/issues/76)
- Adapter is not correctly settable in BindingLinearLayout [\#73](https://github.com/MvvmCross/MvvmCross/issues/73)
- Exists method bug with the File Plugin WinRT implementation [\#72](https://github.com/MvvmCross/MvvmCross/issues/72)
- TriggerFirstNavigate should be protected virtual [\#71](https://github.com/MvvmCross/MvvmCross/issues/71)
- Friend access was granted to System.Net [\#60](https://github.com/MvvmCross/MvvmCross/issues/60)
- MvxActionBasedBindableTableViewSource.GetOrCreateCell For in MonoTouch [\#57](https://github.com/MvvmCross/MvvmCross/issues/57)
- Touch missing ICommand \(still\) [\#56](https://github.com/MvvmCross/MvvmCross/issues/56)
- Problems with master version for winRT [\#47](https://github.com/MvvmCross/MvvmCross/issues/47)
- Android apps loading without a splashscreen [\#46](https://github.com/MvvmCross/MvvmCross/issues/46)
- In old tree \(master\) you can't override an IoC registration [\#43](https://github.com/MvvmCross/MvvmCross/issues/43)
- Unable to work with ViewModel property in monodroid [\#41](https://github.com/MvvmCross/MvvmCross/issues/41)
- Android: MvxBindableListItemView instances leaking [\#17](https://github.com/MvvmCross/MvvmCross/issues/17)
- MvxBind: Warning:  15.04 Unable to bind to source is null [\#7](https://github.com/MvvmCross/MvvmCross/issues/7)

**Closed issues:**

- MvxColor values for ARGB better in byte [\#302](https://github.com/MvvmCross/MvvmCross/issues/302)
- DateTime Elements don't quite match [\#293](https://github.com/MvvmCross/MvvmCross/issues/293)
- Doc updated needed for sqllite-plugin wiki page [\#292](https://github.com/MvvmCross/MvvmCross/issues/292)
- Android: hitting back button doesn't close the app [\#291](https://github.com/MvvmCross/MvvmCross/issues/291)
- In an Android library, Hot Tuna Starter Pack NuGet package adds classes & resources meant for application [\#289](https://github.com/MvvmCross/MvvmCross/issues/289)
- ShowViewModel and ViewModels in multiple assemblies in v3 [\#287](https://github.com/MvvmCross/MvvmCross/issues/287)
- Not sure if this is considered an issue, I'd just like to start contributing :-\) [\#286](https://github.com/MvvmCross/MvvmCross/issues/286)
- GetPosition should be virtual in MvxAdapter [\#285](https://github.com/MvvmCross/MvvmCross/issues/285)
- Request for easier ways to create views [\#284](https://github.com/MvvmCross/MvvmCross/issues/284)
- Make windows language bindings prettier - maybe need to look at attached properties for this? [\#283](https://github.com/MvvmCross/MvvmCross/issues/283)
- Duplicate: ItemLongClick - see \#281 instead [\#282](https://github.com/MvvmCross/MvvmCross/issues/282)
- PropertyChangedEventArgs with NullOrEmpty PropertyName [\#280](https://github.com/MvvmCross/MvvmCross/issues/280)
- Nice to have - a nice SelectedItem binding for UITableView [\#278](https://github.com/MvvmCross/MvvmCross/issues/278)
- Nice to have - a nice binding for UIPickerView [\#277](https://github.com/MvvmCross/MvvmCross/issues/277)
- MvxValueConverter argument names [\#276](https://github.com/MvvmCross/MvvmCross/issues/276)
- Consider using Trace rather than Debug WriteLine [\#275](https://github.com/MvvmCross/MvvmCross/issues/275)
- MvxAndroidSetupSingleton is not threadsafe [\#271](https://github.com/MvvmCross/MvvmCross/issues/271)
- MVVMCross Nuget package issue [\#268](https://github.com/MvvmCross/MvvmCross/issues/268)
- Messenger subscriber not invoked [\#266](https://github.com/MvvmCross/MvvmCross/issues/266)
- Check WeakReferences in custom bindings [\#265](https://github.com/MvvmCross/MvvmCross/issues/265)
- Inconsistencies In Language Binding [\#264](https://github.com/MvvmCross/MvvmCross/issues/264)
- nuget content files for Touch need some tidying [\#262](https://github.com/MvvmCross/MvvmCross/issues/262)
- IMvxMainThreadDispatcher is inconsistent across plattforms [\#260](https://github.com/MvvmCross/MvvmCross/issues/260)
- Add IsolatedStorage Image converter to WP plugin? [\#255](https://github.com/MvvmCross/MvvmCross/issues/255)
- Avoid using IoC like a ServiceLocator, make Mvx.Resolve internal to MvvmCross [\#253](https://github.com/MvvmCross/MvvmCross/issues/253)
- MvxFragmentActivity with an assigned theme crashes [\#251](https://github.com/MvvmCross/MvvmCross/issues/251)
- Identical file name is a compile time error in Unity [\#250](https://github.com/MvvmCross/MvvmCross/issues/250)
- DetailText in MvxTableViewCel null exception [\#249](https://github.com/MvvmCross/MvvmCross/issues/249)
- At some point would be nice if Touch created the presenter automatically in setup.... [\#247](https://github.com/MvvmCross/MvvmCross/issues/247)
- Suggesting backward compatibility of MvxViewModelViewLookupBuilder.cs [\#246](https://github.com/MvvmCross/MvvmCross/issues/246)
- iOS5 MvxSimpleTableView [\#243](https://github.com/MvvmCross/MvvmCross/issues/243)
- NuGet 2.5 Package will not add to PCL [\#237](https://github.com/MvvmCross/MvvmCross/issues/237)
- Code files added by NuGet packages shouldn't need the namespace to be changed manually [\#235](https://github.com/MvvmCross/MvvmCross/issues/235)
- Suggest change to MvxNotifyPropertyChanged to allow raising derived PropertyChangedEventArgs [\#231](https://github.com/MvvmCross/MvvmCross/issues/231)
- Kill the wiki [\#230](https://github.com/MvvmCross/MvvmCross/issues/230)
- Passing parameters to ViewModels in RequestNavigate only works for primitives and registered IoC's [\#228](https://github.com/MvvmCross/MvvmCross/issues/228)
- Recreating ViewModel when navigating back in Win8 RT Page [\#226](https://github.com/MvvmCross/MvvmCross/issues/226)
- Static string to type should be interface [\#225](https://github.com/MvvmCross/MvvmCross/issues/225)
- setup.Initialize throws MissingMethodException [\#224](https://github.com/MvvmCross/MvvmCross/issues/224)
- Can't pass nullable type in ViewModel Init function [\#220](https://github.com/MvvmCross/MvvmCross/issues/220)
- Example code of ChangePresentation for view model to close its view. [\#218](https://github.com/MvvmCross/MvvmCross/issues/218)
- Multiple presenters in the single App ? [\#217](https://github.com/MvvmCross/MvvmCross/issues/217)
- MvxStringToTypeParser TypeSupported don't support float. [\#216](https://github.com/MvvmCross/MvvmCross/issues/216)
- Request for a big fat 'all plugins' plugin [\#214](https://github.com/MvvmCross/MvvmCross/issues/214)
- Accelerometer PLugin for win store needs tidying up [\#211](https://github.com/MvvmCross/MvvmCross/issues/211)
- PictureChooser needed on WindowsStore [\#210](https://github.com/MvvmCross/MvvmCross/issues/210)
- MvxColor needs a diet [\#209](https://github.com/MvvmCross/MvvmCross/issues/209)
- Value Converter naming consistency [\#208](https://github.com/MvvmCross/MvvmCross/issues/208)
- Plugins need configuration [\#206](https://github.com/MvvmCross/MvvmCross/issues/206)
- LoadAllPlugins\(\) [\#205](https://github.com/MvvmCross/MvvmCross/issues/205)
- IoC container by convention [\#204](https://github.com/MvvmCross/MvvmCross/issues/204)
- Fluent bindings please [\#203](https://github.com/MvvmCross/MvvmCross/issues/203)
- Test that MvvmCross v3, Xamarin.iOS and Xamarin.Android, and NuGet \(v2.5+\) work well together [\#202](https://github.com/MvvmCross/MvvmCross/issues/202)
- Apply Convention over Configuration to ValueConverters? [\#200](https://github.com/MvvmCross/MvvmCross/issues/200)
- Add a Commands collection to the MvxViewModel? [\#199](https://github.com/MvvmCross/MvvmCross/issues/199)
- Consider if some coroutine or other approach might be useful [\#197](https://github.com/MvvmCross/MvvmCross/issues/197)
- Should be possible to workaround UITabBarController weird ViewDidLoad sequence [\#196](https://github.com/MvvmCross/MvvmCross/issues/196)
- Make View-ViewModel linking easier [\#195](https://github.com/MvvmCross/MvvmCross/issues/195)
- Change UI threading on Android [\#193](https://github.com/MvvmCross/MvvmCross/issues/193)
- Add IObservableCollection to the Core [\#191](https://github.com/MvvmCross/MvvmCross/issues/191)
- Problem with charset in SQLite-net plugin / Droid [\#190](https://github.com/MvvmCross/MvvmCross/issues/190)
- Retest - can Assembly.Load be used on non-Droid platforms [\#189](https://github.com/MvvmCross/MvvmCross/issues/189)
- Newtonsoft.Json.dll compilation problems [\#187](https://github.com/MvvmCross/MvvmCross/issues/187)
- Remove samples from core repo [\#186](https://github.com/MvvmCross/MvvmCross/issues/186)
- Plugins wrong path in \_NoSample.sln [\#185](https://github.com/MvvmCross/MvvmCross/issues/185)
- Source code compilation [\#184](https://github.com/MvvmCross/MvvmCross/issues/184)
- Should we move more code to CoreCross [\#183](https://github.com/MvvmCross/MvvmCross/issues/183)
- WPF Twitter search opens 2 main windows [\#181](https://github.com/MvvmCross/MvvmCross/issues/181)
- Subscribe on ui thread request for messenger - needed [\#176](https://github.com/MvvmCross/MvvmCross/issues/176)
- Lifecycle views Management  [\#174](https://github.com/MvvmCross/MvvmCross/issues/174)
- VisibilityBinding for Touch  [\#173](https://github.com/MvvmCross/MvvmCross/issues/173)
- SqlBitsX sample isn't good sample of droid lifecycle [\#172](https://github.com/MvvmCross/MvvmCross/issues/172)
- WPF Support [\#171](https://github.com/MvvmCross/MvvmCross/issues/171)
- VeeThree - Guids in Navigation [\#170](https://github.com/MvvmCross/MvvmCross/issues/170)
- Xamarin 2.0 ? [\#168](https://github.com/MvvmCross/MvvmCross/issues/168)
- VeeThree - consider making it even easier to add bindings to a UIView [\#167](https://github.com/MvvmCross/MvvmCross/issues/167)
- VeeThree - View level long click support [\#165](https://github.com/MvvmCross/MvvmCross/issues/165)
- VeeThree - repair damage to OpenNetCf \(PhoneFx did thsi?\) [\#163](https://github.com/MvvmCross/MvvmCross/issues/163)
- Improve Presenter pattern on WinPhone and WinRT [\#160](https://github.com/MvvmCross/MvvmCross/issues/160)
- VeeThree - include some Color bindings by default [\#159](https://github.com/MvvmCross/MvvmCross/issues/159)
- VeeThree - Consider changing Binding registration [\#157](https://github.com/MvvmCross/MvvmCross/issues/157)
- VeeThree - Make it easier to override the Views folder name in WinPhone [\#154](https://github.com/MvvmCross/MvvmCross/issues/154)
- VeeThree - IoC consider options... [\#153](https://github.com/MvvmCross/MvvmCross/issues/153)
- DatePicker and TimePicker binding in Droid [\#152](https://github.com/MvvmCross/MvvmCross/issues/152)
- VeeThree - auto-support 2-way bindings to all UIViews and all Android Widgets with Value and ValueChanged [\#151](https://github.com/MvvmCross/MvvmCross/issues/151)
- VeeThree - Consider adding a DoOnUIthread extension method [\#150](https://github.com/MvvmCross/MvvmCross/issues/150)
- VeeThree - IoC Consider adding a Create\<T\> call to IoC [\#149](https://github.com/MvvmCross/MvvmCross/issues/149)
- Allow viewmodels to correct set property values [\#147](https://github.com/MvvmCross/MvvmCross/issues/147)
- VeeThree - can Tombstoning be supported? [\#144](https://github.com/MvvmCross/MvvmCross/issues/144)
- Android Action Bar Usage [\#141](https://github.com/MvvmCross/MvvmCross/issues/141)
- If you reference MvxRelayCommand in a MonoTouch project, you will get compile error about 'System.Windows.Input.ICommand' is defined in an assembly that is not referenced [\#140](https://github.com/MvvmCross/MvvmCross/issues/140)
- VeeThree - Cirrious.OverIt [\#138](https://github.com/MvvmCross/MvvmCross/issues/138)
- MvxBindableListView.ExecuteCommandOnItem please mark as virtual [\#136](https://github.com/MvvmCross/MvvmCross/issues/136)
- VeeThree: Consider changes to plugins - EnsureLoaded is a PITA [\#134](https://github.com/MvvmCross/MvvmCross/issues/134)
- vnext branch on Monotouch: Tutorial.UI.Touch + Cirricious.Conference.UI.Touch + TwitterSearch.UI.Touch [\#132](https://github.com/MvvmCross/MvvmCross/issues/132)
- Consider supporting -1, -2 indicies in binding [\#131](https://github.com/MvvmCross/MvvmCross/issues/131)
- Switch demos to TouchUpInside [\#128](https://github.com/MvvmCross/MvvmCross/issues/128)
- Remove IMvxServiceConsumer\<T\> and IMvxServiceProducer\<T\> [\#123](https://github.com/MvvmCross/MvvmCross/issues/123)
- Sqlite on WindowsPhone [\#120](https://github.com/MvvmCross/MvvmCross/issues/120)
- VeeThree: Breaking changes to UITableView and its Cell [\#119](https://github.com/MvvmCross/MvvmCross/issues/119)
- Make MvxTouchImageView better known [\#117](https://github.com/MvvmCross/MvvmCross/issues/117)
- Unable to sync my commit [\#114](https://github.com/MvvmCross/MvvmCross/issues/114)
- Workaround for slow solution loading in VS2012 [\#113](https://github.com/MvvmCross/MvvmCross/issues/113)
- Could we drop most of the 'old' ViewModelLocator code? [\#111](https://github.com/MvvmCross/MvvmCross/issues/111)
- Binding to Indexed Item \(Dictionary, array, and custom classes\) [\#110](https://github.com/MvvmCross/MvvmCross/issues/110)
- MvxBaseBindableTableViewSource.ReloadTableData [\#109](https://github.com/MvvmCross/MvvmCross/issues/109)
- File plugin could file-share the base file implementation again [\#106](https://github.com/MvvmCross/MvvmCross/issues/106)
- Consider changing to new DequeueCell API [\#101](https://github.com/MvvmCross/MvvmCross/issues/101)
- Support ios6 collection views [\#100](https://github.com/MvvmCross/MvvmCross/issues/100)
- Add accelerometer plugin from BallControl [\#97](https://github.com/MvvmCross/MvvmCross/issues/97)
- Change geo plugin to allow nullable speed, altittude, etc [\#94](https://github.com/MvvmCross/MvvmCross/issues/94)
- ios6 changes - LocationsUpdated [\#92](https://github.com/MvvmCross/MvvmCross/issues/92)
- Is HackReadValue still needed in MvxAndroidGeoLocationWatcher [\#91](https://github.com/MvvmCross/MvvmCross/issues/91)
- Simplify Binding Description Language  [\#87](https://github.com/MvvmCross/MvvmCross/issues/87)
- IObservableMap - can it be added? [\#84](https://github.com/MvvmCross/MvvmCross/issues/84)
- Provide a simple StartApplicationObject [\#83](https://github.com/MvvmCross/MvvmCross/issues/83)
- Sort out build tree on master [\#82](https://github.com/MvvmCross/MvvmCross/issues/82)
- Add PCL MonoDroid and VSMonoTouch xml files to the source tree as references [\#81](https://github.com/MvvmCross/MvvmCross/issues/81)
- Touch Platform Properties Enhancement [\#79](https://github.com/MvvmCross/MvvmCross/issues/79)
- Add support for Xamarin.Mac [\#77](https://github.com/MvvmCross/MvvmCross/issues/77)
- Sort out unit test in TwitterSearch sample [\#75](https://github.com/MvvmCross/MvvmCross/issues/75)
- Sort out WinRT build paths [\#70](https://github.com/MvvmCross/MvvmCross/issues/70)
- Rejig the simple code so that plugins can be used non-mvvm [\#68](https://github.com/MvvmCross/MvvmCross/issues/68)
- Add Color Extension methods for all platforms [\#67](https://github.com/MvvmCross/MvvmCross/issues/67)
- Consider porting the MvxPage approach from WinRT back to WP [\#66](https://github.com/MvvmCross/MvvmCross/issues/66)
- Allow simpler language binding expressions in Droid [\#63](https://github.com/MvvmCross/MvvmCross/issues/63)
- Allow simpler binding expressions in Droid [\#62](https://github.com/MvvmCross/MvvmCross/issues/62)
- Add a Messenger plugin [\#61](https://github.com/MvvmCross/MvvmCross/issues/61)
- Binary releases [\#59](https://github.com/MvvmCross/MvvmCross/issues/59)
- Localization improvement [\#58](https://github.com/MvvmCross/MvvmCross/issues/58)
- Android needs fragment support now [\#54](https://github.com/MvvmCross/MvvmCross/issues/54)
- Localization improvement: Check if standard language exists then process [\#53](https://github.com/MvvmCross/MvvmCross/issues/53)
- Sort out SelectedItem binding in Droid [\#52](https://github.com/MvvmCross/MvvmCross/issues/52)
- Build to root level directories [\#51](https://github.com/MvvmCross/MvvmCross/issues/51)
- VeeThree - use lightweight text parser for Urls and Intents [\#50](https://github.com/MvvmCross/MvvmCross/issues/50)
- Allow value types in the ViewModel navigation [\#45](https://github.com/MvvmCross/MvvmCross/issues/45)
- There's no way to register a singleton without first creating an instance of it [\#44](https://github.com/MvvmCross/MvvmCross/issues/44)
- Touch missing ICommand? [\#42](https://github.com/MvvmCross/MvvmCross/issues/42)
- MvxTouchTabBarViewController and binding to badge-property [\#40](https://github.com/MvvmCross/MvvmCross/issues/40)
- Notification of update between View and ViewModel [\#39](https://github.com/MvvmCross/MvvmCross/issues/39)
- MvxBindableListView vnext monodroid: binding to any Collection [\#38](https://github.com/MvvmCross/MvvmCross/issues/38)
- MvxBindableGridView [\#37](https://github.com/MvvmCross/MvvmCross/issues/37)
- Problems with generic uiviewcontroller inheritance in 6.0.0.2 of MonoTouch [\#36](https://github.com/MvvmCross/MvvmCross/issues/36)
- iPhone - tutorial solution - Device screen rotation doesn't occur with iOS 6.0 under simulator \(iphone 5 device not tried\) [\#34](https://github.com/MvvmCross/MvvmCross/issues/34)
- Releasing memory of viewmodels [\#33](https://github.com/MvvmCross/MvvmCross/issues/33)
- Binding ignores layout parameters [\#31](https://github.com/MvvmCross/MvvmCross/issues/31)
- Cant deploy Currious Conference to Andorid [\#30](https://github.com/MvvmCross/MvvmCross/issues/30)
- Binded properties are being set multiple times [\#29](https://github.com/MvvmCross/MvvmCross/issues/29)
- Binding IList\<T\> conversion errors [\#27](https://github.com/MvvmCross/MvvmCross/issues/27)
- Support 2-way binding for all ValueElement\<T\> in the Dialog [\#26](https://github.com/MvvmCross/MvvmCross/issues/26)
- Dialog.Touch: StyledStringElement TextLabel background is black when extraInfo is null [\#22](https://github.com/MvvmCross/MvvmCross/issues/22)
- SelectionChanged event not firing when subscribed to Scrolled event [\#21](https://github.com/MvvmCross/MvvmCross/issues/21)
- MvxBindingTouchViewController inside MvxTouchTabBarViewController seem to leak memory on MonoTouch [\#19](https://github.com/MvvmCross/MvvmCross/issues/19)
- Should MvxSimpleBindingActivity have a MvxUnconventionalViewAttribute ? [\#18](https://github.com/MvvmCross/MvvmCross/issues/18)
- Image picker in iOS needs close code changed [\#16](https://github.com/MvvmCross/MvvmCross/issues/16)
- Binding numeric types [\#15](https://github.com/MvvmCross/MvvmCross/issues/15)
- How can i add new Custom Element to ViewModel [\#14](https://github.com/MvvmCross/MvvmCross/issues/14)
- Possible NullReferenceException in DateTimeElement in Touch Dialog [\#13](https://github.com/MvvmCross/MvvmCross/issues/13)
- MvxPictureChooserTask & MfA 4.2.2 [\#12](https://github.com/MvvmCross/MvvmCross/issues/12)
- MvxAndroidViewPresenter Close method bug [\#11](https://github.com/MvvmCross/MvvmCross/issues/11)
- Bindings don't work when changing TextField values in code [\#10](https://github.com/MvvmCross/MvvmCross/issues/10)
- Unable to build Cirrious.MvvmCross.Console [\#8](https://github.com/MvvmCross/MvvmCross/issues/8)
- Android.Widget.ItemEventArg is obsolete Use AdaptorView.ItemClickEventArgs instead [\#6](https://github.com/MvvmCross/MvvmCross/issues/6)
- Suggestion: Extend FullPath method in MvxBaseFileStoreService [\#4](https://github.com/MvvmCross/MvvmCross/issues/4)
- Subscription for INotifyPropertyChanged [\#2](https://github.com/MvvmCross/MvvmCross/issues/2)

**Merged pull requests:**

- All elements should have an empty constructor \(all parameters optional\),... [\#288](https://github.com/MvvmCross/MvvmCross/pull/288) ([csteeg](https://github.com/csteeg))
- Added link to N+1 Days of MvvmCross [\#241](https://github.com/MvvmCross/MvvmCross/pull/241) ([RobGibbens](https://github.com/RobGibbens))
- Enable Strong Naming for Cirrious.MvvmCross and Cirrious.CrossCore [\#233](https://github.com/MvvmCross/MvvmCross/pull/233) ([jonstelly](https://github.com/jonstelly))
- Update readme.md [\#223](https://github.com/MvvmCross/MvvmCross/pull/223) ([RobGibbens](https://github.com/RobGibbens))
- Added back the MvvmCross\_NoSamples solution. [\#222](https://github.com/MvvmCross/MvvmCross/pull/222) ([Cheesebaron](https://github.com/Cheesebaron))
- Added Email plug-in for WPF [\#221](https://github.com/MvvmCross/MvvmCross/pull/221) ([SeanCross](https://github.com/SeanCross))
- MvxObservableCollection with unit-tests [\#219](https://github.com/MvvmCross/MvvmCross/pull/219) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- Adding DrawableId binding for binding image views to drawable resource I... [\#179](https://github.com/MvvmCross/MvvmCross/pull/179) ([steveydee82](https://github.com/steveydee82))
- Another memory leak fix [\#177](https://github.com/MvvmCross/MvvmCross/pull/177) ([steveydee82](https://github.com/steveydee82))
- Fixing bitmap memory leak [\#175](https://github.com/MvvmCross/MvvmCross/pull/175) ([steveydee82](https://github.com/steveydee82))
- Added support for passing Guid parameters into ViewModels [\#169](https://github.com/MvvmCross/MvvmCross/pull/169) ([h2oman](https://github.com/h2oman))
- More elements [\#148](https://github.com/MvvmCross/MvvmCross/pull/148) ([jacobtoye](https://github.com/jacobtoye))
- Make EntryElement.BecomeFirstResponder\(bool\) virtual so it can be overridden [\#142](https://github.com/MvvmCross/MvvmCross/pull/142) ([danzel](https://github.com/danzel))
- Added SQLite mappings [\#129](https://github.com/MvvmCross/MvvmCross/pull/129) ([asednev](https://github.com/asednev))
- Duplicate folders VIews and Views renamed due to case sentivity problem ... [\#127](https://github.com/MvvmCross/MvvmCross/pull/127) ([Alphapage](https://github.com/Alphapage))
- Allow Email/Image tasks to work on Touch.  [\#121](https://github.com/MvvmCross/MvvmCross/pull/121) ([danzel](https://github.com/danzel))
- Fix Sqlite ExecuteScalar regression. [\#115](https://github.com/MvvmCross/MvvmCross/pull/115) ([jacobtoye](https://github.com/jacobtoye))
- Add ExecuteScalar to Sqlite plugin [\#107](https://github.com/MvvmCross/MvvmCross/pull/107) ([danzel](https://github.com/danzel))
- Add FolderExists and DeleteFolder to IMvxSimpleFileStoreService. [\#104](https://github.com/MvvmCross/MvvmCross/pull/104) ([danzel](https://github.com/danzel))
- Fix for RootElement Sections list [\#103](https://github.com/MvvmCross/MvvmCross/pull/103) ([jacobtoye](https://github.com/jacobtoye))
- Add a Console implementation of Sqlite [\#102](https://github.com/MvvmCross/MvvmCross/pull/102) ([danzel](https://github.com/danzel))
- CheckboxElement: check if UITableViewCell is null before setting cell Accessory [\#99](https://github.com/MvvmCross/MvvmCross/pull/99) ([h2oman](https://github.com/h2oman))
- Add more specific type constraints on IoC registration [\#89](https://github.com/MvvmCross/MvvmCross/pull/89) ([gshackles](https://github.com/gshackles))
- V next dialog [\#88](https://github.com/MvvmCross/MvvmCross/pull/88) ([slodge](https://github.com/slodge))
- Changed output paths for projects, to root. [\#86](https://github.com/MvvmCross/MvvmCross/pull/86) ([Cheesebaron](https://github.com/Cheesebaron))
- Added MvvmCross\_All solution and added WinRT to MvvmCrossLibs solution [\#85](https://github.com/MvvmCross/MvvmCross/pull/85) ([Cheesebaron](https://github.com/Cheesebaron))
- Merge Changes back into vnext [\#78](https://github.com/MvvmCross/MvvmCross/pull/78) ([slodge](https://github.com/slodge))
- Basic NuSpec file for MvvmCross [\#65](https://github.com/MvvmCross/MvvmCross/pull/65) ([dsplaisted](https://github.com/dsplaisted))
- Exclude abstract classes from view lookup \(master\) [\#49](https://github.com/MvvmCross/MvvmCross/pull/49) ([gshackles](https://github.com/gshackles))
- Exclude abstract classes from view lookup [\#48](https://github.com/MvvmCross/MvvmCross/pull/48) ([gshackles](https://github.com/gshackles))
- Localvnwxt [\#32](https://github.com/MvvmCross/MvvmCross/pull/32) ([slodge](https://github.com/slodge))
- MvxTouchImageView [\#24](https://github.com/MvvmCross/MvvmCross/pull/24) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- Fixed Element.Bind method with custom source [\#23](https://github.com/MvvmCross/MvvmCross/pull/23) ([SeeD-Seifer](https://github.com/SeeD-Seifer))
- FirePropertyChanged\(\(\) =\> PropertyName\) syntax support for change notifications in ViewModel [\#1](https://github.com/MvvmCross/MvvmCross/pull/1) ([gerich-home](https://github.com/gerich-home))



\* *This Change Log was automatically generated by [github_changelog_generator](https://github.com/skywinder/Github-Changelog-Generator)*