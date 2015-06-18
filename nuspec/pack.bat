del *.nupkg
nuget setapikey 

nuget pack Cirrious.MvvmCross.Droid.Support.AppCompat.nuspec -symbols
nuget pack Cirrious.MvvmCross.Droid.Support.Fragging.nuspec -Symbols
nuget pack Cirrious.MvvmCross.Droid.Support.V4.nuspec -Symbols
nuget pack Cirrious.MvvmCross.Droid.Support.RecyclerView.nuspec -Symbols

for /r %%i in (*.nupkg) do nuget push %%i
pause