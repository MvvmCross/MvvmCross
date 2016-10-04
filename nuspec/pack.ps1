$sourceNugetExe = "http://nuget.org/nuget.exe"
$targetNugetExe = $PSScriptRoot + "\nuget.exe"

if (-Not(Test-Path ("nuget.exe")))
{
	Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
}

Set-Alias nuget $targetNugetExe -Scope Global -Verbose

del *.nupkg

if (-Not(Get-Command "gitlink.exe" -ErrorAction SilentlyContinue))
{
    Write-Host "gitlink.exe is required to provide source information directly from the MvvmCross repositories on github." -ForegroundColor Yellow
	Write-Host "See https://github.com/GitTools/GitLink for more information." -ForegroundColor Yellow
    Write-Host "Please install gitlink via chocolatey" -ForegroundColor Red
	return
}

$sourcePath = (Get-Item $PSScriptRoot).parent.FullName
gitlink $sourcePath -u https://github.com/MvvmCross/MvvmCross

nuget pack MvvmCross.Binding.nuspec
nuget pack MvvmCross.Core.nuspec
nuget pack MvvmCross.Platform.nuspec
nuget pack MvvmCross.Console.Platform.nuspec
nuget pack MvvmCross.StarterPack.nuspec
nuget pack MvvmCross.CodeAnalysis.nuspec
nuget pack MvvmCross.Tests.nuspec
nuget pack MvvmCross.nuspec
nuget pack MvvmCross.Dialog.iOS.nuspec
nuget pack MvvmCross.Dialog.Droid.nuspec
nuget pack MvvmCross.BindingEx.nuspec
nuget pack MvvmCross.AutoView.nuspec
nuget pack MvvmCross.AutoView.iOS.nuspec
nuget pack MvvmCross.AutoView.Droid.nuspec
nuget pack MvvmCross.Droid.FullFragging.nuspec
nuget pack MvvmCross.Droid.Shared.nuspec

pause
