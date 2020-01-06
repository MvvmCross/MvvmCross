# Changelog

## [6.4.2](https://github.com/MvvmCross/MvvmCross/tree/6.4.2) (2020-01-03)

[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.4.1...6.4.2)

**Implemented enhancements:**

- IMvxAndroidBindingResource singleton refactor [\#3649](https://github.com/MvvmCross/MvvmCross/issues/3649)
- Added netcoreapp3.1 support [\#3666](https://github.com/MvvmCross/MvvmCross/pull/3666) ([HaraldMuehlhoffCC](https://github.com/HaraldMuehlhoffCC))
- Add netcoreapp3.0 support [\#3596](https://github.com/MvvmCross/MvvmCross/pull/3596) ([mgochmuradov](https://github.com/mgochmuradov))

**Fixed bugs:**

- Color Plugin: ColorConverter should not convert null values [\#3632](https://github.com/MvvmCross/MvvmCross/issues/3632)
- Possible unhandled NRE in IoCResolver [\#3615](https://github.com/MvvmCross/MvvmCross/issues/3615)
- Running Playground.Wpf crashes with Reflection.TypeLoadException [\#3588](https://github.com/MvvmCross/MvvmCross/issues/3588)
- WPF View lifetime does not handle app closing [\#3481](https://github.com/MvvmCross/MvvmCross/issues/3481)
- MvxApplicationCallbacksCurrentTopActivity is lying about current Activity [\#3455](https://github.com/MvvmCross/MvvmCross/issues/3455)
- Fix loader exception message. [\#3644](https://github.com/MvvmCross/MvvmCross/pull/3644) ([RayMMond](https://github.com/RayMMond))
- 3615 Check for NRE in MvxIocContainer [\#3619](https://github.com/MvvmCross/MvvmCross/pull/3619) ([allexks](https://github.com/allexks))
- Fix NullReferenceException in MvxAndroidBindingContextHelpers [\#3610](https://github.com/MvvmCross/MvvmCross/pull/3610) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix WPF app close view model lifecycle [\#3591](https://github.com/MvvmCross/MvvmCross/pull/3591) ([MartinZikmund](https://github.com/MartinZikmund))
- Getting plugin types safely [\#3590](https://github.com/MvvmCross/MvvmCross/pull/3590) ([MartinZikmund](https://github.com/MartinZikmund))

**Merged pull requests:**

- Downgrade Xamarin.Build.Download to 0.4.11 as latest has a bug [\#3679](https://github.com/MvvmCross/MvvmCross/pull/3679) ([Cheesebaron](https://github.com/Cheesebaron))
- Small Optimizations for Loading Plugins [\#3675](https://github.com/MvvmCross/MvvmCross/pull/3675) ([Strifex](https://github.com/Strifex))
- Mvx bundle optimizations [\#3667](https://github.com/MvvmCross/MvvmCross/pull/3667) ([Strifex](https://github.com/Strifex))
- Fix CurrentTopActivity returning null when App in background [\#3665](https://github.com/MvvmCross/MvvmCross/pull/3665) ([Cheesebaron](https://github.com/Cheesebaron))
- remove duplicated words from readme [\#3657](https://github.com/MvvmCross/MvvmCross/pull/3657) ([mrlacey](https://github.com/mrlacey))
- Refactor \#3649: Change MvxAndroidBindingResource to be registered and used with the IoC [\#3656](https://github.com/MvvmCross/MvvmCross/pull/3656) ([fedemkr](https://github.com/fedemkr))
- MvvmCross.Plugin.Location.Fused refactoring [\#3651](https://github.com/MvvmCross/MvvmCross/pull/3651) ([pinkysek](https://github.com/pinkysek))
- Bump Xamarin.Build.Download from 0.4.11 to 0.7.1 [\#3648](https://github.com/MvvmCross/MvvmCross/pull/3648) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.CodeAnalysis from 3.3.1 to 3.4.0 [\#3646](https://github.com/MvvmCross/MvvmCross/pull/3646) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.SourceLink.GitHub from 1.0.0-beta2-19554-01 to 1.0.0 [\#3641](https://github.com/MvvmCross/MvvmCross/pull/3641) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Newtonsoft.Json from 12.0.2 to 12.0.3 [\#3633](https://github.com/MvvmCross/MvvmCross/pull/3633) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.NET.Test.Sdk from 16.3.0 to 16.4.0 [\#3628](https://github.com/MvvmCross/MvvmCross/pull/3628) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.SourceLink.GitHub from 1.0.0-beta2-19367-01 to 1.0.0-beta2-19554-01 [\#3625](https://github.com/MvvmCross/MvvmCross/pull/3625) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Corrected misspelling of Recycler [\#3613](https://github.com/MvvmCross/MvvmCross/pull/3613) ([pearsonallen](https://github.com/pearsonallen))
- Bump Moq from 4.13.0 to 4.13.1 [\#3608](https://github.com/MvvmCross/MvvmCross/pull/3608) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Edited Installation Instructions  [\#3606](https://github.com/MvvmCross/MvvmCross/pull/3606) ([AceBurton](https://github.com/AceBurton))
- Bump Serilog.Sinks.Xamarin from 0.1.29 to 0.1.37 [\#3604](https://github.com/MvvmCross/MvvmCross/pull/3604) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Serilog from 2.8.0 to 2.9.0 [\#3603](https://github.com/MvvmCross/MvvmCross/pull/3603) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Move signing into own stage [\#3600](https://github.com/MvvmCross/MvvmCross/pull/3600) ([Cheesebaron](https://github.com/Cheesebaron))
- Chore\(readme\): use https [\#3599](https://github.com/MvvmCross/MvvmCross/pull/3599) ([imba-tjd](https://github.com/imba-tjd))
- Android RecyclerView Workaround for GridLayoutManager [\#3598](https://github.com/MvvmCross/MvvmCross/pull/3598) ([dogukandemir](https://github.com/dogukandemir))

## [6.4.1](https://github.com/MvvmCross/MvvmCross/tree/6.4.1) (2019-09-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.4.0...6.4.1)

**Fixed bugs:**

- MvxExpandableTableViewSource doesn't let touches through [\#3574](https://github.com/MvvmCross/MvvmCross/issues/3574)
- MvxLayoutInflater CreateCustomViewInternal fails on Android 10 [\#3550](https://github.com/MvvmCross/MvvmCross/issues/3550)
- NullReferenceException when initializing fragment in ViewPager [\#3535](https://github.com/MvvmCross/MvvmCross/issues/3535)

**Closed issues:**

- Update Fused Location plugin to latest version of Play Services [\#3580](https://github.com/MvvmCross/MvvmCross/issues/3580)
- Nested fragment inside viewpager caused error “Fragment host not found” [\#3380](https://github.com/MvvmCross/MvvmCross/issues/3380)

**Merged pull requests:**

- Fix header cells in MvxExpandableTableViewSource not passing touches to subviews [\#3575](https://github.com/MvvmCross/MvvmCross/pull/3575) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix MvxLayoutInflater throwing NoSuchFieldException on Android Q [\#3573](https://github.com/MvvmCross/MvvmCross/pull/3573) ([Cheesebaron](https://github.com/Cheesebaron))
- Update Play Services in Location Fused plugin [\#3583](https://github.com/MvvmCross/MvvmCross/pull/3583) ([Cheesebaron](https://github.com/Cheesebaron))
- Update nuget packages [\#3582](https://github.com/MvvmCross/MvvmCross/pull/3582) ([martijn00](https://github.com/martijn00))
- Bump to latest version of MSBuild SDK Extras [\#3581](https://github.com/MvvmCross/MvvmCross/pull/3581) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix some comments and obsolete warning [\#3557](https://github.com/MvvmCross/MvvmCross/pull/3557) ([martijn00](https://github.com/martijn00))
- Stop using dynamic [\#3556](https://github.com/MvvmCross/MvvmCross/pull/3556) ([martijn00](https://github.com/martijn00))
- Update Plugins getting-started.md to proper load non explicitly referenced plugins [\#3555](https://github.com/MvvmCross/MvvmCross/pull/3555) ([fedemkr](https://github.com/fedemkr))
- Ci/build script updates [\#3553](https://github.com/MvvmCross/MvvmCross/pull/3553) ([Cheesebaron](https://github.com/Cheesebaron))
- Update android-recyclerview.md [\#3552](https://github.com/MvvmCross/MvvmCross/pull/3552) ([pearsonallen](https://github.com/pearsonallen))
- NullReferenceException when initializing fragment in ViewPager [\#3549](https://github.com/MvvmCross/MvvmCross/pull/3549) ([Prin53](https://github.com/Prin53))
- Views filtering in MvxViewModelViewLookupBuilder [\#3539](https://github.com/MvvmCross/MvvmCross/pull/3539) ([orzech85](https://github.com/orzech85))
- Update github-pages gem and Jekyll [\#3536](https://github.com/MvvmCross/MvvmCross/pull/3536) ([Cheesebaron](https://github.com/Cheesebaron))
- Bump MSBuild.Sdk.Extras from 2.0.41 to 2.0.43 [\#3529](https://github.com/MvvmCross/MvvmCross/pull/3529) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))

## [6.4.0](https://github.com/MvvmCross/MvvmCross/tree/6.4.0) (2019-09-09)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.3.1...6.4.0)

**Implemented enhancements:**

- Prepare MvxIoCTest to allow other IoC providers [\#3484](https://github.com/MvvmCross/MvvmCross/pull/3484) ([SamuelDebruyn](https://github.com/SamuelDebruyn))
- Impliment apply\(\) on dispose of clear binding set [\#3431](https://github.com/MvvmCross/MvvmCross/pull/3431) ([Tyron18](https://github.com/Tyron18))

**Fixed bugs:**

- ViewPager ignores Presentation Values [\#3497](https://github.com/MvvmCross/MvvmCross/issues/3497)
- Modal Popover Presentation Crash  [\#3515](https://github.com/MvvmCross/MvvmCross/issues/3515)
- RequestTranslator ignores Presentation/Parameter values [\#3482](https://github.com/MvvmCross/MvvmCross/issues/3482)
- Prevent null reference when trying to look up latest binding context [\#3518](https://github.com/MvvmCross/MvvmCross/pull/3518) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix ctor name in target binding [\#3511](https://github.com/MvvmCross/MvvmCross/pull/3511) ([Cheesebaron](https://github.com/Cheesebaron))
- RequestTranslator ignores Presentation/Parameter values [\#3487](https://github.com/MvvmCross/MvvmCross/pull/3487) ([Prin53](https://github.com/Prin53))

**Closed issues:**

- MvxSpinner classNotFoundException inside fragment [\#3454](https://github.com/MvvmCross/MvvmCross/issues/3454)
- Bindings not attempted in MvxRecycler item template layout when given item in ItemsSource is null  [\#3424](https://github.com/MvvmCross/MvvmCross/issues/3424)

**Merged pull requests:**

- Switch to Azure Pipelines [\#3397](https://github.com/MvvmCross/MvvmCross/pull/3397) ([Cheesebaron](https://github.com/Cheesebaron))
- MvvmCross 6.4.0 blog post [\#3533](https://github.com/MvvmCross/MvvmCross/pull/3533) ([Cheesebaron](https://github.com/Cheesebaron))
- Update android-spinner.md [\#3530](https://github.com/MvvmCross/MvvmCross/pull/3530) ([SebastienForay](https://github.com/SebastienForay))
- Don't try to sign packages for Pull Requests [\#3525](https://github.com/MvvmCross/MvvmCross/pull/3525) ([Cheesebaron](https://github.com/Cheesebaron))
- Update ios-uirefreshcontrol.md [\#3524](https://github.com/MvvmCross/MvvmCross/pull/3524) ([Nerves82](https://github.com/Nerves82))
- Fix presentation hint for non-forms views in forms presenter [\#3522](https://github.com/MvvmCross/MvvmCross/pull/3522) ([orzech85](https://github.com/orzech85))
- Example of Android Native View for MvvmCross.Forms [\#3521](https://github.com/MvvmCross/MvvmCross/pull/3521) ([orzech85](https://github.com/orzech85))
- Bump Moq from 4.12.0 to 4.13.0 [\#3519](https://github.com/MvvmCross/MvvmCross/pull/3519) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Modal Popover Presentation Crash [\#3516](https://github.com/MvvmCross/MvvmCross/pull/3516) ([Prin53](https://github.com/Prin53))
- Cleanup csproj files [\#3512](https://github.com/MvvmCross/MvvmCross/pull/3512) ([martijn00](https://github.com/martijn00))
- Add more Android Target bindings [\#3510](https://github.com/MvvmCross/MvvmCross/pull/3510) ([Cheesebaron](https://github.com/Cheesebaron))
- Update github-pages gem [\#3509](https://github.com/MvvmCross/MvvmCross/pull/3509) ([Cheesebaron](https://github.com/Cheesebaron))
- ViewPager ignores Presentation Values [\#3501](https://github.com/MvvmCross/MvvmCross/pull/3501) ([Prin53](https://github.com/Prin53))
- Bump MSBuild.Sdk.Extras from 2.0.31 to 2.0.41 [\#3496](https://github.com/MvvmCross/MvvmCross/pull/3496) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.CodeAnalysis from 3.1.0 to 3.2.1 [\#3493](https://github.com/MvvmCross/MvvmCross/pull/3493) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- IocConstruct constructor lookup & constructor injection [\#3491](https://github.com/MvvmCross/MvvmCross/pull/3491) ([SamuelDebruyn](https://github.com/SamuelDebruyn))
- Bump Xamarin.Forms.Platform.WPF from 3.6.0.220655 to 4.1.0.673156 [\#3490](https://github.com/MvvmCross/MvvmCross/pull/3490) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.Forms from 3.6.0.220655 to 4.1.0.673156 [\#3489](https://github.com/MvvmCross/MvvmCross/pull/3489) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Update tizen backend implementation [\#3477](https://github.com/MvvmCross/MvvmCross/pull/3477) ([rookiejava](https://github.com/rookiejava))
- Minor typo [\#3473](https://github.com/MvvmCross/MvvmCross/pull/3473) ([garyng](https://github.com/garyng))
- Bump Microsoft.SourceLink.GitHub from 1.0.0-beta2-19351-01 to 1.0.0-beta2-19367-01 [\#3471](https://github.com/MvvmCross/MvvmCross/pull/3471) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Minor typo in docs [\#3469](https://github.com/MvvmCross/MvvmCross/pull/3469) ([garyng](https://github.com/garyng))
- Bump MSBuild.Sdk.Extras from 2.0.29 to 2.0.31 [\#3468](https://github.com/MvvmCross/MvvmCross/pull/3468) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Breaking up logic for creating view lookup [\#3461](https://github.com/MvvmCross/MvvmCross/pull/3461) ([nickrandolph](https://github.com/nickrandolph))
- Increment Min Android Version [\#3458](https://github.com/MvvmCross/MvvmCross/pull/3458) ([nickrandolph](https://github.com/nickrandolph))
- Replace MvxColor with System.Drawing.Color [\#3456](https://github.com/MvvmCross/MvvmCross/pull/3456) ([Strifex](https://github.com/Strifex))
- Bump Microsoft.SourceLink.GitHub from 1.0.0-beta2-18618-05 to 1.0.0-beta2-19351-01 [\#3451](https://github.com/MvvmCross/MvvmCross/pull/3451) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.NETCore.UniversalWindowsPlatform from 6.2.3 to 6.2.8 [\#3448](https://github.com/MvvmCross/MvvmCross/pull/3448) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump MSBuild.Sdk.Extras from 2.0.24 to 2.0.29 [\#3447](https://github.com/MvvmCross/MvvmCross/pull/3447) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.FFImageLoading from 2.4.9.961 to 2.4.11.982 [\#3446](https://github.com/MvvmCross/MvvmCross/pull/3446) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.CodeAnalysis from 2.10.0 to 3.1.0 [\#3444](https://github.com/MvvmCross/MvvmCross/pull/3444) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.NET.Test.Sdk from 15.9.0 to 16.2.0 [\#3443](https://github.com/MvvmCross/MvvmCross/pull/3443) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Moq from 4.10.1 to 4.12.0 [\#3441](https://github.com/MvvmCross/MvvmCross/pull/3441) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Newtonsoft.Json from 12.0.1 to 12.0.2 [\#3440](https://github.com/MvvmCross/MvvmCross/pull/3440) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Update value-converters.md [\#3437](https://github.com/MvvmCross/MvvmCross/pull/3437) ([An0d](https://github.com/An0d))
- Switch to Azure Pipelines [\#3397](https://github.com/MvvmCross/MvvmCross/pull/3397) ([Cheesebaron](https://github.com/Cheesebaron))
- Tidying up and making startup more consistent [\#3372](https://github.com/MvvmCross/MvvmCross/pull/3372) ([nickrandolph](https://github.com/nickrandolph))

## [6.3.1](https://github.com/MvvmCross/MvvmCross/tree/6.3.1) (2019-06-18)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.3.0...6.3.1)

**Implemented enhancements:**

- Open up methods in PageViewController to be able to customize behavior [\#3406](https://github.com/MvvmCross/MvvmCross/pull/3406) ([martijn00](https://github.com/martijn00))

**Fixed bugs:**

- ios MvxBaseTableViewSource: RowDeselected\(..\) must not call its base method [\#3413](https://github.com/MvvmCross/MvvmCross/issues/3413)
- \[Android\] AppStart deadlocks [\#3268](https://github.com/MvvmCross/MvvmCross/issues/3268)

**Closed issues:**

- Compile issues after updating to 6.3.0 [\#3420](https://github.com/MvvmCross/MvvmCross/issues/3420)

**Merged pull requests:**

- Correct doc from "childs" to "children" [\#3430](https://github.com/MvvmCross/MvvmCross/pull/3430) ([jschmid](https://github.com/jschmid))
- Added MvxPanelHintTypes for MvvmCross 6 [\#3427](https://github.com/MvvmCross/MvvmCross/pull/3427) ([xunreal75](https://github.com/xunreal75))
- Fix: TYPO [\#3426](https://github.com/MvvmCross/MvvmCross/pull/3426) ([konabe](https://github.com/konabe))
- Fix: Typo: Remove extra period [\#3421](https://github.com/MvvmCross/MvvmCross/pull/3421) ([konabe](https://github.com/konabe))
- Make attributes only available on netstandard [\#3419](https://github.com/MvvmCross/MvvmCross/pull/3419) ([martijn00](https://github.com/martijn00))
- Fix for \#3413: RowDeselected\(..\) must not call its base method [\#3415](https://github.com/MvvmCross/MvvmCross/pull/3415) ([markuspalme](https://github.com/markuspalme))
- Update docs on adding sections to a UITableView [\#3410](https://github.com/MvvmCross/MvvmCross/pull/3410) ([c-lamont](https://github.com/c-lamont))
- Facade MvxViewModelInstanceRequest instance overwritten [\#3405](https://github.com/MvvmCross/MvvmCross/pull/3405) ([CMorooney](https://github.com/CMorooney))

## [6.3.0](https://github.com/MvvmCross/MvvmCross/tree/6.3.0) (2019-05-15)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.3...6.3.0)

**Implemented enhancements:**

- Navigating to new Activity always requires a top Activity [\#3311](https://github.com/MvvmCross/MvvmCross/issues/3311)
- CloseFragmentDialog  System.NullReferenceException [\#3310](https://github.com/MvvmCross/MvvmCross/issues/3310)
- Add infos to log when SetValue ignored in binding [\#3298](https://github.com/MvvmCross/MvvmCross/issues/3298)
- Feature: Support YamlLocalization as Plugin. [\#2892](https://github.com/MvvmCross/MvvmCross/issues/2892)

**Fixed bugs:**

- Mvx. namespace shortcut not working in VS2019 16.1 preview2 with D8 and aapt2 enabled [\#3383](https://github.com/MvvmCross/MvvmCross/issues/3383)
- Can not override a built in target binding + fix [\#3352](https://github.com/MvvmCross/MvvmCross/issues/3352)
- Android TextView target binding is using TextView.TextFormatted + fix [\#3351](https://github.com/MvvmCross/MvvmCross/issues/3351)
- MvxGestureRecognizerBehavior should support sending parameters to Command [\#3347](https://github.com/MvvmCross/MvvmCross/issues/3347)
- Follow up to \#1378: NullRef exception in Target Binding when using a value type + fix [\#3343](https://github.com/MvvmCross/MvvmCross/issues/3343)
- MvxBindingLog should not format messages + fix [\#3337](https://github.com/MvvmCross/MvvmCross/issues/3337)
- MvxWindowPresentationAttribute has prohibited parameters types \(nullable structs\). [\#3336](https://github.com/MvvmCross/MvvmCross/issues/3336)
- MvvmCross.Plugin.PictureChooser throws exception with 6.2.3 on device [\#3329](https://github.com/MvvmCross/MvvmCross/issues/3329)
- MvxPageViewController race condition [\#3324](https://github.com/MvvmCross/MvvmCross/issues/3324)
- Xamarin.Forms UWP backnavigation blocks previously pressed button. [\#3306](https://github.com/MvvmCross/MvvmCross/issues/3306)
- MvxBaseTableViewSource dont use SelectionChangedCommand on RowDeselected [\#3305](https://github.com/MvvmCross/MvvmCross/issues/3305)
- MvxPageViewController has changed its page change animation [\#3304](https://github.com/MvvmCross/MvvmCross/issues/3304)
- MvxRecycler view must not post NotifyDataSetChanged on MainLooper [\#3295](https://github.com/MvvmCross/MvvmCross/issues/3295)
- MvxAndroidLifetimeMonitor should lock when accessing \_createdActivityCount [\#3294](https://github.com/MvvmCross/MvvmCross/issues/3294)
- NavigationService doesn't wait for the view to appear in ios [\#3289](https://github.com/MvvmCross/MvvmCross/issues/3289)
- MvxObservableCollection's RemoveRange discrepancy [\#3281](https://github.com/MvvmCross/MvvmCross/issues/3281)
- TryPerformCloseFragmentTransaction is not encapsulated in try/catch [\#3280](https://github.com/MvvmCross/MvvmCross/issues/3280)
- `MvxAndroidViewPresenter.CreateActivityTransitionOptions\(\)` adds shared element transition options if there are no elements to animate [\#3278](https://github.com/MvvmCross/MvvmCross/issues/3278)
- MvxPageViewController doesn't use attributes [\#3230](https://github.com/MvvmCross/MvvmCross/issues/3230)
- First ViewModel being created twice on Android [\#3196](https://github.com/MvvmCross/MvvmCross/issues/3196)
- \[Android\] Bringing application to foreground from Android notification using PendingIntent  [\#3180](https://github.com/MvvmCross/MvvmCross/issues/3180)
- CustomAppStart methods called twice [\#3177](https://github.com/MvvmCross/MvvmCross/issues/3177)
- Proper implementation of UWP suspension [\#3090](https://github.com/MvvmCross/MvvmCross/issues/3090)
- App not starting properly when App is start via Activity [\#3014](https://github.com/MvvmCross/MvvmCross/issues/3014)
- Xamarin.Forms & android notifications [\#2996](https://github.com/MvvmCross/MvvmCross/issues/2996)
- Mvvmcross Android ignores IMvxAsyncCommand on some standard control's events [\#2977](https://github.com/MvvmCross/MvvmCross/issues/2977)
- MvxAndroidSetupSingleton.EnsureSingletonAvailable\(ApplicationContext\) throws null reference [\#2969](https://github.com/MvvmCross/MvvmCross/issues/2969)
- Xamarin Forms UWP Playground application crash on resume [\#2881](https://github.com/MvvmCross/MvvmCross/issues/2881)
- Xamarin Forms Context seem incorrect in MvxFormsAndroidSetup.FormsApplication [\#2832](https://github.com/MvvmCross/MvvmCross/issues/2832)
- Fix crash and improve MvxRecyclerViewAdapter and MvxRecyclerViewHolder [\#3366](https://github.com/MvvmCross/MvvmCross/pull/3366) ([softlion](https://github.com/softlion))
- Fix log format crashing for MvxBindingLog [\#3365](https://github.com/MvvmCross/MvvmCross/pull/3365) ([softlion](https://github.com/softlion))
- Fix null reference in Target Binding when using ValueType [\#3364](https://github.com/MvvmCross/MvvmCross/pull/3364) ([softlion](https://github.com/softlion))
- Fix control being disposed before unhooking event handlers [\#3363](https://github.com/MvvmCross/MvvmCross/pull/3363) ([softlion](https://github.com/softlion))
- Make TextView Text target binding use SetText [\#3362](https://github.com/MvvmCross/MvvmCross/pull/3362) ([softlion](https://github.com/softlion))
- Allow override of built in Binding Target Factories [\#3361](https://github.com/MvvmCross/MvvmCross/pull/3361) ([softlion](https://github.com/softlion))
- Add missing ctors for tvos [\#3319](https://github.com/MvvmCross/MvvmCross/pull/3319) ([martijn00](https://github.com/martijn00))
- Remove calls to AppStart from normal Activities [\#3214](https://github.com/MvvmCross/MvvmCross/pull/3214) ([nmilcoff](https://github.com/nmilcoff))
- Use EnteredBackground to trigger suspension logic [\#3101](https://github.com/MvvmCross/MvvmCross/pull/3101) ([mmangold7](https://github.com/mmangold7))

**Closed issues:**

- MvxAppStart class causing app freeze - wrong async pattern [\#3395](https://github.com/MvvmCross/MvvmCross/issues/3395)
- MvxTapGestureRecognizerBehavior not working for UITextField in iOS 10.3.2 [\#3381](https://github.com/MvvmCross/MvvmCross/issues/3381)
- MvxUIControlTargetBinding.Dispose calling base Dispose before executing its code [\#3349](https://github.com/MvvmCross/MvvmCross/issues/3349)
- How to do the Microsoft Intune Migration or Integration in Xamarin android & iOS with MVVM Cross [\#3346](https://github.com/MvvmCross/MvvmCross/issues/3346)
- Update build.prop with Changelog url for NuGets [\#3334](https://github.com/MvvmCross/MvvmCross/issues/3334)
- Create script for consistent changelog generation [\#3332](https://github.com/MvvmCross/MvvmCross/issues/3332)
- Tizen 4.0 [\#3318](https://github.com/MvvmCross/MvvmCross/issues/3318)
- Tizen 4.0 [\#3317](https://github.com/MvvmCross/MvvmCross/issues/3317)
- Documentation issues in viewmodel-lifecycle.md [\#3301](https://github.com/MvvmCross/MvvmCross/issues/3301)
- Input Fields binding not working as expected in cell [\#3279](https://github.com/MvvmCross/MvvmCross/issues/3279)
- View Controller Missing and xib "... nib but the view outlet was not set." [\#3239](https://github.com/MvvmCross/MvvmCross/issues/3239)
- Xamarin Forms 3.4 not working with WPF [\#3236](https://github.com/MvvmCross/MvvmCross/issues/3236)
- Xamarin forms MvxTabbedPage doesn't render correctly on Android when placement set to bottom [\#3201](https://github.com/MvvmCross/MvvmCross/issues/3201)
- xamarin.android + tabs + viewpager in each tabs [\#3167](https://github.com/MvvmCross/MvvmCross/issues/3167)
- Playground.Mac wont create a window [\#3156](https://github.com/MvvmCross/MvvmCross/issues/3156)
- Mvx Setup randomly fails if the Android app was kept in the background for 4-5 days [\#3146](https://github.com/MvvmCross/MvvmCross/issues/3146)
- Android is crashing if app opens from deep linking [\#3141](https://github.com/MvvmCross/MvvmCross/issues/3141)
- Documentation: .NET Standard Library instead of PCL? [\#3108](https://github.com/MvvmCross/MvvmCross/issues/3108)
- Docs: Navigation is outdated [\#3106](https://github.com/MvvmCross/MvvmCross/issues/3106)
- Propose new docs structure [\#2856](https://github.com/MvvmCross/MvvmCross/issues/2856)

**Merged pull requests:**

- Fix presenter methods [\#3401](https://github.com/MvvmCross/MvvmCross/pull/3401) ([martijn00](https://github.com/martijn00))
- Add a better MvxRecyclerView sample to Playground [\#3400](https://github.com/MvvmCross/MvvmCross/pull/3400) ([Cheesebaron](https://github.com/Cheesebaron))
- Add XmlnsPrefix for MvvmCross.Forms [\#3399](https://github.com/MvvmCross/MvvmCross/pull/3399) ([martijn00](https://github.com/martijn00))
- Fix how C\# code snippet is viewed [\#3398](https://github.com/MvvmCross/MvvmCross/pull/3398) ([diogofr](https://github.com/diogofr))
- Issue 3108: Removed/updated documention: PCL -\> .NET Standard [\#3396](https://github.com/MvvmCross/MvvmCross/pull/3396) ([markuspalme](https://github.com/markuspalme))
- Use lowercase names on android [\#3393](https://github.com/MvvmCross/MvvmCross/pull/3393) ([martijn00](https://github.com/martijn00))
- Don't set transition elements when there are none [\#3392](https://github.com/MvvmCross/MvvmCross/pull/3392) ([martijn00](https://github.com/martijn00))
- Update sdk extras [\#3391](https://github.com/MvvmCross/MvvmCross/pull/3391) ([martijn00](https://github.com/martijn00))
- Update url for release notes [\#3390](https://github.com/MvvmCross/MvvmCross/pull/3390) ([martijn00](https://github.com/martijn00))
- Make close of dialog more safe [\#3389](https://github.com/MvvmCross/MvvmCross/pull/3389) ([martijn00](https://github.com/martijn00))
- Fix attribute for Mac windows presentation [\#3388](https://github.com/MvvmCross/MvvmCross/pull/3388) ([martijn00](https://github.com/martijn00))
- Fix for \#3294: MvxAndroidLifetimeMonitor should lock when accessing \_createdActivityCount [\#3386](https://github.com/MvvmCross/MvvmCross/pull/3386) ([markuspalme](https://github.com/markuspalme))
- Enhanced tap compatibility for UITextField in MvxGestureRecognizerBeh… [\#3382](https://github.com/MvvmCross/MvvmCross/pull/3382) ([JPSiller](https://github.com/JPSiller))
- Fix \#3281 MvxObservableCollection's RemoveRange discrepancy [\#3378](https://github.com/MvvmCross/MvvmCross/pull/3378) ([jz5](https://github.com/jz5))
- NLogLogProvider - Support for structured logging in NLog 4.5  [\#3377](https://github.com/MvvmCross/MvvmCross/pull/3377) ([snakefoot](https://github.com/snakefoot))
- Document: Fix access modifiers [\#3374](https://github.com/MvvmCross/MvvmCross/pull/3374) ([jz5](https://github.com/jz5))
- Update Swipe to Refresh doc [\#3370](https://github.com/MvvmCross/MvvmCross/pull/3370) ([Cheesebaron](https://github.com/Cheesebaron))
- Updated Swipe to refresh [\#3368](https://github.com/MvvmCross/MvvmCross/pull/3368) ([sureshkumar85ios](https://github.com/sureshkumar85ios))
- Pass parameter to command execution method in MvxGestureRecognizerBehavior [\#3367](https://github.com/MvvmCross/MvvmCross/pull/3367) ([miszu](https://github.com/miszu))
- Fixed 2 headers [\#3358](https://github.com/MvvmCross/MvvmCross/pull/3358) ([K232](https://github.com/K232))
- Constructor overload for object creation during dependency injection [\#3356](https://github.com/MvvmCross/MvvmCross/pull/3356) ([nickrandolph](https://github.com/nickrandolph))
- View model life cycle examples in docs are outdated [\#3348](https://github.com/MvvmCross/MvvmCross/pull/3348) ([b099l3](https://github.com/b099l3))
- Some links were broken so I fixed them [\#3345](https://github.com/MvvmCross/MvvmCross/pull/3345) ([b099l3](https://github.com/b099l3))
- Add attribute to show page on iOS [\#3339](https://github.com/MvvmCross/MvvmCross/pull/3339) ([martijn00](https://github.com/martijn00))
- Use license expression for nuget package [\#3338](https://github.com/MvvmCross/MvvmCross/pull/3338) ([martijn00](https://github.com/martijn00))
- Added UpdateChangelog task [\#3333](https://github.com/MvvmCross/MvvmCross/pull/3333) ([Cheesebaron](https://github.com/Cheesebaron))
- Add xml attribute to optimize importing of classes on Forms [\#3331](https://github.com/MvvmCross/MvvmCross/pull/3331) ([martijn00](https://github.com/martijn00))
- Fix for \#3329 - Missing event info in picture chooser [\#3330](https://github.com/MvvmCross/MvvmCross/pull/3330) ([markuspalme](https://github.com/markuspalme))
- Add possibility to fire command on deselect [\#3322](https://github.com/MvvmCross/MvvmCross/pull/3322) ([martijn00](https://github.com/martijn00))
- Add infos to log when SetValue ignored in binding [\#3321](https://github.com/MvvmCross/MvvmCross/pull/3321) ([martijn00](https://github.com/martijn00))
- Fix ctor for UIPageViewController [\#3320](https://github.com/MvvmCross/MvvmCross/pull/3320) ([martijn00](https://github.com/martijn00))
- Update nugets and target framework [\#3316](https://github.com/MvvmCross/MvvmCross/pull/3316) ([martijn00](https://github.com/martijn00))
- Fixed the MvxSimpleTableViewSource; now can work with both storyboard and xib-cells [\#3314](https://github.com/MvvmCross/MvvmCross/pull/3314) ([paulppn](https://github.com/paulppn))
- Adds ability to navigate to an activity if no top activity is available [\#3312](https://github.com/MvvmCross/MvvmCross/pull/3312) ([tbalcom](https://github.com/tbalcom))
- Initialize async issue on Playground.UWP apps [\#3303](https://github.com/MvvmCross/MvvmCross/pull/3303) ([dogukandemir](https://github.com/dogukandemir))
- Return the task to properly await showing view model [\#3291](https://github.com/MvvmCross/MvvmCross/pull/3291) ([AbdelrahmanGIT](https://github.com/AbdelrahmanGIT))
- Encapsulated TryPerformCloseFragmentTransaction logic in try/catch [\#3290](https://github.com/MvvmCross/MvvmCross/pull/3290) ([waseemahmad31](https://github.com/waseemahmad31))
- Added a button to the Forms SplitDetail UI to replicate a navigation issue [\#3185](https://github.com/MvvmCross/MvvmCross/pull/3185) ([michaelkollmann](https://github.com/michaelkollmann))
- 3029 fix close ViewModel behavior [\#3063](https://github.com/MvvmCross/MvvmCross/pull/3063) ([AbdelrahmanGIT](https://github.com/AbdelrahmanGIT))

## [6.2.3](https://github.com/MvvmCross/MvvmCross/tree/6.2.3) (2019-02-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.2...6.2.3)

**Implemented enhancements:**

- MvvmCross.Test.TestLogProvider not logging anywhere [\#3193](https://github.com/MvvmCross/MvvmCross/issues/3193)
- Add more information about exception to log in ExceptionSafeGetTypes [\#3264](https://github.com/MvvmCross/MvvmCross/pull/3264) ([Nickolas-](https://github.com/Nickolas-))
- Make it possible to change binding mode on language binds [\#3263](https://github.com/MvvmCross/MvvmCross/pull/3263) ([martijn00](https://github.com/martijn00))
- Make it possible to bind numberpicker [\#3248](https://github.com/MvvmCross/MvvmCross/pull/3248) ([martijn00](https://github.com/martijn00))
- Allow devs to provide their own logger for UnitTests [\#3206](https://github.com/MvvmCross/MvvmCross/pull/3206) ([Cheesebaron](https://github.com/Cheesebaron))
- Add missing ctors on iOS views [\#3204](https://github.com/MvvmCross/MvvmCross/pull/3204) ([martijn00](https://github.com/martijn00))

**Fixed bugs:**

- MvxCachingFragmentStatePagerAdapter:GetItem NullReferenceException [\#3225](https://github.com/MvvmCross/MvvmCross/issues/3225)
- \(iOS 12.1\) PictureChooser crashes when picture in library is tapped more than once [\#3215](https://github.com/MvvmCross/MvvmCross/issues/3215)
- NullReferenceException in MvxCommandBase in unit tests [\#3197](https://github.com/MvvmCross/MvvmCross/issues/3197)
- iOS MvxSimpleTableSource constructor ambiguity/Nib not registered for reuse [\#3175](https://github.com/MvvmCross/MvvmCross/issues/3175)
- Crash when using MvxLang in Label Trigger [\#3072](https://github.com/MvvmCross/MvvmCross/issues/3072)
- MvxGridView throws NoSuchMethodError on Android versions earlier than Lollipop [\#3284](https://github.com/MvvmCross/MvvmCross/issues/3284)
- `MvxTableViewSource` ignores multi-item animated replace [\#3245](https://github.com/MvvmCross/MvvmCross/issues/3245)
- Xamarin.Forms MvxBindablePropertyTargetBinding does not support binding to behaviors. [\#3241](https://github.com/MvvmCross/MvvmCross/issues/3241)
- Default Click binding is not working on FloatingActionButton [\#3238](https://github.com/MvvmCross/MvvmCross/issues/3238)
- Fixes NoSuchMethodError in MvxGridView [\#3285](https://github.com/MvvmCross/MvvmCross/pull/3285) ([tbalcom](https://github.com/tbalcom))
- GetAppStartHint\(\) passes hint through instead of always returning null. [\#3283](https://github.com/MvvmCross/MvvmCross/pull/3283) ([tbalcom](https://github.com/tbalcom))
- fix FindPropertyInfo method throw AmbiguousMatchException [\#3259](https://github.com/MvvmCross/MvvmCross/pull/3259) ([RayMMond](https://github.com/RayMMond))
- Fixed recursion issue: used result of recursive call [\#3258](https://github.com/MvvmCross/MvvmCross/pull/3258) ([waseemahmad31](https://github.com/waseemahmad31))
- Fix replace of ranges in MvxTableViewSource [\#3252](https://github.com/MvvmCross/MvvmCross/pull/3252) ([martijn00](https://github.com/martijn00))
- Adds default Click binding to FloatingActionButton. [\#3251](https://github.com/MvvmCross/MvvmCross/pull/3251) ([tbalcom](https://github.com/tbalcom))
- Add java reference ctor for splashscreen [\#3247](https://github.com/MvvmCross/MvvmCross/pull/3247) ([martijn00](https://github.com/martijn00))
- Make it safe to try call appstart [\#3246](https://github.com/MvvmCross/MvvmCross/pull/3246) ([martijn00](https://github.com/martijn00))
- Fix Tibet binding expression throw exception in wpf xaml designer. [\#3231](https://github.com/MvvmCross/MvvmCross/pull/3231) ([RayMMond](https://github.com/RayMMond))
- Cache Activity in MvxCachingFragmentStatePagerAdapter [\#3226](https://github.com/MvvmCross/MvvmCross/pull/3226) ([SOFSPEEL](https://github.com/SOFSPEEL))
- Prevents multiple taps on image in UIImagePickerController crash app [\#3220](https://github.com/MvvmCross/MvvmCross/pull/3220) ([ElteHupkes](https://github.com/ElteHupkes))
- Incorrectly using TableView reference rather than tableView parameter… [\#3207](https://github.com/MvvmCross/MvvmCross/pull/3207) ([AlanYost](https://github.com/AlanYost))
- Close any ModalViewControllers that could be open [\#3195](https://github.com/MvvmCross/MvvmCross/pull/3195) ([AnthonyNjuguna](https://github.com/AnthonyNjuguna))

**Closed issues:**

- Failing to build on VS for Mac [\#3273](https://github.com/MvvmCross/MvvmCross/issues/3273)
- does we need to initialize Xamarin.Forms.Init\(\) in android renderer in mvvmcross 6.2 [\#3262](https://github.com/MvvmCross/MvvmCross/issues/3262)
- MvxAppCompatViewPresenter.FindFragmentInChildren not using result of recursive call [\#3250](https://github.com/MvvmCross/MvvmCross/issues/3250)
- Sources compilation [\#3232](https://github.com/MvvmCross/MvvmCross/issues/3232)
- Navigation [\#3228](https://github.com/MvvmCross/MvvmCross/issues/3228)
- ConfigureAwait\(false\) in MvxNavigationService.cs makes  `OnBeforeNavigate` and `OnAfterNavigate` run in different context [\#3223](https://github.com/MvvmCross/MvvmCross/issues/3223)
- Tibet and Rio binding didn't work in wpf [\#3211](https://github.com/MvvmCross/MvvmCross/issues/3211)
- Development on Mac, can't build the projects [\#3210](https://github.com/MvvmCross/MvvmCross/issues/3210)
- Crash when tapping on Android notification [\#3203](https://github.com/MvvmCross/MvvmCross/issues/3203)
- No view model association found for candidate view MainActivity [\#3199](https://github.com/MvvmCross/MvvmCross/issues/3199)
- NullReferenceException in MvxAppCompatViewPresenter.ShowTabLayout [\#3129](https://github.com/MvvmCross/MvvmCross/issues/3129)

**Merged pull requests:**

- Fix NRE in MvxCommandBase when running in UnitTest suite [\#3200](https://github.com/MvvmCross/MvvmCross/pull/3200) ([Cheesebaron](https://github.com/Cheesebaron))
- Enhancing SetProperty with Action parameter [\#3269](https://github.com/MvvmCross/MvvmCross/pull/3269) ([dogukandemir](https://github.com/dogukandemir))
- Update more nugets [\#3260](https://github.com/MvvmCross/MvvmCross/pull/3260) ([martijn00](https://github.com/martijn00))
- Android NumberPicker binding documentation [\#3257](https://github.com/MvvmCross/MvvmCross/pull/3257) ([tbalcom](https://github.com/tbalcom))
- Register nib in ctor [\#3256](https://github.com/MvvmCross/MvvmCross/pull/3256) ([martijn00](https://github.com/martijn00))
- Update nuget packages [\#3255](https://github.com/MvvmCross/MvvmCross/pull/3255) ([martijn00](https://github.com/martijn00))
- Make MvxBindablePropertyTargetBinding use BindableProperty instead of… [\#3253](https://github.com/MvvmCross/MvvmCross/pull/3253) ([martijn00](https://github.com/martijn00))
- Updated documentation to use Mvx.IoCProvider instead of Mvx. [\#3249](https://github.com/MvvmCross/MvvmCross/pull/3249) ([martijn00](https://github.com/martijn00))
- Added UWP and WPF Forms to TipCalc Tutorial [\#3235](https://github.com/MvvmCross/MvvmCross/pull/3235) ([FabriBertani](https://github.com/FabriBertani))
- Documentation: fixed link to style guide [\#3229](https://github.com/MvvmCross/MvvmCross/pull/3229) ([markuspalme](https://github.com/markuspalme))
- Displays logging timestamp in 24h format [\#3219](https://github.com/MvvmCross/MvvmCross/pull/3219) ([bspinner](https://github.com/bspinner))
- Update contribute.md [\#3218](https://github.com/MvvmCross/MvvmCross/pull/3218) ([flyingxu](https://github.com/flyingxu))
- Add GitVersion log to output for diagnostic [\#3205](https://github.com/MvvmCross/MvvmCross/pull/3205) ([Cheesebaron](https://github.com/Cheesebaron))
- Update xUnit to latest 2.4.1 [\#3202](https://github.com/MvvmCross/MvvmCross/pull/3202) ([Cheesebaron](https://github.com/Cheesebaron))

## [6.2.2](https://github.com/MvvmCross/MvvmCross/tree/6.2.2) (2018-11-07)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.1...6.2.2)

**Implemented enhancements:**

- NuGet Package does not contain MvxConsoleSetup in netcoreapp project [\#3190](https://github.com/MvvmCross/MvvmCross/issues/3190)

**Fixed bugs:**

- Visibility plugin not working/loading in 6.1.\* [\#2962](https://github.com/MvvmCross/MvvmCross/issues/2962)

**Closed issues:**

- Change `MvvmCross.Droid.Support.Fragment` root namespace [\#3168](https://github.com/MvvmCross/MvvmCross/issues/3168)
- Update for ShowNestedFragment fragmentHost.IsVisible  [\#3160](https://github.com/MvvmCross/MvvmCross/issues/3160)
- MvxCommand NullRef on construction within tests [\#3282](https://github.com/MvvmCross/MvvmCross/issues/3282)

**Merged pull requests:**

- Switch fragment host visibility exception to warning message [\#3166](https://github.com/MvvmCross/MvvmCross/pull/3166) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Documentation: Updated available platforms for Network plugin [\#3227](https://github.com/MvvmCross/MvvmCross/pull/3227) ([markuspalme](https://github.com/markuspalme))
- Compile .net into netcoreapp [\#3191](https://github.com/MvvmCross/MvvmCross/pull/3191) ([martijn00](https://github.com/martijn00))
- Refactoring registration of action for attributes [\#3183](https://github.com/MvvmCross/MvvmCross/pull/3183) ([nickrandolph](https://github.com/nickrandolph))
- Update support fragment default namespace [\#3181](https://github.com/MvvmCross/MvvmCross/pull/3181) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Set default logging to false [\#3179](https://github.com/MvvmCross/MvvmCross/pull/3179) ([stipegrbic](https://github.com/stipegrbic))
- Mvxgridview toggle nestedscrolling [\#3178](https://github.com/MvvmCross/MvvmCross/pull/3178) ([Tyron18](https://github.com/Tyron18))
- Update namespace for mvx:Bi.nd on WPF [\#3176](https://github.com/MvvmCross/MvvmCross/pull/3176) ([Cheesebaron](https://github.com/Cheesebaron))
- Change MvxItemTemplateSelector to MvxTemplateSelector [\#3174](https://github.com/MvvmCross/MvvmCross/pull/3174) ([KaYLKann](https://github.com/KaYLKann))
- Add FillTargetFactories and FillBindingNames in Platforms.Forms.WPF Setup [\#3162](https://github.com/MvvmCross/MvvmCross/pull/3162) ([flavourous](https://github.com/flavourous))
- Add support for more control over Android PopBackStackImmediate on fragments [\#3159](https://github.com/MvvmCross/MvvmCross/pull/3159) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Update color.md [\#3150](https://github.com/MvvmCross/MvvmCross/pull/3150) ([fedemkr](https://github.com/fedemkr))

## [6.2.1](https://github.com/MvvmCross/MvvmCross/tree/6.2.1) (2018-10-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0...6.2.1)

**Implemented enhancements:**

- MvxMultiRegionWpfViewPresenter - Support for multiple windows needs access to \_frameworkElementsDictionary [\#3121](https://github.com/MvvmCross/MvvmCross/issues/3121)

**Fixed bugs:**

- ExceptionSafeGetTypes throws if log is not ready [\#3149](https://github.com/MvvmCross/MvvmCross/issues/3149)
- No way to detect software back button click in Android device in Xamarin.Forms application that uses MvvmCross. [\#3124](https://github.com/MvvmCross/MvvmCross/issues/3124)
- Xamarin.Forms StarWarsSample stuck on SplashScreen after update to v6.2.0 [\#3104](https://github.com/MvvmCross/MvvmCross/issues/3104)
- MvxAndroidSetup pointing to wrong views namespace [\#3102](https://github.com/MvvmCross/MvvmCross/issues/3102)
- Check for null before trying to Warn in ExceptionSafeGetTypes. [\#3153](https://github.com/MvvmCross/MvvmCross/pull/3153) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix circular references for UITableView and UICollectionView [\#3139](https://github.com/MvvmCross/MvvmCross/pull/3139) ([nmilcoff](https://github.com/nmilcoff))
- Added a protected FrameworkElementsDictionary property to MvxWpfViewP… [\#3127](https://github.com/MvvmCross/MvvmCross/pull/3127) ([HaraldMuehlhoffCC](https://github.com/HaraldMuehlhoffCC))
- Update caching PagerAdapter to AndroidX implementation [\#3120](https://github.com/MvvmCross/MvvmCross/pull/3120) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix views namespaces [\#3103](https://github.com/MvvmCross/MvvmCross/pull/3103) ([Cheesebaron](https://github.com/Cheesebaron))

**Closed issues:**

- Give MvxPopToRootPresentationHint a bundle constructor argument so the Body in it's base class can be set. [\#3134](https://github.com/MvvmCross/MvvmCross/issues/3134)
- iOS 12.0 Missing source event info in MvxWeakEventSubscription [\#3116](https://github.com/MvvmCross/MvvmCross/issues/3116)
- Branch for 4.4.0 and 4.4.0 plugins [\#3110](https://github.com/MvvmCross/MvvmCross/issues/3110)
- Allow ResX key names to comply with naming guideline when using ResXLocalization  [\#3109](https://github.com/MvvmCross/MvvmCross/issues/3109)
- MvxAndroidSetup cause Splash Screen crash \(MvxAndroidApplication works fine\) [\#3099](https://github.com/MvvmCross/MvvmCross/issues/3099)

**Merged pull requests:**

- Add the innerException in MvxApplication.OnNavigationFailed [\#3144](https://github.com/MvvmCross/MvvmCross/pull/3144) ([andrechi1](https://github.com/andrechi1))
- Remove duplicate code in presenters to align them [\#3143](https://github.com/MvvmCross/MvvmCross/pull/3143) ([martijn00](https://github.com/martijn00))
- Align all presentation hints [\#3142](https://github.com/MvvmCross/MvvmCross/pull/3142) ([martijn00](https://github.com/martijn00))
- Added constructor with MvxBundle argument to MvxPopToRootPresentation… [\#3135](https://github.com/MvvmCross/MvvmCross/pull/3135) ([HaraldMuehlhoffCC](https://github.com/HaraldMuehlhoffCC))
- Updated DI documentation to use new API [\#3132](https://github.com/MvvmCross/MvvmCross/pull/3132) ([markuspalme](https://github.com/markuspalme))
- \#3106 - Updated API docs for IMvxNavigationService [\#3131](https://github.com/MvvmCross/MvvmCross/pull/3131) ([markuspalme](https://github.com/markuspalme))
- Add ability to use tags from attributes [\#3128](https://github.com/MvvmCross/MvvmCross/pull/3128) ([Cheesebaron](https://github.com/Cheesebaron))
- Add MvxScaffolding to list of MvvmCross Templates [\#3125](https://github.com/MvvmCross/MvvmCross/pull/3125) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Updated IoC documentation to match new API [\#3122](https://github.com/MvvmCross/MvvmCross/pull/3122) ([markuspalme](https://github.com/markuspalme))
- FragmentJavaName centralized [\#3119](https://github.com/MvvmCross/MvvmCross/pull/3119) ([Cheesebaron](https://github.com/Cheesebaron))
- Add Mvx Toolkit to Template recommendations [\#3113](https://github.com/MvvmCross/MvvmCross/pull/3113) ([jtillman](https://github.com/jtillman))
- Update viewmodel-lifecycle.md [\#3112](https://github.com/MvvmCross/MvvmCross/pull/3112) ([gentilijuanmanuel](https://github.com/gentilijuanmanuel))
- Implement DialogView for Uap [\#3074](https://github.com/MvvmCross/MvvmCross/pull/3074) ([andrechi1](https://github.com/andrechi1))
- Update data-binding.md: fixed typo \(\#2982\) [\#3067](https://github.com/MvvmCross/MvvmCross/pull/3067) ([hyokosdeveloper](https://github.com/hyokosdeveloper))

## [6.2.0](https://github.com/MvvmCross/MvvmCross/tree/6.2.0) (2018-09-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0-beta4...6.2.0)

**Implemented enhancements:**

- Converters for Xamarin.Forms [\#2847](https://github.com/MvvmCross/MvvmCross/issues/2847)
- Android support for the MvxPagePresentationHint. [\#3086](https://github.com/MvvmCross/MvvmCross/pull/3086) ([markuspalme](https://github.com/markuspalme))

**Fixed bugs:**

- Android XAML Preview no longer works in my Xamarin.Forms project using MvvmCross [\#3091](https://github.com/MvvmCross/MvvmCross/issues/3091)
- Cannot use any MainWindow type other than MvxWindow [\#3080](https://github.com/MvvmCross/MvvmCross/issues/3080)
- NavigationService.Navigate\<TResult\>\(\) immediately return null on Wpf [\#3065](https://github.com/MvvmCross/MvvmCross/issues/3065)
- \[6.2.0-beta2\] FrameLayout to show Fragment not found [\#3059](https://github.com/MvvmCross/MvvmCross/issues/3059)
- Splash Screen Crashes on Android when Hard Back or Hard Home button hit  [\#3017](https://github.com/MvvmCross/MvvmCross/issues/3017)
- Getting Exception System.ArgumentNullException. [\#2952](https://github.com/MvvmCross/MvvmCross/issues/2952)
- Playground.Droid crashes in nav stack [\#2931](https://github.com/MvvmCross/MvvmCross/issues/2931)
- Few of the examples compile on develop [\#2930](https://github.com/MvvmCross/MvvmCross/issues/2930)
- IMvxNavigationService.Navigate\<TViewModel, TParam, TResult\> deadlock if the back button is used [\#2924](https://github.com/MvvmCross/MvvmCross/issues/2924)
- Exceptions are swallowed during Android setup [\#2903](https://github.com/MvvmCross/MvvmCross/issues/2903)
- Memory leak on opening browser and returning back on droid [\#2884](https://github.com/MvvmCross/MvvmCross/issues/2884)
- Master Detail never cancel CloseCompletionSource [\#2833](https://github.com/MvvmCross/MvvmCross/issues/2833)
- MvxNavigationService.Navigate\(Type\) returns before completing [\#2827](https://github.com/MvvmCross/MvvmCross/issues/2827)
- mvx:Lang and mvx:Bind crashes in Setter Value [\#3096](https://github.com/MvvmCross/MvvmCross/pull/3096) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix forms xaml preview on android [\#3094](https://github.com/MvvmCross/MvvmCross/pull/3094) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix bug preventing Playground.Droid from starting [\#3084](https://github.com/MvvmCross/MvvmCross/pull/3084) ([tbalcom](https://github.com/tbalcom))
-  Move ViewModel?.ViewDestroy\(\) to MvxWpfView\_Unloaded \(MvxWpfView.cs\) [\#3078](https://github.com/MvvmCross/MvvmCross/pull/3078) ([thongdoan](https://github.com/thongdoan))
- Give some love to our Network plugin [\#3056](https://github.com/MvvmCross/MvvmCross/pull/3056) ([nmilcoff](https://github.com/nmilcoff))
- Fix memory leaks on IMvxMultipleViewModelCache [\#3055](https://github.com/MvvmCross/MvvmCross/pull/3055) ([nmilcoff](https://github.com/nmilcoff))
- Repair NullReferenceException with SelectedViewController is null. [\#3054](https://github.com/MvvmCross/MvvmCross/pull/3054) ([andrechi1](https://github.com/andrechi1))
- Delay creation of UIImagePickerController [\#3038](https://github.com/MvvmCross/MvvmCross/pull/3038) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix crash when switching back to the app after Permission change [\#3032](https://github.com/MvvmCross/MvvmCross/pull/3032) ([vatsalyagoel](https://github.com/vatsalyagoel))
- Android: Add support for ViewPagers inside Fragments [\#3001](https://github.com/MvvmCross/MvvmCross/pull/3001) ([nmilcoff](https://github.com/nmilcoff))

**Closed issues:**

- Make MvxApplicationCallbacksCurrentTopActivity.cs:\_Activities protected to facilitate extension [\#3048](https://github.com/MvvmCross/MvvmCross/issues/3048)
- Build error in VS on Windows: The target "GetBuiltProjectOutputRecursive" does not exist in the project. [\#3043](https://github.com/MvvmCross/MvvmCross/issues/3043)
- MvxIoCResolveException Exception when back button clicked [\#2984](https://github.com/MvvmCross/MvvmCross/issues/2984)
- Working with Xamarin.ios using MvvmCross Framework, getting System.ArgumentNullException. [\#2954](https://github.com/MvvmCross/MvvmCross/issues/2954)
- Custom Presentation Hint Handler is still being ignored [\#2950](https://github.com/MvvmCross/MvvmCross/issues/2950)
- What should come after The Core Project in the TipCalc tutorial? It seems wrong. [\#2920](https://github.com/MvvmCross/MvvmCross/issues/2920)
- Address "RequestMainThreadAction is obsolete" build warnings [\#2859](https://github.com/MvvmCross/MvvmCross/issues/2859)

**Merged pull requests:**

- Blog post for 6.2 release [\#3107](https://github.com/MvvmCross/MvvmCross/pull/3107) ([nmilcoff](https://github.com/nmilcoff))
- Update README.md [\#3105](https://github.com/MvvmCross/MvvmCross/pull/3105) ([asudbury](https://github.com/asudbury))
- Attempt to fix failing navigation service test [\#3100](https://github.com/MvvmCross/MvvmCross/pull/3100) ([Cheesebaron](https://github.com/Cheesebaron))
- Playground.Droid: Remove incorrect button on SplitDetailNavView [\#3097](https://github.com/MvvmCross/MvvmCross/pull/3097) ([nmilcoff](https://github.com/nmilcoff))
- Fixed links to code and documentation [\#3088](https://github.com/MvvmCross/MvvmCross/pull/3088) ([markuspalme](https://github.com/markuspalme))
- Update mvvmcross-overview.md [\#3087](https://github.com/MvvmCross/MvvmCross/pull/3087) ([yehorhromadskyi](https://github.com/yehorhromadskyi))
- Fix issues when pressing back button on splash screen [\#3085](https://github.com/MvvmCross/MvvmCross/pull/3085) ([tbalcom](https://github.com/tbalcom))
- Fix TipCalc Core navigation link [\#3082](https://github.com/MvvmCross/MvvmCross/pull/3082) ([nmilcoff](https://github.com/nmilcoff))
- Check for IMvxWindow instead of MvxWindow on WPF [\#3081](https://github.com/MvvmCross/MvvmCross/pull/3081) ([Cheesebaron](https://github.com/Cheesebaron))
- Update comments in MvxWpfLocationWatcher [\#3079](https://github.com/MvvmCross/MvvmCross/pull/3079) ([fredeil](https://github.com/fredeil))
- It seems like the code owners file is case sensitive [\#3076](https://github.com/MvvmCross/MvvmCross/pull/3076) ([vatsalyagoel](https://github.com/vatsalyagoel))
- Add ApplyWithClearBindingKey to MvxFluentBindingDescriptionSet [\#3073](https://github.com/MvvmCross/MvvmCross/pull/3073) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Support for MvxPagePresentationHint in MvxIosViewPresenter \(\#2518\). [\#3071](https://github.com/MvvmCross/MvvmCross/pull/3071) ([markuspalme](https://github.com/markuspalme))
- Fix failing MvxIocPropertyInjectionTest [\#3069](https://github.com/MvvmCross/MvvmCross/pull/3069) ([Cheesebaron](https://github.com/Cheesebaron))
- Improving documentation of the usage scenario of Presentation Attribute Overriding for iOS and XF [\#3062](https://github.com/MvvmCross/MvvmCross/pull/3062) ([agat366](https://github.com/agat366))
- Add Codeowners [\#3061](https://github.com/MvvmCross/MvvmCross/pull/3061) ([vatsalyagoel](https://github.com/vatsalyagoel))
- Documentation/android spinner [\#3060](https://github.com/MvvmCross/MvvmCross/pull/3060) ([Cheesebaron](https://github.com/Cheesebaron))
- Using ExceptionDispatchInfo to capture exception [\#3058](https://github.com/MvvmCross/MvvmCross/pull/3058) ([nickrandolph](https://github.com/nickrandolph))
- Move VisibleUIViewController to MvxTabBarViewController [\#3057](https://github.com/MvvmCross/MvvmCross/pull/3057) ([andrechi1](https://github.com/andrechi1))
- should in lower case [\#3053](https://github.com/MvvmCross/MvvmCross/pull/3053) ([JTOne123](https://github.com/JTOne123))
- was updated my template version [\#3052](https://github.com/MvvmCross/MvvmCross/pull/3052) ([JTOne123](https://github.com/JTOne123))
- Propagating exceptions out of setup [\#3051](https://github.com/MvvmCross/MvvmCross/pull/3051) ([nickrandolph](https://github.com/nickrandolph))
- Make activities dictionary protected to ease extension [\#3049](https://github.com/MvvmCross/MvvmCross/pull/3049) ([daividssilverio](https://github.com/daividssilverio))
- MvxNativeValueConverter and MvxFormsValueConverter for MvvmCross.Forms [\#3047](https://github.com/MvvmCross/MvvmCross/pull/3047) ([MartinZikmund](https://github.com/MartinZikmund))
- Adding cancel support through navigation [\#3046](https://github.com/MvvmCross/MvvmCross/pull/3046) ([nickrandolph](https://github.com/nickrandolph))
- documentation: MvxRecyclerView [\#3045](https://github.com/MvvmCross/MvvmCross/pull/3045) ([Cheesebaron](https://github.com/Cheesebaron))
- Adding MvxNavigationViewModel to remove injected NavigationService and LogProvider [\#3044](https://github.com/MvvmCross/MvvmCross/pull/3044) ([nickrandolph](https://github.com/nickrandolph))
- Catch exceptions in ProcessMvxIntentResult and log [\#3039](https://github.com/MvvmCross/MvvmCross/pull/3039) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixes \#3028. [\#3036](https://github.com/MvvmCross/MvvmCross/pull/3036) ([tbalcom](https://github.com/tbalcom))
- Fix MvxAutoCompleteTextView not returning results [\#3027](https://github.com/MvvmCross/MvvmCross/pull/3027) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix docs style link on pull request template [\#3026](https://github.com/MvvmCross/MvvmCross/pull/3026) ([borbmizzet](https://github.com/borbmizzet))
- \#3000 Fix ambiguous reference when binding MvxExpandableTableViewSource [\#3024](https://github.com/MvvmCross/MvvmCross/pull/3024) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixing Forms reload issue [\#3023](https://github.com/MvvmCross/MvvmCross/pull/3023) ([nickrandolph](https://github.com/nickrandolph))
- Update missing Fluent binding extensions method and doc entries [\#3020](https://github.com/MvvmCross/MvvmCross/pull/3020) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Adding conditional flag to allow switching between different TFMs [\#3015](https://github.com/MvvmCross/MvvmCross/pull/3015) ([nickrandolph](https://github.com/nickrandolph))
- Adds Android.Support.V7.Preferences.Preference.PreferenceClick target binding [\#3009](https://github.com/MvvmCross/MvvmCross/pull/3009) ([tbalcom](https://github.com/tbalcom))
- Update links in docs [\#3007](https://github.com/MvvmCross/MvvmCross/pull/3007) ([nmilcoff](https://github.com/nmilcoff))
- Make sure navigation is cancel when requested [\#3006](https://github.com/MvvmCross/MvvmCross/pull/3006) ([martijn00](https://github.com/martijn00))
- Update nuget packages [\#3005](https://github.com/MvvmCross/MvvmCross/pull/3005) ([martijn00](https://github.com/martijn00))
- Unit Test Update: Async Dispatcher [\#3003](https://github.com/MvvmCross/MvvmCross/pull/3003) ([johnnywebb](https://github.com/johnnywebb))
- Don't throw when there is no root [\#3002](https://github.com/MvvmCross/MvvmCross/pull/3002) ([martijn00](https://github.com/martijn00))
- Tidying up more obsolete method calls and adding await as appropriate [\#2998](https://github.com/MvvmCross/MvvmCross/pull/2998) ([nickrandolph](https://github.com/nickrandolph))
- Tidying up dispatcher code [\#2997](https://github.com/MvvmCross/MvvmCross/pull/2997) ([nickrandolph](https://github.com/nickrandolph))
- Tidying up references and removing build warnings [\#2994](https://github.com/MvvmCross/MvvmCross/pull/2994) ([nickrandolph](https://github.com/nickrandolph))
- Removing unnecessary sdk library references [\#2992](https://github.com/MvvmCross/MvvmCross/pull/2992) ([nickrandolph](https://github.com/nickrandolph))
- Bugfix/issue templates [\#2989](https://github.com/MvvmCross/MvvmCross/pull/2989) ([Cheesebaron](https://github.com/Cheesebaron))
- Make GetOrCreateViewFor virtual [\#2986](https://github.com/MvvmCross/MvvmCross/pull/2986) ([Cheesebaron](https://github.com/Cheesebaron))
- Update data-binding.md: fixed typo [\#2982](https://github.com/MvvmCross/MvvmCross/pull/2982) ([AndreKraemer](https://github.com/AndreKraemer))
- Upgrading UWP target platform version and support library version [\#2978](https://github.com/MvvmCross/MvvmCross/pull/2978) ([nickrandolph](https://github.com/nickrandolph))
- Version bump to Xamarin.Forms v3.1 [\#2976](https://github.com/MvvmCross/MvvmCross/pull/2976) ([nickrandolph](https://github.com/nickrandolph))
- Default MvxAppCompatSpinner DropDownItemTemplate doesn't display strings or use ToString on models [\#2975](https://github.com/MvvmCross/MvvmCross/pull/2975) ([tbalcom](https://github.com/tbalcom))
- App crashes on resolving IMvxFormsPagePresenter [\#2972](https://github.com/MvvmCross/MvvmCross/pull/2972) ([vatsalyagoel](https://github.com/vatsalyagoel))
- Move Mvx class into IoC [\#2964](https://github.com/MvvmCross/MvvmCross/pull/2964) ([martijn00](https://github.com/martijn00))
- Adding SignClient, updating unit test runner + security fixes [\#2949](https://github.com/MvvmCross/MvvmCross/pull/2949) ([Cheesebaron](https://github.com/Cheesebaron))
- Implementing INotifyPropertyChanging [\#2943](https://github.com/MvvmCross/MvvmCross/pull/2943) ([nickrandolph](https://github.com/nickrandolph))
- Improve issue templates [\#2940](https://github.com/MvvmCross/MvvmCross/pull/2940) ([heytherewill](https://github.com/heytherewill))
- Making IMvxViewPresenter methods async [\#2868](https://github.com/MvvmCross/MvvmCross/pull/2868) ([nickrandolph](https://github.com/nickrandolph))
- Add support for async startup [\#2866](https://github.com/MvvmCross/MvvmCross/pull/2866) ([nickrandolph](https://github.com/nickrandolph))

## [6.2.0-beta4](https://github.com/MvvmCross/MvvmCross/tree/6.2.0-beta4) (2018-09-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0-beta3...6.2.0-beta4)

**Closed issues:**

- Playground.Droid can't navigate to RootViewModel [\#3083](https://github.com/MvvmCross/MvvmCross/issues/3083)
- Fix comments in MvxLocationWatcher WPF [\#2911](https://github.com/MvvmCross/MvvmCross/issues/2911)

## [6.2.0-beta3](https://github.com/MvvmCross/MvvmCross/tree/6.2.0-beta3) (2018-08-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0-beta2...6.2.0-beta3)

**Fixed bugs:**

- \[Android\] 6.2.0-beta2 Playground.Droid shows RootViewModel twice [\#3028](https://github.com/MvvmCross/MvvmCross/issues/3028)
- MvxAutoCompleteTextView PartialText never changes after initial setting [\#3008](https://github.com/MvvmCross/MvvmCross/issues/3008)
- MvxExpandableTableViewSource issue [\#3000](https://github.com/MvvmCross/MvvmCross/issues/3000)
- PictureChooser can't be injected in ViewModel [\#2886](https://github.com/MvvmCross/MvvmCross/issues/2886)

**Closed issues:**

- MvxTabbedPagePresentation with WrapInNavigationPage = false crashes when closing a modal view [\#3050](https://github.com/MvvmCross/MvvmCross/issues/3050)
- Commit d2a7fb2d on June 15 breaks compatibility with PropertyChanged.Fody [\#3016](https://github.com/MvvmCross/MvvmCross/issues/3016)

## [6.2.0-beta2](https://github.com/MvvmCross/MvvmCross/tree/6.2.0-beta2) (2018-07-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0-beta1...6.2.0-beta2)

## [6.2.0-beta1](https://github.com/MvvmCross/MvvmCross/tree/6.2.0-beta1) (2018-07-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.1.2...6.2.0-beta1)

## [6.1.2](https://github.com/MvvmCross/MvvmCross/tree/6.1.2) (2018-06-18)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.1.1...6.1.2)

**Closed issues:**

- MvxFormsAppCompatActivity.OnBackPressed assumes the standard Forms page presenter is being used [\#2965](https://github.com/MvvmCross/MvvmCross/issues/2965)
- Binding stop working after upgrade from 6.0.1 to 6.1.1 on Xamarin.Forms [\#2960](https://github.com/MvvmCross/MvvmCross/issues/2960)

**Merged pull requests:**

- Use interface instead of class for forms presenter [\#2966](https://github.com/MvvmCross/MvvmCross/pull/2966) ([martijn00](https://github.com/martijn00))
- Binding to Child View's BindingContextProperty [\#2959](https://github.com/MvvmCross/MvvmCross/pull/2959) ([borbmizzet](https://github.com/borbmizzet))

## [6.1.1](https://github.com/MvvmCross/MvvmCross/tree/6.1.1) (2018-06-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.1.0...6.1.1)

**Fixed bugs:**

- MvxSetup Assembly.GetEntryAssembly\(\) returns null [\#2957](https://github.com/MvvmCross/MvvmCross/issues/2957)

**Closed issues:**

- \[Android\] 6.1 hangs on splash screen [\#2955](https://github.com/MvvmCross/MvvmCross/issues/2955)
- The app stuck at Splash Screen when using MVVMCross 6.x [\#2953](https://github.com/MvvmCross/MvvmCross/issues/2953)
- Nightly builds are not updated [\#2948](https://github.com/MvvmCross/MvvmCross/issues/2948)
- Failed to resolve "MvvmCross.Core.MvxSetup" reference from "MvvmCross, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null" [\#2956](https://github.com/MvvmCross/MvvmCross/issues/2956)
- Playground.Forms.Droid can't run [\#2951](https://github.com/MvvmCross/MvvmCross/issues/2951)

**Merged pull requests:**

- Use AppDomain.CurrentDomain to find assemblies.  [\#2958](https://github.com/MvvmCross/MvvmCross/pull/2958) ([Cheesebaron](https://github.com/Cheesebaron))

## [6.1.0](https://github.com/MvvmCross/MvvmCross/tree/6.1.0) (2018-06-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.1.0-beta2...6.1.0)

**Implemented enhancements:**

- MvxSimpleTableViewSource for include ViewCell designir in storyboard. [\#2897](https://github.com/MvvmCross/MvvmCross/pull/2897) ([andrechi1](https://github.com/andrechi1))

**Fixed bugs:**

- Support for ViewModel as BindableProperty [\#2934](https://github.com/MvvmCross/MvvmCross/pull/2934) ([martijn00](https://github.com/martijn00))
- Fixing crash on UWP in Release due to plugin [\#2844](https://github.com/MvvmCross/MvvmCross/pull/2844) ([nickrandolph](https://github.com/nickrandolph))

**Closed issues:**

- IMvxMessenger plugin is not loaded, MvxIoCResolveException [\#2937](https://github.com/MvvmCross/MvvmCross/issues/2937)
- \(6.0\) Plugins not loaded unless explicitly referenced [\#2923](https://github.com/MvvmCross/MvvmCross/issues/2923)
- I'ts impossible to use custom MvxSuspensionManager in UWP projects [\#2882](https://github.com/MvvmCross/MvvmCross/issues/2882)
- All bindings stop working when using {Binding Source={x:Reference this}, Path=ViewModel.property} in Xaml derived from MvxContentView\<TViewModel\> [\#2825](https://github.com/MvvmCross/MvvmCross/issues/2825)

**Merged pull requests:**

- Fixing WPF XF build - version mismatch on XF reference [\#2944](https://github.com/MvvmCross/MvvmCross/pull/2944) ([nickrandolph](https://github.com/nickrandolph))
- \#2904 Fix activity handle on current top activity. [\#2941](https://github.com/MvvmCross/MvvmCross/pull/2941) ([Thetyne](https://github.com/Thetyne))
- Fix for unable to resolve plugins in Playground samples [\#2938](https://github.com/MvvmCross/MvvmCross/pull/2938) ([flyingxu](https://github.com/flyingxu))
- Ui split view controller crash trying close null view [\#2935](https://github.com/MvvmCross/MvvmCross/pull/2935) ([alinkhart](https://github.com/alinkhart))
- Updating Xamarin Forms version [\#2933](https://github.com/MvvmCross/MvvmCross/pull/2933) ([nickrandolph](https://github.com/nickrandolph))
- Fixed link to xamarin forms presenter page. [\#2927](https://github.com/MvvmCross/MvvmCross/pull/2927) ([markuspalme](https://github.com/markuspalme))
- Add InOnMainThread [\#2921](https://github.com/MvvmCross/MvvmCross/pull/2921) ([david-laundav](https://github.com/david-laundav))
- Patch 1 [\#2915](https://github.com/MvvmCross/MvvmCross/pull/2915) ([sergeyyurkov](https://github.com/sergeyyurkov))
- Fix failing unit tests [\#2914](https://github.com/MvvmCross/MvvmCross/pull/2914) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixing CI Build [\#2912](https://github.com/MvvmCross/MvvmCross/pull/2912) ([nickrandolph](https://github.com/nickrandolph))
- Small fixes in documentation [\#2909](https://github.com/MvvmCross/MvvmCross/pull/2909) ([robvanuden](https://github.com/robvanuden))
- Reducing parameters on viewmodel [\#2908](https://github.com/MvvmCross/MvvmCross/pull/2908) ([nickrandolph](https://github.com/nickrandolph))
- Fixing Android/Mac Forms build issue [\#2907](https://github.com/MvvmCross/MvvmCross/pull/2907) ([nickrandolph](https://github.com/nickrandolph))
- Make custom Suspensionmanager possible [\#2902](https://github.com/MvvmCross/MvvmCross/pull/2902) ([martijn00](https://github.com/martijn00))
- Typed hint on AppStart and documentation [\#2901](https://github.com/MvvmCross/MvvmCross/pull/2901) ([martijn00](https://github.com/martijn00))
- log4net 2.0.8 netstandard compatibility fix [\#2896](https://github.com/MvvmCross/MvvmCross/pull/2896) ([StevenBonePgh](https://github.com/StevenBonePgh))
- Update WPF version too, to avoid NuGet restore complaining about downgrade [\#2890](https://github.com/MvvmCross/MvvmCross/pull/2890) ([Cheesebaron](https://github.com/Cheesebaron))
- Remove msbuild workaround [\#2889](https://github.com/MvvmCross/MvvmCross/pull/2889) ([martijn00](https://github.com/martijn00))
- Xamarin Forms version bump [\#2887](https://github.com/MvvmCross/MvvmCross/pull/2887) ([nickrandolph](https://github.com/nickrandolph))
- documentation: MvxLinearLayout [\#2879](https://github.com/MvvmCross/MvvmCross/pull/2879) ([Cheesebaron](https://github.com/Cheesebaron))
- documentation: add swipe to refresh android docs [\#2878](https://github.com/MvvmCross/MvvmCross/pull/2878) ([Cheesebaron](https://github.com/Cheesebaron))
- Correct behviour for ViewModel Lifecycle for UWP [\#2875](https://github.com/MvvmCross/MvvmCross/pull/2875) ([ueman](https://github.com/ueman))
- Add Forms WPF into the solution [\#2874](https://github.com/MvvmCross/MvvmCross/pull/2874) ([martijn00](https://github.com/martijn00))
- Update the-tip-calc-navigation.md [\#2873](https://github.com/MvvmCross/MvvmCross/pull/2873) ([Raidervz](https://github.com/Raidervz))
- Handling back pressed for Forms applications [\#2869](https://github.com/MvvmCross/MvvmCross/pull/2869) ([nickrandolph](https://github.com/nickrandolph))
- Added recursive method to search for referenced assemblies and load t… [\#2865](https://github.com/MvvmCross/MvvmCross/pull/2865) ([rdorta](https://github.com/rdorta))
- Update to Forms 3.0 [\#2864](https://github.com/MvvmCross/MvvmCross/pull/2864) ([martijn00](https://github.com/martijn00))
- documentation: Added UIRefreshControl docs [\#2861](https://github.com/MvvmCross/MvvmCross/pull/2861) ([Cheesebaron](https://github.com/Cheesebaron))
- Improve MvvmCross Getting Started Experience \(ReadMe Content & Sample Files\) [\#2858](https://github.com/MvvmCross/MvvmCross/pull/2858) ([andrewtechhelp](https://github.com/andrewtechhelp))
- Implement MvxWindowsPage.ClearBackStack [\#2855](https://github.com/MvvmCross/MvvmCross/pull/2855) ([andrechi1](https://github.com/andrechi1))
- \[documentation\] fixing Next link for UWP project [\#2853](https://github.com/MvvmCross/MvvmCross/pull/2853) ([halex2005](https://github.com/halex2005))
- Fixes \#2736 [\#2852](https://github.com/MvvmCross/MvvmCross/pull/2852) ([tbalcom](https://github.com/tbalcom))
- Fix usings in TipCalcTutorial [\#2850](https://github.com/MvvmCross/MvvmCross/pull/2850) ([gentilijuanmanuel](https://github.com/gentilijuanmanuel))
- Updates "Requesting presentation changes" documentation for MvvmCross 6 [\#2849](https://github.com/MvvmCross/MvvmCross/pull/2849) ([tbalcom](https://github.com/tbalcom))
- TipCalc Tutorial: Assets improvements & typos [\#2845](https://github.com/MvvmCross/MvvmCross/pull/2845) ([nmilcoff](https://github.com/nmilcoff))
- Amended LinkerPleaseInclude \(iOS\) example [\#2843](https://github.com/MvvmCross/MvvmCross/pull/2843) ([JoeCooper](https://github.com/JoeCooper))
- Use legacy properties on the Android TimePicker for versions 5.1 and … [\#2841](https://github.com/MvvmCross/MvvmCross/pull/2841) ([JimWilcox3](https://github.com/JimWilcox3))
- fix mvvmcross-overview link [\#2839](https://github.com/MvvmCross/MvvmCross/pull/2839) ([halex2005](https://github.com/halex2005))
- housekeeping: use https [\#2837](https://github.com/MvvmCross/MvvmCross/pull/2837) ([ghuntley](https://github.com/ghuntley))
- Revitalize Tipcalc tutorial [\#2835](https://github.com/MvvmCross/MvvmCross/pull/2835) ([nmilcoff](https://github.com/nmilcoff))
- Consolidate samples into one playground [\#2828](https://github.com/MvvmCross/MvvmCross/pull/2828) ([martijn00](https://github.com/martijn00))

## [6.1.0-beta2](https://github.com/MvvmCross/MvvmCross/tree/6.1.0-beta2) (2018-06-05)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.1.0-beta1...6.1.0-beta2)

**Closed issues:**

- UWP Dependency Injection on startup [\#2925](https://github.com/MvvmCross/MvvmCross/issues/2925)
- Object reference not set to an instance of an object in MvxChainedSourceBinding.Dispose [\#2922](https://github.com/MvvmCross/MvvmCross/issues/2922)

## [6.1.0-beta1](https://github.com/MvvmCross/MvvmCross/tree/6.1.0-beta1) (2018-05-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.1...6.1.0-beta1)

**Fixed bugs:**

- ViewModel\#Destroy does not get called on UWP [\#2860](https://github.com/MvvmCross/MvvmCross/issues/2860)

**Closed issues:**

- Backstack in mvvmcross android working incorrectly [\#2913](https://github.com/MvvmCross/MvvmCross/issues/2913)
- SplashScreen doesn't automatically start Activity registered for AppStart [\#2895](https://github.com/MvvmCross/MvvmCross/issues/2895)
- UWP Release Builds are crashing at runtime [\#2842](https://github.com/MvvmCross/MvvmCross/issues/2842)
- MvxTimePicker won't bind correctly to Android versions 5.1 and below. [\#2840](https://github.com/MvvmCross/MvvmCross/issues/2840)
- stamp $\(AssemblyName\) \($\(TargetFramework\)\) into builds  [\#2836](https://github.com/MvvmCross/MvvmCross/issues/2836)

## [6.0.1](https://github.com/MvvmCross/MvvmCross/tree/6.0.1) (2018-04-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0...6.0.1)

**Fixed bugs:**

- SplashScreenAppCompat should use AppCompatSetup [\#2821](https://github.com/MvvmCross/MvvmCross/pull/2821) ([drungrin](https://github.com/drungrin))

**Closed issues:**

- Calling async method on ViewModel.Initialize never ends and InitializeTask properties never got updated [\#2829](https://github.com/MvvmCross/MvvmCross/issues/2829)

**Merged pull requests:**

- Small docs fix, renamed to correct method in the events mapping table. [\#2834](https://github.com/MvvmCross/MvvmCross/pull/2834) ([agoransson](https://github.com/agoransson))
- Update Getting-Started and MvvmCross-Overview docs [\#2822](https://github.com/MvvmCross/MvvmCross/pull/2822) ([nmilcoff](https://github.com/nmilcoff))
- Fixing crash when running Android Forms Playground [\#2820](https://github.com/MvvmCross/MvvmCross/pull/2820) ([nickrandolph](https://github.com/nickrandolph))
- Removing calls to base methods to prevent error [\#2819](https://github.com/MvvmCross/MvvmCross/pull/2819) ([nickrandolph](https://github.com/nickrandolph))

## [6.0.0](https://github.com/MvvmCross/MvvmCross/tree/6.0.0) (2018-04-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta8...6.0.0)

## [6.0.0-beta8](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta8) (2018-04-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta7...6.0.0-beta8)

## [6.0.0-beta7](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta7) (2018-03-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta6...6.0.0-beta7)

## [6.0.0-beta6](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta6) (2018-03-19)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.7.0...6.0.0-beta6)

## [5.7.0](https://github.com/MvvmCross/MvvmCross/tree/5.7.0) (2018-03-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta5...5.7.0)

## [6.0.0-beta5](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta5) (2018-03-09)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta4...6.0.0-beta5)

## [6.0.0-beta4](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta4) (2018-03-02)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta3...6.0.0-beta4)

## [6.0.0-beta3](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta3) (2018-02-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta2...6.0.0-beta3)

## [6.0.0-beta2](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta2) (2018-02-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta1...6.0.0-beta2)

## [6.0.0-beta1](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta1) (2018-02-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.3...6.0.0-beta1)

## [5.6.3](https://github.com/MvvmCross/MvvmCross/tree/5.6.3) (2017-12-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.2...5.6.3)

## [5.6.2](https://github.com/MvvmCross/MvvmCross/tree/5.6.2) (2017-12-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.1...5.6.2)

## [5.6.1](https://github.com/MvvmCross/MvvmCross/tree/5.6.1) (2017-12-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.0...5.6.1)

## [5.6.0](https://github.com/MvvmCross/MvvmCross/tree/5.6.0) (2017-12-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.5.2...5.6.0)

## [5.5.2](https://github.com/MvvmCross/MvvmCross/tree/5.5.2) (2017-11-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.5.1...5.5.2)

## [5.5.1](https://github.com/MvvmCross/MvvmCross/tree/5.5.1) (2017-11-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.5.0...5.5.1)

## [5.5.0](https://github.com/MvvmCross/MvvmCross/tree/5.5.0) (2017-11-23)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.4.2...5.5.0)

## [5.4.2](https://github.com/MvvmCross/MvvmCross/tree/5.4.2) (2017-11-07)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.4.1...5.4.2)

## [5.4.1](https://github.com/MvvmCross/MvvmCross/tree/5.4.1) (2017-11-07)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.4.0...5.4.1)

## [5.4.0](https://github.com/MvvmCross/MvvmCross/tree/5.4.0) (2017-10-31)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.3.2...5.4.0)

## [5.3.2](https://github.com/MvvmCross/MvvmCross/tree/5.3.2) (2017-10-23)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.3.1...5.3.2)

## [5.3.1](https://github.com/MvvmCross/MvvmCross/tree/5.3.1) (2017-10-18)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.3.0...5.3.1)

## [5.3.0](https://github.com/MvvmCross/MvvmCross/tree/5.3.0) (2017-10-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.2.1...5.3.0)

## [5.2.1](https://github.com/MvvmCross/MvvmCross/tree/5.2.1) (2017-09-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.2...5.2.1)

## [5.2](https://github.com/MvvmCross/MvvmCross/tree/5.2) (2017-09-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.1.1...5.2)

## [5.1.1](https://github.com/MvvmCross/MvvmCross/tree/5.1.1) (2017-07-28)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.1.0...5.1.1)

## [5.1.0](https://github.com/MvvmCross/MvvmCross/tree/5.1.0) (2017-07-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.6...5.1.0)

## [5.0.6](https://github.com/MvvmCross/MvvmCross/tree/5.0.6) (2017-07-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.5...5.0.6)

## [5.0.5](https://github.com/MvvmCross/MvvmCross/tree/5.0.5) (2017-06-25)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.4...5.0.5)

## [5.0.4](https://github.com/MvvmCross/MvvmCross/tree/5.0.4) (2017-06-23)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.3...5.0.4)

## [5.0.3](https://github.com/MvvmCross/MvvmCross/tree/5.0.3) (2017-06-19)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.2...5.0.3)

## [5.0.2](https://github.com/MvvmCross/MvvmCross/tree/5.0.2) (2017-06-06)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.1...5.0.2)

## [5.0.1](https://github.com/MvvmCross/MvvmCross/tree/5.0.1) (2017-05-26)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0...5.0.1)

## [5.0.0](https://github.com/MvvmCross/MvvmCross/tree/5.0.0) (2017-05-22)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.0.0-beta.12...5.0.0)

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

\* *This Changelog was automatically generated by [github_changelog_generator](https://github.com/github-changelog-generator/github-changelog-generator)*