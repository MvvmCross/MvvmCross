del *.nupkg
nuget setapikey

nuget pack MvvmCross.Droid.Support.Core.UI.nuspec -symbols
nuget pack MvvmCross.Droid.Support.Core.Util.nuspec -symbols
nuget pack MvvmCross.Droid.Support.Fragment.nuspec -symbols
nuget pack MvvmCross.Droid.Support.Design.nuspec -Symbols
nuget pack MvvmCross.Droid.Support.V14.Preference.nuspec -Symbols
nuget pack MvvmCross.Droid.Support.V17.Leanback.nuspec -Symbols
nuget pack MvvmCross.Droid.Support.V4.nuspec
nuget pack MvvmCross.Droid.Support.V7.AppCompat.nuspec -symbols
nuget pack MvvmCross.Droid.Support.V7.Preference.nuspec -Symbols
nuget pack MvvmCross.Droid.Support.V7.RecyclerView.nuspec -Symbols

for /r %%i in (*.nupkg) do (call :pushpackage "%%i")
pause

:pushpackage
  set np=%1
  if "%np%"=="%np:symbols=%" (
	nuget push %np% 
  )