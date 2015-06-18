del *.nupkg
nuget setapikey ---

nuget pack Cirrious.MvvmCross.Droid.Support.AppCompat.nuspec -symbols
nuget pack Cirrious.MvvmCross.Droid.Support.Fragging.nuspec -Symbols
nuget pack Cirrious.MvvmCross.Droid.Support.V4.nuspec -Symbols
nuget pack Cirrious.MvvmCross.Droid.Support.RecyclerView.nuspec -Symbols

nuget push Cirrious.MvvmCross.Droid.Support.AppCompat.1.0.0-alpha1.nupkg
nuget push Cirrious.MvvmCross.Droid.Support.Fragging.1.0.0-alpha1.nupkg
nuget push Cirrious.MvvmCross.Droid.Support.V4.1.0.0-alpha1.nupkg
nuget push Cirrious.MvvmCross.Droid.Support.RecyclerView.1.0.0-alpha1.nupkg
pause