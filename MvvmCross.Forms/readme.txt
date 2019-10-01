---------------------------------
MvvmCross
---------------------------------

IMPORTANT: No NuGet content files will be installed!

--- Quick steps to get MvvmCross with MvvmCross.Forms up and running ---
The following steps should get MvvmCross with MvvmCross.Forms to load in a newly created project. 
MvvmCross provides a lot more features that are not discussed in this readme, this will get you a bare-bones setup. 
See the documentation at https://www.mvvmcross.com/ to learn how to make full use of the featureset MvvmCross provides.

-- Assumptions --

1. Your solution already has a .NET Standard 2.x project where shared code will be located (often referred to as the Core project).
2. Your solution already has a platform specific project for every platform you wish to support. 
   Platforms include Xamarin.iOS, Xamarin.Android and Universal Windows Platform (UWP).
3. Your solution already has an additional .NET Standard 2.x project where your Xamarin.Forms views will be located (often referred to as the Xamarin.Forms UI project).
4. You have added the MvvmCross NuGet package (MvvmCross) to to all projects in your solution.
5. You have added the Xamarin.Forms NuGet package (Xamarin.Forms) to to all the platform specific projects & the Xamarin.Forms UI project.
6. You have added this NuGet package (MvvmCross.Forms) to to all the platform specific projects & the Xamarin.Forms UI project.
7. Each of the platform specific projects and the Xamarin.Forms UI project has a reference to the Core project.
8. Each of the platform specific projects also has a reference to the Xamarin.Forms UI project.

-- Changes to make to your project (Xamarin.Forms Based Solution) --

NOTE: These instructions are for projects that wish to use Xamarin.Forms instead of a traditional Xamarin setup (that is, user interfaces written in each platform's project using that platform's UI tooling/markup).
Xamarin.Forms allows you to write shared code for your user interfaces with the Xamarin.Forms framework generating native looking user interfaces for each platform.

Samples of all files referenced below can be found here:
https://github.com/MvvmCross/MvvmCross/blob/develop/ContentFiles/

Most of these sample files can be copied directly into a newly created solution and will work. 
However, any $rootnamespace$ instances will need to be changed to your project's namespace and you will also need to add appropriate using statements (Visual Studio IntelliSense should suggest these).


- Core project -
1. Add an App class to the root folder (See Core/App.cs.pp in sample files).
2. Add a ViewModels folder to the root of the project and add at least one ViewModel class to this folder (See Core/HomeViewModel.cs.pp in sample files).


- Xamarin.Forms UI project -
1. Add the App.xaml and App.xaml.cs files to the root folder. These are just the standard files that Xamarin.Forms projects use, they don't have any MvvmCross specific modifications.
   (See Forms/FormsUIContent/App.xaml.pp and Forms/FormsUIContent/App.xaml.cs.pp in sample files).
2. Add a Views folder and add at least one XAML page to this folder to correspond to the ViewModel in the Core project (See Forms/FormsUIContent/HomeView.xaml.pp in sample files).
3. Inside the .xaml.cs file that is created when a new XAML page is added, change the class to inherit from MvxContentPage (See Forms/FormsUIContent/HomeView.xaml.cs.pp in sample files).


- Android project (ignore if not building for Android) -
1. Add a reference to the Mono.Android.Export assembly.
2. Inside MainActivity.cs, change the MainActivity class to inherit from MvxFormsAppCompatActivity<MvxFormsAndroidSetup<Core.App, FormsUI.App>, Core.App, FormsUI.App> instead of Activity (See Forms/AndroidContent/MainActivity.cs.pp in sample files).
3. Still inside MainActivity, delete all the pre-populated methods to leave a blank class.
4. Add a new XML file that defines an AppCompat theme to the Resources/values folder called styles.xml. (See Forms/AndroidContent/styles.xml.pp in sample files)
5. Ensure the theme created in the previous step is referenced in the Attribute for the MainActivity class (See Forms/AndroidContent/MainActivity.cs.pp in sample files).


- iOS projects (ignore if not building for iOS) -
1. Inside AppDelegate.cs, change the AppDelegate class to inherit from MvxFormsApplicationDelegate<MvxFormsIosSetup<Core.App, FormsUI.App>, Core.App, FormsUI.App> instead of ApplicationDelegate (See Forms/iOSContent/AppDelegate.cs.pp in sample files).
2. Still inside AppDelegate, delete all the pre-populated methods to leave a blank AppDelegate class.


- Universal Windows Platform (UWP) projects (ignore if not building for UWP) -
1. Inside App.xaml.cs add a new partial class called UWPApplication that inherits from MvxWindowsApplication<MvxFormsWindowsSetup<Core.App, FormsUI.App>, Core.App, FormsUI.App, MainPage> (See Forms/UWPContent/App.xaml.cs.pp in sample files).
2. Still inside App.xaml.cs change the App class to inherit from UWPApplication class created in the previous step (See Forms/UWPContent/App.xaml.cs.pp in sample files).
3. Still inside App.xaml.cs remove all the methods inside the App class except for the constructor. Delete everything except InitializeComponent(); from the constructor (See Forms/UWPContent/App.xaml.cs.pp in sample files).
4. Inside App.xaml change the Application XML tag to local:UWPApplication (See Forms/UWPContent/App.xaml.pp in sample files).
5. Inside MainPage.xaml change the Page XML tag to forms:MvxFormsWindowsPage and add xmlns:forms="using:MvvmCross.Forms.Platforms.Uap.Views" to that XML tag (See Forms/UWPContent/MainPage.xaml.pp in sample files).
6. Inside MainPage.xaml.cs remove the inherritance from the MainPage class so it doesn't inherit from the Page class (See Forms/UWPContent/MainPage.xaml.cs.pp in sample files).


--- Changelog ---
https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md

--- More information ---
This readme contains the bare bones instructions for setting up MvvmCross in a new solution. For complete startup documentation you can read more on our website:

Getting Started Documentation:
https://www.mvvmcross.com/documentation/getting-started/getting-started