del *.nupkg
nuget setapikey

nuget pack MvvmCross.iOS.Support.nuspec -symbols


for /r %%i in (*.nupkg) do (call :pushpackage "%%i")
pause

:pushpackage
  set np=%1
  if "%np%"=="%np:symbols=%" (
	nuget push %np% 
  )