namespace $rootnamespace$

open Android.App
open Android.Content.PM
open MvvmCross.Droid.Views

[<Activity(
    Label = "$rootnamespace$"
    , MainLauncher = true
    , Icon = "@mipmap/icon"
    , Theme = "@style/Theme.Splash"
    , NoHistory = true
    , ScreenOrientation = ScreenOrientation.Portrait)>]
type SplashScreen() =
    inherit MvxSplashScreenActivity(Resource.Layout.SplashScreen)