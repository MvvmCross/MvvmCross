del *.nupkg
nuget setapikey

nuget pack Cirrious.MvvmCross.Droid.Support.AppCompat.nuspec -symbols
nuget pack Cirrious.MvvmCross.Droid.Support.Fragging.nuspec -Symbols
nuget pack Cirrious.MvvmCross.Droid.Support.V4.nuspec -Symbols
nuget pack Cirrious.MvvmCross.Droid.Support.RecyclerView.nuspec -Symbols
nuget pack Cirrious.MvvmCross.Droid.Support.Design.nuspec -Symbols
nuget pack Cirrious.MvvmCross.Droid.Support.Leanback.nuspec -Symbols
nuget pack Cirrious.MvvmCross.Droid.Support.Preference.nuspec -Symbols

for /r %%i in (*.nupkg) do (call :pushpackage "%%i")
pause

:pushpackage
  set np=%1
  if "%np%"=="%np:symbols=%" (
	nuget push %np% 
  )