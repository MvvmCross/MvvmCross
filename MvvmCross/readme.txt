---------------------------------
MvvmCross
---------------------------------

IMPORTANT: No NuGet content files will be installed!

--- Quick steps to get MvvmCross up and running ---
The following steps should get MvvmCross to load in a newly created project.
MvvmCross provides a lot more features that are not discussed in this readme, these steps will just get you a bare-bones setup. 
See the documentation at https://www.mvvmcross.com/ to learn how to make full use of the featureset MvvmCross provides.

-- Assumptions --

1. Your solution already has a .NET Standard 2.x project where shared code will be located (often referred to as the Core project).
2. Your solution already has a platform specific project for every platform you wish to support. 
   Platforms include Xamarin.iOS, Xamarin.Android, Xamarin.Mac, Xamarin.tvOS, Universal Windows Platform (UWP) and Windows Presentation Foundation (WPF).
3. You have added this NuGet package (MvvmCross) to to all projects in your solution.
4. Each of the platform specific projects have a reference to the Core project.

-- Changes to make to your project (Traditional Xamarin Solution - No Xamarin.Forms) --

NOTE: These instructions are for projects that wish to use a traditional Xamarin setup (that is, user interfaces written in each platform's project using that platform's UI tooling/markup).
If you wish to use Xamarin.Forms to write shared code for your user interfaces, disregard the rest of these instructions, install the MvvmCross.Forms NuGet package and follow the instructions
in the Readme file included in that NuGet package instead. An MvvmCross project with Xamarin.Forms requires a slightly different setup and therefore has a different set of instructions to this Readme.

Samples of all files referenced below can be found here:
https://github.com/MvvmCross/MvvmCross/blob/develop/ContentFiles/

Most of these sample files can be copied directly into a newly created solution and will work. 
However, any $rootnamespace$ instances will need to be changed to your project's namespace and you will also need to add appropriate using statements (Visual Studio IntelliSense should suggest these).


- Core project -
1. Add an App class to the root folder (See Core/App.cs.pp in sample files).
2. Add a ViewModels folder to the root of the project and add at least one ViewModel class to this folder (See Core/HomeViewModel.cs.pp in sample files).


- Android projects (ignore if not building for Android) -
1. Add a reference to the Mono.Android.Export assembly.
2. Delete MainActivity.cs, and add a new activity class called SplashScreen to the root folder.
3. Change the SplashScreen activity class to inherit from MvxSplashScreenActivity<MvxAndroidSetup<Core.App>, Core.App> instead of Activity (See Android/SplashScreen.cs.pp in sample files).
4. Add a new Android layout to the Resources/layout folder to correspond to the SplashScreen created in the previous step. (See Android/SplashScreen.axml.pp in sample files).
5. Add a Views folder to the root of the project and add at least one View class to this folder to correspond to the ViewModel class in the Core project (See Android/HomeView.cs.pp in sample files).
6. Add a new Android layout to the Resources/layout folder to correspond to the View class created in the previous step. (See Android/HomeView.axml.pp in sample files).

Note: If you wish to use the AppCompat versions of Android classes, you can follow the above instructions with the following modifications
7. Add the MvvmCross v7 Android AppCompat Support Libraries NuGet package (MvvmCross.Droid.Support.v7.AppCompat) to to your Android platform specific project.
8. When changing the the inheritance for the SplashScreen activity class, use MvxSplashScreenAppCompatActivity instead of MvxSplashScreenActivity.
9. Add a new XML file that defines an AppCompat theme to the Resources/values folder called styles.xml. (See Android/styles.xml.pp in sample files)
10. Ensure the theme created in the previous step is referenced in the Attribute for the MainActivity class (Theme = "@style/MainTheme").


- iOS projects (ignore if not building for iOS) -
1. Inside AppDelegate.cs, change the AppDelegate class to inherit from MvxApplicationDelegate<MvxIosSetup<Core.App>, Core.App> instead of ApplicationDelegate (See iOS/AppDelegate.cs.pp in sample files).
2. Still inside AppDelegate.cs, delete all the pre-populated methods to leave a blank AppDelegate class.
3. Add a Views folder to the root of the project and add at least one View class to this folder to correspond to the ViewModel class in the Core project (See iOS/HomeView.cs.pp in sample files).
4. Add a new iOS Interface Builder layout (XIB) or StoryBoard file to the Views folder to correspond to the View created in the previous step. (See iOS/HomeView.xib.pp in sample files).


- macOS projects (ignore if not building for macOS) -
1. Inside AppDelegate.cs, change the AppDelegate class to inherit from MvxApplicationDelegate<MvxMacSetup<Core.App>, Core.App> instead of ApplicationDelegate (See macOS/AppDelegate.cs.pp in sample files).
2. Still inside AppDelegate.cs replace the contents of the DidFinishLaunching method with the following:
   MvxMacSetupSingleton.EnsureSingletonAvailable(this, MainWindow).EnsureInitialized();
   RunAppStart();
3. Still inside AppDelegate, delete all the other pre-populated methods.
4. Add a Views folder and add at least one View file to this folder to correspond to the ViewModel in the Core project (See macOS/HomeView.cs.pp in sample files).
5. Add a new iOS Interface Builder layout (XIB) or StoryBoard file to the Views folder to correspond to the View created in the previous step. (See macOS/Home.storyboard.pp in sample files).


- tvOS projects (ignore if not building for tvOS) -
1. Inside AppDelegate.cs, change the AppDelegate class to inherit from MvxApplicationDelegate<MvxTvosSetup<Core.App>, Core.App> instead of ApplicationDelegate (See tvOS/AppDelegate.cs.pp in sample files).
2. Still inside AppDelegate.cs, delete all the pre-populated methods to leave a blank AppDelegate class.
3. Add a Views folder and add at least one View file to this folder to correspond to the ViewModel in the Core project (See tvOS/HomeView.cs.pp in sample files).
4. Add a new iOS Interface Builder layout (XIB) or StoryBoard file to the Views folder to correspond to the View created in the previous step. (See tvOS/Home.storyboard.pp in sample files).


- Universal Windows Platform (UWP) projects (ignore if not building for UWP) -
1. Inside App.xaml.cs add a new partial class called UWPApplication that inherits from MvxApplication<MvxWindowsSetup<Core.App>, Core.App> (See UWP/App.xaml.cs.pp in sample files).
2. Still inside App.xaml.cs change the App class to inherit from UWPApplication class created in the previous step (See UWP/App.xaml.cs.pp in sample files).
3. Still inside App.xaml.cs remove all the methods inside the App class except for the constructor. Delete everything except InitializeComponent(); from the constructor (See UWP/App.xaml.cs.pp in sample files).
4. Inside App.xaml change the Application XML tag to local:UWPApplication (See UWP/App.xaml.pp in sample files).
5. Add a Views folder to the root of the project and add at least one XAML page to this folder to correspond to the ViewModel class in the Core project. (See UWP/HomeView.xaml.pp in sample files).
6. Inside the .xaml file that is created when a new XAML page is added, change the Page XML tag to views:MvxWindowsPage and add xmlns:views="using:MvvmCross.Platforms.Uap.Views" to that XML tag.
7. Inside the .xaml.cs file that is created when a new XAML page is added, change the class to inherit from MvxWindowsPage  (See UWP/HomeView.xaml.cs.pp in sample files).


- Windows Presentation Foundation (WPF) projects (ignore if not building for WPF) -
1. Add the MVVMCross WPF Platform NuGet package (MvvmCross.Platforms.Wpf) to your WPF platform specific project.
2. Inside App.xaml.cs change the App class to inherit from MvxApplication (See WPF/App.xaml.cs.pp in sample files).
3. Still inside App.xaml.cs add a constructor to the App class with the following line of code: this.RegisterSetupType<MvxWpfSetup<Core.App>>(); (See WPF/App.xaml.cs.pp in sample files).
4. Inside App.xaml change the Application XML tag to views:MvxApplication (See WPF/App.xaml.pp in sample files).
5. Still inside App.xaml add xmlns:views="clr-namespace:MvvmCross.Platforms.Wpf.Views;assembly=MvvmCross.Platforms.Wpf" to the views:MvxApplication tag (See WPF/App.xaml.pp in sample files).
6. Add a Views folder to the root of the project and add at least one XAML page to this folder to correspond to the ViewModel class in the Core project (See WPF/HomeView.xaml.pp in sample files).
7. Inside the .xaml file that is created when a new XAML page is added, change the Page XML tag to views:MvxWpfView (See WPF/HomeView.xaml.pp in sample files).
8. Still inside the .xaml file that is created when a new XAML page is added add xmlns:views="clr-namespace:MvvmCross.Platforms.Wpf.Views;assembly=MvvmCross.Platforms.Wpf" to the views:MvxWpfView tag (See WPF/HomeView.xaml.pp in sample files).
9. Inside the .xaml.cs file that is created when a new XAML page is added, change the class to inherit from MvxWpfView (See WPF/HomeView.xaml.cs.pp in sample files).
10. Inside MainWindow.xaml change the Application XML tag to views:MvxWindow (See WPF/MainWindow.xaml.pp in sample files).
11. Still inside MainWindow.xaml add xmlns:views="clr-namespace:MvvmCross.Platforms.Wpf.Views;assembly=MvvmCross.Platforms.Wpf" to the views:MvxWindow tag (See WPF/MainWindow.xaml.pp in sample files).
12. Inside MainWindow.xaml.cs change the MainWindow class to inherit from MvxWindow (See WPF/MainWindow.xaml.cs.pp in sample files).


--- Changelog ---
https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md

--- More information ---
This readme contains the bare bones instructions for setting up MvvmCross in a new solution. For complete startup documentation you can read more on our website:

Getting Started Documentation:
https://www.mvvmcross.com/documentation/getting-started/getting-started