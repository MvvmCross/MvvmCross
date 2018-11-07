# Change Log

## [6.2.2](https://github.com/MvvmCross/MvvmCross/tree/6.2.2) (2018-11-07)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.1...6.2.2)

**Fixed bugs:**

- Visibility plugin not working/loading in 6.1.\* [\#2962](https://github.com/MvvmCross/MvvmCross/issues/2962) [[Plugins](https://github.com/MvvmCross/MvvmCross/labels/Plugins)]

**Closed issues:**

- NuGet Package does not contain MvxConsoleSetup in netcoreapp project [\#3190](https://github.com/MvvmCross/MvvmCross/issues/3190)
- Change `MvvmCross.Droid.Support.Fragment` root namespace [\#3168](https://github.com/MvvmCross/MvvmCross/issues/3168) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)]
- Update for ShowNestedFragment fragmentHost.IsVisible  [\#3160](https://github.com/MvvmCross/MvvmCross/issues/3160) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)]

**Merged pull requests:**

- Compile .net into netcoreapp [\#3191](https://github.com/MvvmCross/MvvmCross/pull/3191) ([martijn00](https://github.com/martijn00))
- Refactoring registration of action for attributes [\#3183](https://github.com/MvvmCross/MvvmCross/pull/3183) ([nickrandolph](https://github.com/nickrandolph))
- Update support fragment default namespace [\#3181](https://github.com/MvvmCross/MvvmCross/pull/3181) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Set default logging to false [\#3179](https://github.com/MvvmCross/MvvmCross/pull/3179) ([stipegrbic](https://github.com/stipegrbic))
- Mvxgridview toggle nestedscrolling [\#3178](https://github.com/MvvmCross/MvvmCross/pull/3178) ([Tyron18](https://github.com/Tyron18))
- Update namespace for mvx:Bi.nd on WPF [\#3176](https://github.com/MvvmCross/MvvmCross/pull/3176) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] [[WPF](https://github.com/MvvmCross/MvvmCross/labels/WPF)] ([Cheesebaron](https://github.com/Cheesebaron))
- Change MvxItemTemplateSelector to MvxTemplateSelector [\#3174](https://github.com/MvvmCross/MvvmCross/pull/3174) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([KaYLKann](https://github.com/KaYLKann))
- Switch fragment host visibility exception to warning message [\#3166](https://github.com/MvvmCross/MvvmCross/pull/3166) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([Plac3hold3r](https://github.com/Plac3hold3r))
- Add FillTargetFactories and FillBindingNames in Platforms.Forms.WPF Setup [\#3162](https://github.com/MvvmCross/MvvmCross/pull/3162) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] [[WPF](https://github.com/MvvmCross/MvvmCross/labels/WPF)] ([flavourous](https://github.com/flavourous))
- Add support for more control over Android PopBackStackImmediate on fragments [\#3159](https://github.com/MvvmCross/MvvmCross/pull/3159) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([Plac3hold3r](https://github.com/Plac3hold3r))

## [6.2.1](https://github.com/MvvmCross/MvvmCross/tree/6.2.1) (2018-10-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0...6.2.1)

**Fixed bugs:**

- ExceptionSafeGetTypes throws if log is not ready [\#3149](https://github.com/MvvmCross/MvvmCross/issues/3149)
- No way to detect software back button click in Android device in Xamarin.Forms application that uses MvvmCross. [\#3124](https://github.com/MvvmCross/MvvmCross/issues/3124) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)]
- Xamarin.Forms StarWarsSample stuck on SplashScreen after update to v6.2.0 [\#3104](https://github.com/MvvmCross/MvvmCross/issues/3104)
- MvxAndroidSetup pointing to wrong views namespace [\#3102](https://github.com/MvvmCross/MvvmCross/issues/3102) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- Check for null before trying to Warn in ExceptionSafeGetTypes. [\#3153](https://github.com/MvvmCross/MvvmCross/pull/3153) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix circular references for UITableView and UICollectionView [\#3139](https://github.com/MvvmCross/MvvmCross/pull/3139) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([nmilcoff](https://github.com/nmilcoff))
- Added a protected FrameworkElementsDictionary property to MvxWpfViewP… [\#3127](https://github.com/MvvmCross/MvvmCross/pull/3127) [[WPF](https://github.com/MvvmCross/MvvmCross/labels/WPF)] ([HaraldMuehlhoffCC](https://github.com/HaraldMuehlhoffCC))
- Update caching PagerAdapter to AndroidX implementation [\#3120](https://github.com/MvvmCross/MvvmCross/pull/3120) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([Cheesebaron](https://github.com/Cheesebaron))
- Fix views namespaces [\#3103](https://github.com/MvvmCross/MvvmCross/pull/3103) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] ([Cheesebaron](https://github.com/Cheesebaron))

**Closed issues:**

- Give MvxPopToRootPresentationHint a bundle constructor argument so the Body in it's base class can be set. [\#3134](https://github.com/MvvmCross/MvvmCross/issues/3134)
- iOS 12.0 Missing source event info in MvxWeakEventSubscription [\#3116](https://github.com/MvvmCross/MvvmCross/issues/3116)
- Branch for 4.4.0 and 4.4.0 plugins [\#3110](https://github.com/MvvmCross/MvvmCross/issues/3110)
- Allow ResX key names to comply with naming guideline when using ResXLocalization  [\#3109](https://github.com/MvvmCross/MvvmCross/issues/3109) [[Plugins](https://github.com/MvvmCross/MvvmCross/labels/Plugins)]
- MvxAndroidSetup cause Splash Screen crash \(MvxAndroidApplication works fine\) [\#3099](https://github.com/MvvmCross/MvvmCross/issues/3099)
- MvxMultiRegionWpfViewPresenter - Support for multiple windows needs access to \_frameworkElementsDictionary [\#3121](https://github.com/MvvmCross/MvvmCross/issues/3121)

**Merged pull requests:**

- Update color.md [\#3150](https://github.com/MvvmCross/MvvmCross/pull/3150) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([fedemkr](https://github.com/fedemkr))
- Add the innerException in MvxApplication.OnNavigationFailed [\#3144](https://github.com/MvvmCross/MvvmCross/pull/3144) ([andrechi1](https://github.com/andrechi1))
- Remove duplicate code in presenters to align them [\#3143](https://github.com/MvvmCross/MvvmCross/pull/3143) ([martijn00](https://github.com/martijn00))
- Align all presentation hints [\#3142](https://github.com/MvvmCross/MvvmCross/pull/3142) ([martijn00](https://github.com/martijn00))
- Added constructor with MvxBundle argument to MvxPopToRootPresentation… [\#3135](https://github.com/MvvmCross/MvvmCross/pull/3135) ([HaraldMuehlhoffCC](https://github.com/HaraldMuehlhoffCC))
- Updated DI documentation to use new API [\#3132](https://github.com/MvvmCross/MvvmCross/pull/3132) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([markuspalme](https://github.com/markuspalme))
- \#3106 - Updated API docs for IMvxNavigationService [\#3131](https://github.com/MvvmCross/MvvmCross/pull/3131) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([markuspalme](https://github.com/markuspalme))
- Add ability to use tags from attributes [\#3128](https://github.com/MvvmCross/MvvmCross/pull/3128) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([Cheesebaron](https://github.com/Cheesebaron))
- Add MvxScaffolding to list of MvvmCross Templates [\#3125](https://github.com/MvvmCross/MvvmCross/pull/3125) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Updated IoC documentation to match new API [\#3122](https://github.com/MvvmCross/MvvmCross/pull/3122) ([markuspalme](https://github.com/markuspalme))
- FragmentJavaName centralized [\#3119](https://github.com/MvvmCross/MvvmCross/pull/3119) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([Cheesebaron](https://github.com/Cheesebaron))
- Add Mvx Toolkit to Template recommendations [\#3113](https://github.com/MvvmCross/MvvmCross/pull/3113) ([jtillman](https://github.com/jtillman))
- Update viewmodel-lifecycle.md [\#3112](https://github.com/MvvmCross/MvvmCross/pull/3112) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([gentilijuanmanuel](https://github.com/gentilijuanmanuel))
- Implement DialogView for Uap [\#3074](https://github.com/MvvmCross/MvvmCross/pull/3074) ([andrechi1](https://github.com/andrechi1))
- Update data-binding.md: fixed typo \(\#2982\) [\#3067](https://github.com/MvvmCross/MvvmCross/pull/3067) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([hyokosdeveloper](https://github.com/hyokosdeveloper))

## [6.2.0](https://github.com/MvvmCross/MvvmCross/tree/6.2.0) (2018-09-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0-beta4...6.2.0)

**Fixed bugs:**

- Android XAML Preview no longer works in my Xamarin.Forms project using MvvmCross [\#3091](https://github.com/MvvmCross/MvvmCross/issues/3091) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)]
- Cannot use any MainWindow type other than MvxWindow [\#3080](https://github.com/MvvmCross/MvvmCross/issues/3080) [[WPF](https://github.com/MvvmCross/MvvmCross/labels/WPF)]
- NavigationService.Navigate\<TResult\>\(\) immediately return null on Wpf [\#3065](https://github.com/MvvmCross/MvvmCross/issues/3065) [[WPF](https://github.com/MvvmCross/MvvmCross/labels/WPF)]
- \[6.2.0-beta2\] FrameLayout to show Fragment not found [\#3059](https://github.com/MvvmCross/MvvmCross/issues/3059) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- Splash Screen Crashes on Android when Hard Back or Hard Home button hit  [\#3017](https://github.com/MvvmCross/MvvmCross/issues/3017)
- Getting Exception System.ArgumentNullException. [\#2952](https://github.com/MvvmCross/MvvmCross/issues/2952) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)]
- Playground.Droid crashes in nav stack [\#2931](https://github.com/MvvmCross/MvvmCross/issues/2931) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- Few of the examples compile on develop [\#2930](https://github.com/MvvmCross/MvvmCross/issues/2930) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- IMvxNavigationService.Navigate\<TViewModel, TParam, TResult\> deadlock if the back button is used [\#2924](https://github.com/MvvmCross/MvvmCross/issues/2924) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)]
- Exceptions are swallowed during Android setup [\#2903](https://github.com/MvvmCross/MvvmCross/issues/2903) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- Memory leak on opening browser and returning back on droid [\#2884](https://github.com/MvvmCross/MvvmCross/issues/2884) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)]
- Master Detail never cancel CloseCompletionSource [\#2833](https://github.com/MvvmCross/MvvmCross/issues/2833)
- MvxNavigationService.Navigate\(Type\) returns before completing [\#2827](https://github.com/MvvmCross/MvvmCross/issues/2827) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)]
- RunAppStart isn't called in Xamarin Form - Android project [\#2813](https://github.com/MvvmCross/MvvmCross/issues/2813) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)]
- Failed to resolve type MvvmCross.ViewModels.IMvxAppStart [\#2810](https://github.com/MvvmCross/MvvmCross/issues/2810) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- mvx:Lang and mvx:Bind crashes in Setter Value [\#3096](https://github.com/MvvmCross/MvvmCross/pull/3096) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix forms xaml preview on android [\#3094](https://github.com/MvvmCross/MvvmCross/pull/3094) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([Cheesebaron](https://github.com/Cheesebaron))
- Fix bug preventing Playground.Droid from starting [\#3084](https://github.com/MvvmCross/MvvmCross/pull/3084) ([tbalcom](https://github.com/tbalcom))
-  Move ViewModel?.ViewDestroy\(\) to MvxWpfView\_Unloaded \(MvxWpfView.cs\) [\#3078](https://github.com/MvvmCross/MvvmCross/pull/3078) [[WPF](https://github.com/MvvmCross/MvvmCross/labels/WPF)] ([thongdoan](https://github.com/thongdoan))
- Give some love to our Network plugin [\#3056](https://github.com/MvvmCross/MvvmCross/pull/3056) [[Plugins](https://github.com/MvvmCross/MvvmCross/labels/Plugins)] ([nmilcoff](https://github.com/nmilcoff))
- Fix memory leaks on IMvxMultipleViewModelCache [\#3055](https://github.com/MvvmCross/MvvmCross/pull/3055) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([nmilcoff](https://github.com/nmilcoff))
- Repair NullReferenceException with SelectedViewController is null. [\#3054](https://github.com/MvvmCross/MvvmCross/pull/3054) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([andrechi1](https://github.com/andrechi1))
- Delay creation of UIImagePickerController [\#3038](https://github.com/MvvmCross/MvvmCross/pull/3038) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([Cheesebaron](https://github.com/Cheesebaron))
- Fix crash when switching back to the app after Permission change [\#3032](https://github.com/MvvmCross/MvvmCross/pull/3032) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([vatsalyagoel](https://github.com/vatsalyagoel))
- Android: Add support for ViewPagers inside Fragments [\#3001](https://github.com/MvvmCross/MvvmCross/pull/3001) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([nmilcoff](https://github.com/nmilcoff))

**Closed issues:**

- Make MvxApplicationCallbacksCurrentTopActivity.cs:\_Activities protected to facilitate extension [\#3048](https://github.com/MvvmCross/MvvmCross/issues/3048)
- Build error in VS on Windows: The target "GetBuiltProjectOutputRecursive" does not exist in the project. [\#3043](https://github.com/MvvmCross/MvvmCross/issues/3043)
- MvxIoCResolveException Exception when back button clicked [\#2984](https://github.com/MvvmCross/MvvmCross/issues/2984)
- Working with Xamarin.ios using MvvmCross Framework, getting System.ArgumentNullException. [\#2954](https://github.com/MvvmCross/MvvmCross/issues/2954) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)]
- Custom Presentation Hint Handler is still being ignored [\#2950](https://github.com/MvvmCross/MvvmCross/issues/2950)
- What should come after The Core Project in the TipCalc tutorial? It seems wrong. [\#2920](https://github.com/MvvmCross/MvvmCross/issues/2920) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)]
- Address "RequestMainThreadAction is obsolete" build warnings [\#2859](https://github.com/MvvmCross/MvvmCross/issues/2859)
- Converters for Xamarin.Forms [\#2847](https://github.com/MvvmCross/MvvmCross/issues/2847) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)]
- Update documentation based on new namespaces [\#2621](https://github.com/MvvmCross/MvvmCross/issues/2621) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)]

**Merged pull requests:**

- Blog post for 6.2 release [\#3107](https://github.com/MvvmCross/MvvmCross/pull/3107) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([nmilcoff](https://github.com/nmilcoff))
- Update README.md [\#3105](https://github.com/MvvmCross/MvvmCross/pull/3105) ([asudbury](https://github.com/asudbury))
- Attempt to fix failing navigation service test [\#3100](https://github.com/MvvmCross/MvvmCross/pull/3100) ([Cheesebaron](https://github.com/Cheesebaron))
- Playground.Droid: Remove incorrect button on SplitDetailNavView [\#3097](https://github.com/MvvmCross/MvvmCross/pull/3097) ([nmilcoff](https://github.com/nmilcoff))
- Fixed links to code and documentation [\#3088](https://github.com/MvvmCross/MvvmCross/pull/3088) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([markuspalme](https://github.com/markuspalme))
- Update mvvmcross-overview.md [\#3087](https://github.com/MvvmCross/MvvmCross/pull/3087) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([yehorhromadskyi](https://github.com/yehorhromadskyi))
- Android support for the MvxPagePresentationHint. [\#3086](https://github.com/MvvmCross/MvvmCross/pull/3086) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([markuspalme](https://github.com/markuspalme))
- Fix issues when pressing back button on splash screen [\#3085](https://github.com/MvvmCross/MvvmCross/pull/3085) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] ([tbalcom](https://github.com/tbalcom))
- Fix TipCalc Core navigation link [\#3082](https://github.com/MvvmCross/MvvmCross/pull/3082) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([nmilcoff](https://github.com/nmilcoff))
- Check for IMvxWindow instead of MvxWindow on WPF [\#3081](https://github.com/MvvmCross/MvvmCross/pull/3081) ([Cheesebaron](https://github.com/Cheesebaron))
- Update comments in MvxWpfLocationWatcher [\#3079](https://github.com/MvvmCross/MvvmCross/pull/3079) ([fredeil](https://github.com/fredeil))
- It seems like the code owners file is case sensitive [\#3076](https://github.com/MvvmCross/MvvmCross/pull/3076) ([vatsalyagoel](https://github.com/vatsalyagoel))
- Add ApplyWithClearBindingKey to MvxFluentBindingDescriptionSet [\#3073](https://github.com/MvvmCross/MvvmCross/pull/3073) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Support for MvxPagePresentationHint in MvxIosViewPresenter \(\#2518\). [\#3071](https://github.com/MvvmCross/MvvmCross/pull/3071) ([markuspalme](https://github.com/markuspalme))
- Fix failing MvxIocPropertyInjectionTest [\#3069](https://github.com/MvvmCross/MvvmCross/pull/3069) ([Cheesebaron](https://github.com/Cheesebaron))
- Improving documentation of the usage scenario of Presentation Attribute Overriding for iOS and XF [\#3062](https://github.com/MvvmCross/MvvmCross/pull/3062) ([agat366](https://github.com/agat366))
- Add Codeowners [\#3061](https://github.com/MvvmCross/MvvmCross/pull/3061) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([vatsalyagoel](https://github.com/vatsalyagoel))
- Documentation/android spinner [\#3060](https://github.com/MvvmCross/MvvmCross/pull/3060) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([Cheesebaron](https://github.com/Cheesebaron))
- Using ExceptionDispatchInfo to capture exception [\#3058](https://github.com/MvvmCross/MvvmCross/pull/3058) ([nickrandolph](https://github.com/nickrandolph))
- Move VisibleUIViewController to MvxTabBarViewController [\#3057](https://github.com/MvvmCross/MvvmCross/pull/3057) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([andrechi1](https://github.com/andrechi1))
- should in lower case [\#3053](https://github.com/MvvmCross/MvvmCross/pull/3053) ([JTOne123](https://github.com/JTOne123))
- was updated my template version [\#3052](https://github.com/MvvmCross/MvvmCross/pull/3052) ([JTOne123](https://github.com/JTOne123))
- Propagating exceptions out of setup [\#3051](https://github.com/MvvmCross/MvvmCross/pull/3051) ([nickrandolph](https://github.com/nickrandolph))
- Make activities dictionary protected to ease extension [\#3049](https://github.com/MvvmCross/MvvmCross/pull/3049) ([daividssilverio](https://github.com/daividssilverio))
- MvxNativeValueConverter and MvxFormsValueConverter for MvvmCross.Forms [\#3047](https://github.com/MvvmCross/MvvmCross/pull/3047) ([MartinZikmund](https://github.com/MartinZikmund))
- Adding cancel support through navigation [\#3046](https://github.com/MvvmCross/MvvmCross/pull/3046) ([nickrandolph](https://github.com/nickrandolph))
- documentation: MvxRecyclerView [\#3045](https://github.com/MvvmCross/MvvmCross/pull/3045) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([Cheesebaron](https://github.com/Cheesebaron))
- Adding MvxNavigationViewModel to remove injected NavigationService and LogProvider [\#3044](https://github.com/MvvmCross/MvvmCross/pull/3044) ([nickrandolph](https://github.com/nickrandolph))
- Catch exceptions in ProcessMvxIntentResult and log [\#3039](https://github.com/MvvmCross/MvvmCross/pull/3039) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixes \#3028. [\#3036](https://github.com/MvvmCross/MvvmCross/pull/3036) ([tbalcom](https://github.com/tbalcom))
- Fix MvxAutoCompleteTextView not returning results [\#3027](https://github.com/MvvmCross/MvvmCross/pull/3027) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix docs style link on pull request template [\#3026](https://github.com/MvvmCross/MvvmCross/pull/3026) ([borbmizzet](https://github.com/borbmizzet))
- \#3000 Fix ambiguous reference when binding MvxExpandableTableViewSource [\#3024](https://github.com/MvvmCross/MvvmCross/pull/3024) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([Cheesebaron](https://github.com/Cheesebaron))
- Fixing Forms reload issue [\#3023](https://github.com/MvvmCross/MvvmCross/pull/3023) ([nickrandolph](https://github.com/nickrandolph))
- Update missing Fluent binding extensions method and doc entries [\#3020](https://github.com/MvvmCross/MvvmCross/pull/3020) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Adding conditional flag to allow switching between different TFMs [\#3015](https://github.com/MvvmCross/MvvmCross/pull/3015) ([nickrandolph](https://github.com/nickrandolph))
- Adds Android.Support.V7.Preferences.Preference.PreferenceClick target binding [\#3009](https://github.com/MvvmCross/MvvmCross/pull/3009) ([tbalcom](https://github.com/tbalcom))
- Update links in docs [\#3007](https://github.com/MvvmCross/MvvmCross/pull/3007) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([nmilcoff](https://github.com/nmilcoff))
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
- Update data-binding.md: fixed typo [\#2982](https://github.com/MvvmCross/MvvmCross/pull/2982) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([AndreKraemer](https://github.com/AndreKraemer))
- Upgrading UWP target platform version and support library version [\#2978](https://github.com/MvvmCross/MvvmCross/pull/2978) ([nickrandolph](https://github.com/nickrandolph))
- Version bump to Xamarin.Forms v3.1 [\#2976](https://github.com/MvvmCross/MvvmCross/pull/2976) ([nickrandolph](https://github.com/nickrandolph))
- Default MvxAppCompatSpinner DropDownItemTemplate doesn't display strings or use ToString on models [\#2975](https://github.com/MvvmCross/MvvmCross/pull/2975) ([tbalcom](https://github.com/tbalcom))
- App crashes on resolving IMvxFormsPagePresenter [\#2972](https://github.com/MvvmCross/MvvmCross/pull/2972) ([vatsalyagoel](https://github.com/vatsalyagoel))
- Move Mvx class into IoC [\#2964](https://github.com/MvvmCross/MvvmCross/pull/2964) ([martijn00](https://github.com/martijn00))
- Adding SignClient, updating unit test runner + security fixes [\#2949](https://github.com/MvvmCross/MvvmCross/pull/2949) ([Cheesebaron](https://github.com/Cheesebaron))
- Implementing INotifyPropertyChanging [\#2943](https://github.com/MvvmCross/MvvmCross/pull/2943) ([nickrandolph](https://github.com/nickrandolph))
- Improve issue templates [\#2940](https://github.com/MvvmCross/MvvmCross/pull/2940) ([willsb](https://github.com/willsb))
- Making IMvxViewPresenter methods async [\#2868](https://github.com/MvvmCross/MvvmCross/pull/2868) ([nickrandolph](https://github.com/nickrandolph))
- Add support for async startup [\#2866](https://github.com/MvvmCross/MvvmCross/pull/2866) ([nickrandolph](https://github.com/nickrandolph))

## [6.2.0-beta4](https://github.com/MvvmCross/MvvmCross/tree/6.2.0-beta4) (2018-09-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0-beta3...6.2.0-beta4)

**Closed issues:**

- Playground.Droid can't navigate to RootViewModel [\#3083](https://github.com/MvvmCross/MvvmCross/issues/3083)
- Fix comments in MvxLocationWatcher WPF [\#2911](https://github.com/MvvmCross/MvvmCross/issues/2911) [[WPF](https://github.com/MvvmCross/MvvmCross/labels/WPF)]

## [6.2.0-beta3](https://github.com/MvvmCross/MvvmCross/tree/6.2.0-beta3) (2018-08-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0-beta2...6.2.0-beta3)

**Fixed bugs:**

- \[Android\] 6.2.0-beta2 Playground.Droid shows RootViewModel twice [\#3028](https://github.com/MvvmCross/MvvmCross/issues/3028) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- MvxAutoCompleteTextView PartialText never changes after initial setting [\#3008](https://github.com/MvvmCross/MvvmCross/issues/3008) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- MvxExpandableTableViewSource issue [\#3000](https://github.com/MvvmCross/MvvmCross/issues/3000) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)]
- PictureChooser can't be injected in ViewModel [\#2886](https://github.com/MvvmCross/MvvmCross/issues/2886) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)]

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

- MvxSetup Assembly.GetEntryAssembly\(\) returns null [\#2957](https://github.com/MvvmCross/MvvmCross/issues/2957) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)]

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

**Fixed bugs:**

- MvvmCross.Forms blank page when binding to bool in xaml with MvxBind [\#2623](https://github.com/MvvmCross/MvvmCross/issues/2623) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)]
- Support for ViewModel as BindableProperty [\#2934](https://github.com/MvvmCross/MvvmCross/pull/2934) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([martijn00](https://github.com/martijn00))
- Fixing crash on UWP in Release due to plugin [\#2844](https://github.com/MvvmCross/MvvmCross/pull/2844) [[UWP](https://github.com/MvvmCross/MvvmCross/labels/UWP)] ([nickrandolph](https://github.com/nickrandolph))

**Closed issues:**

- Source viewmodel in CreateBindingSet is ignored [\#2946](https://github.com/MvvmCross/MvvmCross/issues/2946)
- IMvxMessenger plugin is not loaded, MvxIoCResolveException [\#2937](https://github.com/MvvmCross/MvvmCross/issues/2937)
- \[Discussion\]Versioning and Release Schedule [\#2798](https://github.com/MvvmCross/MvvmCross/issues/2798)
- \(6.0\) Plugins not loaded unless explicitly referenced [\#2923](https://github.com/MvvmCross/MvvmCross/issues/2923)
- I'ts impossible to use custom MvxSuspensionManager in UWP projects [\#2882](https://github.com/MvvmCross/MvvmCross/issues/2882)
- All bindings stop working when using {Binding Source={x:Reference this}, Path=ViewModel.property} in Xaml derived from MvxContentView\<TViewModel\> [\#2825](https://github.com/MvvmCross/MvvmCross/issues/2825)
- MvxFormsAppCompatActivity.OnBackPressed override is broken. [\#2817](https://github.com/MvvmCross/MvvmCross/issues/2817)
- Add IsOnMainThread to IMvxMainThreadDispatcher [\#2791](https://github.com/MvvmCross/MvvmCross/issues/2791)

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
- MvxSimpleTableViewSource for include ViewCell designir in storyboard. [\#2897](https://github.com/MvvmCross/MvvmCross/pull/2897) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([andrechi1](https://github.com/andrechi1))
- log4net 2.0.8 netstandard compatibility fix [\#2896](https://github.com/MvvmCross/MvvmCross/pull/2896) ([StevenBonePgh](https://github.com/StevenBonePgh))
- Update WPF version too, to avoid NuGet restore complaining about downgrade [\#2890](https://github.com/MvvmCross/MvvmCross/pull/2890) ([Cheesebaron](https://github.com/Cheesebaron))
- Remove msbuild workaround [\#2889](https://github.com/MvvmCross/MvvmCross/pull/2889) ([martijn00](https://github.com/martijn00))
- Xamarin Forms version bump [\#2887](https://github.com/MvvmCross/MvvmCross/pull/2887) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([nickrandolph](https://github.com/nickrandolph))
- documentation: MvxLinearLayout [\#2879](https://github.com/MvvmCross/MvvmCross/pull/2879) ([Cheesebaron](https://github.com/Cheesebaron))
- documentation: add swipe to refresh android docs [\#2878](https://github.com/MvvmCross/MvvmCross/pull/2878) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([Cheesebaron](https://github.com/Cheesebaron))
- Correct behviour for ViewModel Lifecycle for UWP [\#2875](https://github.com/MvvmCross/MvvmCross/pull/2875) ([ueman](https://github.com/ueman))
- Add Forms WPF into the solution [\#2874](https://github.com/MvvmCross/MvvmCross/pull/2874) ([martijn00](https://github.com/martijn00))
- Update the-tip-calc-navigation.md [\#2873](https://github.com/MvvmCross/MvvmCross/pull/2873) ([Raidervz](https://github.com/Raidervz))
- Handling back pressed for Forms applications [\#2869](https://github.com/MvvmCross/MvvmCross/pull/2869) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([nickrandolph](https://github.com/nickrandolph))
- Added recursive method to search for referenced assemblies and load t… [\#2865](https://github.com/MvvmCross/MvvmCross/pull/2865) ([rdorta](https://github.com/rdorta))
- Update to Forms 3.0 [\#2864](https://github.com/MvvmCross/MvvmCross/pull/2864) ([martijn00](https://github.com/martijn00))
- documentation: Added UIRefreshControl docs [\#2861](https://github.com/MvvmCross/MvvmCross/pull/2861) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([Cheesebaron](https://github.com/Cheesebaron))
- Improve MvvmCross Getting Started Experience \(ReadMe Content & Sample Files\) [\#2858](https://github.com/MvvmCross/MvvmCross/pull/2858) ([andrewtechhelp](https://github.com/andrewtechhelp))
- Implement MvxWindowsPage.ClearBackStack [\#2855](https://github.com/MvvmCross/MvvmCross/pull/2855) [[UWP](https://github.com/MvvmCross/MvvmCross/labels/UWP)] ([andrechi1](https://github.com/andrechi1))
- \[documentation\] fixing Next link for UWP project [\#2853](https://github.com/MvvmCross/MvvmCross/pull/2853) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([halex2005](https://github.com/halex2005))
- Fixes \#2736 [\#2852](https://github.com/MvvmCross/MvvmCross/pull/2852) ([tbalcom](https://github.com/tbalcom))
- Fix usings in TipCalcTutorial [\#2850](https://github.com/MvvmCross/MvvmCross/pull/2850) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([gentilijuanmanuel](https://github.com/gentilijuanmanuel))
- Updates "Requesting presentation changes" documentation for MvvmCross 6 [\#2849](https://github.com/MvvmCross/MvvmCross/pull/2849) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([tbalcom](https://github.com/tbalcom))
- TipCalc Tutorial: Assets improvements & typos [\#2845](https://github.com/MvvmCross/MvvmCross/pull/2845) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([nmilcoff](https://github.com/nmilcoff))
- Amended LinkerPleaseInclude \(iOS\) example [\#2843](https://github.com/MvvmCross/MvvmCross/pull/2843) ([JoeCooper](https://github.com/JoeCooper))
- Use legacy properties on the Android TimePicker for versions 5.1 and … [\#2841](https://github.com/MvvmCross/MvvmCross/pull/2841) ([JimWilcox3](https://github.com/JimWilcox3))
- fix mvvmcross-overview link [\#2839](https://github.com/MvvmCross/MvvmCross/pull/2839) ([halex2005](https://github.com/halex2005))
- housekeeping: use https [\#2837](https://github.com/MvvmCross/MvvmCross/pull/2837) ([ghuntley](https://github.com/ghuntley))
- Revitalize Tipcalc tutorial [\#2835](https://github.com/MvvmCross/MvvmCross/pull/2835) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([nmilcoff](https://github.com/nmilcoff))
- Consolidate samples into one playground [\#2828](https://github.com/MvvmCross/MvvmCross/pull/2828) ([martijn00](https://github.com/martijn00))
- Allow options to be supplied to IocConstruct [\#2814](https://github.com/MvvmCross/MvvmCross/pull/2814) ([martijn00](https://github.com/martijn00))
- Implement basic infrastructure for Tizen and Tizen.Forms [\#2669](https://github.com/MvvmCross/MvvmCross/pull/2669) ([martijn00](https://github.com/martijn00))

## [6.1.0-beta2](https://github.com/MvvmCross/MvvmCross/tree/6.1.0-beta2) (2018-06-05)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.1.0-beta1...6.1.0-beta2)

**Closed issues:**

- UWP Dependency Injection on startup [\#2925](https://github.com/MvvmCross/MvvmCross/issues/2925)
- Object reference not set to an instance of an object in MvxChainedSourceBinding.Dispose [\#2922](https://github.com/MvvmCross/MvvmCross/issues/2922)

## [6.1.0-beta1](https://github.com/MvvmCross/MvvmCross/tree/6.1.0-beta1) (2018-05-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.1...6.1.0-beta1)

**Fixed bugs:**

- ViewModel\#Destroy does not get called on UWP [\#2860](https://github.com/MvvmCross/MvvmCross/issues/2860) [[UWP](https://github.com/MvvmCross/MvvmCross/labels/UWP)]
- Soft back button doesn't work in Playground.Android [\#2736](https://github.com/MvvmCross/MvvmCross/issues/2736) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]

**Closed issues:**

- Backstack in mvvmcross android working incorrectly [\#2913](https://github.com/MvvmCross/MvvmCross/issues/2913)
- SplashScreen doesn't automatically start Activity registered for AppStart [\#2895](https://github.com/MvvmCross/MvvmCross/issues/2895)
- UWP Release Builds are crashing at runtime [\#2842](https://github.com/MvvmCross/MvvmCross/issues/2842) [[UWP](https://github.com/MvvmCross/MvvmCross/labels/UWP)]
- MvxTimePicker won't bind correctly to Android versions 5.1 and below. [\#2840](https://github.com/MvvmCross/MvvmCross/issues/2840)
- stamp $\(AssemblyName\) \($\(TargetFramework\)\) into builds  [\#2836](https://github.com/MvvmCross/MvvmCross/issues/2836)

## [6.0.1](https://github.com/MvvmCross/MvvmCross/tree/6.0.1) (2018-04-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0...6.0.1)

**Fixed bugs:**

- SplashScreenAppCompat should use AppCompatSetup [\#2821](https://github.com/MvvmCross/MvvmCross/pull/2821) ([drungrin](https://github.com/drungrin))

**Closed issues:**

- Calling async method on ViewModel.Initialize never ends and InitializeTask properties never got updated [\#2829](https://github.com/MvvmCross/MvvmCross/issues/2829)
- Error when using the Initialize event in a view model since upgrade to Version 6.0 [\#2808](https://github.com/MvvmCross/MvvmCross/issues/2808)
- MvvmCross.Platforms.Android namespace missing from MvvmCross 6.0.0 package [\#2807](https://github.com/MvvmCross/MvvmCross/issues/2807)
- Documentation request [\#2806](https://github.com/MvvmCross/MvvmCross/issues/2806)
- Resource.Layout' does not contain a definition for '\[ViewName\]' [\#2801](https://github.com/MvvmCross/MvvmCross/issues/2801)
- MvxFormsIosSetup.CreateViewPresenter called too soon? [\#2802](https://github.com/MvvmCross/MvvmCross/issues/2802)

**Merged pull requests:**

- Small docs fix, renamed to correct method in the events mapping table. [\#2834](https://github.com/MvvmCross/MvvmCross/pull/2834) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([agoransson](https://github.com/agoransson))
- Update Getting-Started and MvvmCross-Overview docs [\#2822](https://github.com/MvvmCross/MvvmCross/pull/2822) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([nmilcoff](https://github.com/nmilcoff))
- Fixing crash when running Android Forms Playground [\#2820](https://github.com/MvvmCross/MvvmCross/pull/2820) ([nickrandolph](https://github.com/nickrandolph))
- Removing calls to base methods to prevent error [\#2819](https://github.com/MvvmCross/MvvmCross/pull/2819) ([nickrandolph](https://github.com/nickrandolph))
- Bugfix/fix playground wpf setup [\#2811](https://github.com/MvvmCross/MvvmCross/pull/2811) ([Cheesebaron](https://github.com/Cheesebaron))
- Android margin extensions method bind and binding docs updates [\#2809](https://github.com/MvvmCross/MvvmCross/pull/2809) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Update Acr.UserDialogs link [\#2805](https://github.com/MvvmCross/MvvmCross/pull/2805) ([vatsalyagoel](https://github.com/vatsalyagoel))
- Make it easier to override the Forms Page presenter [\#2803](https://github.com/MvvmCross/MvvmCross/pull/2803) ([martijn00](https://github.com/martijn00))

## [6.0.0](https://github.com/MvvmCross/MvvmCross/tree/6.0.0) (2018-04-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta8...6.0.0)

**Fixed bugs:**

- MvvmCross doesn't work with F\# Android app resources [\#2772](https://github.com/MvvmCross/MvvmCross/issues/2772) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- Playground.Droid creates multiple instances of RootViewModel [\#2782](https://github.com/MvvmCross/MvvmCross/issues/2782) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- Xamarin Forms Images is not shown on Android when using mvvmcross [\#2770](https://github.com/MvvmCross/MvvmCross/issues/2770) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)]
- Crash when Close Viewmodel With Result using MasterDetail [\#2757](https://github.com/MvvmCross/MvvmCross/issues/2757) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)]
- Playground SheetView crashes Android application [\#2722](https://github.com/MvvmCross/MvvmCross/issues/2722) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- Android app hangs on SplashScreen [\#2721](https://github.com/MvvmCross/MvvmCross/issues/2721) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- MvxViewPagerAdapter and MvxStateViewPagerAdapter ignore the presence of view model instance inside MvxViewPagerFragmentInfo [\#2718](https://github.com/MvvmCross/MvvmCross/issues/2718) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)]
- RegisterAttribute doesn't always match the new MvvmCross 6 namespace [\#2688](https://github.com/MvvmCross/MvvmCross/issues/2688) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)]
- App didn't show the right view after add SplashScreen on WPF [\#2684](https://github.com/MvvmCross/MvvmCross/issues/2684) [[WPF](https://github.com/MvvmCross/MvvmCross/labels/WPF)]
- \[iOS\] Text Replacement does not apply change through the binding [\#2681](https://github.com/MvvmCross/MvvmCross/issues/2681) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)]
- Language files are not loaded in iOS project [\#2678](https://github.com/MvvmCross/MvvmCross/issues/2678) [[Plugins](https://github.com/MvvmCross/MvvmCross/labels/Plugins)] [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)]
- Lack of Initialization from MvxSplashScreenActivity causes App start from external Intent \(ie, Uri routing\) to fail in Forms app [\#2624](https://github.com/MvvmCross/MvvmCross/issues/2624) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)]
- Xamarin.Forms Android - First page cannot reference Application level StaticResources [\#2622](https://github.com/MvvmCross/MvvmCross/issues/2622) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)]
- PluginLoaders not found for platform specific plugins [\#2611](https://github.com/MvvmCross/MvvmCross/issues/2611) [[Plugins](https://github.com/MvvmCross/MvvmCross/labels/Plugins)]
- Child View Presentation does not work when using More Tabs \(more than five tabs\) \[iOS\]  [\#2609](https://github.com/MvvmCross/MvvmCross/issues/2609) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)]
- Adjusting the resolution of the resource assembly [\#2777](https://github.com/MvvmCross/MvvmCross/pull/2777) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([nickrandolph](https://github.com/nickrandolph))
- Fixing issue with CurrentActivity being null in Playground.Droid [\#2775](https://github.com/MvvmCross/MvvmCross/pull/2775) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] ([nickrandolph](https://github.com/nickrandolph))
- Find resource type based on Android.Runtime.ResourceDesignerAttribute [\#2774](https://github.com/MvvmCross/MvvmCross/pull/2774) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] ([nosami](https://github.com/nosami))
- Make show and close of iOS views respect Animated [\#2767](https://github.com/MvvmCross/MvvmCross/pull/2767) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([martijn00](https://github.com/martijn00))
- MvxUISliderValueTargetBinding: Add missing return [\#2750](https://github.com/MvvmCross/MvvmCross/pull/2750) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([nmilcoff](https://github.com/nmilcoff))
- Fixes \#2722 [\#2730](https://github.com/MvvmCross/MvvmCross/pull/2730) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([tbalcom](https://github.com/tbalcom))
- Make sure Forms is loaded when not using a splashscreen [\#2729](https://github.com/MvvmCross/MvvmCross/pull/2729) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([martijn00](https://github.com/martijn00))
- Android add MvxViewVodelRequest to fragment forward life cycle events [\#2728](https://github.com/MvvmCross/MvvmCross/pull/2728) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([Plac3hold3r](https://github.com/Plac3hold3r))
- Android Dialogs: Fix close & do not keep references to instances [\#2711](https://github.com/MvvmCross/MvvmCross/pull/2711) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([nmilcoff](https://github.com/nmilcoff))
- Improvements & register fix for Visibility / Messenger / PictureChooser plugins [\#2704](https://github.com/MvvmCross/MvvmCross/pull/2704) [[Plugins](https://github.com/MvvmCross/MvvmCross/labels/Plugins)] ([nmilcoff](https://github.com/nmilcoff))
- Fix moving items in the MvxRecyclerAdapter [\#2664](https://github.com/MvvmCross/MvvmCross/pull/2664) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([kjeremy](https://github.com/kjeremy))
- MvxBaseTableViewSource: Fix wrong height for xib based cells  [\#2644](https://github.com/MvvmCross/MvvmCross/pull/2644) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([nmilcoff](https://github.com/nmilcoff))
- Apply default templates to MvxAppCompatSpinner [\#2640](https://github.com/MvvmCross/MvvmCross/pull/2640) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([kjeremy](https://github.com/kjeremy))
- Binding types fix [\#2632](https://github.com/MvvmCross/MvvmCross/pull/2632) ([Saratsin](https://github.com/Saratsin))
- Fixed issue \#2515 where MvxObservableCollection.AddRange\(\) reports wrong index [\#2614](https://github.com/MvvmCross/MvvmCross/pull/2614) ([Strifex](https://github.com/Strifex))

**Closed issues:**

- Generic UWP views break compiled bindings [\#2653](https://github.com/MvvmCross/MvvmCross/issues/2653) [[UWP](https://github.com/MvvmCross/MvvmCross/labels/UWP)]
- Attribute "MvxBind" has already been defined [\#2800](https://github.com/MvvmCross/MvvmCross/issues/2800) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)]
- WPF Support Missing From MvvmCross 6.0.0 [\#2796](https://github.com/MvvmCross/MvvmCross/issues/2796)
- \[iOS\] Using VS AppCenter "AppCenter.Start" while MvxApplication.Initialize results in deadlock since MVX 6 beta7 [\#2745](https://github.com/MvvmCross/MvvmCross/issues/2745)
- WPF Presenter documentation is out of date [\#2743](https://github.com/MvvmCross/MvvmCross/issues/2743) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] [[WPF](https://github.com/MvvmCross/MvvmCross/labels/WPF)]
- NuGet package descriptions are missing from csproj files [\#2742](https://github.com/MvvmCross/MvvmCross/issues/2742) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)]
- Need an example of custom activity transitions [\#2659](https://github.com/MvvmCross/MvvmCross/issues/2659) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)]
- -	Skipping DigitalWorkReport.Droid.Resource.String.fab\_scroll\_shrink\_grow\_autohide\_behavior [\#2645](https://github.com/MvvmCross/MvvmCross/issues/2645) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)]
- Cleanup "Sidebar" plugin [\#2626](https://github.com/MvvmCross/MvvmCross/issues/2626) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)]
- MvvmCross.Forms version out of sync with Xamarin.Forms Tutorial [\#2620](https://github.com/MvvmCross/MvvmCross/issues/2620) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)]

**Merged pull requests:**

- Update 2018-4-14-mvvmcross-6.0.0-release.md [\#2799](https://github.com/MvvmCross/MvvmCross/pull/2799) ([fedemkr](https://github.com/fedemkr))
- Making it easier to override creation of injected pages [\#2793](https://github.com/MvvmCross/MvvmCross/pull/2793) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([nickrandolph](https://github.com/nickrandolph))
- Improving Forms Android support for setup [\#2790](https://github.com/MvvmCross/MvvmCross/pull/2790) ([nickrandolph](https://github.com/nickrandolph))
- Split out WPF [\#2789](https://github.com/MvvmCross/MvvmCross/pull/2789) ([martijn00](https://github.com/martijn00))
- Update 3rd-party-plugins.md [\#2788](https://github.com/MvvmCross/MvvmCross/pull/2788) ([vurf](https://github.com/vurf))
- Bugfix/aapt error workaround [\#2787](https://github.com/MvvmCross/MvvmCross/pull/2787) ([Cheesebaron](https://github.com/Cheesebaron))
- Added Margin target binding for Android [\#2780](https://github.com/MvvmCross/MvvmCross/pull/2780) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] ([Cheesebaron](https://github.com/Cheesebaron))
- Fix duplicated entry for SplitView attribute on iOS ViewPresenter [\#2779](https://github.com/MvvmCross/MvvmCross/pull/2779) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([nmilcoff](https://github.com/nmilcoff))
- Update json.md [\#2778](https://github.com/MvvmCross/MvvmCross/pull/2778) ([dawidstefaniak](https://github.com/dawidstefaniak))
- Make build.sh executable so that `./build.sh` works [\#2773](https://github.com/MvvmCross/MvvmCross/pull/2773) ([nosami](https://github.com/nosami))
- Making IMvxViewDispatcher async aware [\#2771](https://github.com/MvvmCross/MvvmCross/pull/2771) ([nickrandolph](https://github.com/nickrandolph))
- Make setup interface standard [\#2769](https://github.com/MvvmCross/MvvmCross/pull/2769) ([martijn00](https://github.com/martijn00))
- Match docs with code for WPF presenter [\#2768](https://github.com/MvvmCross/MvvmCross/pull/2768) ([martijn00](https://github.com/martijn00))
- Descriptions from old nuspecs in csprojs added [\#2766](https://github.com/MvvmCross/MvvmCross/pull/2766) ([orzech85](https://github.com/orzech85))
- Upgrading XF references [\#2764](https://github.com/MvvmCross/MvvmCross/pull/2764) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([nickrandolph](https://github.com/nickrandolph))
- Fixing issue with navigating back with result from master details [\#2763](https://github.com/MvvmCross/MvvmCross/pull/2763) ([nickrandolph](https://github.com/nickrandolph))
- Adding Application Startup method to be invoked on UI thread [\#2761](https://github.com/MvvmCross/MvvmCross/pull/2761) ([nickrandolph](https://github.com/nickrandolph))
- Switch to new Project SDK style for MSBuildExtras + build log [\#2759](https://github.com/MvvmCross/MvvmCross/pull/2759) ([Cheesebaron](https://github.com/Cheesebaron))
- Update appveyor and Android support library [\#2758](https://github.com/MvvmCross/MvvmCross/pull/2758) ([martijn00](https://github.com/martijn00))
- Fix MvvmCross.Forms project dependencies [\#2756](https://github.com/MvvmCross/MvvmCross/pull/2756) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([Cheesebaron](https://github.com/Cheesebaron))
- Adding Async dispatcher [\#2755](https://github.com/MvvmCross/MvvmCross/pull/2755) ([nickrandolph](https://github.com/nickrandolph))
- Only set build script names once [\#2753](https://github.com/MvvmCross/MvvmCross/pull/2753) ([martijn00](https://github.com/martijn00))
- Improve a couple of build and project files [\#2752](https://github.com/MvvmCross/MvvmCross/pull/2752) ([martijn00](https://github.com/martijn00))
- Mark iOS and tvOS AppDelegate windows as virtual [\#2747](https://github.com/MvvmCross/MvvmCross/pull/2747) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] [[tvOS](https://github.com/MvvmCross/MvvmCross/labels/tvOS)] ([nmilcoff](https://github.com/nmilcoff))
- MvxSetupSingleton optimizations / Fix SplashScreen initialization on Android [\#2746](https://github.com/MvvmCross/MvvmCross/pull/2746) ([nmilcoff](https://github.com/nmilcoff))
- Improvements for v6 docs [\#2739](https://github.com/MvvmCross/MvvmCross/pull/2739) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([nmilcoff](https://github.com/nmilcoff))
- Fix linker problem [\#2737](https://github.com/MvvmCross/MvvmCross/pull/2737) ([martijn00](https://github.com/martijn00))
- Added generic versions of MvxApplication for WPF and UWP [\#2735](https://github.com/MvvmCross/MvvmCross/pull/2735) [[UWP](https://github.com/MvvmCross/MvvmCross/labels/UWP)] [[WPF](https://github.com/MvvmCross/MvvmCross/labels/WPF)] ([nmilcoff](https://github.com/nmilcoff))
- Clean up some long time obsolete API's [\#2734](https://github.com/MvvmCross/MvvmCross/pull/2734) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Update nuget packages [\#2733](https://github.com/MvvmCross/MvvmCross/pull/2733) ([martijn00](https://github.com/martijn00))
- MvxAppStart: Add non-generic version [\#2732](https://github.com/MvvmCross/MvvmCross/pull/2732) ([nmilcoff](https://github.com/nmilcoff))
- Setup fixes [\#2731](https://github.com/MvvmCross/MvvmCross/pull/2731) ([martijn00](https://github.com/martijn00))
- Add missing license to SelectedItemRecyclerAdapter [\#2727](https://github.com/MvvmCross/MvvmCross/pull/2727) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Make it easier to build on Mac and fix some build bugs [\#2726](https://github.com/MvvmCross/MvvmCross/pull/2726) ([martijn00](https://github.com/martijn00))
- Android add Shared Elements support [\#2725](https://github.com/MvvmCross/MvvmCross/pull/2725) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] ([Plac3hold3r](https://github.com/Plac3hold3r))
- Removed no longer relevant comment in MvxAppCompatSetup [\#2724](https://github.com/MvvmCross/MvvmCross/pull/2724) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([Plac3hold3r](https://github.com/Plac3hold3r))
- Helpers for testing RaiseCanExecuteChanged [\#2720](https://github.com/MvvmCross/MvvmCross/pull/2720) ([jacobduijzer](https://github.com/jacobduijzer))
- \#2718 Ensure that existing fragment info's VM instance is used whenev… [\#2719](https://github.com/MvvmCross/MvvmCross/pull/2719) [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([wh1t3cAt1k](https://github.com/wh1t3cAt1k))
- Add SplashScreen for WPF Playground to demonstrate loading issue [\#2716](https://github.com/MvvmCross/MvvmCross/pull/2716) [[WPF](https://github.com/MvvmCross/MvvmCross/labels/WPF)] ([Happin3ss](https://github.com/Happin3ss))
- Move bootstrap explanation to the upgrade [\#2715](https://github.com/MvvmCross/MvvmCross/pull/2715) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([martijn00](https://github.com/martijn00))
- Target WPF 4.6.1 so it is aligned with .net standard [\#2714](https://github.com/MvvmCross/MvvmCross/pull/2714) ([martijn00](https://github.com/martijn00))
- Refactoring Setup Singleton  [\#2713](https://github.com/MvvmCross/MvvmCross/pull/2713) ([nickrandolph](https://github.com/nickrandolph))
- Avoid using reflection to create the setup [\#2710](https://github.com/MvvmCross/MvvmCross/pull/2710) ([nickrandolph](https://github.com/nickrandolph))
- ItemTemplateId added to IMvxTemplateSelector [\#2709](https://github.com/MvvmCross/MvvmCross/pull/2709) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] ([orzech85](https://github.com/orzech85))
- Update WPF playground [\#2708](https://github.com/MvvmCross/MvvmCross/pull/2708) ([jz5](https://github.com/jz5))
- Remove MvxImageView and the likes [\#2706](https://github.com/MvvmCross/MvvmCross/pull/2706) ([nmilcoff](https://github.com/nmilcoff))
- V6 blog post & migration guide [\#2705](https://github.com/MvvmCross/MvvmCross/pull/2705) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([nmilcoff](https://github.com/nmilcoff))
- Make application class virtual [\#2703](https://github.com/MvvmCross/MvvmCross/pull/2703) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([martijn00](https://github.com/martijn00))
- Align setup and not override window [\#2702](https://github.com/MvvmCross/MvvmCross/pull/2702) ([martijn00](https://github.com/martijn00))
- Fix readme and some small issue's [\#2701](https://github.com/MvvmCross/MvvmCross/pull/2701) ([martijn00](https://github.com/martijn00))
- Change platform to platforms [\#2700](https://github.com/MvvmCross/MvvmCross/pull/2700) ([martijn00](https://github.com/martijn00))
- Update ios-tables-and-cells.md [\#2699](https://github.com/MvvmCross/MvvmCross/pull/2699) ([jfversluis](https://github.com/jfversluis))
- Make it easier to change csproj versions and update nugets [\#2695](https://github.com/MvvmCross/MvvmCross/pull/2695) ([martijn00](https://github.com/martijn00))
- Lists inconsistencies [\#2693](https://github.com/MvvmCross/MvvmCross/pull/2693) [[Android](https://github.com/MvvmCross/MvvmCross/labels/Android)] [[AndroidSupport](https://github.com/MvvmCross/MvvmCross/labels/AndroidSupport)] ([orzech85](https://github.com/orzech85))
- MvxPreferenceValueTargetBinding: Fix method call: Warn -\> Warning [\#2692](https://github.com/MvvmCross/MvvmCross/pull/2692) ([nmilcoff](https://github.com/nmilcoff))
- Updates RegisterAttribute to match new Mvx 6 namespaces [\#2691](https://github.com/MvvmCross/MvvmCross/pull/2691) ([tbalcom](https://github.com/tbalcom))
- v6 Blog post & migration guide updates [\#2689](https://github.com/MvvmCross/MvvmCross/pull/2689) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([nmilcoff](https://github.com/nmilcoff))
- Use previous version of VS2017 on AppVeyor [\#2685](https://github.com/MvvmCross/MvvmCross/pull/2685) ([Cheesebaron](https://github.com/Cheesebaron))
- Add a playground page to test whether MvxContentViews load their view models [\#2683](https://github.com/MvvmCross/MvvmCross/pull/2683) ([jessewdouglas](https://github.com/jessewdouglas))
- \[iOS\] Update UITextField binding when editing did end \(Text Replacement fix\) [\#2682](https://github.com/MvvmCross/MvvmCross/pull/2682) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([alexshikov](https://github.com/alexshikov))
- Fixes calling methods deprecated in Android API26. [\#2679](https://github.com/MvvmCross/MvvmCross/pull/2679) ([tbalcom](https://github.com/tbalcom))
- Update docs broken Playground sample link [\#2677](https://github.com/MvvmCross/MvvmCross/pull/2677) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([fedemkr](https://github.com/fedemkr))
- Move hints and attributes into the right folder [\#2675](https://github.com/MvvmCross/MvvmCross/pull/2675) ([martijn00](https://github.com/martijn00))
- Ensuring Start completed before initializing Xamarin Forms [\#2674](https://github.com/MvvmCross/MvvmCross/pull/2674) ([nickrandolph](https://github.com/nickrandolph))
- Update changelog [\#2673](https://github.com/MvvmCross/MvvmCross/pull/2673) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([Cheesebaron](https://github.com/Cheesebaron))
- Added RaiseCanExecuteChanged interface definition to IMvxCommand\<T\> [\#2672](https://github.com/MvvmCross/MvvmCross/pull/2672) ([jnosek](https://github.com/jnosek))
- Cleanup csproj files and add missing headers [\#2671](https://github.com/MvvmCross/MvvmCross/pull/2671) ([martijn00](https://github.com/martijn00))
- Add check in navigation service to see if viewmodels are available [\#2670](https://github.com/MvvmCross/MvvmCross/pull/2670) ([martijn00](https://github.com/martijn00))
- Add generic setup to all platforms [\#2668](https://github.com/MvvmCross/MvvmCross/pull/2668) ([martijn00](https://github.com/martijn00))
- Update upgrade-to-mvvmcross-60.md [\#2667](https://github.com/MvvmCross/MvvmCross/pull/2667) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([asterixorobelix](https://github.com/asterixorobelix))
- Fix some projects not building in specific configs [\#2663](https://github.com/MvvmCross/MvvmCross/pull/2663) ([martijn00](https://github.com/martijn00))
- Delete empty IMvxModalIosView [\#2660](https://github.com/MvvmCross/MvvmCross/pull/2660) [[Plugins](https://github.com/MvvmCross/MvvmCross/labels/Plugins)] [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([martijn00](https://github.com/martijn00))
- Add readme.txt file to open on nuget install [\#2658](https://github.com/MvvmCross/MvvmCross/pull/2658) ([martijn00](https://github.com/martijn00))
- Rename test to tests to align with old nuget package and current naming [\#2657](https://github.com/MvvmCross/MvvmCross/pull/2657) ([martijn00](https://github.com/martijn00))
- Version bump for UWP and SDKExtras nuget packages [\#2656](https://github.com/MvvmCross/MvvmCross/pull/2656) [[UWP](https://github.com/MvvmCross/MvvmCross/labels/UWP)] ([nickrandolph](https://github.com/nickrandolph))
- Documentation: MvxNotifyTask, ViewModel-Lifecycle location, Samples [\#2655](https://github.com/MvvmCross/MvvmCross/pull/2655) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([nmilcoff](https://github.com/nmilcoff))
- Merge master back into develop to update docs [\#2654](https://github.com/MvvmCross/MvvmCross/pull/2654) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([nmilcoff](https://github.com/nmilcoff))
- add throw for Exception in MvxNavigationService [\#2651](https://github.com/MvvmCross/MvvmCross/pull/2651) ([kvandake](https://github.com/kvandake))
- Add docs for resharper annotations [\#2648](https://github.com/MvvmCross/MvvmCross/pull/2648) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([mterwoord](https://github.com/mterwoord))
- MvxNotifyTask improvements [\#2642](https://github.com/MvvmCross/MvvmCross/pull/2642) ([nmilcoff](https://github.com/nmilcoff))
- Update nuget packages [\#2639](https://github.com/MvvmCross/MvvmCross/pull/2639) ([martijn00](https://github.com/martijn00))
- Don't put a user dependency on Microsoft.CSharp [\#2638](https://github.com/MvvmCross/MvvmCross/pull/2638) ([martijn00](https://github.com/martijn00))
- Adding support for custom renderer assemblies to MvxFormsWindowsSetup [\#2635](https://github.com/MvvmCross/MvvmCross/pull/2635) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] [[UWP](https://github.com/MvvmCross/MvvmCross/labels/UWP)] ([MartinZikmund](https://github.com/MartinZikmund))
- Rename folders from iOS to Ios [\#2630](https://github.com/MvvmCross/MvvmCross/pull/2630) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([nmilcoff](https://github.com/nmilcoff))
- Fix the compiling error of Playground.Forms.Uwp [\#2628](https://github.com/MvvmCross/MvvmCross/pull/2628) ([flyingxu](https://github.com/flyingxu))
- Align namespaces: Rename iOS namespaces to Ios [\#2627](https://github.com/MvvmCross/MvvmCross/pull/2627) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] ([nmilcoff](https://github.com/nmilcoff))
- Remove DownloadCache plugin [\#2625](https://github.com/MvvmCross/MvvmCross/pull/2625) [[Plugins](https://github.com/MvvmCross/MvvmCross/labels/Plugins)] ([nmilcoff](https://github.com/nmilcoff))
- Add observable collection tests [\#2618](https://github.com/MvvmCross/MvvmCross/pull/2618) ([Cheesebaron](https://github.com/Cheesebaron))
- Use the basic Forms application type instead of the Mvx one [\#2617](https://github.com/MvvmCross/MvvmCross/pull/2617) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] ([martijn00](https://github.com/martijn00))
- Improve the IMvxPluginManager interface [\#2616](https://github.com/MvvmCross/MvvmCross/pull/2616) [[Plugins](https://github.com/MvvmCross/MvvmCross/labels/Plugins)] ([willsb](https://github.com/willsb))
- Reducing code to get started for UWP projects [\#2615](https://github.com/MvvmCross/MvvmCross/pull/2615) [[Forms](https://github.com/MvvmCross/MvvmCross/labels/Forms)] [[UWP](https://github.com/MvvmCross/MvvmCross/labels/UWP)] ([nickrandolph](https://github.com/nickrandolph))
- Fix a bunch of Warnings [\#2613](https://github.com/MvvmCross/MvvmCross/pull/2613) ([Cheesebaron](https://github.com/Cheesebaron))
- Update Forms namespaces to match traditional Xamarin  [\#2612](https://github.com/MvvmCross/MvvmCross/pull/2612) ([nmilcoff](https://github.com/nmilcoff))
- Allow more than 5 children in MoreNavigationController [\#2610](https://github.com/MvvmCross/MvvmCross/pull/2610) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)] [[tvOS](https://github.com/MvvmCross/MvvmCross/labels/tvOS)] ([thefex](https://github.com/thefex))
- Cleanup: Create Presenters & Commands folders / Remove {PlatformName}.Base folders [\#2606](https://github.com/MvvmCross/MvvmCross/pull/2606) ([nmilcoff](https://github.com/nmilcoff))

## [6.0.0-beta8](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta8) (2018-04-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta7...6.0.0-beta8)

**Closed issues:**

- ViewControllers animation can't be disabled for VM Navigate\(\) / Close\(\) [\#2762](https://github.com/MvvmCross/MvvmCross/issues/2762) [[iOS](https://github.com/MvvmCross/MvvmCross/labels/iOS)]

**Merged pull requests:**

- Reverts IsXamarinForms changes in Playground [\#2754](https://github.com/MvvmCross/MvvmCross/pull/2754) ([tbalcom](https://github.com/tbalcom))
- Update ContentFiles Referenced In Readme.txt To Use MVVMCross 6.x Namespaces \(& Remove MvxTrace References\) [\#2748](https://github.com/MvvmCross/MvvmCross/pull/2748) ([andrewtechhelp](https://github.com/andrewtechhelp))
- Add doc about linking [\#2744](https://github.com/MvvmCross/MvvmCross/pull/2744) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([Cheesebaron](https://github.com/Cheesebaron))
- Update messenger.md [\#2741](https://github.com/MvvmCross/MvvmCross/pull/2741) [[Documentation](https://github.com/MvvmCross/MvvmCross/labels/Documentation)] ([orzech85](https://github.com/orzech85))

## [6.0.0-beta7](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta7) (2018-03-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta6...6.0.0-beta7)

**Closed issues:**

- Add the ability to only use one instance of a given fragment [\#2694](https://github.com/MvvmCross/MvvmCross/issues/2694)

## [6.0.0-beta6](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta6) (2018-03-19)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.7.0...6.0.0-beta6)

**Closed issues:**

- Using MvvmCross 5.6 NavigationService with autofac IoC [\#2636](https://github.com/MvvmCross/MvvmCross/issues/2636)

**Merged pull requests:**

- Release/5.7.0 [\#2687](https://github.com/MvvmCross/MvvmCross/pull/2687) ([Cheesebaron](https://github.com/Cheesebaron))
- Create 2018-03-14-mvvmcross-5.7.0-release.md [\#2686](https://github.com/MvvmCross/MvvmCross/pull/2686) ([Cheesebaron](https://github.com/Cheesebaron))

## [5.7.0](https://github.com/MvvmCross/MvvmCross/tree/5.7.0) (2018-03-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta5...5.7.0)

## [6.0.0-beta5](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta5) (2018-03-09)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta4...6.0.0-beta5)

**Closed issues:**

- Support for netstandard1.4 in version 6.0 [\#2649](https://github.com/MvvmCross/MvvmCross/issues/2649)
- get view api [\#2643](https://github.com/MvvmCross/MvvmCross/issues/2643)

## [6.0.0-beta4](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta4) (2018-03-02)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta3...6.0.0-beta4)

**Closed issues:**

- MvvmCross.Plugin.Location.Fused 5.6.3 is not compatible with netstandard2.0 [\#2607](https://github.com/MvvmCross/MvvmCross/issues/2607)

**Merged pull requests:**

- Revert "MvxNavigationService.cs. Add "throw" for an exception in the method NavigationRouteRequest." [\#2650](https://github.com/MvvmCross/MvvmCross/pull/2650) ([Cheesebaron](https://github.com/Cheesebaron))
- MvxNavigationService.cs. Add "throw" for an exception in the method NavigationRouteRequest. [\#2647](https://github.com/MvvmCross/MvvmCross/pull/2647) ([kvandake](https://github.com/kvandake))

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


\* *This Change Log was automatically generated by [github_changelog_generator](https://github.com/skywinder/Github-Changelog-Generator)*