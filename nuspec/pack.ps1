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
nuget pack MvvmCross.Plugin.Bookmarks.nuspec -Symbols
nuget pack MvvmCross.Plugin.Color.nuspec -Symbols
nuget pack MvvmCross.Plugin.DownloadCache.nuspec -Symbols
nuget pack MvvmCross.Plugin.Email.nuspec -Symbols
nuget pack MvvmCross.Plugin.FieldBinding.nuspec -Symbols
nuget pack MvvmCross.Plugin.File.nuspec -Symbols
nuget pack MvvmCross.Plugin.Json.nuspec -Symbols
nuget pack MvvmCross.Plugin.JsonLocalization.nuspec -Symbols
nuget pack MvvmCross.Plugin.Location.nuspec -Symbols
nuget pack MvvmCross.Plugin.Messenger.nuspec -Symbols
nuget pack MvvmCross.Plugin.MethodBinding.nuspec -Symbols
nuget pack MvvmCross.Plugin.Network.nuspec -Symbols
nuget pack MvvmCross.Plugin.PhoneCall.nuspec -Symbols
nuget pack MvvmCross.Plugin.PictureChooser.nuspec -Symbols
nuget pack MvvmCross.Plugin.ReflectionEx.nuspec -Symbols
nuget pack MvvmCross.Plugin.ResourceLoader.nuspec -Symbols
nuget pack MvvmCross.Plugin.Share.nuspec -Symbols
nuget pack MvvmCross.Plugin.SoundEffects.nuspec -Symbols
nuget pack MvvmCross.Plugin.SQLitePCL.nuspec -Symbols
nuget pack MvvmCross.Plugin.ThreadUtils.nuspec -Symbols
nuget pack MvvmCross.Plugin.Visibility.nuspec -Symbols
nuget pack MvvmCross.Plugin.WebBrowser.nuspec -Symbols

# HotTuna legacy
nuget pack deprecated\MvvmCross.HotTuna.Plugin.Accelerometer.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.All.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.Bookmarks.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.Color.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.DownloadCache.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.Email.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.FieldBinding.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.File.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.Json.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.JsonLocalisation.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.Location.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.Messenger.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.MethodBinding.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.Network.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.PhoneCall.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.PictureChooser.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.ReflectionEx.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.ResourceLoader.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.SQLite.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.SQLitePCL.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.Share.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.SoundEffects.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.ThreadUtils.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.Visibility.nuspec
nuget pack deprecated\MvvmCross.HotTuna.Plugin.WebBrowser.nuspec

pause
