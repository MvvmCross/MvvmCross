del *.nupkg
nuget setapikey

nuget pack MvvmCross.Droid.Support.V7.AppCompat.nuspec -symbols
nuget pack MvvmCross.Droid.Support.Design.nuspec -Symbols
nuget pack MvvmCross.Droid.Support.V7.Fragging.nuspec -Symbols
nuget pack MvvmCross.Droid.Support.V17.Leanback.nuspec -Symbols
nuget pack MvvmCross.Droid.Support.V7.Preference.nuspec -Symbols
nuget pack MvvmCross.Droid.Support.V7.RecyclerView.nuspec -Symbols
nuget pack MvvmCross.Droid.Support.V4.nuspec -Symbols
nuget pack MvvmCross.Droid.Support.V14.Preference.nuspec -Symbols

# Cirrious legacy
nuget pack deprecated\Cirrious.MvvmCross.Droid.Support.AppCompat.nuspec
nuget pack deprecated\Cirrious.MvvmCross.Droid.Support.Design.nuspec
nuget pack deprecated\Cirrious.MvvmCross.Droid.Support.Fragging.nuspec
nuget pack deprecated\Cirrious.MvvmCross.Droid.Support.Leanback.nuspec
nuget pack deprecated\Cirrious.MvvmCross.Droid.Support.Preference.nuspec
nuget pack deprecated\Cirrious.MvvmCross.Droid.Support.RecyclerView.nuspec
nuget pack deprecated\Cirrious.MvvmCross.Droid.Support.V4.nuspec
nuget pack deprecated\Cirrious.MvvmCross.Droid.Support.V7.Preference.nuspec
nuget pack deprecated\Cirrious.MvvmCross.Droid.Support.V14.Preference.nuspec

for /r %%i in (*.nupkg) do (call :pushpackage "%%i")
pause

:pushpackage
  set np=%1
  if "%np%"=="%np:symbols=%" (
	nuget push %np% 
  )