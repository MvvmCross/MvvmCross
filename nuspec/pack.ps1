$sourceNugetExe = "http://nuget.org/nuget.exe"
$targetNugetExe = $PSScriptRoot + "\nuget.exe"

if (-Not(Test-Path ("nuget.exe")))
{
	Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
}

Set-Alias nuget $targetNugetExe -Scope Global -Verbose

del *.nupkg
nuget pack MvvmCross.Binding.nuspec -Symbols
nuget pack MvvmCross.Core.nuspec -Symbols
nuget pack MvvmCross.Console.Platform.nuspec -Symbols
# note no -Symbols
nuget pack MvvmCross.StarterPack.nuspec
nuget pack MvvmCross.Tests.nuspec -Symbols
# note no -Symbols
nuget pack MvvmCross.nuspec
nuget pack MvvmCross.Touch.Dialog.nuspec -Symbols
nuget pack MvvmCross.Droid.Dialog.nuspec -Symbols
nuget pack MvvmCross.BindingEx.nuspec -Symbols
nuget pack MvvmCross.AutoViews.nuspec -Symbols
nuget pack MvvmCross.Touch.AutoViews.nuspec -Symbols
nuget pack MvvmCross.Droid.AutoViews.nuspec -Symbols
nuget pack MvvmCross.Droid.FullFragging.nuspec -Symbols

pause
