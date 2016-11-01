$sourceNugetExe = "http://nuget.org/nuget.exe"
$targetNugetExe = $PSScriptRoot + "\nuget.exe"

if (-Not(Test-Path ("nuget.exe")))
{
	Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
}

Set-Alias nuget $targetNugetExe -Scope Global -Verbose

del *.nupkg

nuget pack MvvmCross.Plugin.Accelerometer.nuspec -Symbols
nuget pack MvvmCross.Plugin.All.nuspec -Symbols
nuget pack MvvmCross.Plugin.Color.nuspec -Symbols
nuget pack MvvmCross.Plugin.DownloadCache.nuspec -Symbols
nuget pack MvvmCross.Plugin.Email.nuspec -Symbols
nuget pack MvvmCross.Plugin.FieldBinding.nuspec -Symbols
nuget pack MvvmCross.Plugin.File.nuspec -Symbols
nuget pack MvvmCross.Plugin.Json.nuspec -Symbols
nuget pack MvvmCross.Plugin.JsonLocalization.nuspec -Symbols
nuget pack MvvmCross.Plugin.Location.nuspec -Symbols
nuget pack MvvmCross.Plugin.Location.Fused.nuspec -Symbols
nuget pack MvvmCross.Plugin.Messenger.nuspec -Symbols
nuget pack MvvmCross.Plugin.MethodBinding.nuspec -Symbols
nuget pack MvvmCross.Plugin.Network.nuspec -Symbols
nuget pack MvvmCross.Plugin.PhoneCall.nuspec -Symbols
nuget pack MvvmCross.Plugin.PictureChooser.nuspec -Symbols
nuget pack MvvmCross.Plugin.ReflectionEx.nuspec -Symbols
nuget pack MvvmCross.Plugin.ResourceLoader.nuspec -Symbols
nuget pack MvvmCross.Plugin.ResxLocalization.nuspec -Symbols
nuget pack MvvmCross.Plugin.Share.nuspec -Symbols
nuget pack MvvmCross.Plugin.SQLitePCL.nuspec -Symbols
nuget pack MvvmCross.Plugin.ThreadUtils.nuspec -Symbols
nuget pack MvvmCross.Plugin.Visibility.nuspec -Symbols
nuget pack MvvmCross.Plugin.WebBrowser.nuspec -Symbols

pause
