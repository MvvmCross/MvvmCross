del *.nupkg


nuget pack Cirrious.MvvmCross.Forms.Presenter.nuspec -symbols

for /r %%i in (*.symbols.nupkg) do (call :pushpackage "%%i")
pause

:pushpackage
  set np=%1
  nuget push -source c:\dev_nupkg\ %np%
