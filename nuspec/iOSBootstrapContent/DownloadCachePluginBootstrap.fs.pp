namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type DownloadCachePluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.DownloadCache.PluginLoader, MvvmCross.Plugins.DownloadCache.iOS.Plugin>()