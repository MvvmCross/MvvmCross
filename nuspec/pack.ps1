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
nuget pack MvvmCross.Platform.nuspec -Symbols
# note no -Symbols
nuget pack MvvmCross.StarterPack.nuspec
nuget pack MvvmCross.Tests.nuspec -Symbols
# note no -Symbols
nuget pack MvvmCross.nuspec
nuget pack MvvmCross.Dialog.Touch.nuspec -Symbols
nuget pack MvvmCross.Dialog.Droid.nuspec -Symbols
nuget pack MvvmCross.BindingEx.nuspec -Symbols
nuget pack MvvmCross.AutoView.nuspec -Symbols
nuget pack MvvmCross.AutoView.Touch.nuspec -Symbols
nuget pack MvvmCross.AutoView.Droid.nuspec -Symbols
nuget pack MvvmCross.Droid.FullFragging.nuspec -Symbols

pause
