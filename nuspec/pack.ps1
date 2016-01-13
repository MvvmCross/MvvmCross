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
nuget pack MvvmCross.Console.Platform.nuspec -Symbols
# note no -Symbols
nuget pack MvvmCross.StarterPack.nuspec
nuget pack MvvmCross.Tests.nuspec -Symbols
# note no -Symbols
nuget pack MvvmCross.nuspec
nuget pack MvvmCross.Dialog.iOS.nuspec -Symbols
nuget pack MvvmCross.Dialog.Droid.nuspec -Symbols
nuget pack MvvmCross.BindingEx.nuspec -Symbols
nuget pack MvvmCross.AutoView.nuspec -Symbols
nuget pack MvvmCross.AutoView.iOS.nuspec -Symbols
nuget pack MvvmCross.AutoView.Droid.nuspec -Symbols
nuget pack MvvmCross.Droid.FullFragging.nuspec -Symbols

# HotTuna legacy
nuget pack deprecated\MvvmCross.HotTuna.Binding.nuspec
nuget pack deprecated\MvvmCross.HotTuna.CrossCore.nuspec
nuget pack deprecated\MvvmCross.HotTuna.MvvmCrossLibraries.nuspec
nuget pack deprecated\MvvmCross.HotTuna.StarterPack.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Tests.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Touch.Dialog.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Droid.Dialog.nuspec
nuget pack deprecated\MvvmCross.HotTuna.AutoViews.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Touch.AutoViews.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Droid.AutoViews.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Droid.FullFragging.nuspec

pause
