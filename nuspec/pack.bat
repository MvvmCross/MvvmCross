del *.nupkg
nuget setapikey ---

nuget pack Cheesebaron.MvxPlugins.AppId.nuspec -symbols
nuget pack Cheesebaron.MvxPlugins.Sms.nuspec -symbols
nuget pack Cheesebaron.MvxPlugins.Settings.nuspec -symbols
nuget pack Cheesebaron.MvxPlugins.SimpleWebToken.nuspec -symbols
nuget pack Cheesebaron.MvxPlugins.FormsPresenters.nuspec -symbols
nuget pack Cheesebaron.MvxPlugins.Notifications.nuspec -symbols

for /r %%i in (*.nupkg) do (call :pushpackage "%%i")
pause

:pushpackage
  set np=%1
  if "%np%"=="%np:symbols=%" (
	nuget push %np% 
  )