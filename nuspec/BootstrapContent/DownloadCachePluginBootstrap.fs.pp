namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type DownloadCachePluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.DownloadCache.PluginLoader>()