---------------------------------
MvvmCross
---------------------------------

IMPORTANT: No nuget content files will be installed!

Files to change yourself in every platform project:

- Core
* Add App.cs

- All app platforms
* Add Setup.cs

- Android, iOS, tvOS, MacOS
* Add LinkerPleaseInclude.cs

- Android
* Extend MainActivity from MvxAppCompatActivity. ( MainActivity : MvxAppCompatActivity { } )
* Add SplashScreen.cs if you want a splashscreen

- iOS, MacOS, tvOS
* Extend AppDelegate from MvxAppDelegate. ( AppDelegate : MvxAppDelegate { } )

- UWP, WPF
* Extend App from MvxApplication. ( App : MvxApplication { } )

Sample files are available at:
https://github.com/MvvmCross/MvvmCross/blob/develop/ContentFiles/

Changelog:
https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md

For complete startup documentation you can read more on our website:

Getting Started Documentation:
https://www.mvvmcross.com/documentation/getting-started/getting-started