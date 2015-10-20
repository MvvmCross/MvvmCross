$sourceNugetExe = "http://nuget.org/nuget.exe"
$targetNugetExe = $PSScriptRoot + "\nuget.exe"

if (-Not(Test-Path ("nuget.exe")))
{
	Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
}

Set-Alias nuget $targetNugetExe -Scope Global -Verbose

del *.nupkg
nuget pack MvvmCross.HotTuna.Binding.nuspec -Symbols
nuget pack MvvmCross.HotTuna.CrossCore.nuspec -Symbols
nuget pack MvvmCross.HotTuna.MvvmCrossLibraries.nuspec -Symbols
# note no -Symbols
nuget pack MvvmCross.HotTuna.StarterPack.nuspec
nuget pack MvvmCross.HotTuna.Tests.nuspec -Symbols
# note no -Symbols
nuget pack MvvmCross.nuspec
nuget pack MvvmCross.HotTuna.Touch.Dialog.nuspec -Symbols
nuget pack MvvmCross.HotTuna.Droid.Dialog.nuspec -Symbols
nuget pack MvvmCross.BindingEx.nuspec -Symbols
nuget pack MvvmCross.HotTuna.AutoViews.nuspec -Symbols
nuget pack MvvmCross.HotTuna.Touch.AutoViews.nuspec -Symbols
nuget pack MvvmCross.HotTuna.Droid.AutoViews.nuspec -Symbols
nuget pack MvvmCross.HotTuna.Droid.FullFragging.nuspec -Symbols

pause
